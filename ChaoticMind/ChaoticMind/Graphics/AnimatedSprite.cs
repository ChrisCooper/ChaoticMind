using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticMind
{
    /// <summary>
    /// This serves to provide actual instances of an animations to be tied to specific instances of game objects.
    /// It does not contain image data, but rather, it references a specific set of images stored in a SpriteAnimationSequence.
    /// This class keeps track of its own framerate, current frame, and uses this info to request the correct image
    /// from the SpriteAnimationSequence.
    /// </summary>
    class AnimatedSprite
    {
        // contains th actual image data 
        SpriteAnimationSequence _animationSequence;

        // Total time the animation should take to play.
        // Time per frame is _animationDuration / numberOfFrames.
        float _animationDuration;

        //The time that has passed since the animation started
        float _elapsedTime = 0.0f;

        // Keeps track of what frame of the animation we are on
        int _currentFrameIndex = 0;

        public AnimatedSprite(String resourcePrefix, int numFrames, float animationDuration)
        {
            _animationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence(resourcePrefix, numFrames);
            _animationDuration = animationDuration;
        }

        public void Update(float deltaTime)
        {
            //Update the elapsed time, and set the _currentFrameIndex accordingly
            _elapsedTime += deltaTime;
            
            _currentFrameIndex = (int)((_elapsedTime / _animationDuration) * _animationSequence.NumFrames) % _animationSequence.NumFrames;
        }

        public Texture2D CurrentTexture
        {
            get
            {
                return _animationSequence.getTexture(_currentFrameIndex);
            }
        }
        public Vector2 CurrentTextureOrigin
        {
            get { return new Vector2(CurrentTexture.Bounds.Center.X, CurrentTexture.Bounds.Center.Y); }
        }
    }
}
