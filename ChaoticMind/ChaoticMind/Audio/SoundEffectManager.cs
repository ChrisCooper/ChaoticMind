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

        private Dictionary<String, SoundEffectInstance> _sInstance;
        private Dictionary<String, SoundEffect> _sEffect;

        static SoundEffectManager _mainInstance;

        public static void Initilize(){
            _mainInstance = new SoundEffectManager();

            //instances have features like pausing and resuming (but don't stack)
            //effects just play (but stack on top of each other)
            
            //load instances
            _mainInstance._sInstance = new Dictionary<String, SoundEffectInstance>();
            _mainInstance._sInstance["shift"] = SharedContentManager.Load<SoundEffect>("SoundEffects/stonescraping").CreateInstance();
            _mainInstance._sInstance["shift"].IsLooped = true;
            _mainInstance._sInstance["heartbeat"] = SharedContentManager.Load<SoundEffect>("SoundEffects/heartbeat").CreateInstance();
            _mainInstance._sInstance["heartbeat"].IsLooped = true;

            //load effects
            _mainInstance._sEffect = new Dictionary<String, SoundEffect>();
            _mainInstance._sEffect["pistol"] = SharedContentManager.Load<SoundEffect>("SoundEffects/pistol");
            _mainInstance._sEffect["grenade"] = SharedContentManager.Load<SoundEffect>("SoundEffects/grenade");
            _mainInstance._sEffect["item-collect"] = SharedContentManager.Load<SoundEffect>("SoundEffects/item-collect");
            _mainInstance._sEffect["impact"] = SharedContentManager.Load<SoundEffect>("SoundEffects/impact");
            _mainInstance._sEffect["cinematicboom"] = SharedContentManager.Load<SoundEffect>("SoundEffects/cinematicboom");
            _mainInstance._sEffect["reload"] = SharedContentManager.Load<SoundEffect>("SoundEffects/reload");
        }

        //do to all instances (for pausing game)
        public static void PauseInstances() {
            for (int i = 0; i < _mainInstance._sInstance.Count; i++) {
                SoundEffectInstance temp = _mainInstance._sInstance.ElementAt(i).Value;
                if (temp.State == SoundState.Playing)
                    _mainInstance._sInstance.ElementAt(i).Value.Pause();
            }
        }
        public static void ResumeInstances() {
            for (int i = 0; i < _mainInstance._sInstance.Count; i++) {
                SoundEffectInstance temp = _mainInstance._sInstance.ElementAt(i).Value;
                if (temp.State == SoundState.Paused)
                    _mainInstance._sInstance.ElementAt(i).Value.Resume();
            }
        }
        public static void StopInstances() {
            for (int i = 0; i < _mainInstance._sInstance.Count; i++) {
                _mainInstance._sInstance.ElementAt(i).Value.Stop();
            }
        }

        //individual instances
        public static void PlaySound(String key) {
            _mainInstance._sInstance[key].Play();
        }
        public static void StopSound(String key) {
            if (_mainInstance._sInstance[key].State == SoundState.Playing) {
                _mainInstance._sInstance[key].Stop();
            }
        }
        public static void PauseSound(String key) {
            if (_mainInstance._sInstance[key].State == SoundState.Playing) {
                _mainInstance._sInstance[key].Pause();
            }
        }
        public static SoundState GetSoundState (String key) {
            return _mainInstance._sInstance[key].State;
        }

        //individual effects
        public static void PlayEffect(String key, float volume) {
            _mainInstance._sEffect[key].Play(volume, 0, 0);
        }
    }
}
