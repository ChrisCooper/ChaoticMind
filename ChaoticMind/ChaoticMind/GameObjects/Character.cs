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

        //current weapon
        protected Weapon _curWeapon = null;

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

        //hand off the reloading/shooting stuff to the currently equipped weapon
        protected void Reload() {
            if (_curWeapon != null) {
                _curWeapon.Reload();
            }
        }
        protected void Shoot() {
            if (_curWeapon != null) {
                _curWeapon.Shoot(_body.Position + Vector2.Normalize(_locationToFace - _body.Position) * (_sprite.EntitySize / 1.5f), _locationToFace - _body.Position);
            }
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