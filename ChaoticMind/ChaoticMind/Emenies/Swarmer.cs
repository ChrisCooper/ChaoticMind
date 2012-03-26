using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class Swarmer : Character {

        private float lunge_chance = 1.0f/60.0f/3f; //once every three seconds

        private AnimatedSprite _minimapSprite;

        public Swarmer(CharacterType characterType, Vector2 startingPosition)
            : base(characterType, startingPosition) {

                _body.UserData = this;
                _body.LinearDamping = characterType.LinearDampening;

            _minimapSprite = new StaticSprite("Minimap/EnemyMinimap", MapTile.TileSideLength / 5);
        }

        //Use input (in the case of a controllable character)
        // or an AI routine to decide what direction this character should try to face, and move
        protected override void decideOnMovementTargets() {
            LocationToMoveToward = Player.Instance.FuturePosition + Utilities.randomNormalizedVector() * 50.0f;
            LocationToFace = Player.Instance.FuturePosition;
        }

        protected override void performTypeUniqueMovements(float deltaTime) {
            if (Utilities.randomDouble() < lunge_chance) {
                lunge(Player.Instance.Position);
            }
        }

        private void lunge(Vector2 LocationToMoveToward) {
            Vector2 direction = LocationToMoveToward - Position;
            direction.Normalize();
            _body.ApplyLinearImpulse(direction * _characterType.MaxMovementForce * 5.0f);
        }

        public override AnimatedSprite MapSprite {
            get { return _minimapSprite; }
        }
    }
}
