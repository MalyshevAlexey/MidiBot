using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiBot.AudioLib
{
    public interface IWaveProvider
    {
        WaveFormat WaveFormat { get; }
        int Read(byte[] buffer, int offset, int count);
    }
}
