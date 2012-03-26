﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;

namespace ChaoticMind {
    class Collectable : DrawableGameObject {

        bool _shouldBeRemoved = false;

        public Collectable(CollectibleType collectibleType, Vector2 startingPosition)
            : base(startingPosition, collectibleType.Sprite) {

            //set up the body
            _body = BodyFactory.CreateCircle(Program.SharedGame.MainWorld, collectibleType.Radius, 0.5f);
            _body.BodyType = BodyType.Dynamic;
            _body.AngularDamping = 0;
            _body.AngularVelocity = 5;
            _body.Position = startingPosition;
            _body.CollisionCategories = Category.Cat3;
            _body.CollidesWith = Category.All & ~Category.Cat2;
            _body.UserData = this;

            _minimapSprite = collectibleType.MiniMapSprite;
        }

        public void SetPosition(Vector2 startingPosition) {
            _body.Position = startingPosition;
        }

        public void MarkForDeath() {
            _shouldBeRemoved = true;
        }

        public override bool ShouldDieNow() {
            return _shouldBeRemoved;
        }
    }
}
