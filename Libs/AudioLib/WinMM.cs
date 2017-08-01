using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MidiBot.AudioLib
{
    internal class WinMM
    {
        private const string mmdll = "winmm.dll";

        // IN

        [DllImport(mmdll)]
        public static extern int waveInGetNumDevs();
        [DllImport("winmm.dll")]
        internal static extern int waveInGetDevCaps(int deviceID, ref WaveInCaps waveInCaps, int sizeOfWaveInCaps);

        // OUT

        [DllImport(mmdll)]
        public static extern int waveOutGetNumDevs();
        [DllImport(mmdll)]
        public static extern int waveInOpen(out IntPtr phwi, int deviceID, WaveFormat lpFormat, WaveDelegate dwCallback, IntPtr dwInstance, int dwFlags);
    }
}
