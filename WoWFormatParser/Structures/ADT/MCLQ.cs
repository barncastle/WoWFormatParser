using System.IO;
using System.Runtime.InteropServices;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.ADT
{
    public class MCLQ
    {
        public CRange Height;
        public object[,] Verts;
        public byte[,] Tiles;
        public uint NFlowvs;
        public SWFlowv[] Flowvs;

        public MCLQ(BinaryReader br, MCNK_Flags flag)
        {
            Height = br.ReadStruct<CRange>();

            switch (flag)
            {
                case MCNK_Flags.IsOcean:
                    Verts = br.ReadJaggedArray(9, 9, () => (object)br.ReadStruct<SOVert>());
                    break;
                case MCNK_Flags.IsMagma:
                    Verts = br.ReadJaggedArray(9, 9, () => (object)br.ReadStruct<SMVert>());
                    break;
                default:
                    Verts = br.ReadJaggedArray(9, 9, () => (object)br.ReadStruct<SWVert>());
                    break;
            }

            Tiles = br.ReadJaggedArray(8, 8, () => br.ReadByte());
            NFlowvs = br.ReadUInt32();
            Flowvs = br.ReadStructArray<SWFlowv>(2);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SWVert : IStringDescriptor
    {
        public byte Depth;
        public byte Flow0Pct;
        public byte Flow1Pct;
        public byte Filler;
        public float Height;

        public override string ToString() => $"Depth: {Depth}, Flow0Pct: {Flow0Pct}, Flow1Pct: {Flow1Pct}, Height: {Height}";
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SOVert : IStringDescriptor
    {
        public byte Depth;
        public byte Foam;
        public byte Wet;
        public byte Filler;
        public float Height;

        public override string ToString() => $"Depth: {Depth}, Foam: {Foam}, Wet: {Wet}, Height: {Height}";
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SMVert : IStringDescriptor
    {
        public ushort S;
        public ushort T;
        public float Height;

        public override string ToString() => $"S: {S}, T: {T}, Height: {Height}";
    }

    public struct SWFlowv
    {
        public CSphere Sphere;
        public C3Vector Dir;
        public float Velocity;
        public float Amplitude;
        public float Frequency;
    }
}
