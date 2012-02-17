using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ChaoticMind
{
    class SpriteAnimationSequence
    {
        //The number of frames in the animation
        int _numFrames;

        //The prefix for the name of the resources to be loaded as this animation.
        //E.g. if this string is "person", and it has 4 frames, the resources loaded
        // will be "person0", "person1", "person2", and "person3".
        String _resourcePrefix;

        //One texture for each frame of the animation.
        //Possible TODO: A simpl graphics optimization to make is 
        // to change this class to use sprite sheets instead of individual textures
        // for each frame, but this is not a high-priority change until we have
        // performance issues.
        Texture2D[] _frameTextures;

        //Used to load images, and can be shared accross all sequence instances
        public static ContentManager SharedContentManager;

        //Maps resource names to actual existing animation sequence objects.
        //This is because duplicate sequences don't make sense to have.
        static Dictionary<String, SpriteAnimationSequence> ExisitingSequences = new Dictionary<string,SpriteAnimationSequence>();

        public static SpriteAnimationSequence newOrExistingSpriteAnimationSequence(String resourcePrefix, int numFrames)
        {
            SpriteAnimationSequence sequence;
            if (ExisitingSequences.ContainsKey(resourcePrefix)) {
                sequence = ExisitingSequences[resourcePrefix];
                return sequence;
            }
            return new SpriteAnimationSequence(resourcePrefix, numFrames);
        }

        private SpriteAnimationSequence(String resourcePrefix, int numFrames)
        {
            _numFrames = numFrames;
            _resourcePrefix = resourcePrefix;
            _frameTextures = new Texture2D[_numFrames];
            LoadContent();
        }

        public void LoadContent()
        {
            //Given the instance's _resourcePrefix, e.g. "examplePrefix",
            //Loads the textures "examplePrefix0" through "examplePrefixN",
            //where N is  _numFrames-1
            for (int i = 0; i < _numFrames; i++)
            {
                String fileName = _resourcePrefix + i.ToString();
                _frameTextures[i] = SharedContentManager.Load<Texture2D>(fileName);
            }
        }

        public Texture2D getTexture(int frameIndex)
        {
            return _frameTextures[frameIndex];
        }

        public int NumFrames
        {
            get
            {
                return _numFrames;
            }
        }
    }
}
