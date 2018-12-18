using System.IO;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.Common
{
    public class FixedPoint : IStringDescriptor
    {
        public readonly float Value;

        public FixedPoint(short value, int intbits, int decbits)
        {
            int decpart = value & ((1 << decbits) - 1);
            int intpart = (value >> decbits) & ((1 << intbits) - 1);
            bool signed = ((value >> (intbits + decbits)) & 1) == 1;
            float factor = intbits > 0 ? 1 << decbits : (1 << (decbits + 1)) - 1;

            Value = (signed ? -1f : 1f) * (intpart + decpart / factor);
        }

        public override string ToString() => Value.ToString();
    }


    public class FixedPoint_0_15 : FixedPoint
    {
        public FixedPoint_0_15(BinaryReader br) : base(br.ReadInt16(), 0, 15) { }
    }

    public class FixedPoint_6_9 : FixedPoint
    {
        public FixedPoint_6_9(BinaryReader br) : base(br.ReadInt16(), 6, 9) { }
    }

    public class FixedPoint_2_5 : FixedPoint
    {
        public FixedPoint_2_5(BinaryReader br) : base(br.ReadByte(), 2, 5) { }
    }
}
