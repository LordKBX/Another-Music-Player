using System;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Win32;
using NAudio.Dsp;
using NAudio.Wave;

namespace AnotherMusicPlayer
{
    class EqualizerBand
    {
        public float Frequency { get; set; }
        public float Gain { get; set; }
        public float Bandwidth { get; set; }
    }

    /// <summary>
    /// Basic example of a multi-band eq
    /// uses the same settings for both channels in stereo audio
    /// Call Update after you've updated the bands
    /// Potentially to be added to NAudio in a future version
    /// </summary>
    class Equalizer : ISampleProvider
    {
        private readonly ISampleProvider sourceProvider;
        private readonly EqualizerBand[] bands;
        private readonly BiQuadFilter[,] filters;
        private readonly int channels;
        private readonly int bandCount;
        private bool updated;

        public Equalizer(ISampleProvider sourceProvider, EqualizerBand[] bands)
        {
            this.sourceProvider = sourceProvider;
            this.bands = bands;
            channels = sourceProvider.WaveFormat.Channels;
            bandCount = bands.Length;
            filters = new BiQuadFilter[channels, bands.Length];
            CreateFilters();
        }

        private void CreateFilters()
        {
            for (int bandIndex = 0; bandIndex < bandCount; bandIndex++)
            {
                var band = bands[bandIndex];
                for (int n = 0; n < channels; n++)
                {
                    if (filters[n, bandIndex] == null)
                        filters[n, bandIndex] = BiQuadFilter.PeakingEQ(sourceProvider.WaveFormat.SampleRate, band.Frequency, band.Bandwidth, band.Gain);
                    else
                        filters[n, bandIndex].SetPeakingEq(sourceProvider.WaveFormat.SampleRate, band.Frequency, band.Bandwidth, band.Gain);
                }
            }
        }

        public void Update()
        {
            updated = true;
            CreateFilters();
        }

        public WaveFormat WaveFormat => sourceProvider.WaveFormat;

        public int Read(float[] buffer, int offset, int count)
        {
            int samplesRead = 0;
            try
            {
                samplesRead = sourceProvider.Read(buffer, offset, count);
                if (updated)
                {
                    CreateFilters();
                    updated = false;
                }

                for (int n = 0; n < samplesRead; n++)
                {
                    int ch = n % channels;

                    for (int band = 0; band < bandCount; band++)
                    {
                        buffer[offset + n] = filters[ch, band].Transform(buffer[offset + n]);
                    }
                }
            }
            catch { }
            return samplesRead;
        }
    }

}
