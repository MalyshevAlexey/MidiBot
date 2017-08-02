using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiBot.AudioLib
{
    public class BufferedWaveProvider : IWaveProvider
    {
        private readonly WaveFormat waveFormat;
        public int BufferLength { get; set; }

        public BufferedWaveProvider(WaveFormat waveFormat)
        {
            this.waveFormat = waveFormat;
            this.BufferLength = waveFormat.AverageBytesPerSecond * 5;
        }

        public WaveFormat WaveFormat
        {
            get { return waveFormat; }
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            
            return count;
        }
    }
}
