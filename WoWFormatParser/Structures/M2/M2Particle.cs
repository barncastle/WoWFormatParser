using System;
using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.M2
{
    public class M2Particle : IVersioned
    {
        public int ParticleId;
        public uint Flags;
        public C3Vector Position;
        public ushort BoneIndex;
        public ushort TextureIndex;
        public string GeometryMdl;
        public string RecursionMdl;
        public ushort BlendMode;
        public PARTICLE_EMITTER_TYPE EmitterType;
        public ParticleType ParticleType;
        public CellType CellType;
        public short PriorityPlane;
        public ushort Rows;
        public ushort Cols;
        public M2Track<float> SpeedTrack;
        public M2Track<float> VariationTrack;
        public M2Track<float> LongitudeTrack;
        public M2Track<float> LatitudeTrack;
        public M2Track<float> GravityTrack;
        public M2Track<float> LifeTrack;
        public M2Track<float> EmissionRateTrack;
        public M2Track<float> LengthTrack;
        public M2Track<float> WidthTrack;
        public M2Track<float> ZSourceTrack;
        public float MiddleTime;
        public CImVector StartColor;
        public CImVector MiddleColor;
        public CImVector EndColor;
        public float StartScale;
        public float MiddleScale;
        public float EndScale;
        public ushort LifespanUVAnimStart;
        public ushort LifespanUVAnimEnd;
        public ushort LifespanUVAnimRepeat;
        public ushort DecayUVAnimStart;
        public ushort DecayUVAnimEnd;
        public ushort DecayUVAnimRepeat;
        public ushort TailUVAnimStart;
        public ushort TailUVAnimEnd;
        public ushort TailDecayUVAnimStart;
        public ushort TailDecayUVAnimEnd;
        public float TailLength;
        public float TwinkleFPS;
        public float TwinklePercent;
        public float TwinkleScaleMin;
        public float TwinkleScaleMax;
        public float IvelScale;
        public float Drag;
        public float Spin;
        public float TumblexMin;
        public float TumblexMax;
        public float TumbleyMin;
        public float TumbleyMax;
        public float TumblezMin;
        public float TumblezMax;
        public C3Vector WindVector;
        public float WindTime;
        public float FollowSpeed1;
        public float FollowScale1;
        public float FollowSpeed2;
        public float FollowScale2;
        public M2Array<C3Vector> Splines;
        public M2Track<bool> EnabledIn;

        public M2Particle(BinaryReader br, uint build)
        {
            ParticleId = br.ReadInt32();
            Flags = br.ReadUInt32();
            Position = br.ReadStruct<C3Vector>();
            BoneIndex = br.ReadUInt16();
            TextureIndex = br.ReadUInt16();
            GeometryMdl = br.ReadM2Array<byte>(build)?.AsString();
            RecursionMdl = br.ReadM2Array<byte>(build)?.AsString();
            BlendMode = br.ReadUInt16();
            EmitterType = br.ReadEnum<PARTICLE_EMITTER_TYPE>();
            ParticleType = br.ReadEnum<ParticleType>();
            CellType = br.ReadEnum<CellType>();
            PriorityPlane = br.ReadInt16();
            Rows = br.ReadUInt16();
            Cols = br.ReadUInt16();
            SpeedTrack = new M2Track<float>(br, build);
            VariationTrack = new M2Track<float>(br, build);
            LatitudeTrack = new M2Track<float>(br, build);
            LongitudeTrack = new M2Track<float>(br, build);
            GravityTrack = new M2Track<float>(br, build);
            LifeTrack = new M2Track<float>(br, build);
            EmissionRateTrack = new M2Track<float>(br, build);
            WidthTrack = new M2Track<float>(br, build);
            LengthTrack = new M2Track<float>(br, build);
            ZSourceTrack = new M2Track<float>(br, build);
            MiddleTime = br.ReadSingle();
            StartColor = br.ReadStruct<CImVector>();
            MiddleColor = br.ReadStruct<CImVector>();
            EndColor = br.ReadStruct<CImVector>();
            StartScale = br.ReadSingle();
            MiddleScale = br.ReadSingle();
            EndScale = br.ReadSingle();
            LifespanUVAnimStart = br.ReadUInt16();
            LifespanUVAnimEnd = br.ReadUInt16();
            LifespanUVAnimRepeat = br.ReadUInt16();
            DecayUVAnimStart = br.ReadUInt16();
            DecayUVAnimEnd = br.ReadUInt16();
            DecayUVAnimRepeat = br.ReadUInt16();
            TailUVAnimStart = br.ReadUInt16();
            TailUVAnimEnd = br.ReadUInt16();
            TailDecayUVAnimStart = br.ReadUInt16();
            TailDecayUVAnimEnd = br.ReadUInt16(); // ?
            TailLength = br.ReadSingle();
            TwinkleFPS = br.ReadSingle();
            TwinklePercent = br.ReadSingle();
            TwinkleScaleMin = br.ReadSingle();
            TwinkleScaleMax = br.ReadSingle();
            IvelScale = br.ReadSingle();
            Drag = br.ReadSingle();
            Spin = br.ReadSingle();
            TumblexMin = br.ReadSingle();
            TumblexMax = br.ReadSingle();
            TumbleyMin = br.ReadSingle();
            TumbleyMax = br.ReadSingle();
            TumblezMin = br.ReadSingle();
            TumblezMax = br.ReadSingle();
            WindVector = br.ReadStruct<C3Vector>();
            WindTime = br.ReadSingle();
            FollowSpeed1 = br.ReadSingle();
            FollowScale1 = br.ReadSingle();
            FollowSpeed2 = br.ReadSingle();
            FollowScale2 = br.ReadSingle();
            Splines = br.ReadM2Array<C3Vector>(build);
            EnabledIn = new M2Track<bool>(br, build);
        }
    }

    public enum PARTICLE_EMITTER_TYPE : ushort
    {
        PET_BASE = 0x0,
        PET_PLANE = 0x1,
        PET_SPHERE = 0x2,
        PET_SPLINE = 0x3,
        PET_BONE = 0x4,
    }

    public enum ParticleType : byte
    {
        Normal = 0,
        UseLargeQuad = 1,
        Unknown_0x2 = 2
    }

    [Flags]
    public enum CellType : byte
    {
        Head = 0,
        Tail = 1
    }

    [Flags]
    public enum ParticleFlags
    {
        Unshaded = 0x1,
        SortPrimitivesFarZ = 0x2,
        LineEmitter = 0x4,
        Unfogged = 0x8,
        UseModelSpace = 0x10,
        InheritScale = 0x20,
        InstantVelocityLin = 0x40,
        ZeroXKill = 0x80,
        ZVelocityOnly = 0x100,
        Tumbler = 0x200,
        TailGrows = 0x400,
        Extrude = 0x800,
        XYQuads = 0x1000,
        Project = 0x2000,
        Follow = 0x4000,
    }
}
