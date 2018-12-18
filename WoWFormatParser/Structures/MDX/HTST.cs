using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;

namespace WoWFormatParser.Structures.MDX
{
    public class HTST : GenObject
    {
        public uint Size;
        public GeomShape Type;
        public CAaBox? Box;
        public CCylinder? Cylinder;
        public CSphere? Sphere;
        public CPlane? Plane;

        public HTST(BinaryReader br)
        {
            Size = br.ReadUInt32();
            Load(br);

            Type = (GeomShape)br.ReadByte();
            switch (Type)
            {
                case GeomShape.Box: Box = br.ReadStruct<CAaBox>(); break;
                case GeomShape.Cylinder: Cylinder = br.ReadStruct<CCylinder>(); break;
                case GeomShape.Plane: Plane = br.ReadStruct<CPlane>(); break;
                case GeomShape.Sphere: Sphere = br.ReadStruct<CSphere>(); break;
            }
        }
    }

    public enum GeomShape : byte
    {
        Box = 0x0,
        Cylinder = 0x1,
        Sphere = 0x2,
        Plane = 0x3,
    }
}
