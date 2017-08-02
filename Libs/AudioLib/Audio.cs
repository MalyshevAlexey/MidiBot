﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MidiBot.AudioLib
{
    

    public class Audio
    {
        //

        //private const int WAVEOUT_DATA = 0x3C4;
        //private IntPtr waveInHandle;
        //private IntPtr waveOutHandle;
        //WaveDelegate callback;
        //WaveFormat waveFormat;
        //WaveHeader header;
        //
        //byte[] buffer;
        //GCHandle hBuffer;
        //GCHandle hHeader;
        //GCHandle hThis;

        WaveIn wi;





        public Audio()
        {
            wi = new WaveIn();
            wi.DeviceNumber = 0;
            wi.WaveFormat = new WaveFormat();
            wi.DataAvailable += new EventHandler<WaveInEventArgs>(wi_DataAvailable);
            wi.StartRecording();
        }

        void wi_DataAvailable(object sender, WaveInEventArgs e)
        {
            Console.WriteLine("da");
        }

        private void oldCode()
        { 
            //WaveInCaps caps = new WaveInCaps();
            //int num = WinMM.waveOutGetNumDevs();
            //int capSize = Marshal.SizeOf(typeof(WaveInCaps));
            //for (int i = 0; i < num; i++)
            //{
            //    WinMM.waveInGetDevCaps(i, ref caps, capSize);
            //    Console.WriteLine(i);
            //    Console.WriteLine(caps.mid);
            //    Console.WriteLine(caps.pid);
            //    Console.WriteLine(caps.driverVersion);
            //    Console.WriteLine(caps.name);
            //    Console.WriteLine(Convert.ToString(caps.formats, 2));
            //    Console.WriteLine(caps.channels);
            //    Console.WriteLine(caps.reserved);
            //}
            //waveFormat = new WaveFormat();
            //callback = new WaveDelegate(CallBack);
            //IntPtr pointer = Marshal.GetIUnknownForObject(this);
            //WinMM.waveInOpen(out waveInHandle, 0, waveFormat, callback, pointer, CALLBACK_FUNCTION);
            //AddInBuffer();
            //WinMM.waveInStart(waveInHandle);
            //int result = WinMM.waveOutOpen(out waveInHandle, 0, waveFormat, callback, pointer, CALLBACK_FUNCTION);
            //Console.WriteLine(result);
            


        }

        //private void AddInBuffer()
        //{
        //    buffer = new byte[bufferSize];
        //    hBuffer = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        //    header = new WaveHeader();
        //    hHeader = GCHandle.Alloc(header, GCHandleType.Pinned);
        //    header.dataBuffer = hBuffer.AddrOfPinnedObject();
        //    header.bufferLength = bufferSize;
        //    header.loops = 1;
        //    hThis = GCHandle.Alloc(this);
        //    header.userData = (IntPtr)hThis;
        //    WinMM.waveInPrepareHeader(waveInHandle, header, Marshal.SizeOf(header));
        //    WinMM.waveInAddBuffer(waveInHandle, header, Marshal.SizeOf(header));
        //}

        //public void ReuseInBuffer()
        //{
        //    buffer = new byte[bufferSize];
        //    WinMM.waveInUnprepareHeader(waveInHandle, header, Marshal.SizeOf(header));
        //    WinMM.waveInPrepareHeader(waveInHandle, header, Marshal.SizeOf(header));
        //    WinMM.waveInAddBuffer(waveInHandle, header, Marshal.SizeOf(header));
        //}



        //public enum MMRESULT : uint
        //{
        //    MMSYSERR_NOERROR = 0,
        //    MMSYSERR_ERROR = 1,
        //    MMSYSERR_BADDEVICEID = 2,
        //    MMSYSERR_NOTENABLED = 3,
        //    MMSYSERR_ALLOCATED = 4,
        //    MMSYSERR_INVALHANDLE = 5,
        //    MMSYSERR_NODRIVER = 6,
        //    MMSYSERR_NOMEM = 7,
        //    MMSYSERR_NOTSUPPORTED = 8,
        //    MMSYSERR_BADERRNUM = 9,
        //    MMSYSERR_INVALFLAG = 10,
        //    MMSYSERR_INVALPARAM = 11,
        //    MMSYSERR_HANDLEBUSY = 12,
        //    MMSYSERR_INVALIDALIAS = 13,
        //    MMSYSERR_BADDB = 14,
        //    MMSYSERR_KEYNOTFOUND = 15,
        //    MMSYSERR_READERROR = 16,
        //    MMSYSERR_WRITEERROR = 17,
        //    MMSYSERR_DELETEERROR = 18,
        //    MMSYSERR_VALNOTFOUND = 19,
        //    MMSYSERR_NODRIVERCB = 20,
        //    WAVERR_BADFORMAT = 32,
        //    WAVERR_STILLPLAYING = 33,
        //    WAVERR_UNPREPARED = 34
        //}
    }
}
