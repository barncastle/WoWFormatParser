using System.Runtime.InteropServices;

namespace WoWFormatParser.Structures.ADT
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MPHD
    {
        public uint NDoodadNames;
        public uint OffsDoodadNames;   // MDNM
        public uint NMapObjNames;
        public uint OffsMapObjNames;   // MONM

        public override string ToString() => $"NDoodadNames: {NDoodadNames}, OffsDoodadNames: {OffsDoodadNames}, NMapObjNames: {NMapObjNames}, OffsMapObjNames: {OffsDoodadNames}";
    }
}
