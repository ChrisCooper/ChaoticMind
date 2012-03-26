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
        float _futurePositionInterval = 0.5f; //seconds in the future that the position is estimated

        //OOB stuff
        private const int OutsideBoardDamageAmount = 10;
        private int _OutsideBoardDamageTimer;

        //current weapon
        protected Weapon _curWeapon = null;

        //health stuff
        protected int _maxHealth;
        protected int _currentHealth;

        public Character(CharacterType characterType, Vector2 startingPosition)
            : base(characterType.SpriteName, characterType.XFrames, characterType.YFrames, characterType.EntitySize, characterType.AnimationDuration, startingPosition) {
            _characterType = characterType;

            switch (characterType.ObjectShape) {
                case ObjectShapes.RECTANGLE:
                    _body = BodyFactory.CreateRectangle(Program.SharedGame.MainWorld, _characterType.EntitySize, _characterType.EntitySize, _characterType.Density);
                    break;
                case ObjectShapes.CIRCLE:
                    _body = BodyFactory.CreateCircle(Program.SharedGame.MainWorld, _characterType.EntitySize / 2, _characterType.Density);
                    break;
            }

            _body.BodyType = BodyType.Dynamic;
            _body.Position = startingPosition;

            _maxHealth = _currentHealth = _characterType.Health;

            _locationToFace = new Vector2(0, 1);

            //init OOB timer
            _OutsideBoardDamageTimer = TimeDelayManager.InitTimer(0.5f);
        }

        public override void Update(float deltaTime) {
            decideOnMovementTargets();
            performMovement(deltaTime);
            performTypeUniqueMovements(deltaTime);

            //damage idiots who go outside the map 
            if (TimeDelayManager.Finished(_OutsideBoardDamageTimer) &&
                (GridCoordinate.X < 0 || GridCoordinate.X > Program.SharedGame.MapManager.GridDimension ||
                GridCoordinate.Y < 0 || GridCoordinate.Y > Program.SharedGame.MapManager.GridDimension)) {
                ApplyDamage(OutsideBoardDamageAmount);
                TimeDelayManager.RestartTimer(_OutsideBoardDamageTimer);
            }

            base.Update(deltaTime);
        }

        //I.e. ranged units move back if too close, parasites lunge, etc.
        protected virtual void performTypeUniqueMovements(float deltaTime) {
        }

        //Use input (in the case of a controllable character)
        // or an AI routine to decide what direction this character should try to face, and move
        protected virtual void decideOnMovementTargets() {
        }

        private void performMovement(float deltaTime) {
            Vector2 movement = (_locationToMoveToward - _body.Position);
            //Kills game? wtf??
            //movement.Normalize();

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
                _curWeapon.Shoot(_body.Position + Vector2.Normalize(_locationToFace - _body.Position) * (EntitySize / 1.5f), _locationToFace - _body.Position);
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

        public Vector2 FuturePosition {
            get {
                return _body.Position + _body.LinearVelocity * _futurePositionInterval;
            }
        }

        //destroy the object when they die
        public override bool ShouldDieNow() {
            return _currentHealth <= 0;
        }

        //
        public void ApplyDamage(int amount) {
            _currentHealth-= amount;
            _currentHealth = Math.Max(0, Math.Min(_currentHealth, _maxHealth));
        }
    }
}