using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;

namespace WoWFormatParser.Structures.MDX
{
    public class HTST : GenObject
    {
        public uint Size;
        public GEOM_SHAPE Type;
        public CAaBox? Box;
        public CCylinder? Cylinder;
        public CSphere? Sphere;
        public CPlane? Plane;

        public HTST(BinaryReader br)
        {
            Size = br.ReadUInt32();
            Load(br);

            Type = (GEOM_SHAPE)br.ReadByte();
            switch (Type)
            {
                case GEOM_SHAPE.Box: Box = br.ReadStruct<CAaBox>(); break;
                case GEOM_SHAPE.Cylinder: Cylinder = br.ReadStruct<CCylinder>(); break;
                case GEOM_SHAPE.Plane: Plane = br.ReadStruct<CPlane>(); break;
                case GEOM_SHAPE.Sphere: Sphere = br.ReadStruct<CSphere>(); break;
            }
        }
    }

    public enum GEOM_SHAPE : byte
    {
        Box = 0x0,
        Cylinder = 0x1,
        Sphere = 0x2,
        Plane = 0x3,
    }
}
