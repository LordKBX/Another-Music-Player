using System;
using System.Collections.Generic;
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
    /// Call Update after you've updated the EqualizerBands
    /// Potentially to be added to NAudio in a future version
    /// </summary>
    class Equalizer : ISampleProvider
    {
        private readonly ISampleProvider sourceProvider;
        private readonly EqualizerBand[] EqualizerBands;
        private readonly BiQuadFilter[,] filters;
        private readonly int channels;
        private readonly int bandCount;
        private bool updated;

        public Equalizer(ISampleProvider sourceProvider, EqualizerBand[] EqualizerBands)
        {
            this.sourceProvider = sourceProvider;
            this.EqualizerBands = EqualizerBands;
            channels = sourceProvider.WaveFormat.Channels;
            bandCount = EqualizerBands.Length;
            filters = new BiQuadFilter[channels, EqualizerBands.Length];
            CreateFilters();
        }

        private void CreateFilters()
        {
            for (int bandIndex = 0; bandIndex < bandCount; bandIndex++)
            {
                var band = EqualizerBands[bandIndex];
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

        public int Read(float[] buffer, int offset = 0, int count = 0)
        {
            int samplesRead = 0;
            if (buffer == null) { return 0; }
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

    public partial class Player
    {
        /// <summary> list of equalizer EqualizerBands </summary>
        private EqualizerBand[] EqualizerBands;
        /// <summary> Maximum negative gain on an equalizer band </summary>
        public readonly int MinimumGain = -20;
        /// <summary> Maximum gain on an equalizer band </summary>
        public readonly int MaximumGain = 20;

        /// <summary> Initialize equalizer </summary>
        public void InitializeEqualizer()
        {
            EqualizerBands = new EqualizerBand[]
                {
                    new EqualizerBand {Bandwidth = 0.8f, Frequency = 60, Gain = 0},
                    new EqualizerBand {Bandwidth = 0.8f, Frequency = 170, Gain = 0},
                    new EqualizerBand {Bandwidth = 0.8f, Frequency = 310, Gain = 0},
                    new EqualizerBand {Bandwidth = 0.8f, Frequency = 600, Gain = 0},
                    new EqualizerBand {Bandwidth = 0.8f, Frequency = 1000, Gain = 0},
                    new EqualizerBand {Bandwidth = 0.8f, Frequency = 3000, Gain = 0},
                    new EqualizerBand {Bandwidth = 0.8f, Frequency = 6000, Gain = 0},
                    new EqualizerBand {Bandwidth = 0.8f, Frequency = 12000, Gain = 0},
                    new EqualizerBand {Bandwidth = 0.8f, Frequency = 14000, Gain = 0},
                    new EqualizerBand {Bandwidth = 0.8f, Frequency = 16000, Gain = 0}
                };
        }

        /// <summary> update an equalizer band Gain value </summary>
        public void UpdateEqualizer(int Band, float Gain)
        {
            try { EqualizerBands[Band].Gain = Gain; } catch { }
        }

        /// <summary> update an equalizer with List<(int,float)>, int = band indicator, float = band gain </summary>
        public void UpdateEqualizer(List<(int, float)> tab)
        {
            foreach ((int, float) band in tab)
            {
                try { EqualizerBands[band.Item1].Gain = band.Item2; } catch { }
            }
        }
    }

}
