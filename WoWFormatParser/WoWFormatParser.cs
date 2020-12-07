using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WoWFormatParser.Readers;
using WoWFormatParser.Structures;

namespace WoWFormatParser
{
    public class WoWFormatParser : IDisposable
    {
        public readonly WoWBuild Build;
        public readonly string Directory;
        public readonly Options Options;

        private readonly DirectoryReader _directoryReader;
        private readonly MPQReader _mpqReader;
        private const StringComparison Comparison = StringComparison.OrdinalIgnoreCase;

        public WoWFormatParser(string directory, WoWBuild build, Options options = null)
        {
            Directory = directory;
            if (!System.IO.Directory.Exists(Directory))
                throw new ArgumentException("Invalid Directory argument.");

            Build = build;
            if (Build == null)
                throw new ArgumentException("Invalid Build argument.");

            Options = options ?? new Options();

            _directoryReader = new DirectoryReader(Directory, Options, Build);
            _mpqReader = new MPQReader(Options, Build, _directoryReader.GetPatchArchives());
        }


        #region File Reading Methods
        /// <summary>
        /// Parses files found in the WoW directory.
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public IEnumerable<IFormat> EnumerateDirectory(string searchPattern = "*", bool useParallelization = true)
        {
            return _directoryReader.Enumerate(searchPattern, useParallelization);
        }
        /// <summary>
        /// Parses files found in the WoW archives.
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <param name="useParallelization"></param>
        /// <returns></returns>
        public IEnumerable<IFormat> EnumerateArchives(string searchPattern = "*", bool useParallelization = true)
        {
            var archives = _directoryReader.GetArchives("*", true);
            var fileLookup = _mpqReader.GetFileLookup(archives, searchPattern).AsParallel();

            if (!useParallelization)
                fileLookup = fileLookup.WithDegreeOfParallelism(1);

            return fileLookup.Select(lookup => _mpqReader.ReadFile(lookup.Key, lookup.Value));
        }
        /// <summary>
        /// Parses files found in the WoW archives as the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searchPattern"></param>
        /// <param name="useParallelization"></param>
        /// <returns></returns>
        public IEnumerable<T> EnumerateArchives<T>(string searchPattern = "*", bool useParallelization = true) where T : Format
        {
            return EnumerateArchives(searchPattern, useParallelization).Where(x => x.Is<T>()).Cast<T>();
        }
        /// <summary>
        /// Parses an external file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="build"></param>
        /// <returns></returns>
        public IFormat ParseFile(string path, WoWBuild build = null)
        {
            var _fileReader = new FileReader(build ?? Build, Options);
            return _fileReader.Read(path);
        }
        /// <summary>
        /// Parses an external file as the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="build"></param>
        /// <returns></returns>
        public T ParseFile<T>(string path, WoWBuild build = null) where T : Format
        {
            var _fileReader = new FileReader(build ?? Build, Options);
            return _fileReader.Read(path) as T;
        }
        /// <summary>
        /// Parses a one or more external files.
        /// </summary>
        /// <param name="build"></param>
        /// <param name="paths"></param>
        /// <returns></returns>
        public IEnumerable<IFormat> ParseFiles(WoWBuild build = null, params string[] paths)
        {
            var _fileReader = new FileReader(build ?? Build, Options);
            return paths.Select(path => _fileReader.Read(path));
        }
        /// <summary>
        /// Parses a one or more external files as the specified type.
        /// </summary>
        /// <param name="build"></param>
        /// <param name="paths"></param>
        /// <returns></returns>
        public IEnumerable<T> ParseFiles<T>(WoWBuild build = null, params string[] paths) where T : Format
        {
            return ParseFiles(build, paths).Where(x => x.Is<T>()).Cast<T>();
        }

        #endregion

        #region File Listing Methods
        /// <summary>
        /// Returns a list of filenames from the WoW directory.
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <param name="includeFileSystemPath"></param>
        /// <returns></returns>
        public IEnumerable<string> GetLocalFiles(string searchPattern = "*", bool includeFileSystemPath = false)
        {
            string localDir = Directory + Path.DirectorySeparatorChar;
            var files = _directoryReader.GetFiles(searchPattern);

            if (includeFileSystemPath)
                return files;

            return files.Select(x => x.Replace(localDir, "", Comparison));
        }
        /// <summary>
        /// Returns a list of filenames from the WoW archives.
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public IEnumerable<string> GetListFile(string searchPattern = "*")
        {
            var archives = _directoryReader.GetArchives("*", true);
            return _mpqReader.GetFileLookup(archives, searchPattern).Select(x => x.Key).OrderBy(x => x);
        }
        /// <summary>
        /// Returns a list of the WoW archive names.
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <param name="includeFileSystemPath"></param>
        /// <returns></returns>
        public IEnumerable<string> GetArchives(string searchPattern = "*", bool includeFileSystemPath = false)
        {
            string localDir = Directory + Path.DirectorySeparatorChar;
            var files = _directoryReader.GetArchives(searchPattern, false);

            if (includeFileSystemPath)
                return files;

            return files.Select(x => x.Replace(localDir, "", Comparison));
        }
        #endregion

        #region File Parsing Methods
        /// <summary>
        /// Jsonifies the parsed content of local and archived files.
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public IEnumerable<string> Jsonify(string searchPattern = "*")
        {
            Serializer.Serializer serializer = new Serializer.Serializer(Options.SerializerOptions);
            var items = EnumerateDirectory(searchPattern).Concat(EnumerateArchives(searchPattern));
            return serializer.Serialize(items);

        }
        /// <summary>
        /// Jsonifies the parsed content of the provided files.
        /// </summary>
        /// <param name="formats"></param>
        /// <returns></returns>
        public IEnumerable<string> Jsonify(IEnumerable<IFormat> formats)
        {
            Serializer.Serializer serializer = new Serializer.Serializer(Options.SerializerOptions);
            return serializer.Serialize(formats);
        }
        /// <summary>
        /// Exports the jsonified parsed content of local and archived files.
        /// <para>This runs as a background task.</para>
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public CancellationTokenSource Export(string searchPattern = "*")
        {
            var cts = new CancellationTokenSource();

            Task.Run(() =>
            {
                Serializer.Serializer serializer = new Serializer.Serializer(Options.SerializerOptions);
                var files = EnumerateDirectory(searchPattern).Concat(EnumerateArchives(searchPattern));
                serializer.Export(files);

            }, cts.Token);

            return cts;
        }
        /// <summary>
        /// Exports the jsonified parsed content of the provided files.
        /// <para>This runs as a background task.</para>
        /// </summary>
        /// <param name="formats"></param>
        /// <returns></returns>
        public CancellationTokenSource Export(IEnumerable<IFormat> formats)
        {
            var cts = new CancellationTokenSource();

            Task.Run(() =>
            {
                Serializer.Serializer serializer = new Serializer.Serializer(Options.SerializerOptions);
                serializer.Export(formats);
            }, cts.Token);

            return cts;
        }
        #endregion


        public void Dispose()
        {
            _mpqReader?.Dispose();
        }
    }
}
