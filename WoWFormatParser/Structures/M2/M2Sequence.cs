using System;
using System.IO;
using Newtonsoft.Json;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.M2
{
    public class M2Sequence : IVersioned
    {
        public ushort Id;
        public ushort VariationIndex;
        public CiRange Time;
        public float MoveSpeed;
        public M2Sequence_Flags Flags;
        public short Frequency;
        [JsonIgnore]
        public short Pad;
        public CiRange Replay;
        public uint BlendTime;
        public CExtent Bounds;
        public short NextVariation;
        public short NextAlias;

        public M2Sequence(BinaryReader br, uint build)
        {
            Id = br.ReadUInt16();
            VariationIndex = br.ReadUInt16();
            Time = br.ReadStruct<CiRange>();
            MoveSpeed = br.ReadSingle();
            Flags = br.ReadEnum<M2Sequence_Flags>();
            Frequency = br.ReadInt16();
            Pad = br.ReadInt16();
            Replay = br.ReadStruct<CiRange>();
            BlendTime = br.ReadUInt32();
            Bounds = br.ReadStruct<CExtent>();
            NextVariation = br.ReadInt16();
            NextVariation = br.ReadInt16();
        }
    }

    [Flags]
    public enum M2Sequence_Flags : uint
    {
        None = 0,
        SetBlendAnimation = 0x01,
        Unknown_0x2 = 0x02,
        Unknown_0x4 = 0x04,
        Unknown_0x8 = 0x08,
        LoadedAsLowPrioritySequence = 0x10,
        Looping = 0x20,
        IsAliasedAndHasFollowupAnimation = 0x40,
        IsBlended = 0x80,
        LocallyStoredSequence = 0x100,
    }
}
