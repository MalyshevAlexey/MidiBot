using System;

namespace MidiBot.AudioLib
{
    internal class SignalGenerator
    {
        private WaveFormat WaveFormat { get; set; }

        public SignalGenerator(WaveFormat waveFormat)
        {
            WaveFormat = waveFormat;
        }
        public double[] GenerateSignal()
        {
            int _samples = 16384;
            double _frequency = 5000.0;
            double _amplitude = 32768.0;
            double[] values = new double[_samples];
            double theta = 2.0 * Math.PI * _frequency / WaveFormat.SampleRate;
            for (int i = 0; i < _samples; i++)
            {
                values[i] = _amplitude * Math.Sin(i * theta);
            }
            return values;
        }
    }

    
}