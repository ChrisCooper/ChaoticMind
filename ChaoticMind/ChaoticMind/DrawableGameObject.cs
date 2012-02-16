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
        AnimatedSprite _sprite;

        public DrawableGameObject(String spriteResourcePrefix, int numFrames, World world, Vector2 startingPosition)
            : base(world, startingPosition)
        {
            _sprite = new AnimatedSprite(spriteResourcePrefix, numFrames, 1.0f);
        }

        public new void Update(float deltaTime)
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
    }
}
