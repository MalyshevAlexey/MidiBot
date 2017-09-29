using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MidiBot.AudioLib
{
    public class WaveOut : IDisposable
    {
        private IntPtr waveOutHandle;
        public int DeviceNumber { get; set; }
        private WaveOutBuffer[] buffers;
        private int lastReturnedBufferIndex;
        public WaveFormat WaveFormat { get; set; }
        private readonly WinMM.WaveCallback callback;
        public int NumberOfBuffers { get; set; }
        private volatile bool playing;
        private WaveIn waveIn;

        public WaveOut(WaveIn WaveIn)
        {
            waveIn = WaveIn;
            waveIn.DataAvailable += new EventHandler<WaveInEventArgs>(DataAvailable);
            DeviceNumber = 0;
            WaveFormat = waveIn.WaveFormat;
            NumberOfBuffers = waveIn.NumberOfBuffers;
            callback = new WinMM.WaveCallback(CallBack);
            WinMM.waveOutOpen(out waveOutHandle, DeviceNumber, WaveFormat, callback, IntPtr.Zero, WinMM.CallbackFunction);
            CreateBuffers();
        }

        private void CallBack(IntPtr hdrvr, int msg, IntPtr dwUser, WaveHeader waveHeader, int dwParam2)
        {
            Console.WriteLine("WaveOut");
            if (msg == WinMM.WaveOutDone && playing)
            {
                try
                {
                    GCHandle hBuffer = (GCHandle)waveHeader.userData;
                    WaveOutBuffer buffer = (WaveOutBuffer)hBuffer.Target;
                    lastReturnedBufferIndex = Array.IndexOf(buffers, buffer);
                    //buffer.Use();
                }
                catch
                {
                }
            }
        }
        private void CreateBuffers()
        {
            buffers = new WaveOutBuffer[NumberOfBuffers];
            for (int n = 0; n < buffers.Length; n++)
                buffers[n] = new WaveOutBuffer(waveOutHandle, waveIn.bufferSize);
        }
        private void EnqueueBuffers()
        {
            foreach (var buffer in buffers)
                if (!buffer.InQueue)
                    buffer.OnDone();
        }
        
        public void StartPlayback()
        {
            
            playing = true;
            EnqueueBuffers();

            Console.WriteLine("Playback started " + WinMM.waveOutGetNumDevs());
            //generatorThread = new Thread(signalGenerator.GenerateSignal);
            //generatorThread.IsBackground = true;
            //generatorThread.Start();
        }
        

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        void DataAvailable(object sender, WaveInEventArgs e)
        {
            Console.WriteLine("Play available");
        }
    }
}
