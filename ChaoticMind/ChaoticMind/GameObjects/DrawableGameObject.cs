using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace ChaoticMind {
    abstract class DrawableGameObject : GameObject, IMiniMapable, IDrawable {
        protected AnimatedSprite _sprite;
        protected AnimatedSprite _minimapSprite;

        public DrawableGameObject(String spriteResource, int xFrames, int yFrames, float entitySize, float animationDuration, float drawLayer, Vector2 startingPosition)
            : base(startingPosition) {
            _sprite = new AnimatedSprite(spriteResource, xFrames, yFrames, entitySize, animationDuration, drawLayer);
            _sprite.RandomizeFrame();
        }

        public DrawableGameObject(Vector2 startingPosition, SpriteAnimationSequence spriteSequence, float entitySize, float animationDuration, float drawLayer)
            : base(startingPosition) {
                if (spriteSequence != null) {
                    _sprite = new AnimatedSprite(spriteSequence, entitySize, animationDuration, drawLayer);
                    _sprite.RandomizeFrame();
                }
        }

        public DrawableGameObject(Vector2 startingPosition)
            : base(startingPosition) {
        }

        public override void Update(float deltaTime) {
            //shift the object
            if (!_body.IsDisposed && MapManager.isShifting(_body.Position)) {
                _body.Position += MapManager.shiftAmount() * deltaTime;
            }

            //draw the object
            if (_sprite != null)
                _sprite.Update(deltaTime);

            base.Update(deltaTime);
        }

        //drawing
        public virtual void Draw(Camera mainCamera) {
            mainCamera.Draw(this);
        }
        public virtual void DrawMiniMap(Camera mainCamera) {
            mainCamera.DrawMinimap(this);
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

        //returns the index of the map array the object is currently in
        public virtual Vector2 GridCoordinate {
            get { return MapTile.GridPositionForWorldCoordinates(Position); }
        }
    }
}
