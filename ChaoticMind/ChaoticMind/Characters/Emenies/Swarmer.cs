using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class Swarmer : Enemy {

        const float movementJitteriness = 25.0f;
        const float attackInterval = 1.0f;
        static float _range = 5.0f;

        Timer _attackTimer;

        public Swarmer(Vector2 startingPosition)
            : base(CharacterType.Swarmer, startingPosition) {

                _attackTimer = new Timer(attackInterval, true);
        }

        //Use input (in the case of a controllable character)
        // or an AI routine to decide what direction this character should try to face, and move
        protected override void decideOnMovementTargets() {
            LocationToMoveToward = Player.Instance.FuturePosition + Utilities.randomNormalizedVector() * movementJitteriness;

            //Don't rotate. Just face up.
            LocationToFace = _body.Position + Vector2.UnitY;
        }

        protected override void performTypeUniqueMovements(float deltaTime) {
            _attackTimer.Update(deltaTime);

            if (_attackTimer.isFinished && Vector2.Distance(Player.Instance.Position, Position) <= _range){
                Player.Instance.ApplyDamage(_characterType.MainAttackDamage);
                _attackTimer.Reset();
                ParticleManager.CreateParticle(Position, Utilities.AngleTowards(Position, Player.Instance.Position), ParticleType.SwarmerAttack);
            }
        }

        protected override void DropDeathParticle() {
            ParticleManager.CreateParticle(Position, (float)(Utilities.randomDouble() * Math.PI * 2), _characterType.DeathParticle);
        }

        public static float AttackRange { get { return _range; } }
    }
}
