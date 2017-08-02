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
        private readonly Int32 bufferSize; // allocated bytes, may not be the same as bytes read
        private readonly byte[] buffer;
        private GCHandle hBuffer;
        private IntPtr waveInHandle;
        private GCHandle hHeader; // we need to pin the header structure
        private GCHandle hThis; // for the user callback
    }
}
