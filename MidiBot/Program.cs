﻿using MidiBot.MidiLib;
using MidiBot;
using System;
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
        [STAThread]
        static void Main()
        {
            //Midi midi = new Midi();
            //midi.InOpen("Ableton Push 2");
            //midi.OutOpen("Ableton Push 2");
            //midi.SendMidi(new byte[] { 0x90, 0x3C, 0x7F, 0x00 });
            //Thread.Sleep(1000);
            //midi.SendMidi(new byte[] { 0x80, 0x3C, 0x00, 0x00 });

            Push2Controller push2 = new Push2Controller();
            

            //Usb usb = new Usb();

            Console.ReadKey();

            
        }
    }
}
