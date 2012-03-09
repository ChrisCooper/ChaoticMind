using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ChaoticMind {
    class SpriteAnimationSequence {
        //The name of the name of the resource to be loaded as this animation.
        String _spriteResource;

        //One texture for all animation frames
        Texture2D _texture;

        //size of the texture (allows for spritesheets with mutiple rows)
        int _xFrames;
        int _yFrames;

        //sections of the spritesheet (length = number of frames)
        Rectangle[] _frameRects;

        //Used to load images, and can be shared accross all sequence instances
        public static ContentManager SharedContentManager;

        //Maps resource names to actual existing animation sequence objects.
        //This is because duplicate sequences don't make sense to have.
        static Dictionary<String, SpriteAnimationSequence> ExisitingSequences = new Dictionary<string, SpriteAnimationSequence>();

        public static SpriteAnimationSequence newOrExistingSpriteAnimationSequence(String spriteResource, int xFrames, int yFrames) {
            SpriteAnimationSequence sequence;
            if (ExisitingSequences.ContainsKey(spriteResource)) {
                sequence = ExisitingSequences[spriteResource];
            }
            else {
                sequence = new SpriteAnimationSequence(spriteResource, xFrames, yFrames);
                ExisitingSequences[spriteResource] = sequence;
            }
            return sequence;
        }

        private SpriteAnimationSequence(String spriteResource, int xFrames, int yFrames) {
            _spriteResource = spriteResource;
            _xFrames = xFrames;
            _yFrames = yFrames;
            LoadContent();
        }

        public void LoadContent() {
            //load the texture
            _texture = SharedContentManager.Load<Texture2D>(_spriteResource);

            //calculate the number of frames and allocates the req # of rectangles
            int xSize = _texture.Width / _xFrames;
            int ySize = _texture.Height / _yFrames;
            _frameRects = new Rectangle[_yFrames * _xFrames];

            for (int row = 0; row < _yFrames; row++) {
                for (int col = 0; col < _xFrames; col++) {
                    _frameRects[row * _xFrames + col] = new Rectangle(col * xSize, (_yFrames - row - 1) * ySize, xSize, ySize);
                }
            }
        }

        public Texture2D Texture {
            get {
                return _texture;
            }
        }

        public Rectangle getFrameRect(int frameIndex) {
            return _frameRects[frameIndex];
        }

        public int NumFrames {
            get {
                return _frameRects.GetLength(0);
            }
        }
    }
}
