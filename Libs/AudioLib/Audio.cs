using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MidiBot.AudioLib
{
    internal delegate void WaveDelegate(IntPtr hdrvr, int msg, IntPtr instance, ref WaveHeader wavhdr, int dwParam2);

    public class Audio
    {
        private const int CALLBACK_FUNCTION = 0x00030000;
        private IntPtr waveInHandle;

        internal static void CallBack(IntPtr hdrvr, int msg, IntPtr instance, ref WaveHeader wavhdr, int dwParam2)
        {
            Console.WriteLine("da");
        }

        public Audio()
        {

            WaveInCaps caps = new WaveInCaps();
            int num = WinMM.waveOutGetNumDevs();
            int capSize = Marshal.SizeOf(typeof(WaveInCaps));
            for (int i = 0; i < num; i++)
            {
                WinMM.waveInGetDevCaps(i, ref caps, capSize);
                Console.WriteLine(i);
                Console.WriteLine(caps.mid);
                Console.WriteLine(caps.pid);
                Console.WriteLine(caps.driverVersion);
                Console.WriteLine(caps.name);
                Console.WriteLine(Convert.ToString(caps.formats,2));
                Console.WriteLine(caps.channels);
                Console.WriteLine(caps.reserved);
            }
            WaveFormat waveFormat = new WaveFormat();
            WaveDelegate callback = new WaveDelegate(CallBack);
            int result = WinMM.waveInOpen(out waveInHandle, 0, waveFormat, callback, IntPtr.Zero, CALLBACK_FUNCTION);
            Console.WriteLine("Result: " + result);
        }

        
        
        
    }
}
