using System;
using System.IO;
using System.Runtime.InteropServices;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.LIT
{
    public sealed class LIT : Format
    {
        public string Version;
        public int LightCount;
        public LIT_ListData[] LightList;
        public LIT_LightData[] LightData;

        public LIT(BinaryReader br)
        {
            Version = br.ReadBytes(4).ToHex(true);
            LightCount = br.ReadInt32();

            if (LightCount != 0)
            {
                if (LightCount > 0)
                    LightList = br.ReadArray(LightCount, () => new LIT_ListData(br));

                // -1 is a special case containing only the first LightDataItem
                bool partial = LightCount == -1;

                LightData = new LIT_LightData[Math.Abs(LightCount)];
                for (int i = 0; i < LightData.Length; i++)
                    LightData[i] = new LIT_LightData(br, Version, partial);
            }

            if (br.BaseStream.Position != br.BaseStream.Length)
                throw new UnreadContentException();
        }
    }

    public class LIT_ListData
    {
        public C2iVector Chunk;
        public int ChunkRadius;
        public C3Vector LightLocation;
        public float LightRadius;
        public float LightDropoff;
        public string LightName; // 32, 8.3 is not 0 padded

        public LIT_ListData(BinaryReader br)
        {
            Chunk = br.ReadStruct<C2iVector>();
            ChunkRadius = br.ReadInt32();
            LightLocation = br.ReadStruct<C3Vector>();
            LightRadius = br.ReadSingle();
            LightDropoff = br.ReadSingle();
            LightName = br.ReadString(32).TrimEnd('\xFFFD', '\0');
        }
    }

    public class LIT_LightData
    {
        public LIT_LightDataItem LightData;
        public LIT_LightDataItem StormData;
        public LIT_LightDataItem LightDataWater;
        public LIT_LightDataItem StormDataWater;

        public LIT_LightData(BinaryReader br, string version, bool partial = false)
        {
            LightData = new LIT_LightDataItem(br, version);

            if (!partial)
            {
                StormData = new LIT_LightDataItem(br, version);
                LightDataWater = new LIT_LightDataItem(br, version);
                StormDataWater = new LIT_LightDataItem(br, version);
            }
        }
    }

    public class LIT_LightDataItem
    {
        public int[] HighlightCount; //[18];
        public LightMarker[,] HighlightMarker; //[18][32];
        public float[] FogEnd; //[32];
        public float[] FogStartScaler; //[32];
        public int HighlightSky;
        public float[,] SkyData; //[4][32];
        public uint CloudMask;
        public float[,] ParamData; //[4][10];

        public LIT_LightDataItem(BinaryReader br, string version)
        {
            int numHighlights = version == "80000003" ? 14 : 18;

            HighlightCount = br.ReadStructArray<int>(numHighlights);
            HighlightMarker = br.ReadJaggedArray(numHighlights, 32, () => br.ReadStruct<LightMarker>());
            FogEnd = br.ReadStructArray<float>(32);
            FogStartScaler = br.ReadStructArray<float>(32);
            HighlightSky = br.ReadInt32();
            SkyData = br.ReadJaggedArray(4, 32, () => br.ReadSingle());
            CloudMask = br.ReadUInt32();

            if (version == "80000005")
                ParamData = br.ReadJaggedArray(4, 10, () => br.ReadSingle());
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LightMarker : IStringDescriptor
    {
        public int Time;
        public CImVector Color;

        public override string ToString() => $"Time: {Time}, Color: {Color}";
    }
}
