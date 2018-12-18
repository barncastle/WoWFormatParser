using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace WoWFormatParser.Tests
{
    [TestClass]
    public class Tests
    {
        const string Directory = @"C:\Users\TomSpearman\Downloads\WoW.Alpha.ANi\WoW.Alpha.ANi\WoW";
        readonly WoWBuild Build = WoWBuild.Parse("0.5.3.3368");


        [TestMethod]
        public void TestFileLists()
        {
            using (WoWFormatParser parser = new WoWFormatParser(Directory, Build))
            {
                var localexes = parser.GetLocalFiles("*.exe");
                var mpqDBCs = parser.GetListFile("*.dbc");
                var mpqNames = parser.GetArchives("*model*");

                Assert.IsTrue(localexes.Any());
                Assert.IsTrue(mpqDBCs.Any());
                Assert.IsTrue(mpqNames.Any());
            }
        }

        [TestMethod]
        public void TestDefaultReading()
        {
            var options = new Options() { IncludeUnsupportedAndInvalidFiles = true };

            using (WoWFormatParser parser = new WoWFormatParser(Directory, Build, options))
            {
                var wowexe = parser.EnumerateDirectory("*wow.exe").FirstOrDefault();
                var factiondbc = parser.EnumerateArchives("*faction.db*").FirstOrDefault();

                Assert.IsNotNull(wowexe);
                Assert.IsNotNull(factiondbc);

                Assert.IsInstanceOfType(wowexe, typeof(Structures.SimpleFormat));
                Assert.IsInstanceOfType(factiondbc, typeof(Structures.Meta.DBCMeta));
            }
        }

        [TestMethod]
        public void TestJson()
        {
            var options = new Options()
            {
                IncludeUnsupportedAndInvalidFiles = true,
                ExportType = ExportType.Both
            };
            options.SerializerOptions.ExportType = ExportType.Both;
            options.SerializerOptions.IgnoreNullOrEmpty = true;
            options.SerializerOptions.RenameIgnoreResolver.RenameProperty(typeof(Structures.Meta.DBCMeta), "Name", "NameOverrideTest");

            using (WoWFormatParser parser = new WoWFormatParser(Directory, Build, options))
            {
                var factiondbc = parser.EnumerateArchives("*faction.db*").FirstOrDefault();

                Assert.IsNotNull(factiondbc);
                Assert.IsInstanceOfType(factiondbc, typeof(Structures.Meta.DBCMeta));

                // test serialization
                string json = factiondbc.ToJson(options.SerializerOptions);
                Assert.IsFalse(string.IsNullOrEmpty(json));
                Assert.IsTrue(json.Contains("StringTableSize"));
                Assert.IsTrue(json.Contains("Checksum"));

                // test property rename
                Assert.IsTrue(json.Contains("NameOverrideTest"));

                // test ignore null or empty
                var shouldbeempty = new TestEmptyClass().ToJson(options.SerializerOptions);
                Assert.IsTrue(shouldbeempty == "{}");
            }
        }


        [TestMethod]
        [Ignore]
        public void DebugStuff()
        {
            string directory = @"E:\World of Warcraft 0.6.0";

            var options = new Options()
            {
                ExportType = ExportType.Both,
                IncludeUnsupportedAndInvalidFiles = true,
            };
            //options.IgnoredFormats.Add(SupportedFormats.ADT);
            //options.IgnoredDirectories.Add("World\\Maps");

            WoWFormatParser parser = new WoWFormatParser(directory, WoWBuild.Parse("0.6.0.3592"), options);
            //var localfiles = parser.EnumerateDirectory("*.txt");
            //var mpqs = parser.GetArchives();
            //var archivednames = parser.GetListFile("*.lit");
            //var localnames = parser.GetLocalFiles();

            //var t = parser.EnumerateArchives("*.m2").ToArray()
            //                   .Where(x => x.Is<Structures.M2.M2>())
            //                   .Cast<Structures.M2.M2>()
            //                   .Where(x => x.Particles != null)
            //                   .Where(x => x.Particles.Any(y => y.TailUVAnimRepeat != 0)).ToArray();

            var t = parser.EnumerateArchives<Structures.WMO.WMO>("*GoldshireInn.wmo").ToArray();

            var t2 = parser.EnumerateArchives<Structures.WMO.WMO>("*.wmo")
                        .Where(x => x.Lights != null)
                        .Where(x => x.Lights.Any(y => y.Unknown_0x18 != null && (y.Unknown_0x18[0] != 0 || y.Unknown_0x18[1] != 0))).ToArray();


            //var json = test.ToJson();
        }

        private class TestEmptyClass : Structures.SimpleFormat
        {
            public List<string> Test1 = new List<string>();
            public string[] Test2 = new string[0];
            public string Test3 = "";
        }
    }
}
