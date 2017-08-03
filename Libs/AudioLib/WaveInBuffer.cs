using System;
using System.Runtime.InteropServices;

namespace MidiBot.AudioLib
{
    class WaveInBuffer
    {
        private readonly WaveHeader header;
        private readonly int bufferSize; // allocated bytes, may not be the same as bytes read
        private readonly byte[] buffer;
        private GCHandle hBuffer;
        private IntPtr waveInHandle;
        private GCHandle hHeader; // we need to pin the header structure
        private GCHandle hThis; // for the user callback

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

        public WaveInBuffer(IntPtr WaveInHandle, int BufferSize)
        {
            bufferSize = BufferSize;
            buffer = new byte[bufferSize];
            hBuffer = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            waveInHandle = WaveInHandle;

            header = new WaveHeader();
            hHeader = GCHandle.Alloc(header, GCHandleType.Pinned);
            header.dataBuffer = hBuffer.AddrOfPinnedObject();
            header.bufferLength = bufferSize;
            header.loops = 1;
            hThis = GCHandle.Alloc(this);
            header.userData = (IntPtr)hThis;
        }

        public void Use()
        {
            //WinMM.waveInUnprepareHeader(waveInHandle, header, Marshal.SizeOf(header));
            WinMM.waveInPrepareHeader(waveInHandle, header, Marshal.SizeOf(header));
            WinMM.waveInAddBuffer(waveInHandle, header, Marshal.SizeOf(header));
        }

        public bool InQueue
        {
            get
            {
                return (header.flags & WaveHeaderFlags.InQueue) == WaveHeaderFlags.InQueue;
            }
        }
    }
}
