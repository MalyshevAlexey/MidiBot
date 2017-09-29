using System.Runtime.InteropServices;

namespace MidiBot.AudioLib
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct WaveOutCaps
    {
        public short mid;
        public short pid;
        public int driverVersion;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
        public string name;
        public uint formats;
        public short channels;
        public short reserved;
    }
}
