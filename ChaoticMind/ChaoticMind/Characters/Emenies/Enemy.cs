using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class Enemy : Character {

        public Enemy(CharacterType characterType, Vector2 startingPosition)
            : base(characterType, startingPosition) {
        }

        //Use input (in the case of a controllable character)
        // or an AI routine to decide what direction this character should try to face, and move
        protected override void decideOnMovementTargets() {
            LocationToMoveToward = Player.Instance.Position + Utilities.randomNormalizedVector() * 50.0f;
            LocationToFace = Player.Instance.Position;
        }
    }
}
