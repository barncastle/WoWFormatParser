using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using WoWFormatParser.Archives.MPQ;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures;

namespace WoWFormatParser.Readers
{
    using IELookup = IEnumerable<KeyValuePair<string, string>>;

    internal class MPQReader : IDisposable
    {
        const string EMPTY_CHECKSUM = "00000000000000000000000000000000";
        const string LISTFILE_NAME = "(ListFile)";
        private readonly string DATA_DIRECTORY = Path.DirectorySeparatorChar + "DATA" + Path.DirectorySeparatorChar;

        private readonly string _directory;
        private readonly Options _options;
        private readonly WoWBuild _build;
        private readonly FileReader _fileReader;
        private readonly string[] _patchArchives;

        private ConcurrentDictionary<string, MpqArchive> _archiveLocks;

        public MPQReader(string directory, Options options, WoWBuild build, IEnumerable<string> patchArchives)
        {
            _directory = directory;
            _options = options;
            _build = build;
            _fileReader = new FileReader(_build, _options);
            _patchArchives = patchArchives.ToArray();

            _archiveLocks = new ConcurrentDictionary<string, MpqArchive>();
        }


        /// <summary>
        /// Reads a specific file from an archive. Will parse the archive name if no filename is specified (Hot Swappable).
        /// </summary>
        /// <param name="archivename"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public IFormat ReadFile(string filename, string archivename)
        {
            if (!_archiveLocks.TryGetValue(archivename, out MpqArchive mpq))
            {
                mpq = new MpqArchive(archivename, FileAccess.Read);
                mpq.AddPatchArchives(_patchArchives);
                _archiveLocks.TryAdd(archivename, mpq);
            }

            // add the filename to the alpha mpqs an set the flat filename
            string overridename = null;
            if (archivename.Contains(filename, StringComparison.OrdinalIgnoreCase))
            {
                overridename = Path.GetFileName(filename);
                mpq.AddListFile(overridename);
            }

            return ReadFileImpl(mpq, filename, overridename);
        }

        /// <summary>
        /// Builds a lookup of all files and their container that meet the search criteria.
        /// </summary>
        /// <param name="archives"></param>
        /// <returns></returns>
        public IELookup GetFileLookup(IEnumerable<string> archives, string searchPattern)
        {
            Clear();

            var lookup = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            searchPattern = Utils.CorrectSearchPattern(searchPattern);

            foreach (string archive in archives)
            {
                if (TryGetListFile(archive, searchPattern, out var filteredlistfile))
                {
                    foreach (string filename in filteredlistfile)
                        lookup.TryAdd(filename, archive);
                }
                else if (IsLikleyPreAlpha(archive))
                {
                    string filename = Path.ChangeExtension(FormatAlphaFilename(archive), null).WoWNormalize();
                    if (!Utils.IsInvalidFile(filename, _options, searchPattern))
                        lookup.TryAdd(filename, archive);
                }
                else
                {
                    // TODO MPQs without a listfile
                    // throw new NotImplementedException($"MPQs without ListFiles are currently unsupported '{Path.GetFileName(archive)}'.");
                }
            }

            return lookup;
        }

        /// <summary>
        /// Parses the archive file.
        /// </summary>
        /// <param name="mpq"></param>
        /// <param name="filename"></param>
        /// <param name="overridename">Alpha MPQ filename override.</param>
        /// <returns></returns>
        private IFormat ReadFileImpl(MpqArchive mpq, string filename, string overridename = null)
        {
            using (var md5 = MD5.Create())
            using (var stream = mpq.OpenFile(overridename ?? filename))
            {
                if (!stream.CanRead || stream.Length <= 1)
                    return null;

                // validate filesize limit
                if (_options.MaxFileSize > 0 && stream.Length > _options.MaxFileSize)
                    return null;

                Structures.Meta.FileInfo entry = new Structures.Meta.FileInfo()
                {
                    Build = _build.Build,
                    Name = filename.WoWNormalize()
                };

                if (_options.ParseMode.HasFlag(ParseMode.FileInfo))
                {
                    entry.Checksum = stream.GetMd5Hash();
                    if (string.IsNullOrWhiteSpace(entry.Checksum) || entry.Checksum == EMPTY_CHECKSUM)
                        entry.Checksum = md5.ComputeHash(stream).ToChecksum();
                    entry.Created = stream.CreatedDate;
                    entry.Size = (uint)stream.Length;
                }

                stream.FileName = filename;

                using (var bs = new BufferedStream(stream))
                    return _fileReader.Read(bs, entry);
            }
        }


        /// <summary>
        /// Removes the local directory information.
        /// </summary>
        /// <param name="archivename"></param>
        /// <returns></returns>
        private string FormatAlphaFilename(string archivename)
        {
            int index = archivename.IndexOf(DATA_DIRECTORY, StringComparison.OrdinalIgnoreCase);
            return archivename.Substring(index + DATA_DIRECTORY.Length);
        }

        /// <summary>
        /// Validates pre-0.6.0 build and if the pre-MPQ extension is a known filetype.
        /// </summary>
        /// <param name="archivename"></param>
        /// <returns></returns>
        private bool IsLikleyPreAlpha(string archivename)
        {
            string extension = Path.GetFileNameWithoutExtension(archivename).GetExtensionExt();
            if (_build.Build < 3529 && Enum.TryParse(typeof(WoWFormat), extension, true, out var dump))
                return true;

            return false;
        }

        /// <summary>
        /// Attempts to open and filter the ListFile.
        /// </summary>
        /// <param name="archivename"></param>
        /// <param name="filteredlist"></param>
        /// <returns></returns>
        private bool TryGetListFile(string archivename, string searchPattern, out IEnumerable<string> filteredlist)
        {
            using (var mpq = new MpqArchive(archivename, FileAccess.Read))
            {
                mpq.AddPatchArchives(_patchArchives);

                if (mpq.HasFile(LISTFILE_NAME))
                {
                    filteredlist = FilterListFile(mpq, searchPattern).ToArray();
                    return true;
                }
            }

            filteredlist = null;
            return false;
        }

        /// <summary>
        /// Filters the ListFile as per the Options.
        /// </summary>
        /// <param name="mpq"></param>
        /// <returns></returns>
        private IEnumerable<string> FilterListFile(MpqArchive mpq, string searchPattern)
        {
            using (var file = mpq.OpenFile(LISTFILE_NAME))
            using (var sr = new StreamReader(file))
            {
                while (!sr.EndOfStream)
                {
                    string filename = sr.ReadLine();
                    if (!Utils.IsInvalidFile(filename, _options, searchPattern))
                        yield return filename;
                }
            }
        }


        #region Cleanup Functions
        private bool disposedValue = false;

        private void Clear()
        {
            foreach (var archive in _archiveLocks)
                archive.Value?.Dispose();
            _archiveLocks.Clear();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                    Clear();

                disposedValue = true;
            }
        }

        public void Dispose() => Dispose(true);
        #endregion
    }
}
