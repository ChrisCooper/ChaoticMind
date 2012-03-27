using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class Swarmer : Enemy {

        private float jitteriness = 25.0f;

        public Swarmer(Vector2 startingPosition)
            : base(CharacterType.Swarmer, startingPosition) {
        }

        //Use input (in the case of a controllable character)
        // or an AI routine to decide what direction this character should try to face, and move
        protected override void decideOnMovementTargets() {
            LocationToMoveToward = Player.Instance.FuturePosition + Utilities.randomNormalizedVector() * jitteriness;

            //Don't rotate. Just face up.
            LocationToFace = _body.Position + Vector2.UnitY;
        }

        protected override void performTypeUniqueMovements(float deltaTime) {
        }
    }
}
