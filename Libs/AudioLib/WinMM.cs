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
        internal delegate void WaveCallback(IntPtr hdrvr, int msg, IntPtr instance, WaveHeader wavhdr, int dwParam2);

        private const string mmdll = "winmm.dll";

        // IN

        public const int WaveInData = 0x3C0;

        public const int CallbackFunction = 0x00030000;

        [DllImport(mmdll)]
        public static extern int waveInGetNumDevs();

        [DllImport(mmdll)]
        internal static extern int waveInGetDevCaps(int deviceID, ref WaveInCaps waveInCaps, int sizeOfWaveInCaps);

        [DllImport(mmdll)]
        public static extern int waveInOpen(out IntPtr phwi, int deviceID, WaveFormat lpFormat, WaveCallback dwCallback, IntPtr dwInstance, int dwFlags);

        [DllImport(mmdll)]
        public static extern int waveInPrepareHeader(IntPtr hWaveIn, WaveHeader lpWaveInHdr, int uSize);

        [DllImport(mmdll)]
        public static extern int waveInUnprepareHeader(IntPtr hWaveIn, WaveHeader lpWaveInHdr, int uSize);

        [DllImport(mmdll)]
        internal static extern int waveInAddBuffer(IntPtr hwi, WaveHeader pwh, int cbwh);

        [DllImport(mmdll)]
        public static extern int waveInStart(IntPtr hwi);

        // OUT

        public const int WaveOutDone = 0x3BD;

        [DllImport(mmdll)]
        public static extern int waveOutGetNumDevs();

        [DllImport(mmdll)]
        public static extern int waveOutGetDevCaps(IntPtr deviceID, ref WaveOutCaps waveOutCaps, int waveOutCapsSize);

        [DllImport(mmdll)]
        public static extern int waveOutOpen(out IntPtr phwi, int deviceID, WaveFormat lpFormat, WaveCallback dwCallback, IntPtr dwInstance, int dwFlags);

        [DllImport(mmdll)]
        public static extern int waveOutPrepareHeader(IntPtr hWaveOut, WaveHeader lpWaveOutHdr, int uSize);

        [DllImport(mmdll)]
        public static extern int waveOutWrite(IntPtr hWaveOut, WaveHeader lpWaveOutHdr, int uSize);
    }
}
