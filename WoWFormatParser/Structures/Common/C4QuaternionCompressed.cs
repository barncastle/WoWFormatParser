using System;
using System.Runtime.InteropServices;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.Common
{
    [StructLayout(LayoutKind.Sequential)]
    public struct C4QuaternionCompressed : IStringDescriptor
    {
        public long Value;

        public C4Vector GetC4Vector()
        {
            const double multiplier = 0.00000095367432;

            C4Vector vector = new C4Vector
            {
                X = (float)((Value >> 42) * (multiplier / 2.0)),
                Y = (float)(((Value << 22) >> 43) * multiplier),
                Z = (float)((Value & 0x1FFFFF) * multiplier)
            };

            // calculate W
            double len = vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z;
            if (Math.Abs(len - 1.0) >= multiplier)
            {
                double w = 1.0 - len;
                if (w >= 0)
                    vector.W = (float)(Math.Sqrt(w));
            }

            return vector;
        }


        public override string ToString() => GetC4Vector().ToString();
    }
}
