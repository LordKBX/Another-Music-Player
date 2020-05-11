using System;
using System.Windows;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace AnotherMusicPlayer
{
    /// <summary> Class for event OnPositionChanged or OnPlayStoped </summary>
    public class MediaPositionChangedEventParams { public long Position = 0; public long duration = 0; }
    /// <summary> Class for event OnLengthChanged </summary>
    public class MediaLengthChangedEventParams { public long duration = 0; }

    public partial class Player
    {
        /// <summary> Delegate OnPositionChanged </summary>
        public delegate void OnPositionChanged(object sender, MediaPositionChangedEventParams e);
        /// <summary> Define Event OnPositionChanged </summary>
        public event OnPositionChanged PositionChanged;

        /// <summary> Delegate OnLengthChanged </summary>
        public delegate void OnLengthChanged(object sender, MediaLengthChangedEventParams e);
        /// <summary> Define Event OnLengthChanged </summary>
        public event OnLengthChanged LengthChanged;

        /// <summary> Delegate OnPlayStoped </summary>
        public delegate void OnPlayStoped(object sender, MediaPositionChangedEventParams e);
        /// <summary> Define Event OnPlayStoped </summary>
        public event OnPlayStoped PlayStoped;
    }
}
