using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

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
    /// -See if something other than WAV files can be used (prohibitively large for music)
    /// </summary>
    class MusicController
    {
        public static ContentManager SharedContentManager; //Set at execution in ChaoticMindGame init

        internal SoundEffectInstance musicData;

        public MusicController()
        {
            SoundEffect music = SharedContentManager.Load<SoundEffect>("Music/FieldsOfUtopiaCutDemoLoop");
            musicData = music.CreateInstance();
            musicData.IsLooped = true;
        }

        /// <summary>
        /// Starts playing through the queue of music
        /// </summary>
        public void Play()
        {
            musicData.Play();
        }

        /// <summary>
        /// Stops playing through the queue of music
        /// </summary>
        public void Stop()
        {
            musicData.Stop();
        }


        /// <summary>
        /// 
        /// </summary>
        public void ClearQueue()
        {

            return;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status code: Not 0 is error.</returns>
        public int Dequeue(int id)
        {

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status code: Not 0 is error.</returns>
        public int Dequeue(String id)
        {

            return 0;
        }
    }
}
