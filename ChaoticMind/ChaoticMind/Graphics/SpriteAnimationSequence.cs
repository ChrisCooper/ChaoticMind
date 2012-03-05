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
        //The name of the name of the resource to be loaded as this animation.
        String _spriteResource;

        //One texture for all animation frames
        Texture2D _texture;

        //size of the texture (allows for spritesheets with mutiple rows)
        int _xSize;
        int _ySize;

        //sections of the spritesheet (length = number of frames)
        Rectangle[] _frameRects;

        //Used to load images, and can be shared accross all sequence instances
        public static ContentManager SharedContentManager;

        //Maps resource names to actual existing animation sequence objects.
        //This is because duplicate sequences don't make sense to have.
        static Dictionary<String, SpriteAnimationSequence> ExisitingSequences = new Dictionary<string,SpriteAnimationSequence>();

        public static SpriteAnimationSequence newOrExistingSpriteAnimationSequence(String spriteResource, int xSize, int ySize)
        {
            SpriteAnimationSequence sequence;
            if (ExisitingSequences.ContainsKey(spriteResource))
            {
                sequence = ExisitingSequences[spriteResource];
            }
            else
            {
                sequence = new SpriteAnimationSequence(spriteResource, xSize, ySize);
                ExisitingSequences[spriteResource] = sequence;
            }
            return sequence;
        }

        private SpriteAnimationSequence(String spriteResource, int xSize, int ySize)
        {
            _spriteResource = spriteResource;
            _xSize = xSize;
            _ySize = ySize;
            LoadContent();
        }

        public void LoadContent()
        {
            //load the texture
            _texture = SharedContentManager.Load<Texture2D>(_spriteResource);

            //calculate the number of frames and allocates the req # of rectangles
            int framesPerRow = _texture.Width / _xSize;
            int numRows = _texture.Height / _ySize;
            _frameRects = new Rectangle[framesPerRow * numRows];
            
            for (int i = 0 ; i < numRows; i++)
            {
                for (int j = 0; j < framesPerRow; j++)
                {
                    _frameRects[i * framesPerRow + j] = new Rectangle(j * _xSize, (numRows - i - 1) * _ySize, _xSize, _ySize);
                }
            }
        }

        public Texture2D Texture
        {
            get
            {
                return _texture;
            }
        }

        public Rectangle getFrameRect (int frameIndex)
        {
            return _frameRects[frameIndex];
        }

        public int NumFrames
        {
            get
            {
                return _frameRects.GetLength(0);
            }
        }
    }
}
