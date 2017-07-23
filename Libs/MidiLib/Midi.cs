using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MidiBot.MidiLib
{
    internal delegate void MidiProc(int inHandle, int msg, IntPtr instance, int data, int time);

    public class Midi
    {
        private int inHandle;
        private int outHandle;
        internal const int CALLBACK_FUNCTION = 0x00030000;
        internal const int MIM_DATA = 0x3C3;
        internal const int MIM_LONGDATA = 0x3C4;

        static public void CallBack(int inHandle, int msg, IntPtr instance, int data, int time)
        {
            switch (msg)
            {
                case MIM_DATA:
                    Console.WriteLine("MIDI Message: {0} {1} {2}", msg, BitConverter.ToString(BitConverter.GetBytes(data)), TimeSpan.FromMilliseconds(time).ToString());
                    break;
            }
        }
        public void InOpen(string deviceName)
        {
            MidiInCaps caps = new MidiInCaps();
            int deviceID = -1;
            for (int i = 0; i < WinMM.midiInGetNumDevs(); i++)
            {
                WinMM.midiInGetDevCaps(i, ref caps, Marshal.SizeOf(typeof(MidiInCaps)));
                if (caps.name == deviceName)
                {
                    deviceID = i;
                    break;
                }
            }
            if (deviceID == -1) return;
            Console.WriteLine(deviceID);
            MidiProc midiProc = new MidiProc(CallBack);
            IntPtr pointer = Marshal.GetIUnknownForObject(this);
            WinMM.midiInOpen(ref inHandle, deviceID, midiProc, pointer, CALLBACK_FUNCTION);
            WinMM.midiInStart(inHandle);
        }

        public void OutOpen(string deviceName)
        {
            MidiOutCaps caps = new MidiOutCaps();
            int deviceID = -1;
            for (int i = 0; i < WinMM.midiInGetNumDevs(); i++)
            {
                WinMM.midiOutGetDevCaps(i, ref caps, Marshal.SizeOf(typeof(MidiOutCaps)));
                if (caps.name == deviceName)
                {
                    deviceID = i;
                    break;
                }
            }
            WinMM.midiOutOpen(ref outHandle, deviceID, null, 0, 0);
        }

        public void SendMidi(byte[] midi)
        {
            uint msg = BitConverter.ToUInt32(midi, 0);
            WinMM.midiOutShortMsg(outHandle, (int)msg);
        }
    }
}
