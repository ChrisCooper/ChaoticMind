using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace ChaoticMind {
    abstract class DrawableGameObject : GameObject, IMiniMapable, IDrawable, IGameObject {
        protected AnimatedSprite _sprite;
        protected AnimatedSprite _minimapSprite;

        public DrawableGameObject(GameObjects owner, String spriteResource, int xFrames, int yFrames, float entitySize, float animationDuration, float drawLayer, Vector2 startingPosition)
            : base(owner, startingPosition) {
            _sprite = new AnimatedSprite(spriteResource, xFrames, yFrames, entitySize, animationDuration, drawLayer);
            _sprite.RandomizeFrame();
        }

        public DrawableGameObject(GameObjects owner, Vector2 startingPosition, SpriteAnimationSequence spriteSequence, float entitySize, float animationDuration, float drawLayer)
            : base(owner, startingPosition) {
            if (spriteSequence != null) {
                _sprite = new AnimatedSprite(spriteSequence, entitySize, animationDuration, drawLayer);
                _sprite.RandomizeFrame();
            }
        }

        public DrawableGameObject(GameObjects owner, Vector2 startingPosition)
            : base(owner, startingPosition) {
        }

        public override void Update(float deltaTime) {
            //draw the object
            if (_sprite != null)
                _sprite.Update(deltaTime);

            base.Update(deltaTime);
        }

        //minimap stuff
        public virtual AnimatedSprite MapSprite {
            get { return _minimapSprite; }
        }

        public AnimatedSprite Sprite {
            get { return _sprite; }
        }

        public float EntitySize {
            get {
                if (_sprite != null)
                    return _sprite.EntitySize;
                return 0.1f;
            }
        }

        public float Alpha {
            get { return 1.0f; }
        }

        public virtual Vector2 MapPosition {
            get { return Position; }
        }
        public virtual float MapRotation {
            get { return 0; }
        }

        public virtual float MapDrawLayer {
            get { return this._minimapSprite.DrawLayer; }
        }

        public float DrawLayer {
            get { return _sprite.DrawLayer; }
        }

        public virtual AnimatedSprite GlowSprite {
            get;
            set;
        }
    }
}
