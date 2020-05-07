using System;
using System.Windows;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace MediaPlayer
{
    public class MediaPositionChangedEventParams { public long Position = 0; public long Duration = 0; }
    public class MediaLengthChangedEventParams { public long Duration = 0; }
    public partial class Player2
    {
        public delegate void OnPositionChanged(object sender, MediaPositionChangedEventParams e);
        public event OnPositionChanged PositionChanged;

        public delegate void OnLengthChanged(object sender, MediaLengthChangedEventParams e);
        public event OnLengthChanged LengthChanged;

        public delegate void OnPlayStoped(object sender, MediaPositionChangedEventParams e);
        public event OnPlayStoped PlayStoped;
    }
}
