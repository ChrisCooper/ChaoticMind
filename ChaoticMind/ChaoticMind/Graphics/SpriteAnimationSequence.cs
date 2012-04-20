using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ChaoticMind {
    class SpriteAnimationSequence {

        //One texture for all animation frames
        private Texture2D _texture;

        //the scaling of the sprite
        private float _pixelsPerMeter;

        //sections of the spritesheet (length = number of frames)
        private Rectangle[] _frameRects;

        //Used to load images, and can be shared accross all sequence instances
        public static ContentManager SharedContentManager;

        //Maps resource names to actual existing animation sequence objects.
        //This is because duplicate sequences don't make sense to have.
        static Dictionary<string, SpriteAnimationSequence> ExisitingSequences = new Dictionary<string, SpriteAnimationSequence>();

        public static SpriteAnimationSequence newOrExistingSpriteAnimationSequence(String spriteResource, int xFrames, int yFrames, float entitySize) {
            SpriteAnimationSequence sequence;
            if (ExisitingSequences.ContainsKey(spriteResource)) {
                sequence = ExisitingSequences[spriteResource];
            }
            else {
                sequence = new SpriteAnimationSequence(spriteResource, xFrames, yFrames, entitySize);
                ExisitingSequences[spriteResource] = sequence;
            }
            return sequence;
        }

        private SpriteAnimationSequence(String spriteResource, int xFrames, int yFrames, float entitySize) {
            //load the texture
            _texture = SharedContentManager.Load<Texture2D>(spriteResource);

            //calculate the number of frames and allocates the req # of rectangles
            int xSize = _texture.Width / xFrames;
            int ySize = _texture.Height / yFrames;
            _frameRects = new Rectangle[yFrames * xFrames];

            //compute the rectangles for each frame
            for (int row = 0; row < yFrames; row++) {
                for (int col = 0; col < xFrames; col++) {
                    _frameRects[row * xFrames + col] = new Rectangle(col * xSize, (yFrames - row - 1) * ySize, xSize, ySize);
                }
            }

            //set pixelsPerMeter ratio (no seperate xAddition/yAddition size, square/circular entities only)
            _pixelsPerMeter = xSize / entitySize;
        }

        public Texture2D Texture {
            get {
                return _texture;
            }
        }

        public Rectangle getFrameRect(int frameIndex) {
            return _frameRects[frameIndex];
        }

        public float PixelsPerMeter {
            get { return _pixelsPerMeter; }
        }

        public int NumFrames {
            get {
                return _frameRects.GetLength(0);
            }
        }
    }
}
