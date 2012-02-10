using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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

        //Whether the animation will restart once it reaches the last frame;
        bool _shouldRepeatAnimation;

        //The time that has passed since the animation started
        float _elapsedTime = 0.0f;

        // Keeps track of what frame of the animation we are on
        int _currentFrameIndex = 0;

        public AnimatedSprite(SpriteAnimationSequence animationSequence, float animationDuration, bool shouldRepeat)
        {
            _animationSequence = animationSequence;
            _animationDuration = animationDuration;
            _shouldRepeatAnimation = shouldRepeat;
        }

        public void update(GameTime gameTime)
        {
            //Update the elapsed time, and set the _currentFrameIndex accordingly
            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (_elapsedTime > _animationDuration) {
                _elapsedTime -= _animationDuration;
            }
            _currentFrameIndex = (int)((_elapsedTime / _animationDuration) * _animationSequence.NumFrames);
        }
    }
}
