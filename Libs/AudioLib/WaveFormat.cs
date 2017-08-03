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
        protected short waveFormatTag;
        protected short channels;
        protected int sampleRate;
        protected int averageBytesPerSecond;
        protected short blockAlign;
        protected short bitsPerSample;
        protected short extraSize;

        public int Channels => channels;
        public int SampleRate => sampleRate;
        public int AverageBytesPerSecond => averageBytesPerSecond;
        public int BlockAlign => blockAlign;
        public int BitsPerSample => bitsPerSample;
        public int ExtraSize => extraSize;

        public WaveFormat() 
            : this(44100, 16, 1)
        {
        }

        public WaveFormat(int sampleRate, int channels)
            : this(sampleRate, 16, channels)
        {
        }

        public WaveFormat(int SampleRate, int BitsPerSample, int Channels)
        {
            waveFormatTag = (short)WaveFormats.Pcm;
            channels = (short)Channels;
            sampleRate = SampleRate;
            bitsPerSample = (short)BitsPerSample;
            extraSize = 0;
            blockAlign = (short)(Channels * (bitsPerSample / 8));
            averageBytesPerSecond = sampleRate * blockAlign;
        }

        
    }
}
