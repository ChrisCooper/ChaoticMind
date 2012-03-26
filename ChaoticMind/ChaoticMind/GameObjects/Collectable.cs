using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;

namespace ChaoticMind {
    class Collectable : DrawableGameObject {

        const float ENTITY_SIZE = 2f;
        bool _killMe;

        AnimatedSprite _minimapSprite;

        public Collectable(String resource, int xFrames, int yFrames, float animationDuration, Vector2 startingPosition)
            : base(resource, xFrames, yFrames, ENTITY_SIZE, animationDuration, startingPosition) {

            //set up the body
            _body = BodyFactory.CreateRectangle(Program.SharedGame.MainWorld, ENTITY_SIZE, ENTITY_SIZE, 1);
            _body.BodyType = BodyType.Kinematic;
            _body.AngularDamping = 0;
            _body.AngularVelocity = 5;
            _body.Position = startingPosition;
            _body.CollisionCategories = Category.Cat3;
            _body.CollidesWith = Category.All & ~Category.Cat2;
            _body.UserData = this;

            _minimapSprite = new StaticSprite("Minimap/CollectableMinimap", MapTile.TileSideLength / 2);

            _killMe = false;
        }

        public void SetPosition(Vector2 startingPosition) {
            _body.Position = startingPosition;
        }

        public void MarkForDeath() {
            _killMe = true;
        }

        //minimap stuff
        public override AnimatedSprite MapSprite{
            get { return _minimapSprite; }
        }

        public override bool ShouldDieNow() {
            return _killMe;
        }
    }
}
