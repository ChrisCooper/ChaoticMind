using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class Parasite : Enemy {

        private float lunge_chance = 1.0f / 60.0f / 3f; //once every three seconds
        private float jitteriness = 25.0f;

        bool _isLunging = false;

        public Parasite(Vector2 startingPosition)
            : base(CharacterType.Parasite, startingPosition) {
                _body.OnCollision += new FarseerPhysics.Dynamics.OnCollisionEventHandler(_body_OnCollision);
        }

        bool _body_OnCollision(FarseerPhysics.Dynamics.Fixture fixtureA, FarseerPhysics.Dynamics.Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact) {
            Player player = fixtureB.Body.UserData as Player;

            if (player == null) {
                return true;
            }

            if (_isLunging) {
                _isLunging = false;
                player.ApplyDamage(_characterType.MainAttackDamage);
            }

            return true;
        }

        //Use input (in the case of a controllable character)
        // or an AI routine to decide what direction this character should try to face, and move
        protected override void decideOnMovementTargets() {
            LocationToMoveToward = Player.Instance.FuturePosition + Utilities.randomNormalizedVector() * jitteriness;
            //LocationToMoveToward = Position;
            LocationToFace = Player.Instance.Position;
        }

        protected override void performTypeUniqueMovements(float deltaTime) {
            if (Utilities.randomDouble() < lunge_chance) {
                lunge(Player.Instance.Position);
            }
        }

        private void lunge(Vector2 LocationToMoveToward) {
            _isLunging = true;
            Vector2 direction = LocationToMoveToward - Position;
            direction.Normalize();
            _body.ApplyLinearImpulse(direction * _characterType.MaxMovementForce * 5.0f);
        }
    }
}
