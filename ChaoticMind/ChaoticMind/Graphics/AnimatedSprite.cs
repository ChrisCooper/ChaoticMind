using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticMind {
    /// <summary>
    /// This serves to provide actual instances of an animations to be tied to specific instances of game objects.
    /// It does not contain image data, but rather, it references a specific set of images stored in a AnimationSequence.
    /// This class keeps track of its own framerate, current frame, and uses this info to request the correct image
    /// from the AnimationSequence.
    /// </summary>
    class AnimatedSprite {
        // contains the actual image data 
        SpriteAnimationSequence _animationSequence;

        // Total time the animation should take to play.
        // Time per frame is _animationDuration / numberOfFrames.
        float _animationDuration;

        //The time that has passed since the animation started
        float _elapsedTime = 0.0f;

        // Keeps track of what frame of the animation we are on
        int _currentFrameIndex = 0;

        //stores the size of the sprite
        float _entitySize;

        private bool _shouldRepeat;

        public AnimatedSprite(String spriteResource, int xFrames, int yFrames, float entitySize, float animationDuration, float drawLayer) {
            _animationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence(spriteResource, xFrames, yFrames, entitySize);
            _animationDuration = animationDuration;
            _entitySize = entitySize;
            _shouldRepeat = true;
            DrawLayer = drawLayer;
        }

        public AnimatedSprite(SpriteAnimationSequence spriteAnimationSequence, float entitySize, float animationDuration, float drawLayer)
        : this(spriteAnimationSequence, entitySize, animationDuration, drawLayer, true) {
        }

        public AnimatedSprite(SpriteAnimationSequence spriteAnimationSequence, float entitySize, float animationDuration, float drawLayer, bool shouldRepeat) {
            _animationSequence = spriteAnimationSequence;
            this._entitySize = entitySize;
            _animationDuration = animationDuration;
            _shouldRepeat = shouldRepeat;
            DrawLayer = drawLayer;
        }

        public void Update(float deltaTime) {
            //Update the elapsed time, and set the _currentFrameIndex accordingly
            _elapsedTime += deltaTime;

            if (_shouldRepeat) {
                _currentFrameIndex = (int)((_elapsedTime / _animationDuration) * _animationSequence.NumFrames) % _animationSequence.NumFrames;
            }
            else {
                _currentFrameIndex = (int)Math.Min(((_elapsedTime / _animationDuration) * _animationSequence.NumFrames), _animationSequence.NumFrames - 1);
            }
        }

        public float PixelsPerMeter {
            get { return _animationSequence.PixelsPerMeter; }
        }

        public Texture2D Texture {
            get {
                return _animationSequence.Texture;
            }
        }

        public Rectangle CurrentTextureBounds {
            get {
                return _animationSequence.getFrameRect(_currentFrameIndex);
            }
        }
        public Vector2 CurrentTextureOrigin {
            get { return new Vector2(CurrentTextureBounds.Width / 2, CurrentTextureBounds.Height / 2); }
        }

        public float EntitySize {
            get { return _entitySize; }
        }

        public float DrawLayer { get; set; }
    }
}
