using MidiBot.MidiLib;
using MidiBot;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MidiBot.Push2;

namespace MidiBot
{
    static class Program
    {
        private delegate bool ConsoleEventDelegate(int eventType);
        static ConsoleEventDelegate handler;
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);
        static bool ConsoleEventCallback(int eventType)
        {
            if (eventType == 2)
                midi.Close();
                
            return false;
        }

        static Midi midi;

        [STAThread]
        static int Main()
        {
            try
            {
                handler = new ConsoleEventDelegate(ConsoleEventCallback);
                SetConsoleCtrlHandler(handler, true);

                midi = new Midi();
                midi.InOpen("testMidi");

                //midi.InOpen("Ableton Push 2");
                //midi.OutOpen("Ableton Push 2"); 
                ////midi.OutOpen("MIDIOUT2 (Ableton Push 2)");
                ////midi.InOpen("MIDIIN2 (Ableton Push 2)");
                ////midi.OutOpen("fromDAW");
                ////midi.InOpen("toDAW");
                //midi.SendMidi(new byte[] { 0x90, 0x3C, 0x7F, 0x00 });
                //Thread.Sleep(1000);
                //midi.SendMidi(new byte[] { 0x80, 0x3C, 0x00, 0x00 });
                //midi.SendSysex(new byte[] { 0xF0, 0x00, 0x21, 0x1D, 0x01, 0x01, 0x0A, 0x00, 0xF7 });
                ////midi.SendSysex(new byte[] { 0xF0, 0x00, 0x21, 0x1D, 0x01, 0x01, 0x0B, 0x00, 0xF7 });
                ////midi.SendSysex(new byte[] { 0xF0, 0x00, 0x21, 0x1D, 0x01, 0x01, 0x0C, 0x00, 0xF7 });
                //midi.OnShortReceive = test;
                //midi.OnLongReceive = test;





                //Push2Controller push2 = new Push2Controller();

                midi.OutClose();
                midi.InClose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                return -1;
            }
            finally
            {
                Console.WriteLine("Block finally");
            }
            Console.ReadKey();
            return 0;
        }

        private static void test(byte[] data, int time)
        {
            Console.WriteLine("Message received {0} at {1}", BitConverter.ToString(data), TimeSpan.FromMilliseconds(time));
        }
    }
}
