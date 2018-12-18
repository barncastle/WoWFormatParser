using System.IO;
using WoWFormatParser.Helpers;

namespace WoWFormatParser.Structures.MDX
{
    public class MDXSimpleTrack
    {
        public int Count;
        public int GlobalSequenceId;
        public uint[] Time;
        public uint[] Keys;

        public MDXSimpleTrack(BinaryReader br)
        {
            string name = br.ReadString(4);
            Count = br.ReadInt32();
            GlobalSequenceId = br.ReadInt32();

            if (Count <= 0)
                return;

            if (name == "KEVT")
            {
                Time = br.ReadStructArray<uint>(Count);
            }
            else
            {
                Time = new uint[Count];
                Keys = new uint[Count];
                for (int i = 0; i < Count; i++)
                {
                    Time[i] = br.ReadUInt32();
                    Keys[i] = br.ReadUInt32();
                }
            }
        }
    }
}
