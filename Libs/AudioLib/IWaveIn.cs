using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiBot.AudioLib
{
    public interface IWaveIn : IDisposable
    {
        WaveFormat WaveFormat { get; set; }
        void StartRecording();
        void StopRecording();
        event EventHandler<WaveInEventArgs> DataAvailable;
        event EventHandler<StoppedEventArgs> RecordingStopped;
    }
}
