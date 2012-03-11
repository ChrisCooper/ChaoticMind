using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace ChaoticMind {
    class Player : Character {

        public Player(CharacterType characterType, World world, Vector2 startingPosition)
            : base(characterType, world, startingPosition) {
            _body.LinearDamping = 30;
            _body.AngularDamping = 5;

            //_minimapRepresentation = imagemanager.load("");

        }

        public Body Body {
            get { return _body; }
        }

        //Use input to decide what direction this character should try to face and move
        protected override void decideOnMovementTargets() {
            KeyboardState keyState = Keyboard.GetState();

            LocationToMoveToward = _body.Position;

            if (keyState.IsKeyDown(Keys.A)) {
                LocationToMoveToward -= Vector2.UnitX;
            }
            if (keyState.IsKeyDown(Keys.D)) {
                LocationToMoveToward += Vector2.UnitX;
            }
            if (keyState.IsKeyDown(Keys.W)) {
                LocationToMoveToward -= Vector2.UnitY;
            }
            if (keyState.IsKeyDown(Keys.S)) {
                LocationToMoveToward += Vector2.UnitY;
            }

            //Vector2 mouseLocation = InputManager.MouseWorldPosition;
            LocationToFace = InputManager.MouseWorldPosition;
        }
    }
}
