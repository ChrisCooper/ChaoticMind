using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;

namespace ChaoticMind {
    class Collectable : DrawableGameObject {

        CollectibleType _collectibleType;

        public Collectable(CollectibleType collectibleType, Vector2 startingPosition)
            : base(startingPosition, collectibleType.AnimationSequence, collectibleType.VisibleEntitySize, collectibleType.AnimationDuration, collectibleType.DrawLayer) {

                _collectibleType = collectibleType;

            //set up the body
            _body = BodyFactory.CreateCircle(Program.DeprecatedObjects.PhysicsWorld, collectibleType.Radius, 0.5f);
            _body.BodyType = BodyType.Kinematic;
            _body.AngularDamping = 0;
            _body.AngularVelocity = 5;
            _body.Position = startingPosition;
            _body.CollisionCategories = Category.Cat3;
            _body.CollidesWith = Category.All & ~Category.Cat2;
            _body.UserData = this;

            _body.OnCollision += new OnCollisionEventHandler(_body_OnCollision);

            _minimapSprite = collectibleType.MiniMapSprite;
        }

        public override void Update(float deltaTime) {
            base.Update(deltaTime);
            if (_collectibleType == CollectibleType.ObjectiveType && MapTile.isOutOfBounds(Position)) {
                Program.DeprecatedObjects.MainPlayer.ApplyDamage(50);
                _shouldBeKilledFlag = true;
                GameState.spawnNewObjective();
            }
        }

        bool _body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact) {
            if (fixtureB.Body.UserData == Program.DeprecatedObjects.MainPlayer) {
                _shouldBeKilledFlag = true;
                if (_collectibleType == CollectibleType.ObjectiveType) {
                    GameState.ObjectiveWasCollected();
                }
            }
            return false;
        }

        public override AnimatedSprite GlowSprite {
            get { return null; }
        }
    }
}
