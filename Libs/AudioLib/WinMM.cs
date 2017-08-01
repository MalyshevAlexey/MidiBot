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

        [DllImport(mmdll)]
        internal static extern int waveInGetDevCaps(int deviceID, ref WaveInCaps waveInCaps, int sizeOfWaveInCaps);

        [DllImport(mmdll)]
        public static extern int waveInOpen(out IntPtr phwi, int deviceID, WaveFormat lpFormat, WaveDelegate dwCallback, IntPtr dwInstance, int dwFlags);

        [DllImport(mmdll)]
        public static extern int waveInPrepareHeader(IntPtr hWaveIn, WaveHeader lpWaveInHdr, int uSize);

        [DllImport(mmdll)]
        public static extern int waveInUnprepareHeader(IntPtr hWaveIn, WaveHeader lpWaveInHdr, int uSize);

        [DllImport(mmdll)]
        internal static extern int waveInAddBuffer(IntPtr hwi, WaveHeader pwh, int cbwh);

        [DllImport(mmdll)]
        public static extern int waveInStart(IntPtr hwi);

        // OUT

        [DllImport(mmdll)]
        public static extern int waveOutGetNumDevs();

        [DllImport(mmdll)]
        public static extern int waveOutOpen(out IntPtr phwi, int deviceID, WaveFormat lpFormat, WaveDelegate dwCallback, IntPtr dwInstance, int dwFlags);

    }
}
