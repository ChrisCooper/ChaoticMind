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
    class SoundEffects {
        public static ContentManager SharedContentManager; //Set at execution in ChaoticMindGame init

        private Dictionary<String, SoundEffect> _sEffects = new Dictionary<String, SoundEffect>();

        public SoundEffects() {
            //Enumerate and load contents of Music resources folder
            DirectoryInfo dir = new DirectoryInfo(SharedContentManager.RootDirectory + "/SoundEffects");
            if (!dir.Exists)
                throw new DirectoryNotFoundException();

            FileInfo[] files = dir.GetFiles("*.wma"); //This may need to be changed, depends on the output of the SoundEffect content processor
            foreach (FileInfo file in files) {
                string key = Path.GetFileNameWithoutExtension(file.Name);
                _sEffects[key] = SharedContentManager.Load<SoundEffect>("SoundEffects/" + key);
            }
        }

        public Dictionary<String, SoundEffect> LoadedSoundEffects {
            get { return _sEffects; }
            set { }
        }

    }
}
