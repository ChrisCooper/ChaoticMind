using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace ChaoticMind
{

    /// <summary>
    /// Plays music from the Music content folder by
    /// enumerates its contents, and maintaining a queue
    /// of music.
    /// Plays and stops through method access, looping if
    /// not stopped before the end of the queue is reached.
    /// 
    /// Currently just very simple PoC
    /// 
    /// TODO:
    /// -Queue/Looping of Queue
    /// -Enumeration of Music
    /// </summary>
    class MusicController
    {
        public static ContentManager SharedContentManager; //Set at execution in ChaoticMindGame init

        internal bool _playing;
        internal List<Song> _songs = new List<Song>(); //XNA provides no way of having your own playlist in the game

        public MusicController()
        {
            String currSongName;
            Song currSong;

            //for each in Music ... TBI
            currSongName = "Music/FieldsOfUtopiaCutDemoLoop"; //temp value
            currSong = SharedContentManager.Load<Song>(currSongName);
            _songs.Add(currSong);



            MediaPlayer.Volume = 0.5f;
            MediaPlayer.IsRepeating = true;

            //Register event handler on MediaStateChanged
            //Need to be able to play the next song after one finishes
            MediaPlayer.MediaStateChanged += new EventHandler<System.EventArgs>(HandleMediaStateChanged);
        }


        ////////////////////////////////////////
        //  Play state control/event methods  //
        ////////////////////////////////////////

        /// <summary>
        /// Starts playing through the queue of music
        /// </summary>
        public void Play()
        {
            

            Song nextSong = _songs[0];
            

            _playing = true;
            MediaPlayer.Play(nextSong);
        }

        /// <summary>
        /// Stops playing through the queue of music
        /// </summary>
        public void Stop()
        {
            _playing = false;
            MediaPlayer.Stop();
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEventArgs"></typeparam>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void HandleMediaStateChanged<TEventArgs>(object sender, TEventArgs e)
        {
            //Am targeting events where song ends.
            //might also need to handle (ignore) events where this class has called stop/play
            //check _playing bool for this.
        }

        
        /////////////////////////////////
        //  _songList control methods  //
        /////////////////////////////////

        /// <summary>
        /// 
        /// </summary>
        public void ClearQueue()
        {
            _songs.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status code: Not 0 is error.</returns>
        public int Enqueue(int id)
        {

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status code: Not 0 is error.</returns>
        public int Enqueue(String id)
        {

            return 0;
        }
    }
}
