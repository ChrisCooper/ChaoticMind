using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace ChaoticMind {

    /// <summary>
    /// Plays music from the Music content folder by
    /// enumerates its contents, and maintaining a queue
    /// of music.
    /// Plays and stops through method access, looping if
    /// not stopped before the end of the queue is reached.
    /// </summary>
    class MusicController {
        public static ContentManager SharedContentManager; //Set at execution in ChaoticMindGame init

        //Used in the event handler to know if we are still playing through the playlist
        //private bool _playing;
        private int _ownMediaStateActionCount;
        
        //XNA provides no way of having your own playlist in the game, so we need to make our own.
        private List<Song> _playlist;
        private int _playlistIndex;

        //All songs in Music are loaded at execution
        private Dictionary<String, Song> _songs = new Dictionary<String, Song>();
        

        public MusicController() {
            _playlist = new List<Song>();
            _playlistIndex = 0;

            _ownMediaStateActionCount = 0;

            //Enumerate and load contents of Music resources folder
            DirectoryInfo dir = new DirectoryInfo(SharedContentManager.RootDirectory + "/Music");
            if (!dir.Exists)
                throw new DirectoryNotFoundException();

            FileInfo[] files = dir.GetFiles("*.wma");
            foreach (FileInfo file in files) {
                string key = Path.GetFileNameWithoutExtension(file.Name);
                _songs[key] = SharedContentManager.Load<Song>("Music/" + key);
            }
            
            MediaPlayer.Volume = 0.2f; //Arbitrary, range 1 to 0 float

            //Register event handler on MediaStateChanged
            //Need to be able to play the next song after one finishes
            MediaPlayer.MediaStateChanged += new EventHandler<System.EventArgs>(HandleMediaStateChanged);
        }


        public List<Song> CurrentPlaylist {
            get { return _playlist; }
            set { }
        }

        public Song CurrentSong {
            get { return _playlist[_playlistIndex]; }
            set { }
        }

        public Dictionary<String, Song> LoadedSongs {
            get { return _songs; }
            set { }
        }


        ////////////////////////////////////////
        //  Play state control/event methods  //
        ////////////////////////////////////////

        /// <summary>
        /// Starts playing through the queue of music
        /// </summary>
        public void Play() {
            Song currentSong = _playlist[_playlistIndex];

            _ownMediaStateActionCount++;
            try {
                MediaPlayer.Play(currentSong);
            }
            catch (InvalidOperationException e) {
                Console.WriteLine("Couldn't play background music (DRM/Codec issue)");
            }
 
            //_playing = true;
        }


        /// <summary>
        /// Stops playing through the queue of music
        /// </summary>
        public void Stop() {
            //_playing = false;
            _ownMediaStateActionCount++;
            MediaPlayer.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        private void NextSong() {

            if (_playlistIndex < _playlist.Count-1) {
                _playlistIndex++;
            }
            else {
                _playlistIndex = 0;
            }

            Song nextSong = _playlist[_playlistIndex];

            //_playing = false;
            _ownMediaStateActionCount += 1;
            MediaPlayer.Stop();
            MediaPlayer.Play(nextSong);
            //_playing = true;
        }

        /// <summary>
        /// Used to play next song when a song ends
        /// </summary>
        /// <typeparam name="TEventArgs"></typeparam>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleMediaStateChanged<TEventArgs>(object sender, TEventArgs e) {
            //Targeting events where song ends.
            //might also need to handle (ignore) events where this class has called stop/play
            //check _playing bool for this.
            //if (_playing) {
            //    NextSong();
            //}

            if (_ownMediaStateActionCount != 0) {
                _ownMediaStateActionCount--;
            } else {
                NextSong();
            }
        }


        /////////////////////////////////
        //  _playlist control methods  //
        /////////////////////////////////

        /// <summary>
        /// Empties the playlist
        /// </summary>
        public void ClearQueue() {
            _playlist.Clear();
            _playlistIndex = -1; //Indicate that the playlist is/was clear
        }


        /// <summary>
        /// Appends an internally loaded song to the playlist
        /// </summary>
        /// <param name="id">string name of song</param>
        public void Enqueue(string id) {
            _playlist.Add(_songs[id]);

            //If the playlist was clear before this Song was added
            if (_playlistIndex == -1) {
                _playlistIndex = 0;
            }
        }
    }
}
