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
    class Character : DrawableGameObject, IDamageable, IGameObject {

        protected CharacterType _characterType;

        Vector2 _locationToFace;
        Vector2 _locationToMoveToward;
        float _futurePositionInterval = 0.5f; //seconds in the future that the position is estimated

        //Outside board stuff
        const int OutsideBoardDamageAmount = 10;
        Timer _OutsideBoardDamageTimer;
        float _outsideBoardDamageInterval = 0.5f;

        //current weapon
        protected List<Weapon> _weapons;
        int _currentWeaponIndex;

        //health stuff
        protected float _maxHealth;
        protected float _currentHealth;

        public Character(CharacterType characterType, Vector2 startingPosition)
            : base(startingPosition, characterType.SpriteAnimationSequence, characterType.VisibleEntitySize, characterType.AnimationDuration, characterType.DrawLayer) {
            _characterType = characterType;

            switch (characterType.ObjectShape) {
                case ObjectShapes.RECTANGLE:
                    _body = BodyFactory.CreateRectangle(Program.SharedGame.MainWorld, _characterType.PhysicalEntitySize, _characterType.PhysicalEntitySize, _characterType.Density);
                    break;
                case ObjectShapes.CIRCLE:
                    _body = BodyFactory.CreateCircle(Program.SharedGame.MainWorld, _characterType.PhysicalEntitySize / 2, _characterType.Density);
                    break;
            }

            _body.BodyType = BodyType.Dynamic;
            _body.LinearDamping = characterType.LinearDampening;
            _body.Friction = 0.0f;
            _body.Position = startingPosition;
            _body.UserData = this;

            _maxHealth = _currentHealth = _characterType.Health;

            _locationToFace = new Vector2(0, 1);

            _minimapSprite = characterType.MinimapSprite;

            //init OOB timer
            _OutsideBoardDamageTimer = new Timer(_outsideBoardDamageInterval);
        }

        public override void Update(float deltaTime) {
            _OutsideBoardDamageTimer.Update(deltaTime);
            decideOnMovementTargets();
            performTypeUniqueMovements(deltaTime);
            performMovement(deltaTime);


            if (HasWeapons) {
                CurrentWeapon.update(deltaTime);
            }

            //damage idiots who go outside the map 
            if (_OutsideBoardDamageTimer.isFinished && MapTile.isOutOfBounds(_body.Position)) {
                ApplyDamage(OutsideBoardDamageAmount);
                _OutsideBoardDamageTimer.Reset();
            }

            base.Update(deltaTime);
        }

        public override void WasKilled() {
            if (_characterType.DeathParticle != null) {
                DropDeathParticle();
            }
            WasCleared();
        }

        protected virtual void DropDeathParticle() {
            Program.Objects.Particles.Add(new Particle(Position, Rotation, _characterType.DeathParticle));
        }

        //for e.g. ranged units move back if too close, parasites who lunge, etc.
        protected virtual void performTypeUniqueMovements(float deltaTime) {
        }

        //Use input (in the case of a controllable character)
        // or an AI routine to decide what direction this character should try to face, and move
        protected virtual void decideOnMovementTargets() {
        }

        private void performMovement(float deltaTime) {
            Vector2 movement = (_locationToMoveToward - _body.Position);
            //Kills game? wtf??
            if (movement != Vector2.Zero) {
                movement.Normalize();
            }

            _body.ApplyLinearImpulse(_characterType.MaxMovementForce * movement * deltaTime);

            //face the correct location
            float angle = Utilities.AngleTowards(Position, _locationToFace);

            //I had no end of problems trying to get it to work by applying forces so it's set for now
            _body.Rotation = angle;

        }

        //hand off the reloading/shooting stuff to the currently equipped weapon
        protected void Reload() {
            if (HasWeapons) {
                CurrentWeapon.Reload();
            }
        }
        protected void Shoot() {
            if (HasWeapons) {
                CurrentWeapon.Shoot(_body.Position + Vector2.Normalize(_locationToFace - _body.Position) * (EntitySize / 1.5f), _locationToFace - _body.Position);
            }
        }

        public bool HasWeapons {
            get { return _weapons != null; }
        }

        public Weapon CurrentWeapon {
            get { return _weapons[_currentWeaponIndex]; }
        }


        protected void GoToNextWeapon() {
            _currentWeaponIndex = (_currentWeaponIndex + 1) % _weapons.Count;
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

        public override bool ShouldBeKilled {
            get { return _currentHealth <= 0; }
        }

        //
        public virtual void ApplyDamage(float amount) {
            _currentHealth -= amount;
            _currentHealth = Math.Max(0, Math.Min(_currentHealth, _maxHealth));
        }

        public float CurrentHealth {
            get { return _currentHealth; }
        }
        public float MaxHealth {
            get { return _maxHealth; }
        }
        public float PercentHealth {
            get { return _currentHealth / _maxHealth; }
        }

        public float PhysicalEntitySize { get { return _characterType.PhysicalEntitySize; } }
    }

    internal interface IDamageable {
        void ApplyDamage(float damage);
        float CurrentHealth {
            get;
        }
        float MaxHealth {
            get;
        }
        float PercentHealth {
            get;
        }
    }
}