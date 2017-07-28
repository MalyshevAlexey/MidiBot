using MidiBot.Interfaces;
using MidiBot.UsbLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace MidiBot.Push2
{
    public class Push2Controller : IMidiController
    {
        const short VENDOR_ID = 0x2982;
        const short PRODUCT_ID = 0x1967;
        const byte BULK_EP_OUT = 0x01;
        const int TRANSFER_TIMEOUT = 1000;
        const int DISPLAY_WIDTH = 960;
        const int DISPLAY_HEIGHT = 160;
        const int LINE_BUFFER_SIZE = 2048;
        const int LINE_GUTTER_SIZE = 128;
        byte[] frame_header =
        {
            0xff, 0xcc, 0xaa, 0x88,
            0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00
        };
        private readonly ushort[] xOrMasks = { 0xf3e7, 0xffe7 };
        private byte[] frame = new byte[327680];

        Bitmap bmp;
        Graphics g;
        Pen pen;
        object locker = new object();

        public Push2Controller()
        {
            bmp = new Bitmap(960, 160, PixelFormat.Format24bppRgb);
            g = Graphics.FromImage(bmp);
            pen = new Pen(Color.DarkRed);
            g.Clear(Color.White);
            g.Flush();
            Thread display = new Thread(Display);
            display.IsBackground = true;
            display.Start();
            int total = 180;
            for (float coef = 1; coef < 1800; coef += 0.02f)
            {
                lock (locker)
                {
                    g.Clear(Color.White);
                    for (int n = 0; n < total; n++)
                        ArcLine(n * 2, n * 2 * coef);
                    g.Flush();
                }
                Thread.Sleep(10);
            }
        }

        private void Display()
        {
            Usb usb = new Usb(VENDOR_ID, PRODUCT_ID);
            while (true)
            {
                lock (locker)
                    frame = MakeFrame(bmp);
                DateTime start = DateTime.Now;
                usb.Write(frame_header, 100);
                usb.Write(frame, 100);
                //Console.WriteLine(DateTime.Now - start);
            }
        }

            int zoom = 80;
        private void ArcLine(float alfa, float beta)
        {
            float x1 = zoom + (float)Math.Cos(alfa / 180.0 * Math.PI) * zoom;
            float y1 = zoom - (float)Math.Sin(alfa / 180.0 * Math.PI) * zoom;
            float x2 = zoom + (float)Math.Cos(beta / 180.0 * Math.PI) * zoom;
            float y2 = zoom - (float)Math.Sin(beta / 180.0 * Math.PI) * zoom;
            g.DrawLine(pen, x1, y1, x2, y2);
        }

        private byte[] BitmapToArray(Bitmap bmp)
        {
            return BitmapToArray(bmp, DISPLAY_WIDTH, DISPLAY_HEIGHT);
        }

        private byte[] BitmapToArray(Bitmap bmp, int width, int height)
        {
            BitmapData bmpdata = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, bmp.PixelFormat);
            int numbytes = bmpdata.Stride * bmp.Height;
            byte[] bytedata = new byte[numbytes];
            IntPtr ptr = bmpdata.Scan0;
            Marshal.Copy(ptr, bytedata, 0, numbytes);
            bmp.UnlockBits(bmpdata);
            return bytedata;
        }

        private int PixelConverter(byte b, byte g, byte r) // blue green red
        {
            int pixel = b >> 3; // 1111 1111 -> 0001 1111
            pixel <<= 6;        // 0001 1111 -> 0111 1100 0000
            pixel += g >> 2;    // 1111 1111 -> 0011 1111, 0111 1100 0000 -> 0111 1111 1111
            pixel <<= 5;        // 0111 1111 1111 -> 1111 1111 1110 0000
            pixel += r >> 3;    // 1111 1111 -> 0001 1111, 1111 1111 1110 0000 -> 1111 1111 1111 1111
            return pixel;       // 5 bytes blue - 6 bytes green - 5 bytes red
        }

        public byte[] MakeFrame(Bitmap bmp)
        {
            byte[] bytedata = BitmapToArray(bmp);
            int count = 0;
            int next = 0;
            for (int y = 0; y < 160; y++)       // Iterate all lines
            {
                for (int x = 0; x < 960; x++)   // Iterate each pixes in line
                {
                    int pixel = PixelConverter(bytedata[next++], bytedata[next++], bytedata[next++]); // Blue Green Red
                    int pixelXor = pixel ^ xOrMasks[x % 2];     // xOring with first or second short
                    frame[count++] = (byte)(pixelXor & 0xFF);   // getting lower byte
                    frame[count++] = (byte)(pixelXor >> 8);     // getting upper byte
                }
                count += 128; // line mus be 2048 bytes
            }
            return frame;
        }
    }
}
