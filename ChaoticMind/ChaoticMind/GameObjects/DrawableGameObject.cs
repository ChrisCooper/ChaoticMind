using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace ChaoticMind {
    abstract class DrawableGameObject : GameObject, IMiniMapable {
        protected AnimatedSprite _sprite;

        public DrawableGameObject(String spriteResource, int xFrames, int yFrames, float entitySize, float animationDuration, Vector2 startingPosition)
            : base(startingPosition) {
            _sprite = new AnimatedSprite(spriteResource, xFrames, yFrames, entitySize, animationDuration);
        }

        public DrawableGameObject (Vector2 startingPosition, AnimatedSprite sprite)
            : base(startingPosition) {
            _sprite = sprite;
        }

        public DrawableGameObject(Vector2 startingPosition)
            : base(startingPosition) {
        }

        public override void Update(float deltaTime) {
            base.Update(deltaTime);
            if (_sprite != null)
                _sprite.Update(deltaTime);
        }

        //called when the object is being shifted
        public void Shift(Vector2 deltaPos) {
            if (!_body.IsDisposed) {
                _body.Position += deltaPos;
            }
        }

        public Texture2D Texture {
            get {
                if (_sprite != null)
                    return _sprite.Texture;
                return null;
            }
        }

        public Rectangle CurrentTextureBounds {
            get {
                if (_sprite != null)
                    return _sprite.CurrentTextureBounds;
                return Rectangle.Empty;
            }
        }

        public Vector2 CurrentTextureOrigin {
            get {
                if (_sprite != null)
                    return _sprite.CurrentTextureOrigin;
                return Vector2.Zero;
            }
        }

        public float PixelsPerMeter {
            get {
                if (_sprite != null)
                    return _sprite.PixelsPerMeter;
                return 1;
            }
        }

        public float EntitySize {
            get {
                if (_sprite != null)
                    return _sprite.EntitySize;
                return 0.1f;
            }
        }

        //minimap stuff
        public virtual AnimatedSprite MapSprite {
            get { return null; }
        }
        public virtual Vector2 MapPosition {
            get { return Position; }
        }
        public virtual float MapRotation {
            get { return 0; }
        }
    }
}
