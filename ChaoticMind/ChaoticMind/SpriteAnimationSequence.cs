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
        //Possible TODO: The simplest graphics optimization to make is 
        // to change this class to use sprite sheets instead of individual textures,
        // but this is not a high-priority change until we have performance issues.
        Texture2D[] _frameTextures;

        public SpriteAnimationSequence(String resourcePrefix, int numFrames)
        {
            _numFrames = numFrames;
            _resourcePrefix = resourcePrefix;
            _frameTextures = new Texture2D[_numFrames];
        }

        public void LoadContent(ContentManager contentManager)
        {
            for (int i = 0; i < _numFrames; i++)
            {
                String fileName = _resourcePrefix + i.ToString();
                _frameTextures[i] = contentManager.Load<Texture2D>(fileName);
            }
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
