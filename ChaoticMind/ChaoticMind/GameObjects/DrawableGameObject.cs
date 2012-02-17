using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace ChaoticMind
{
    class DrawableGameObject : GameObject
    {
        protected AnimatedSprite _sprite;
        float _pixelsPerMeter;

        public DrawableGameObject(String spriteResourcePrefix, int numFrames, float animationDuration, float pixelsPerMeter, World world, Vector2 startingPosition)
            : base(world, startingPosition)
        {
            _sprite = new AnimatedSprite(spriteResourcePrefix, numFrames, animationDuration);
            _pixelsPerMeter = pixelsPerMeter;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            _sprite.Update(deltaTime);
        }

        public Texture2D CurrentTexture
        {
            get
            {
                return _sprite.CurrentTexture;
            }
        }

        public Vector2 CurrentTextureOrigin
        {
            get
            {
                return _sprite.CurrentTextureOrigin;
            }
        }

        public float PixelsPerMeter
        {
            get
            {
                return _pixelsPerMeter;
            }
        }
    }
}
