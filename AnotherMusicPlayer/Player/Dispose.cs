using System;
using System.Collections.Generic;
using System.Text;

namespace AnotherMusicPlayer
{
    public partial class Player
    {
        /// <summary>
        /// Dispose all used resources.
        /// </summary>
        public static void Dispose()
        {
            StopAll();
            ThreadList = null;
            AudioList = null;
            PlayStatus = null;
            PlayNewPositions = null;

            PlayList.Clear();
        }
    }
}
