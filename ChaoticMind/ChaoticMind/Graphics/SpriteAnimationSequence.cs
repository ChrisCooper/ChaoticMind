using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ChaoticMind
{
    class SpriteAnimationSequence
    {
        //The number of frames in the animation
        int _numFrames;

        //The name of the name of the resource to be loaded as this animation.
        String _spriteResource;

        //One texture for each frame of the animation.
        //Possible TODO: A simpl graphics optimization to make is 
        // to change this class to use sprite sheets instead of individual textures
        // for each frame, but this is not a high-priority change until we have
        // performance issues.
        Texture2D _texture;
        Rectangle[] _frameRects;

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

        private SpriteAnimationSequence(String spriteResource, int numFrames)
        {
            _numFrames = numFrames;
            _spriteResource = spriteResource;
            _frameRects = new Rectangle[numFrames];
            LoadContent();
        }

        public void LoadContent()
        {
            //Given the instance's _resourcePrefix, e.g. "examplePrefix",
            //Loads the textures "examplePrefix0" through "examplePrefixN",
            //where N is  _numFrames-1
            _texture = SharedContentManager.Load<Texture2D>(_spriteResource);

            for (int i = 0; i < _numFrames; i++)
            {
                _frameRects[i] = new Rectangle(i*64, 0, 64, 64);
            }
        }

        public Texture2D Texture
        {
            get
            {
                return _texture;
            }
        }

        public Rectangle getRectangle (int frameIndex)
        {
            return _frameRects[frameIndex];
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
