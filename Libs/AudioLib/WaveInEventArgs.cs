using System;

namespace MidiBot.AudioLib
{
    public class WaveInEventArgs : EventArgs
    {
        private byte[] buffer;
        private int bytes;
        public WaveInEventArgs(byte[] buffer, int bytes)
        {
            this.buffer = buffer;
            this.bytes = bytes;
        }
        public byte[] Buffer
        {
            get { return buffer; }
        }
        public int BytesRecorded
        {
            get { return bytes; }
        }
    }
}
