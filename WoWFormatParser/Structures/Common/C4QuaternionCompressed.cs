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
            const float multiplier = 0.00000095367432f;

            C4Vector vector = new C4Vector
            {
                X = (Value >> 42) * (multiplier / 2f),
                Y = ((Value << 22) >> 43) * multiplier,
                Z = ((int)(Value << 11) >> 11) * multiplier
            };

            // calculate W
            var len = 1.0f - (vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z);
            if (len >= multiplier)
                vector.W = (float)Math.Sqrt(len);

            return vector;
        }


        public override string ToString() => GetC4Vector().ToString();
    }
}
