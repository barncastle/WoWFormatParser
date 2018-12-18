using System;
using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;

namespace WoWFormatParser.Structures.MDX
{
    public sealed class MDX : Format
    {
        public string Magic;
        public uint Version;
        public string Name;
        public string AnimationFile;
        public CExtent Bounds;
        public uint BlendTime;
        public Model_Flags Flags;

        public SEQS[] Sequences;
        public MTLS[] Materials;
        public TEXS[] Textures;
        public IGEOS[] Geosets;
        public BONE[] Bones;
        public HELP[] Helpers;
        public ATCH[] Attachments;
        public C3Vector[] Pivots;
        public CAMS[] Cameras;
        public EVTS[] Events;
        public HTST[] HitTestShapes;
        public CLID Collision;
        public uint[] GlobalSequences;
        public GEOA[] GeosetAnimations;
        public PRE2[] ParticleEmitters;
        public RIBB[] Ribbons;
        public LITE[] Lights;
        public TXAN[] TextureAnimations;

        public MDX(BinaryReader br)
        {
            Magic = br.ReadString(4);

            while (br.BaseStream.Position < br.BaseStream.Length - 8) // some files are aligned?
            {
                var (Token, Size) = br.ReadIffChunk();
                if (Size <= 0)
                    continue;

                int count = 0;
                switch (Token)
                {
                    case "VERS":
                        Version = br.ReadUInt32();
                        break;
                    case "MODL":
                        Name = br.ReadString(80).TrimEnd('\0');
                        AnimationFile = br.ReadString(260).TrimEnd('\0');
                        Bounds = br.ReadStruct<CExtent>();
                        BlendTime = br.ReadUInt32();
                        Flags = br.ReadEnum<Model_Flags>();
                        break;
                    case "SEQS":
                        count = br.ReadInt32();
                        Sequences = br.ReadArray(count, () => new SEQS(br));
                        break;
                    case "MTLS":
                        count = br.ReadInt32();
                        br.ReadInt32(); // unused
                        Materials = br.ReadArray(count, () => new MTLS(br));
                        break;
                    case "TEXS":
                        Textures = br.ReadArray(Size / 268, () => new TEXS(br));
                        break;
                    case "GEOS":
                        Geosets = ReadGeoSets(br);
                        break;
                    case "BONE":
                        count = br.ReadInt32();
                        Bones = br.ReadArray(count, () => new BONE(br));
                        break;
                    case "HELP":
                        count = br.ReadInt32();
                        Helpers = br.ReadArray(count, () => new HELP(br));
                        break;
                    case "ATCH":
                        count = br.ReadInt32();
                        br.ReadInt32(); // unused
                        Attachments = br.ReadArray(count, () => new ATCH(br));
                        break;
                    case "PIVT":
                        Pivots = br.ReadStructArray<C3Vector>(Size / 12);
                        break;
                    case "CAMS":
                        count = br.ReadInt32();
                        Cameras = br.ReadArray(count, () => new CAMS(br));
                        break;
                    case "EVTS":
                        count = br.ReadInt32();
                        Events = br.ReadArray(count, () => new EVTS(br));
                        break;
                    case "HTST":
                        count = br.ReadInt32();
                        HitTestShapes = br.ReadArray(count, () => new HTST(br));
                        break;
                    case "CLID":
                        Collision = new CLID(br);
                        break;
                    case "GLBS":
                        GlobalSequences = br.ReadStructArray<uint>(Size / 4);
                        break;
                    case "GEOA":
                        count = br.ReadInt32();
                        GeosetAnimations = br.ReadArray(count, () => new GEOA(br));
                        break;
                    case "PRE2":
                        count = br.ReadInt32();
                        ParticleEmitters = br.ReadArray(count, () => new PRE2(br));
                        break;
                    case "RIBB":
                        count = br.ReadInt32();
                        Ribbons = br.ReadArray(count, () => new RIBB(br));
                        break;
                    case "LITE":
                        count = br.ReadInt32();
                        Lights = br.ReadArray(count, () => new LITE(br));
                        break;
                    case "TXAN":
                        count = br.ReadInt32();
                        TextureAnimations = br.ReadArray(count, () => new TXAN(br));
                        break;
                    default:
                        throw new NotImplementedException("Unknown token " + Token);
                }
            }
        }

        private IGEOS[] ReadGeoSets(BinaryReader br)
        {
            if (Version == 1500)
                return br.ReadArray(1, () => new GEOS_1500(br));

            return br.ReadArray(br.ReadInt32(), () => new GEOS(br));
        }
    }

    [Flags]
    public enum Model_Flags : byte
    {
        TrackYawOnly = 0x0,
        TrackPitchYaw = 0x1,
        TrackPitchYawRoll = 0x2,
        AlwaysAnimate = 0x4
    }
}
