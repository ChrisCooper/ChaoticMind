using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Audio;

namespace ChaoticMind {
    class Player : Character {

        public float SightGridDistance = 1;

        public Player(GameObjects owner, Vector2 startingPosition)
            : base(owner, CharacterType.PlayerType, startingPosition) {

            _weapons = new List<Weapon>();
            _weapons.Add(new Weapon(WeaponType.AssaultRifle, 7, this));
            _weapons.Add(new Weapon(WeaponType.EnergyRifle, 7, this));

            _body.UserData = this;
        }

        //Use input to decide what direction this character should try to face and move
        protected override void decideOnMovementTargets() {
            //movement
            LocationToMoveToward = _body.Position;
            LocationToFace = _owner.MainCamera.screenPointToWorld(InputManager.MouseScreenPosition);
        }

        public override void ApplyDamage(float amount) {
            PainStaticMaker.AddDamage(amount);

            base.ApplyDamage(amount);
        }


        //for e.g. ranged units move back if too close, parasites who lunge, etc.
        protected override void performTypeUniqueMovements(float deltaTime) {

            Vector2 movementVector = Vector2.Zero;

            if (InputManager.IsKeyDown(KeyInput.MOVE_LEFT)) {
                movementVector -= Vector2.UnitX;
            }
            if (InputManager.IsKeyDown(KeyInput.MOVE_RIGHT)) {
                movementVector += Vector2.UnitX;
            }
            if (InputManager.IsKeyDown(KeyInput.MOVE_FORWARD)) {
                movementVector -= Vector2.UnitY;
            }
            if (InputManager.IsKeyDown(KeyInput.MOVE_BACKWARD)) {
                movementVector += Vector2.UnitY;
            }

            if (movementVector != Vector2.Zero) {
                movementVector.Normalize();
            }

            movementVector *= _characterType.MaxMovementForce;
            _body.ApplyLinearImpulse(movementVector * deltaTime);

            //shooting
            if (InputManager.IsMouseDown) {
                Shoot();
            }

            //reload
            if (InputManager.IsKeyClicked(KeyInput.RELOAD)) {
                Reload();
            }

            //Switch weapon
            if (InputManager.IsKeyClicked(KeyInput.CHANGE_WEAPON)) {
                GoToNextWeapon();
            }
        }

        internal void GoToFullHealth() {
            _currentHealth = _maxHealth;
        }
    }
}
