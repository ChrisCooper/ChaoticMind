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
    abstract class DrawableGameObject : GameObject
    {
        protected AnimatedSprite _sprite;

        public DrawableGameObject(String spriteResource, int xFrames, int yFrames, float entitySize, float animationDuration, World world, Vector2 startingPosition)
            : base(world, startingPosition)
        {
            _sprite = new AnimatedSprite(spriteResource, xFrames, yFrames, entitySize, animationDuration);
        }

        public DrawableGameObject(World world, Vector2 startingPosition)
            : base(world, startingPosition)
        {
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            _sprite.Update(deltaTime);
        }

        public Texture2D Texture
        {
            get
            {
                return _sprite.Texture;
            }
        }

        public Rectangle CurrentTextureBounds
        {
            get
            {
                return _sprite.CurrentTextureBounds;
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
                return _sprite.PixelsPerMeter;
            }
        }
    }
}
