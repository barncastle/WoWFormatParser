using System;
using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;

namespace WoWFormatParser.Structures.MDX
{
    public class PRE2 : GenObject
    {
        public uint Size;

        public int EmitterSize;
        public EmitterType EmitterType; //PARTICLE_EMITTER_TYPE
        public float Speed;
        public float Variation;
        public float Latitude;
        public float Longitude;
        public float Gravity;
        public float ZSource;
        public float Lifespan;
        public float EmissionRate;
        public float Length;
        public float Width;
        public int Rows;
        public int Cols;
        public CellType ParticleType; //PARTICLE_TYPE	
        public float TailLength;
        public float MiddleTime;
        public CBGR StartColor;
        public CBGR MiddleColor;
        public CBGR EndColor;
        public float StartAlpha;
        public float MiddleAlpha;
        public float EndAlpha;
        public float StartScale;
        public float MiddleScale;
        public float EndScale;
        public uint LifespanUVAnimStart;
        public uint LifespanUVAnimEnd;
        public uint LifespanUVAnimRepeat;
        public uint DecayUVAnimStart;
        public uint DecayUVAnimEnd;
        public uint DecayUVAnimRepeat;
        public uint TailUVAnimStart;
        public uint TailUVAnimEnd;
        public uint TailUVAnimRepeat;
        public uint TailDecayUVAnimStart;
        public uint TailDecayUVAnimEnd;
        public uint TailDecayUVAnimRepeat;
        public BlendMode BlendMode;
        public uint TextureId;
        public int PriorityPlane;
        public uint ReplaceableId;
        public string GeometryModel;
        public string RecursionModel;
        public float TwinkleFPS;
        public float TwinkleOnOff;
        public float TwinkleScaleMin;
        public float TwinkleScaleMax;
        public float IvelScale;
        public CRange TumbleX;
        public CRange TumbleY;
        public CRange TumbleZ;
        public float Drag;
        public float Spin;
        public C3Vector WindVector;
        public float WindTime;
        public float FollowSpeed1;
        public float FollowScale1;
        public float FollowSpeed2;
        public float FollowScale2;
        public C3Vector[] Splines;
        public bool Squirts;

        public MDXTrack<float> SpeedKeys;
        public MDXTrack<float> VariationKeys;
        public MDXTrack<float> LatitudeKeys;
        public MDXTrack<float> LongitudeKeys;
        public MDXTrack<float> ZSourceKeys;
        public MDXTrack<float> LifespanKeys;
        public MDXTrack<float> GravityKeys;
        public MDXTrack<float> EmissionRateKeys;
        public MDXTrack<float> WidthKeys;
        public MDXTrack<float> LengthKeys;
        public MDXTrack<float> VisibilityKeys;

        public PRE2(BinaryReader br)
        {
            Size = br.ReadUInt32();
            Load(br);

            EmitterSize = br.ReadInt32();
            EmitterType = (EmitterType)br.ReadUInt32();
            Speed = br.ReadSingle();
            Variation = br.ReadSingle();
            Latitude = br.ReadSingle();
            Longitude = br.ReadSingle();
            Gravity = br.ReadSingle();
            ZSource = br.ReadSingle();
            Lifespan = br.ReadSingle();
            EmissionRate = br.ReadSingle();
            Length = br.ReadSingle();
            Width = br.ReadSingle();
            Rows = br.ReadInt32();
            Cols = br.ReadInt32();
            ParticleType = (CellType)br.ReadUInt32();
            TailLength = br.ReadSingle();
            MiddleTime = br.ReadSingle();
            StartColor = br.ReadStruct<CBGR>();
            MiddleColor = br.ReadStruct<CBGR>();
            EndColor = br.ReadStruct<CBGR>();
            StartAlpha = br.ReadByte() / 255f;
            MiddleAlpha = br.ReadByte() / 255f;
            EndAlpha = br.ReadByte() / 255f;
            StartScale = br.ReadSingle();
            MiddleScale = br.ReadSingle();
            EndScale = br.ReadSingle();
            LifespanUVAnimStart = br.ReadUInt32();
            LifespanUVAnimEnd = br.ReadUInt32();
            LifespanUVAnimRepeat = br.ReadUInt32();
            DecayUVAnimStart = br.ReadUInt32();
            DecayUVAnimEnd = br.ReadUInt32();
            DecayUVAnimRepeat = br.ReadUInt32();
            TailUVAnimStart = br.ReadUInt32();
            TailUVAnimEnd = br.ReadUInt32();
            TailUVAnimRepeat = br.ReadUInt32();
            TailDecayUVAnimStart = br.ReadUInt32();
            TailDecayUVAnimEnd = br.ReadUInt32();
            TailDecayUVAnimRepeat = br.ReadUInt32();
            BlendMode = (BlendMode)br.ReadUInt32();
            TextureId = br.ReadUInt32();
            PriorityPlane = br.ReadInt32();
            ReplaceableId = br.ReadUInt32();
            GeometryModel = br.ReadString(260).TrimEnd('\0');
            RecursionModel = br.ReadString(260).TrimEnd('\0');
            TwinkleFPS = br.ReadSingle();
            TwinkleOnOff = br.ReadSingle();
            TwinkleScaleMin = br.ReadSingle();
            TwinkleScaleMax = br.ReadSingle();
            IvelScale = br.ReadSingle();
            TumbleX = br.ReadStruct<CRange>();
            TumbleY = br.ReadStruct<CRange>();
            TumbleZ = br.ReadStruct<CRange>();
            Drag = br.ReadSingle();
            Spin = br.ReadSingle();
            WindVector = br.ReadStruct<C3Vector>();
            WindTime = br.ReadSingle();
            FollowSpeed1 = br.ReadSingle();
            FollowScale1 = br.ReadSingle();
            FollowSpeed2 = br.ReadSingle();
            FollowScale2 = br.ReadSingle();
            Splines = br.ReadStructArray<C3Vector>(br.ReadInt32());
            Squirts = br.ReadUInt32() == 1;  // for footsteps and impact spell effects

            while (true)
            {
                string token = br.ReadString(4);
                br.BaseStream.Position -= 4;
                switch (token)
                {
                    case "KP2S": SpeedKeys = new MDXTrack<float>(br); break;
                    case "KP2R": VariationKeys = new MDXTrack<float>(br); break;
                    case "KP2G": GravityKeys = new MDXTrack<float>(br); break;
                    case "KP2W": WidthKeys = new MDXTrack<float>(br); break;
                    case "KP2N": LengthKeys = new MDXTrack<float>(br); break;
                    case "KVIS": VisibilityKeys = new MDXTrack<float>(br); break;
                    case "KP2E": EmissionRateKeys = new MDXTrack<float>(br); break;
                    case "KP2L": LatitudeKeys = new MDXTrack<float>(br); break;
                    case "KLIF": LifespanKeys = new MDXTrack<float>(br); break;
                    case "KPLN": LongitudeKeys = new MDXTrack<float>(br); break;
                    case "KP2Z": ZSourceKeys = new MDXTrack<float>(br); break;
                    default: return;
                }
            }
        }
    }

    public enum EmitterType : uint
    {
        Base = 0x0,
        Plane = 0x1,
        Sphere = 0x2,
        Spline = 0x3,
    }

    public enum BlendMode : uint
    {
        Blend = 0x0,
        Add = 0x1,
        Modulate = 0x2,
        Modulate2X = 0x3,
        AlphaKey = 0x4,
    }

    [Flags]
    public enum CellType : uint
    {
        Head = 0x0,
        Tail = 0x1,
    }

}
