using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoticMind {
    class ParticleType {

        public static ParticleType ParasiteDeath = new ParticleType();
        public static ParticleType SwarmerDeath = new ParticleType();
        public static ParticleType AssaultRifleButtleDeath = new ParticleType();
        public static ParticleType SwarmerAttack = new ParticleType();

        SpriteAnimationSequence _spriteAnimationSequence;
        float _animationDuration;
        private float _entitySize;

        public static void Initialize() {
            //Parasite Death
            ParasiteDeath._spriteAnimationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence("Enemies/ParasiteDeath", 5, 1, CharacterType.Parasite.VisibleEntitySize);
            ParasiteDeath._animationDuration = 0.4f;
            ParasiteDeath._entitySize = CharacterType.Parasite.VisibleEntitySize;
            ParasiteDeath.Lifespan = 4.0f;

            //Swarmer Death
            SwarmerDeath._spriteAnimationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence("Enemies/SwarmerDeath", 4, 1, CharacterType.Swarmer.VisibleEntitySize);
            SwarmerDeath._animationDuration = 0.5f;
            SwarmerDeath._entitySize = CharacterType.Swarmer.VisibleEntitySize;
            SwarmerDeath.Lifespan = 4.0f;

            //Assault Rifle Bullet Death
            AssaultRifleButtleDeath._entitySize = ProjectileType.AssaultRifleBullet.VisibleEntitySize *1.5f;
            AssaultRifleButtleDeath._spriteAnimationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence("Projectiles/AssaultRifleBulletDeath", 3, 1, AssaultRifleButtleDeath._entitySize);
            AssaultRifleButtleDeath._animationDuration = 0.25f;
            AssaultRifleButtleDeath.Lifespan = AssaultRifleButtleDeath._animationDuration;

            //Swarmer Attack
            SwarmerAttack._entitySize = Swarmer.AttackRange/10.0f;
            SwarmerAttack._spriteAnimationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence("Projectiles/SwarmerBolt", 1, 1, SwarmerAttack._entitySize);
            SwarmerAttack._animationDuration = 0.5f;
            SwarmerAttack.Lifespan = 1.0f;
        }

        public SpriteAnimationSequence SpriteAnimationSequence {
            get { return _spriteAnimationSequence; }
        }

        public float AnimationDuration {
            get { return _animationDuration; }
        }

        public float EntitySize { get { return _entitySize; }}

        public float Lifespan { get; set; }
    }
}
