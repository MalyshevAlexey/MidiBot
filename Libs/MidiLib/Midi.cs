using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MidiBot.MidiLib
{
    internal delegate void MidiProc(int inHandle, int msg, IntPtr instance, int data, int time);    // delegate for callback function

    public class Midi
    {
        /// <summary>
        /// callback function delegate
        /// </summary>
        private static MidiProc midiProc;
        private int inHandle;                                   // midi IN handle
        private int outHandle;                                  // midi OUT handle
        private MidiHeader header;                              // midi header
        private const int CALLBACK_FUNCTION = 0x00030000;       // flag for callback function
        private const int MIM_DATA = 0x3C3;                     // constant identify that short data received
        private const int MIM_LONGDATA = 0x3C4;                 // constant idnetify that long data received
        public LongAnswer longAnswer;
        public byte[] shortAnswer;
        /// <summary> fires when short message received </summary>
        public Action<byte[], int> OnShortReceive;              // 
        public Action<byte[], int> OnLongReceive;               // fires when long message received
        public Action<byte[]> AfterShortSent;                   // fires after sort message sent
        /// <summary>
        /// 
        /// </summary>
        public Action<byte[]> AfterLongSent;                    // fires after long message sent;

        static private void CallBack(int inHandle, int msg, IntPtr instance, int data, int time)
        {
            Midi midi = (Midi)Marshal.GetObjectForIUnknown(instance); // get midi object from pointer
            switch (msg)
            {
                case MIM_DATA: // if short message received
                    midi.shortAnswer = BitConverter.GetBytes(data); // save last message
                    midi.OnShortReceive?.Invoke(BitConverter.GetBytes(data), time); //if not null invoke
                    break;
                case MIM_LONGDATA: // if long message received
                    midi.longAnswer = new LongAnswer(); // init LongAnswer structure
                    MidiHeader header = (MidiHeader)Marshal.PtrToStructure((IntPtr)data, typeof(MidiHeader)); // get header from pointer
                    if (header.bytesRecorded != 0) // if answer is valid
                    {
                        byte[] message = new byte[header.bytesRecorded]; // init message array
                        Marshal.Copy(header.data, message, 0, header.bytesRecorded); // get message from pointer
                        midi.longAnswer.Data = message; // save last message
                        midi.longAnswer.Time = TimeSpan.FromMilliseconds(time); // save last time
                        midi.OnLongReceive?.Invoke(message, time); // if not null invoke
                        midi.AddSysexBuffer(); // add buffer to receive next long message
                    }
                    break;
                    
            }
        }

        private void GetInNames()
        {
            Console.WriteLine("In Devices: ");
            MidiInCaps caps = new MidiInCaps();
            for (int i = 0; i < WinMM.midiInGetNumDevs(); i++)
            {
                WinMM.midiInGetDevCaps(i, ref caps, Marshal.SizeOf(typeof(MidiInCaps)));
                Console.WriteLine(caps.name);
            }
        }

        private int GetInIdByName(string deviceName)
        {
            MidiInCaps caps = new MidiInCaps();
            int deviceID = -1;
            int num = WinMM.midiInGetNumDevs();
            if (num == 0)
                throw new Exception("There are no midi IN devices");
            int capSize = Marshal.SizeOf(typeof(MidiInCaps));
            for (int i = 0; i < num; i++)
            {
                if (WinMM.midiInGetDevCaps(i, ref caps, capSize) != 0)
                    throw new Exception("Cannot get midi IN device with ID " + i);
                if (caps.name == deviceName)
                {
                    deviceID = i;
                    break;
                }
            }
            if (deviceID == -1)
                throw new Exception("Midi IN device " + deviceName + " not found");
            return deviceID;
        }

        public int InOpen(string deviceName)
        {
            int result = -1;
            int deviceID = GetInIdByName(deviceName);
            IntPtr pointer = Marshal.GetIUnknownForObject(this);
            midiProc = new MidiProc(CallBack);
            result = WinMM.midiInOpen(ref inHandle, deviceID, midiProc, pointer, CALLBACK_FUNCTION);
            if (result != 0)
                throw new Exception("Cannot open IN device " + deviceName);
            AddSysexBuffer();
            result = WinMM.midiInStart(inHandle);
            if (result != 0)
                throw new Exception("Cannot start IN device " + deviceName);
            return result;
        }

        private int AddSysexBuffer()
        {
            int result = -1;
            int shdr = Marshal.SizeOf(typeof(MidiHeader));
            IntPtr nhdr = Marshal.AllocHGlobal(shdr);
            header = new MidiHeader();
            header.bufferLength = header.bytesRecorded = 128;
            header.bytesRecorded = 0;
            header.data = Marshal.AllocHGlobal(128);
            header.flags = 0;
            Marshal.StructureToPtr(header, nhdr, false);
            result = WinMM.midiInPrepareHeader(inHandle, nhdr, shdr);
            if (result != 0)
                throw new Exception("Cannot prepare IN header");
            result = WinMM.midiInAddBuffer(inHandle, nhdr, shdr);
            if (result != 0)
                throw new Exception("Cannot add IN buffer");
            return result;
        }

        private void GetOutNames()
        {
            Console.WriteLine("Out Devices: ");
            MidiOutCaps caps = new MidiOutCaps();
            for (int i = 0; i < WinMM.midiOutGetNumDevs(); i++)
            {
                WinMM.midiOutGetDevCaps(i, ref caps, Marshal.SizeOf(typeof(MidiOutCaps)));
                Console.WriteLine(caps.name);
            }
        }

        private int GetOutIdByName(string deviceName)
        {
            MidiOutCaps caps = new MidiOutCaps();
            int deviceID = -1;
            int num = WinMM.midiOutGetNumDevs();
            for (int i = 0; i < num; i++)
            {
                WinMM.midiOutGetDevCaps(i, ref caps, Marshal.SizeOf(typeof(MidiOutCaps)));
                if (caps.name == deviceName)
                {
                    deviceID = i;
                    break;
                }
            }
            if (deviceID == -1)
                throw new Exception("Midi OUT device " + deviceName + " not found");
            return deviceID;
        }

        public int OutOpen(string deviceName)
        {
            int result = -1;
            int deviceID = GetOutIdByName(deviceName);
            result = WinMM.midiOutOpen(ref outHandle, deviceID, null, 0, 0);
            if (result != 0)
                throw new Exception("Cannot open OUT device " + deviceName);
            return result;
        }

        public int SendMidi(byte[] midi)
        {
            int result = -1;
            int msg = BitConverter.ToInt32(midi, 0); // convert byte[] to int
            result = WinMM.midiOutShortMsg(outHandle, msg); // sending message
            if (result != 0)
                throw new Exception("Cannot send short message");
            AfterShortSent?.Invoke(midi); // if not null Invoke
            return result;
        }

        public int SendSysex(byte[] sx)
        {
            int result = -1;
            int shdr = Marshal.SizeOf(typeof(MidiHeader));
            var mhdr = new MidiHeader();
            mhdr.bufferLength = mhdr.bytesRecorded = sx.Length;
            mhdr.data = Marshal.AllocHGlobal(mhdr.bufferLength);
            Marshal.Copy(sx, 0, mhdr.data, mhdr.bufferLength);
            IntPtr nhdr = Marshal.AllocHGlobal(shdr);
            Marshal.StructureToPtr(mhdr, nhdr, false);
            result = WinMM.midiOutPrepareHeader(outHandle, nhdr, shdr);
            if (result != 0)
                throw new Exception("Cannot prepare OUT header");
            result = WinMM.midiOutLongMsg(outHandle, nhdr, shdr);
            if (result != 0)
                throw new Exception("Cannot send long message");
            result = WinMM.midiOutUnprepareHeader(outHandle, nhdr, shdr);
            if (result != 0)
                throw new Exception("Cannot ");
            AfterLongSent?.Invoke(sx);                                  // if not null Invoke
            return result;
        }

        public void OutClose()
        {
            WinMM.midiOutClose(outHandle);
        }

        public void InClose()
        {
            int shdr = Marshal.SizeOf(typeof(MidiHeader));
            IntPtr nhdr = Marshal.AllocHGlobal(shdr);
            WinMM.midiInUnprepareHeader(inHandle, nhdr, shdr);
            WinMM.midiInReset(inHandle);
            WinMM.midiInStop(inHandle);
            WinMM.midiInClose(inHandle);
        }

        public void Close()
        {
            OutClose();
            InClose();
        }
    }
}
