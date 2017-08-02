using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MidiBot.AudioLib
{
    public enum WaveFormats
    {
        Pcm = 1,
        Float = 3
    }
    [StructLayout(LayoutKind.Sequential)]
    public class WaveFormat
    {
        public short wFormatTag;
        public short nChannels;
        public int nSamplesPerSec;
        public int AverageBytesPerSecond;
        public short nBlockAlign;
        public short wBitsPerSample;
        public short cbSize;

        public WaveFormat() 
            : this(44100, 16, 2)
        {
        }

        public WaveFormat(int sampleRate, int channels)
            : this(sampleRate, 16, channels)
        {
        }

        public WaveFormat(int rate, int bits, int channels)
        {
            wFormatTag = (short)WaveFormats.Pcm;
            nChannels = (short)channels;
            nSamplesPerSec = rate;
            wBitsPerSample = (short)bits;
            cbSize = 0;

            nBlockAlign = (short)(channels * (bits / 8));
            AverageBytesPerSecond = nSamplesPerSec * nBlockAlign;
        }
    }
}
