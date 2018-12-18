using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser
{
    using ReaderFunc = Func<Stream, Structures.Meta.FileInfo, IFormat>;

    internal class FileReader
    {
        public readonly WoWBuild Build;

        private readonly Options _options;
        private readonly Dictionary<WoWFormat, ReaderFunc> _readers;

        private readonly bool _readFileInfo;
        private readonly bool _readData;

        public FileReader(WoWBuild build, Options options)
        {
            Build = build;
            _options = options;

            _readData = _options.ExportType.HasFlag(ExportType.Data);
            _readFileInfo = _options.ExportType.HasFlag(ExportType.FileInfo);


            // create referenced readers
            _readers = new Dictionary<WoWFormat, ReaderFunc>()
            {
                { WoWFormat.DBC, ReadMeta<Structures.Meta.DBCMeta> },
                { WoWFormat.TOC, ReadMeta<Structures.Meta.TOCMeta> },
                { WoWFormat.WDB, ReadMeta<Structures.Meta.WDBMeta> },

                { WoWFormat.BLP, ReadFormat<Structures.BLP.BLP> },
                { WoWFormat.BLS, ReadFormat<Structures.BLS.BLS> },
                { WoWFormat.DB, ReadFormat<Structures.DB.DB> },
                { WoWFormat.DEF, ReadFormat<Structures.DEF.DEF> },
                { WoWFormat.MDX, ReadFormat<Structures.MDX.MDX> },
                { WoWFormat.LIT, ReadFormat<Structures.LIT.LIT> },
                { WoWFormat.M2, ReadFormat<Structures.M2.M2> },
                { WoWFormat.WDL, ReadFormat<Structures.WDL.WDL> },
                { WoWFormat.WDT, ReadFormat<Structures.WDT.WDT> },
                { WoWFormat.WLQ, ReadFormat<Structures.WLX.WLQ> },
                { WoWFormat.WLX, ReadFormat<Structures.WLX.WLX> },

                { WoWFormat.ADT, ReadVersioned<Structures.ADT.ADT> },
                { WoWFormat.WDTADT, ReadVersioned<Structures.ADT.ADT> },
                { WoWFormat.WMO, ReadVersioned<Structures.WMO.WMO> },
                { WoWFormat.WMOGROUP, ReadVersioned<Structures.WMO.WMOGroup> },
            };
        }

        #region Read Methods
        public IFormat Read(string filename, uint build = 0)
        {
            var entry = new Structures.Meta.FileInfo()
            {
                Build = Build.Build,
                Name = filename.WoWNormalize()
            };

            using (var md5 = MD5.Create())
            using (var stream = File.OpenRead(filename))
            {
                if (_readFileInfo)
                {
                    entry.Checksum = md5.ComputeHash(stream).ToChecksum();
                    entry.Created = Utils.GetLocalFileCreated(filename);
                    entry.Size = (uint)stream.Length;
                }

                return Read(stream, entry);
            }
        }

        public IFormat Read(Stream stream, Structures.Meta.FileInfo file)
        {
            stream.Position = 0;

            try
            {
                if (_readData && _readers.TryGetValue(file.Format, out var readerFunc))
                    return readerFunc.Invoke(stream, file);
            }
            catch (System.Reflection.TargetInvocationException) { }
            catch (EndOfStreamException) { }

            return ReadSimple(file);
        }
        #endregion


        #region Format Reading Methods
        private IFormat ReadVersioned<T>(Stream stream, Structures.Meta.FileInfo file) where T : Format, IVersioned
        {
            if (stream.CanRead)
            {
                using (var br = new BinaryReader(stream))
                {
                    T chunked = (T)Activator.CreateInstance(typeof(T), br, Build.Build);
                    chunked.FileName = file.Name;
                    if (_readFileInfo)
                        chunked.FileInfo = file;

                    return chunked;
                }
            }

            return ReadSimple(file);
        }

        private IFormat ReadFormat<T>(Stream stream, Structures.Meta.FileInfo file) where T : Format
        {
            if (stream.CanRead)
            {
                using (var br = new BinaryReader(stream))
                {
                    T format = (T)Activator.CreateInstance(typeof(T), br);
                    format.FileName = file.Name;
                    if (_readFileInfo)
                        format.FileInfo = file;

                    return format;
                }
            }

            return ReadSimple(file);
        }

        private IFormat ReadMeta<T>(Stream stream, Structures.Meta.FileInfo file) where T : Format, Structures.Meta.IMetaFormat
        {
            if (stream.CanRead)
            {
                T format = (T)Activator.CreateInstance(typeof(T), file.Name, Build.Build, stream);
                if (_readFileInfo)
                    format.FileInfo = file;

                return format;
            }

            return ReadSimple(file);
        }

        private IFormat ReadSimple(Structures.Meta.FileInfo file)
        {
            if (!_options.IncludeUnsupportedAndInvalidFiles)
                return null;

            return new SimpleFormat()
            {
                FileName = file.Name,
                FileInfo = _readFileInfo ? file : null
            };
        }

        #endregion
    }
}
