using System;
using System.Runtime.InteropServices;

namespace MidiBot.MidiLib
{
    internal static class WinMM
    {
        // IN
        [DllImport("winmm.dll")]
        internal static extern int midiInGetNumDevs();

        [DllImport("winmm.dll")]
        internal static extern int midiInGetDevCaps(int deviceId, ref MidiInCaps caps, int sizeOfMidiInCaps);

        [DllImport("winmm.dll")]
        internal static extern int midiInOpen(ref int handle, int deviceId, MidiProc proc, IntPtr instance, int flags);

        [DllImport("winmm.dll")]
        internal static extern int midiInStart(int handle);

        [DllImport("winmm.dll")]
        internal static extern int midiInPrepareHeader(int handle, IntPtr headerPtr, int sizeOfMidiHeader);

        [DllImport("winmm.dll")]
        internal static extern int midiInUnprepareHeader(int handle, IntPtr headerPtr, int sizeOfMidiHeader);

        [DllImport("winmm.dll")]
        internal static extern int midiInAddBuffer(int handle, IntPtr headerPtr, int sizeOfMidiHeader);

        [DllImport("winmm.dll")]
        internal static extern int midiInStop(int handle);

        [DllImport("winmm.dll")]
        internal static extern int midiInReset(int handle);

        [DllImport("winmm.dll")]
        internal static extern int midiInClose(int handle);

        // OUT
        [DllImport("winmm.dll")]
        internal static extern int midiOutGetNumDevs();

        [DllImport("winmm.dll")]
        internal static extern int midiOutGetDevCaps(int deviceId, ref MidiOutCaps caps, int sizeOfMidiOutCaps);

        [DllImport("winmm.dll")]
        internal static extern int midiOutOpen(ref int handle, int deviceId, MidiProc proc, int instance, int flags);

        [DllImport("winmm.dll")]
        internal static extern int midiOutShortMsg(int handle, int message);

        [DllImport("winmm.dll")]
        internal static extern int midiOutPrepareHeader(int handle, IntPtr headerPtr, int sizeOfMidiHeader);

        [DllImport("winmm.dll")]
        internal static extern int midiOutUnprepareHeader(int handle, IntPtr headerPtr, int sizeOfMidiHeader);

        [DllImport("winmm.dll")]
        internal static extern int midiOutLongMsg(int handle, IntPtr headerPtr, int sizeOfMidiHeader);

        [DllImport("winmm.dll")]
        internal static extern int midiOutClose(int handle);
    }
}
