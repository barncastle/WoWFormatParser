using System.IO;

namespace WoWFormatParser.Structures.Meta
{
    public sealed class TOCMeta : Format, IMetaFormat
    {
        public string Version;
        public uint Build;
        public string Author;
        public string Title;
        public string Interface;
        public string Notes;
        public bool LoadOnDemand;

        public TOCMeta(string name, uint build, Stream stream)
        {
            using var sr = new StreamReader(stream);
            FileName = name;
            Build = build;

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] parts = line.Split(new char[] { ':' }, 2);

                if (parts.Length == 2)
                {
                    switch (parts[0].Trim().ToLower())
                    {
                        case "## author":
                            Author = parts[1].Trim();
                            break;
                        case "## title":
                            Title = parts[1].Trim();
                            break;
                        case "## interface":
                            Interface = parts[1].Trim();
                            break;
                        case "## version":
                            Version = parts[1].Trim();
                            break;
                        case "## loadondemand":
                            LoadOnDemand = (parts[1].Trim() == "1");
                            break;
                        case "## notes":
                            Notes = parts[1].Trim();
                            break;
                    }
                }
            }
        }
    }
}
