using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoticMind {
    class ParticleType {

        public static ParticleType ParasiteDeath = new ParticleType();
        public static ParticleType SwarmerDeath = new ParticleType();
        public static ParticleType AssaultRifleButtleDeath = new ParticleType();

        SpriteAnimationSequence _spriteAnimationSequence;
        float _animationDuration;
        private float _entitySize;

        public static void Initialize() {
            //Parasite Death
            ParasiteDeath._spriteAnimationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence("Enemies/ParasiteDeath", 12, 1, CharacterType.Parasite.Sprite.EntitySize);
            ParasiteDeath._animationDuration = 1.0f;
            ParasiteDeath._entitySize = CharacterType.Parasite.Sprite.EntitySize;
            ParasiteDeath.Lifespan = 4.0f;

            //Parasite Death
            SwarmerDeath._spriteAnimationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence("Enemies/SwarmerDeath", 4, 1, CharacterType.Swarmer.Sprite.EntitySize);
            SwarmerDeath._animationDuration = 0.5f;
            SwarmerDeath._entitySize = CharacterType.Swarmer.Sprite.EntitySize;
            SwarmerDeath.Lifespan = 4.0f;

            //Assault Rifle Bullet Death
            AssaultRifleButtleDeath._entitySize = ProjectileType.AssaultRifleBullet.Animation.EntitySize;
            AssaultRifleButtleDeath._spriteAnimationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence("Projectiles/AssaultRifleBulletDeath", 3, 1, AssaultRifleButtleDeath._entitySize);
            AssaultRifleButtleDeath._animationDuration = 0.5f;
            AssaultRifleButtleDeath.Lifespan = AssaultRifleButtleDeath._animationDuration;
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
