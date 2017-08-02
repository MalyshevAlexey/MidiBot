using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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

        public WaveInBuffer(IntPtr waveInHandle, int bufferSize)
        {
            this.bufferSize = bufferSize;
            this.buffer = new byte[bufferSize];
            this.hBuffer = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            this.waveInHandle = waveInHandle;

            header = new WaveHeader();
            hHeader = GCHandle.Alloc(header, GCHandleType.Pinned);
            header.dataBuffer = hBuffer.AddrOfPinnedObject();
            header.bufferLength = bufferSize;
            header.loops = 1;
            hThis = GCHandle.Alloc(this);
            header.userData = (IntPtr)hThis;

            WinMM.waveInPrepareHeader(waveInHandle, header, Marshal.SizeOf(header));
            WinMM.waveInAddBuffer(waveInHandle, header, Marshal.SizeOf(header));
        }

        public void Reuse()
        {
            //WinMM.waveInUnprepareHeader(waveInHandle, header, Marshal.SizeOf(header));
            WinMM.waveInPrepareHeader(waveInHandle, header, Marshal.SizeOf(header));
            WinMM.waveInAddBuffer(waveInHandle, header, Marshal.SizeOf(header));
        }
    }
}
