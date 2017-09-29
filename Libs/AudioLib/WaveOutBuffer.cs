using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace MidiBot.AudioLib
{
    class WaveOutBuffer
    {
        private readonly WaveHeader header;
        private readonly int bufferSize;
        private readonly byte[] buffer;
        private GCHandle hBuffer;
        private IntPtr waveOutHandle;
        private GCHandle hHeader;
        private GCHandle hThis;

        public byte[] Data
        {
            get
            {
                return buffer;
            }
        }
        public int BytesRecorded
        {
            get
            {
                return header.bytesRecorded;
            }
        }
        public int BufferSize
        {
            get
            {
                return bufferSize;
            }
        }

        public WaveOutBuffer(IntPtr WaveOutHandle, int BufferSize)
        {
            waveOutHandle = WaveOutHandle;
            bufferSize = BufferSize;
            buffer = new byte[bufferSize];
            hBuffer = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            header = new WaveHeader();
            hHeader = GCHandle.Alloc(header, GCHandleType.Pinned);
            header.dataBuffer = hBuffer.AddrOfPinnedObject();
            header.bufferLength = bufferSize;
            header.loops = 1;
            hThis = GCHandle.Alloc(this);
            header.userData = (IntPtr)hThis;
            WinMM.waveOutPrepareHeader(waveOutHandle, header, Marshal.SizeOf(header));
        }

        public bool InQueue
        {
            get
            {
                return (header.flags & WaveHeaderFlags.InQueue) == WaveHeaderFlags.InQueue;
            }
        }

        internal void OnDone()
        {
            
        }
    }
}
