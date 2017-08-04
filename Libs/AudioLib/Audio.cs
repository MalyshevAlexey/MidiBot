using System;

namespace MidiBot.AudioLib
{
    public class Audio
    {
        private WaveIn wi;
        private readonly object lockObject;

        public Audio()
        {
            lockObject = new object();
            wi = new WaveIn();
            wi.DataAvailable += new EventHandler<WaveInEventArgs>(wi_DataAvailable);
            wi.StartRecording();
        }

        void wi_DataAvailable(object sender, WaveInEventArgs e)
        {
            int[] test = new int[e.Buffer.Length / wi.WaveFormat.BlockAlign];
            for (int i = 0; i < test.Length; i ++)
            {
                byte upper = e.Buffer[i * 2 + 1];
                byte lower = e.Buffer[i * 2];
                test[i] = (short)((upper << 8) | lower);
                //Console.WriteLine(test[i]);
            }
        }

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
