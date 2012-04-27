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

        public Swarmer(GameObjects owner, Vector2 startingPosition)
            : base(owner, CharacterType.SwarmerType, startingPosition) {

                _attackTimer = new Timer(attackInterval, true);
        }

        //Use input (in the case of a controllable character)
        // or an AI routine to decide what direction this character should try to face, and move
        protected override void decideOnMovementTargets() {
            LocationToMoveToward = PathFinder.NextLocationForPathToPlayer(Position, true).ImmediateDestination;
            LocationToMoveToward += Utilities.randomNormalizedVector() * movementJitteriness;

            //Don't rotate. Just face up.
            LocationToFace = _body.Position + Vector2.UnitY;
        }

        protected override void performTypeUniqueMovements(float deltaTime) {
            _attackTimer.Update(deltaTime);

            if (_attackTimer.isFinished && Vector2.Distance(_owner.MainPlayer.Position, Position) <= _range && _owner.MainPlayer.GridCoordinate == GridCoordinate) {
                _owner.MainPlayer.ApplyDamage(_characterType.MainAttackDamage);
                _attackTimer.Reset();
                _owner.Particles.Add(new Particle(Position, Utilities.AngleTowards(Position, _owner.MainPlayer.Position), ParticleType.SwarmerAttack));
            }
        }

        protected override void DropDeathParticle() {
            _owner.Particles.Add(new Particle(Position, (float)(Utilities.randomDouble() * Math.PI * 2), _characterType.DeathParticle));
        }

        public static float AttackRange { get { return _range; } }
    }
}
