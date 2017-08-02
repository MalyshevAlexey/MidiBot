using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MidiBot.AudioLib
{
    public class WaveIn : IWaveIn
    {
        private IntPtr waveInHandle;
        private volatile bool recording;
        private WaveInBuffer buffer;
        private readonly WinMM.WaveCallback callback;

        public int DeviceNumber { get; set; }
        public int NumberOfBuffers { get; set; }
        public WaveFormat WaveFormat { get; set; }
        public event EventHandler<WaveInEventArgs> DataAvailable;
        public event EventHandler<StoppedEventArgs> RecordingStopped;

        int bufferSize = 16384;

        public WaveIn()
        {
            callback = new WinMM.WaveCallback(CallBack);
        }

        private void CallBack(IntPtr hdrvr, int msg, IntPtr instance, WaveHeader waveHeader, int reserved)
        {
            if (msg == WinMM.WaveInData && recording)
            {
                var hBuffer = (GCHandle)waveHeader.userData;
                var buffer = (WaveInBuffer)hBuffer.Target;
                if (buffer == null) return;
                DataAvailable?.Invoke(this, new WaveInEventArgs(buffer.Data, buffer.BytesRecorded));
                buffer.Reuse();
            }
        }

        public void StartRecording()
        {
            WinMM.waveInOpen(out waveInHandle, DeviceNumber, WaveFormat, callback, IntPtr.Zero, WinMM.CallbackFunction);
            buffer = new WaveInBuffer(waveInHandle, bufferSize);
            WinMM.waveInStart(waveInHandle);
            recording = true;
        }

        public void StopRecording()
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
