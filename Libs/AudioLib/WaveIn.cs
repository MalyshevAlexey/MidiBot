using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MidiBot.AudioLib
{
    public class WaveIn : IWaveIn
    {
        private IntPtr waveInHandle;
        private volatile bool recording;
        private WaveInBuffer[] buffers;
        private int lastReturnedBufferIndex;
        private readonly WinMM.WaveCallback callback;
        private SignalGenerator signalGenerator;
        private Thread generatorThread;

        public int DeviceNumber { get; set; }
        public int BufferMilliseconds { get; set; }
        public int NumberOfBuffers { get; set; }
        public WaveFormat WaveFormat { get; set; }
        public event EventHandler<WaveInEventArgs> DataAvailable;
        public event EventHandler<StoppedEventArgs> RecordingStopped;

        public WaveIn()
        {
            DeviceNumber = 0;
            WaveFormat = new WaveFormat();
            BufferMilliseconds = 100;
            NumberOfBuffers = 3;
            callback = new WinMM.WaveCallback(CallBack);
            signalGenerator = new SignalGenerator(WaveFormat);
        }

        private void CallBack(IntPtr hdrvr, int msg, IntPtr instance, WaveHeader waveHeader, int reserved)
        {
            if (msg == WinMM.WaveInData && recording)
            {
                var hBuffer = (GCHandle)waveHeader.userData;
                var buffer = (WaveInBuffer)hBuffer.Target;
                if (buffer == null) return;
                lastReturnedBufferIndex = Array.IndexOf(buffers, buffer);
                DataAvailable?.Invoke(this, new WaveInEventArgs(buffer.Data, buffer.BytesRecorded));
                buffer.Use();
            }
        }

        private void CreateBuffers()
        {
            int bufferSize = BufferMilliseconds * WaveFormat.AverageBytesPerSecond / 1000;
            if (bufferSize % WaveFormat.BlockAlign != 0)
                bufferSize -= bufferSize % WaveFormat.BlockAlign;
            buffers = new WaveInBuffer[NumberOfBuffers];
            for (int n = 0; n < buffers.Length; n++)
                buffers[n] = new WaveInBuffer(waveInHandle, bufferSize);
        }

        private void EnqueueBuffers()
        {
            foreach (var buffer in buffers)
                if (!buffer.InQueue)
                    buffer.Use();
        }

        public void StartRecording()
        {
            WinMM.waveInOpen(out waveInHandle, DeviceNumber, WaveFormat, callback, IntPtr.Zero, WinMM.CallbackFunction);
            CreateBuffers();
            EnqueueBuffers();
            WinMM.waveInStart(waveInHandle);
            recording = true;
            //generatorThread = new Thread(signalGenerator.GenerateSignal);
            //generatorThread.IsBackground = true;
            //generatorThread.Start();
        }

        public void StopRecording()
        {
            recording = false;
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
