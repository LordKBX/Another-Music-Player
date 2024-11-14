using System;
using System.Windows;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace AnotherMusicPlayer
{
    /// <summary> Class for event OnPositionChanged or OnPlayStoped </summary>
    public class PlayerPositionChangedEventParams { public long Position = 0; public long duration = 0; }
    /// <summary> Class for event OnLengthChanged </summary>
    public class PlayerLengthChangedEventParams { public long duration = 0; }
    /// <summary> Class for event OnPlaylistChanged </summary>
    public class PlayerPlaylistChangeParams { public string[] playlist = new string[] { }; }
    /// <summary> Class for event OnPlaylistPositionChanged </summary>
    public class PlayerPlaylistPositionChangeParams { public int Position = 0; }

    public partial class Player
    {
        /// <summary> Delegate OnPositionChanged </summary>
        public delegate void OnPositionChanged(PlayerPositionChangedEventParams e);
        /// <summary> Define Event OnPositionChanged </summary>
        public static event OnPositionChanged PositionChanged;

        /// <summary> Delegate OnLengthChanged </summary>
        public delegate void OnLengthChanged(PlayerLengthChangedEventParams e);
        /// <summary> Define Event OnLengthChanged </summary>
        public static event OnLengthChanged LengthChanged;

        /// <summary> Delegate OnPlayStoped </summary>
        public delegate void OnPlayStoped(PlayerPositionChangedEventParams e);
        /// <summary> Define Event OnPlayStoped </summary>
        public static event OnPlayStoped PlayStoped;

        /// <summary> Delegate OnPlaylistChanged </summary>
        public delegate void OnPlaylistChanged(PlayerPlaylistChangeParams e);
        /// <summary> Define Event OnPlaylistChanged </summary>
        public static event OnPlaylistChanged PlaylistChanged;

        /// <summary> Delegate OnPlaylistPositionChanged </summary>
        public delegate void OnPlaylistPositionChanged(PlayerPlaylistPositionChangeParams e);
        /// <summary> Define Event OnPlaylistPositionChanged </summary>
        public static event OnPlaylistPositionChanged PlaylistPositionChanged;
    }
}
