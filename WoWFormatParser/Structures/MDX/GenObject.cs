using System;
using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;

namespace WoWFormatParser.Structures.MDX
{
    public class GenObject
    {
        public int ObjSize;
        public string Name;
        public int ObjectId;
        public int ParentId;
        public Flags Flags;

        public MDXTrack<C3Vector> TranslationKeys;
        public MDXTrack<C4QuaternionCompressed> RotationKeys;
        public MDXTrack<C3Vector> ScaleKeys;

        protected void Load(BinaryReader br)
        {
            ObjSize = br.ReadInt32();
            Name = br.ReadString(80).TrimEnd('\0');
            ObjectId = br.ReadInt32();
            ParentId = br.ReadInt32();
            Flags = (GenObj_Flags)br.ReadUInt32();

            while (true)
            {
                string token = br.ReadString(4);
                br.BaseStream.Position -= 4;
                switch (token)
                {
                    case "KGTR": TranslationKeys = new MDXTrack<C3Vector>(br); break;
                    case "KGRT": RotationKeys = new MDXTrack<C4QuaternionCompressed>(br); break;
                    case "KGSC": ScaleKeys = new MDXTrack<C3Vector>(br); break;
                    default: return;
                }
            }
        }
    }

    [Flags]
    public enum GenObj_Flags : uint
    {
        DontInheritTranslation = 0x00000001,
        DontInheritScaling = 0x00000002,
        DontInheritRotation = 0x00000004,
        Billboard = 0x00000008,
        BillboardLockX = 0x00000010,
        BillboardLockY = 0x00000020,
        BillboardLockZ = 0x00000040,
        GenobjectMDLBonesection = 0x00000080,
        GenobjectMDLLightsection = 0x00000100,
        GenobjectMDLEventsection = 0x00000200,
        GenobjectMDLAttachmentsection = 0x00000400,
        GenobjectMDLParticleemitter2 = 0x00000800,
        GenobjectMDLHittestshape = 0x00001000,
        GenobjectMDLRibbonemitter = 0x00002000,
        Project = 0x00004000,
        EmitterUsesTga = 0x00008000,
        Unshaded = 0x00008000,
        EmitterUsesMDL = 0x00010000,
        SortPrimitivesFarZ = 0x00010000,
        LineEmitter = 0x00020000,
        ParticleUnfogged = 0x00040000,
        ParticleUseModelSpace = 0x00080000,
        ParticleInheritScale = 0x00100000,
        ParticleInstantVelocityLin = 0x00200000,
        Particle0xKill = 0x00400000,
        ParticleZVelocityOnly = 0x00800000,
        ParticleTumbler = 0x01000000,
        ParticleTailGrows = 0x02000000,
        ParticleExtrude = 0x04000000,
        ParticleXYQuads = 0x08000000,
        ParticleProject = 0x10000000,
        ParticleFollow = 0x20000000,
    }
}
