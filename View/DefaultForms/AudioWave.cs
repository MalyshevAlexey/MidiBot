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
        Graphics g;
        Pen pen;
        WaveIn wi;
        double[] waveLeft;
        double[] waveRight;

        public AudioWave()
        {
            InitializeComponent();
            bmpWave = new Bitmap(pictureWave.Width, pictureWave.Height);
            g = Graphics.FromImage(bmpWave);
            pen = new System.Drawing.Pen(Color.WhiteSmoke);
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
            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        public void RenderTimeDomain()
        {
            g.Clear(pictureWave.BackColor);
            // Determine channnel boundries
            int width = bmpWave.Width;
            int center = bmpWave.Height / 2;
            int height = bmpWave.Height;

            g.DrawLine(pen, 0, center, width, center);

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
                    g.DrawLine(pen, xPrevLeft, yPrevLeft, xAxis, yAxis);
                    xPrevLeft = xAxis;
                    yPrevLeft = yAxis;
                }
            }
            pictureWave.Image = bmpWave;
        }
    }
}
