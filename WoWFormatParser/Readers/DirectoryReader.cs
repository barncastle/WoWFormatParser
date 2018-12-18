using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures;

namespace WoWFormatParser.Readers
{
    internal class DirectoryReader
    {
        private readonly string _directory;
        private readonly Options _options;
        private readonly WoWBuild _build;
        private const StringComparison _comparison = StringComparison.OrdinalIgnoreCase;

        public DirectoryReader(string directory, Options options, WoWBuild build)
        {
            _directory = directory;
            _options = options;
            _build = build;
        }

        /// <summary>
        /// Enumerates all MPQ files in the WoW directory with optional Blizzard-style sorting.
        /// </summary>
        /// <param name="blizzardSorting"></param>
        /// <returns></returns>
        public IEnumerable<string> GetArchives(string searchPattern, bool blizzardSorting)
        {
            searchPattern = Utils.CorrectSearchPattern(searchPattern);
            if (!searchPattern.EndsWith("mpq", _comparison))
                searchPattern += "*.mpq";

            // enumerates all MPQs, removes invalid directories and then optionally applies Blizzard-style sorting
            var archives = Directory.EnumerateFiles(_directory, searchPattern, SearchOption.AllDirectories).ToList();
            archives.RemoveAll(x => _options.HasIgnoredDirectory(x));

            if (blizzardSorting)
            {
                // remove patch archives
                archives.RemoveAll(x => Path.GetFileName(x).StartsWith("wow-update", _comparison));
                archives.Sort(MPQSorter.Sort);
            }

            return archives;
        }

        /// <summary>
        /// Returns the Incremental Patch Archives used in Cata+.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetPatchArchives()
        {
            return Directory.EnumerateFiles(_directory, "*wow-update*.mpq", SearchOption.AllDirectories);
        }

        /// <summary>
        /// Returns the filenames of all local files in the WoW directory.
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
		public IEnumerable<string> GetFiles(string searchPattern)
        {
            return Directory.EnumerateFiles(_directory, Utils.CorrectSearchPattern(searchPattern));
        }

        /// <summary>
        /// Enumerates local files in the WoW directory
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public IEnumerable<IFormat> Enumerate(string searchPattern, bool useParallelization = true)
        {
            searchPattern = Utils.CorrectSearchPattern(searchPattern);

            var fileReader = new FileReader(_build, _options);
            var files = Directory.EnumerateFiles(_directory, searchPattern, SearchOption.AllDirectories);

            bool includeFileInfo = _options.ExportType.HasFlag(ExportType.FileInfo);
            string localDir = _directory + Path.DirectorySeparatorChar;

            ConcurrentBag<IFormat> resultSet = new ConcurrentBag<IFormat>();

            ParallelOptions options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = useParallelization ? -1 : 1
            };

            Parallel.ForEach(files, options, file =>
            {
                string filename = file.Replace(localDir, "", _comparison).WoWNormalize();

                // validate search criteria
                if (Utils.IsInvalidFile(filename, _options))
                    return;

                using (var md5 = MD5.Create())
                using (var stream = File.OpenRead(file))
                using (var bs = new BufferedStream(stream))
                {
                    Structures.Meta.FileInfo entry = new Structures.Meta.FileInfo()
                    {
                        Build = _build.Build,
                        Name = filename
                    };

                    if (includeFileInfo)
                    {
                        entry.Checksum = md5.ComputeHash(stream).ToChecksum();
                        entry.Created = Utils.GetLocalFileCreated(filename);
                        entry.Size = (uint)stream.Length;
                    }

                    resultSet.Add(fileReader.Read(bs, entry));
                }
            });

            return resultSet;
        }
    }
}
