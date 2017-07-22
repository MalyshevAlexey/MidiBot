using System;
using System.Runtime.InteropServices;

namespace MidiBot.MidiLib
{
    internal static class WinMM
    {
        [DllImport("winmm.dll")]
        internal static extern int midiInGetNumDevs();

        [DllImport("winmm.dll")]
        internal static extern int midiInGetDevCaps(int deviceId, ref MidiInCaps caps, int sizeOfMidiInCaps);

        [DllImport("winmm.dll")]
        internal static extern int midiInOpen(ref int handle, int deviceId, MidiProc proc, IntPtr instance, int flags);

        [DllImport("winmm.dll")]
        internal static extern int midiInStart(int handle);



        [DllImport("winmm.dll")]
        internal static extern int midiOutGetNumDevs();

        [DllImport("winmm.dll")]
        internal static extern int midiOutGetDevCaps(int deviceId, ref MidiOutCaps caps, int sizeOfMidiOutCaps);

        [DllImport("winmm.dll")]
        internal static extern int midiOutOpen(ref int handle, int deviceId, MidiProc proc, int instance, int flags);

        [DllImport("winmm.dll")]
        internal static extern int midiOutShortMsg(int handle, int message);
    }
}
