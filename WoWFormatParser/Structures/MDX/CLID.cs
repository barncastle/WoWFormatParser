using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;

namespace WoWFormatParser.Structures.MDX
{
    public class CLID
    {
        public C3Vector[] Vertices;
        public ushort[] TriIndices;
        public C3Vector[] FacetNormals;

        public CLID(BinaryReader br)
        {
            br.AssertTag("VRTX");
            Vertices = br.ReadStructArray<C3Vector>(br.ReadInt32());

            br.AssertTag("TRI ");
            TriIndices = br.ReadStructArray<ushort>(br.ReadInt32());

            br.AssertTag("NRMS");
            FacetNormals = br.ReadStructArray<C3Vector>(br.ReadInt32());
        }
    }
}
