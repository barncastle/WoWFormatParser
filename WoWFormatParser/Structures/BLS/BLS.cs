using System.Collections.Generic;
using System.IO;
using System.Linq;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.BLS
{
    public sealed class BLS : Format
    {
        public string Magic;
        public string Version;
        public DirEntry[] Dir;
        public uint? Unk_0x8;
        public uint[] Offsets;
        public DirEntry? EndOfFile;
        public List<CGxShader> Blocks;

        public BLS(BinaryReader br)
        {
            Magic = br.ReadString(4).FastReverse();
            Version = br.ReadBytes(4).ToHex(true).TrimStart('0');
            Blocks = new List<CGxShader>();

            if (Version == "10003")
            {
                Unk_0x8 = br.ReadUInt32();
                Offsets = br.ReadArray(Magic == "GXPS" ? 12 : 6, () => br.ReadUInt32());

                Blocks.AddRange(ReadBlocks(Offsets, br, Version));
            }
            else
            {
                Dir = br.ReadStructArray<DirEntry>(Magic == "GXPS" ? 11 : 3);

                if (Version == "10002")
                    EndOfFile = br.ReadStruct<DirEntry>();

                var offsets = Dir.Where(x => x.Count > 0).Select(x => x.Start);
                Blocks.AddRange(ReadBlocks(offsets, br, Version));
            }

            // check remaining is just padding
            long len = br.BaseStream.Length - br.BaseStream.Position;
            bool padded = br.ReadBytes((int)len).All(x => x == 0);
            if (len > 0 && !padded)
                throw new UnreadContentException();
        }

        private IEnumerable<CGxShader> ReadBlocks(IEnumerable<uint> offsets, BinaryReader br, string version)
        {
            foreach (uint off in offsets)
            {
                if (off == 0)
                    continue;

                yield return new CGxShader(br, Version);
            }
        }
    }

    public struct DirEntry : IStringDescriptor
    {
        public uint Start; // offset to block
        public uint Count; // total size of block

        public override string ToString() => $"Start: {Start}, Count: {Count}";
    }

    public class CGxShader
    {
        public uint CCount;
        public CGxShaderParam[] Consts;
        public uint PCount;
        public CGxShaderParam[] Params;
        public uint? Unk_0xC;
        public uint Bytes;
        public byte[] Code;

        public CGxShader(BinaryReader br, string version)
        {
            CCount = br.ReadUInt32();
            if (CCount > 0)
                Consts = br.ReadArray((int)CCount, () => new CGxShaderParam(br, version));

            PCount = br.ReadUInt32();
            if (PCount > 0)
                Params = br.ReadArray((int)PCount, () => new CGxShaderParam(br, version));

            if (version != "10001")
                Unk_0xC = br.ReadUInt32();

            Bytes = br.ReadUInt32();
            Code = br.ReadBytes((int)Bytes);
        }
    }

    public class CGxShaderParam
    {
        public string Name; //[64]
        public ShaderType Type;
        public float[] F; //[16];
        public uint? Index;
        public uint? Unk_0x84;
        public uint? Unk_0x88;
        public uint? Unk_0x8C;

        public CGxShaderParam(BinaryReader br, string version)
        {
            int strLen = version == "10001" ? 32 : 64;

            Name = br.ReadString(strLen).TrimEnd('\xFFFD', '\0');
            Type = br.ReadEnum<ShaderType>();
            F = br.ReadStructArray<float>(16);

            if (version != "10001")
            {
                Unk_0x84 = br.ReadUInt32();
                Unk_0x88 = br.ReadUInt32();
                Unk_0x8C = br.ReadUInt32();
            }
            else
            {
                Index = br.ReadUInt32();
            }
        }
    }

    public enum ShaderType : uint
    {
        Type_Vector4 = 0x0,            // C4Vector
        Type_Matrix34 = 0x1,           // C34Matrix
        Type_Matrix44 = 0x2,           // C44Matrix
        Type_3 = 0x3,                  // used in terrain3*.bls, terrain4*.bls
        Type_4 = 0x4,                  // used in terrain3*.bls, terrain4*.bls
        Type_Force32Bit = 0xFFFFFFFF,
    }
}
