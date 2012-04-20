using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class Enemy : Character {

        const int distanceLimitToPlayer = 3;

        public Enemy(CharacterType characterType, Vector2 startingPosition)
            : base(characterType, startingPosition) {
        }

        public override void Update(float deltaTime) {
            Vector2 playerDist = GridCoordinate - Program.Objects.MainPlayer.GridCoordinate;

            if (Math.Abs(playerDist.X) > distanceLimitToPlayer || Math.Abs(playerDist.Y) > distanceLimitToPlayer) {
                _currentHealth = 0;
                return;
            }

            base.Update(deltaTime);
        }

        //Use input (in the case of a controllable character)
        // or an AI routine to decide what direction this character should try to face, and move
        protected override void decideOnMovementTargets() {
            LocationToMoveToward = Program.Objects.MainPlayer.Position + Utilities.randomNormalizedVector() * 50.0f;
            LocationToFace = Program.Objects.MainPlayer.Position;
        }
    }
}
