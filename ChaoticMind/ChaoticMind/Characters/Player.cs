using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace ChaoticMind {
    class Player : Character {

        static Player _instance;

        public static Player Instance {
            get { return _instance; }
        }

        public Player(Vector2 startingPosition)
            : base(CharacterType.Player, startingPosition) {

            _curWeapon = new Weapon(WeaponType.AssaultRifle, 5);

            _instance = this;
        }

        //Use input to decide what direction this character should try to face and move
        protected override void decideOnMovementTargets() {

            //movement
            LocationToMoveToward = _body.Position;
            if (InputManager.IsKeyDown(Keys.A)) {
                LocationToMoveToward -= Vector2.UnitX;
            }
            if (InputManager.IsKeyDown(Keys.D)) {
                LocationToMoveToward += Vector2.UnitX;
            }
            if (InputManager.IsKeyDown(Keys.W)) {
                LocationToMoveToward -= Vector2.UnitY;
            }
            if (InputManager.IsKeyDown(Keys.S)) {
                LocationToMoveToward += Vector2.UnitY;
            }

            //shooting
            LocationToFace = InputManager.MouseWorldPosition;
            if (InputManager.IsMouseDown()) {
                Shoot();
            }

            //reload
            if (InputManager.IsKeyClicked(Keys.E)){
                Reload();
            }
        }
    }
}
