using System.IO;
using System.Runtime.InteropServices;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.WMO
{
    public class MLIQ
    {
        public C2iVector VertexCount;
        public C2iVector TileCount;
        public C3Vector Corner;
        public ushort MaterialId;
        public SMOLVert[,] LiquidVertexList;
        public SMOLTile[,] LiquidTileList;

        public MLIQ(BinaryReader br)
        {
            VertexCount = br.ReadStruct<C2iVector>();
            TileCount = br.ReadStruct<C2iVector>();
            Corner = br.ReadStruct<C3Vector>();
            MaterialId = br.ReadUInt16();

            // HACK
            LiquidVertexList = br.ReadJaggedArray(VertexCount.x, VertexCount.y, () => br.ReadStruct<SMOLVert>());
            LiquidTileList = br.ReadJaggedArray(TileCount.x, TileCount.y, () => new SMOLTile(br));
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct SMOLVert
    {
        [FieldOffset(0)]
        public SMOWVert waterVert;
        [FieldOffset(0)]
        public SMOMVert magmaVert;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SMOWVert : IStringDescriptor
    {
        public byte Flow1;
        public byte Flow2;
        public byte Flow1Pct;
        public byte Filler;
        public float Height;

        public override string ToString() => $"Flow1: {Flow1}, Flow2: {Flow2}, Flow1Pct: {Flow1Pct}, Height: {Height}";
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SMOMVert : IStringDescriptor
    {
        public ushort S;
        public ushort T;
        public float Height;

        public override string ToString() => $"S: {S}, T: {T}, Height: {Height}";
    }

    public class SMOLTile : IStringDescriptor
    {
        public byte Liquid;
        public bool Fishable;
        public bool Shared;

        public SMOLTile(BinaryReader br)
        {
            byte data = br.ReadByte();

            Liquid = (byte)(data & ~0xC0);
            Fishable = (data & 0x40) == 0x40;
            Shared = (data & 0x80) == 0x80;
        }

        public override string ToString() => $"Liquid: {Liquid}, Fishable: {Fishable}, Shared: {Shared}";
    }
}
