using System.Runtime.InteropServices;
using WoWFormatParser.Structures.Common;

namespace WoWFormatParser.Structures.ADT
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MCSE
    {
        public uint SoundPointID;
        public uint SoundNameID;
        public C3Vector Pos;
        public float MinDistance;
        public float MaxDistance;
        public float CutOffDistance;
        public ushort StartTime;
        public ushort EndTime;
        public ushort Mode;
        public ushort GroupSilenceMin;
        public ushort GroupSilenceMax;
        public ushort PlayInstancesMin;
        public ushort PlayInstancesMax;
        public byte LoopCountMin;
        public byte LoopCountMax;
        public ushort InterSoundGapMin;
        public ushort InterSoundGapMax;
    }
}
