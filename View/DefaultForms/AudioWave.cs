using MidiBot.AudioLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MidiBot.Views.DefaultForms
{
    public partial class AudioWave : Form
    {
        Bitmap bmpWave;
        Bitmap bmpFFT;
        WaveIn wi;
        double[] waveLeft;
        double[] waveRight;

        public AudioWave()
        {
            InitializeComponent();
            wi = new WaveIn();
            wi.DataAvailable += new EventHandler<WaveInEventArgs>(DataAvailable);
            wi.StartRecording();
        }

        void DataAvailable(object sender, WaveInEventArgs e)
        {
            
            waveLeft = new double[e.Buffer.Length / 2];
            for (int i = 0; i < waveLeft.Length; i++)
                waveLeft[i] = BitConverter.ToInt16(e.Buffer, i * 2);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            RenderTimeDomain();
            sw.Stop();
            Console.WriteLine(sw.ElapsedTicks);
            
        }

        public void RenderTimeDomain()
        {
            // Set up for drawing
            bmpWave = new Bitmap(pictureWave.Width, pictureWave.Height);
            Graphics offScreenDC = Graphics.FromImage(bmpWave);
            SolidBrush brush = new System.Drawing.SolidBrush(Color.FromArgb(0, 0, 0));
            Pen pen = new System.Drawing.Pen(Color.WhiteSmoke);

            // Determine channnel boundries
            int width = bmpWave.Width;
            int center = bmpWave.Height / 2;
            int height = bmpWave.Height;

            offScreenDC.DrawLine(pen, 0, center, width, center);

            int leftLeft = 0;
            int leftTop = 0;
            int leftRight = width;
            int leftBottom = center - 1;

            int rightLeft = 0;
            int rightTop = center + 1;
            int rightRight = width;
            int rightBottom = height;

            // Draw left channel
            double yCenterLeft = (leftBottom - leftTop) / 2;
            double yScaleLeft = 0.5 * (leftBottom - leftTop) / 32768;  // a 16 bit sample has values from -32768 to 32767
            int xPrevLeft = 0, yPrevLeft = 0;
            pen.Color = Color.LimeGreen;
            for (int xAxis = leftLeft; xAxis < leftRight; xAxis++)
            {
                int yAxis = (int)(yCenterLeft + (waveLeft[waveLeft.Length / (leftRight - leftLeft) * xAxis] * yScaleLeft));
                if (xAxis == 0)
                {
                    xPrevLeft = 0;
                    yPrevLeft = yAxis;
                }
                else
                {
                    offScreenDC.DrawLine(pen, xPrevLeft, yPrevLeft, xAxis, yAxis);
                    xPrevLeft = xAxis;
                    yPrevLeft = yAxis;
                }
            }

            // Clean up
            pictureWave.Image = bmpWave;
            offScreenDC.Dispose();
        }
    }
}
