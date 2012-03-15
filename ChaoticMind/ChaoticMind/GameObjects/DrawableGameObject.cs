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

        public DrawableGameObject(String spriteResource, int xFrames, int yFrames, float entitySize, float animationDuration, World world, Vector2 startingPosition)
            : base(world, startingPosition) {
            _sprite = new AnimatedSprite(spriteResource, xFrames, yFrames, entitySize, animationDuration);
        }

        public DrawableGameObject(World world, Vector2 startingPosition)
            : base(world, startingPosition) {
        }

        public override void Update(float deltaTime) {
            base.Update(deltaTime);

            _sprite.Update(deltaTime);
        }

        public Texture2D Texture {
            get {
                return _sprite.Texture;
            }
        }

        public Rectangle CurrentTextureBounds {
            get {
                return _sprite.CurrentTextureBounds;
            }
        }

        public Vector2 CurrentTextureOrigin {
            get {
                return _sprite.CurrentTextureOrigin;
            }
        }

        public float PixelsPerMeter {
            get {
                return _sprite.PixelsPerMeter;
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

        //returns the index of the map array the object is currently in
        public virtual Vector2 MapTileIndex {
            get { return new Vector2((float)Math.Floor((_body.Position.X + MapTile.TileSideLength / 2.0f) / MapTile.TileSideLength), (float)Math.Floor((_body.Position.Y + MapTile.TileSideLength / 2.0f) / MapTile.TileSideLength)); }
        }
    }
}
