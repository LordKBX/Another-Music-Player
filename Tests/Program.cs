using LibVLCSharp;
using LibVLCSharp.Shared;
using LibVLCSharp.Shared.MediaPlayerElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Tests
{
    internal class Program
    {
        private static LibVLC _libVLC;
        private static MediaPlayer _mp;
        private const long OFFSET = 5000;


        public static void Main(string[] args)
        {
            Core.Initialize();

            _libVLC = new LibVLC();
            _mp = new MediaPlayer(_libVLC);

            using (var media = new Media(_libVLC, new Uri("D:\\Music\\$Films\\48 Main Theme (From _A Fistful of Dollars_).mp3"), ":no-video"))
                _mp.Media = media;

            // subscribe to libvlc playback events
            _mp.TimeChanged += TimeChanged;
            _mp.PositionChanged += PositionChanged;
            _mp.LengthChanged += LengthChanged;
            _mp.EndReached += EndReached;
            _mp.Playing += Playing;
            _mp.Paused += Paused;

            // subscribe to UI app events for seeking.

            //MessagingCenter.Subscribe<string>(MessengerKeys.App, MessengerKeys.Rewind, vm =>
            //{
            //    Debug.WriteLine("Rewind");
            //    _mp.Time -= OFFSET;
            //});

            //MessagingCenter.Subscribe<string>(MessengerKeys.App, MessengerKeys.Forward, vm =>
            //{
            //    Debug.WriteLine("Forward");
            //    _mp.Time += OFFSET;
            //});

            _mp.Play();

            Console.WriteLine("Hello, World!");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new UI());


        }

        public static void Play(bool play)
        {
            if (play)
                _mp.Play();
            else _mp.Pause();
        }

        // when the libvlc mediaplayer events fire, publish an event with the MessagingCenter

        private static void PositionChanged(object sender, MediaPlayerPositionChangedEventArgs e){
            //MessagingCenter.Send(MessengerKeys.App, MessengerKeys.Position, e.Position);
        }

        private static void Paused(object sender, System.EventArgs e) { 
            //MessagingCenter.Send(MessengerKeys.App, MessengerKeys.Play, false);
        }

        private static void Playing(object sender, System.EventArgs e) {
            //MessagingCenter.Send(MessengerKeys.App, MessengerKeys.Play, true);
        }

        private static void EndReached(object sender, System.EventArgs e) { 
            //MessagingCenter.Send(MessengerKeys.App, MessengerKeys.EndReached);
        }

        private static void LengthChanged(object sender, MediaPlayerLengthChangedEventArgs e) {
            //MessagingCenter.Send(MessengerKeys.App, MessengerKeys.Length, e.Length);
        }

        private static void TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e) {
            //MessagingCenter.Send(MessengerKeys.App, MessengerKeys.Time, e.Time);
        }

    }
}
