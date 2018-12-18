using System.IO;

namespace WoWFormatParser.Structures.MDX
{
    public class BONE : GenObject
    {
        public int GeosetId;
        public int GeosetAnimationId;

        public BONE(BinaryReader br)
        {
            Load(br);

            GeosetId = br.ReadInt32();
            GeosetAnimationId = br.ReadInt32();
        }
    }
}
