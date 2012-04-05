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
    /// Enumerates SoundEffects contents and loads for later access.
    /// </summary>
    class SoundEffectManager {
        
        public static ContentManager SharedContentManager; //Set at execution in ChaoticMindGame init

        private Dictionary<String, SoundEffectInstance> _sEffects;

        static SoundEffectManager _mainInstance;

        public static void Initilize(){
            _mainInstance = new SoundEffectManager();
            _mainInstance._sEffects = new Dictionary<String, SoundEffectInstance>();
            _mainInstance._sEffects["shift"] = SharedContentManager.Load<SoundEffect>("SoundEffects/stonescraping").CreateInstance();
            _mainInstance._sEffects["shift"].IsLooped = true;

            //pshhhhh, elegance
            /*
            //Enumerate and load contents of Music resources folder
            DirectoryInfo dir = new DirectoryInfo(SharedContentManager.RootDirectory + "/SoundEffects");
            if (!dir.Exists)
                throw new DirectoryNotFoundException();

            FileInfo[] files = dir.GetFiles("*.wma"); //This may need to be changed, depends on the output of the SoundEffect content processor
            foreach (FileInfo file in files) {
                string key = Path.GetFileNameWithoutExtension(file.Name);
                _sEffects[key] = SharedContentManager.Load<SoundEffect>("SoundEffects/" + key);
            }
            */
        }
        
        //do to all (for pausing game)
        public static void PauseAll() {
            for (int i = 0; i < _mainInstance._sEffects.Count; i++) {
                _mainInstance._sEffects.ElementAt(i).Value.Pause();
            }
        }
        public static void ResumeAll() {
            for (int i = 0; i < _mainInstance._sEffects.Count; i++) {
                _mainInstance._sEffects.ElementAt(i).Value.Resume();
            }
        }
        public static void StopAll() {
            for (int i = 0; i < _mainInstance._sEffects.Count; i++) {
                _mainInstance._sEffects.ElementAt(i).Value.Stop();
            }
        }

        //individual files
        public static void PlaySound(String key) {
            _mainInstance._sEffects[key].Play();
        }
        public static void StopSound(String key) {
            if (_mainInstance._sEffects[key].State == SoundState.Playing) {
                _mainInstance._sEffects[key].Stop();
            }
        }
        public static void PauseSound(String key) {
            if (_mainInstance._sEffects[key].State == SoundState.Playing) {
                _mainInstance._sEffects[key].Pause();
            }
        }
        public static SoundState GetState (String key) {
            return _mainInstance._sEffects[key].State;
        }
    }
}
