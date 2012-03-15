using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;

namespace ChaoticMind {
    //This class is for all intelligent actors in the game, such as 
    // the main player character, and enemies.
    // (Enemies are coming at a later stage of development)
    class Character : DrawableGameObject {

        protected CharacterType _characterType;

        private Vector2 _locationToFace;
        private Vector2 _locationToMoveToward;

        protected World _world;

        //weapon temp stuff (until a weapon class is made)
        public float _weaponCooldown = 200;
        private DateTimeOffset _lastShotTime = DateTimeOffset.Now;

        public Character(CharacterType characterType, World world, Vector2 startingPosition)
            : base(characterType.SpriteName, characterType.XFrames, characterType.YFrames, characterType.EntitySize, characterType.AnimationDuration, world, startingPosition) {
            _characterType = characterType;

            switch (characterType.ObjectShape) {
                // This method creates a body (has mass, position, rotation),
                // as well as a rectangular fixture, which is just a shape stapled to the body.
                // The fixture is what collides with other objects and impacts how the body moves
                case ObjectShapes.RECTANGLE:
                    _body = BodyFactory.CreateRectangle(world, _characterType.EntitySize, _characterType.EntitySize, _characterType.Density);
                    break;
                case ObjectShapes.CIRCLE:
                    _body = BodyFactory.CreateCircle(world, _characterType.EntitySize / 2, _characterType.Density);
                    break;
            }

            _body.BodyType = BodyType.Dynamic;
            _body.Position = startingPosition;

            _locationToFace = new Vector2(0, 1);

            //bandaid fix, can probably make this globally accessable since we only use one world
            _world = world;
        }

        public override void Update(float deltaTime) {
            decideOnMovementTargets();
            performMovement(deltaTime);

            base.Update(deltaTime);
        }

        //Use input (in the case of a controllable character)
        // or an AI routine to decide what direction this character should try to face, and move
        protected virtual void decideOnMovementTargets() {
        }

        private void performMovement(float deltaTime) {
            Vector2 movement = _locationToMoveToward - _body.Position;

            _body.ApplyLinearImpulse(_characterType.MaxMovementForce * movement * deltaTime);

            //face the correct location
            float angle = (float)(Math.Atan2(-(Position.X - _locationToFace.X), Position.Y - _locationToFace.Y));

            //I had no end of problems trying to get it to work by applying forces so it's set for now
            _body.Rotation = angle;

        }

        protected void Shoot() {
            //will be based on current weapon in the future
            if (ShootPercent() == 1) {
                ProjectileManager.CreateProjectile(_body.Position + Vector2.Normalize(_locationToFace - _body.Position), _locationToFace - _body.Position, 1000, 0.1f, 20.0f, _world);
                _lastShotTime = DateTimeOffset.Now;
            }
        }

        protected float ShootPercent() {
            //will be based on current weapon in the future
            double temp = (DateTimeOffset.Now - _lastShotTime).TotalMilliseconds / _weaponCooldown;
            return temp > 1 ? 1 : (float)temp;
        }

        //The location in global coordinates that this character will attempt to
        // move torward (by strafing if necessary)
        protected Vector2 LocationToMoveToward {
            get { return _locationToMoveToward; }
            set { _locationToMoveToward = value; }
        }

        //The location in global coordinates that this character will attempt to
        // turn torward
        protected Vector2 LocationToFace {
            get { return _locationToFace; }
            set { _locationToFace = value; }
        }
    }
}