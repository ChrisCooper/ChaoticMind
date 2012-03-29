using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoticMind {
    class ParticleType {

        public static ParticleType ParasiteDeath = new ParticleType();
        public static ParticleType SwarmerDeath = new ParticleType();
        public static ParticleType AssaultRifleBulletDeath = new ParticleType();
        public static ParticleType EnergyRifleBulletDeath = new ParticleType();
        public static ParticleType SwarmerAttack = new ParticleType();

        SpriteAnimationSequence _spriteAnimationSequence;
        float _animationDuration;
        private float _entitySize;

        public static void Initialize() {
            //Parasite Death
            ParasiteDeath._spriteAnimationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence("Enemies/ParasiteDeath", 5, 1, CharacterType.Parasite.VisibleEntitySize);
            ParasiteDeath._animationDuration = 0.4f;
            ParasiteDeath.DrawLayer = DrawLayers.LowerParticles;
            ParasiteDeath._entitySize = CharacterType.Parasite.VisibleEntitySize;
            ParasiteDeath.Lifespan = 4.0f;

            //Swarmer Death
            SwarmerDeath._spriteAnimationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence("Enemies/SwarmerDeath", 4, 1, CharacterType.Swarmer.VisibleEntitySize);
            SwarmerDeath._animationDuration = 0.5f;
            SwarmerDeath.DrawLayer = DrawLayers.LowerParticles;
            SwarmerDeath._entitySize = CharacterType.Swarmer.VisibleEntitySize;
            SwarmerDeath.Lifespan = 4.0f;

            //Assault Rifle Bullet Death
            AssaultRifleBulletDeath._entitySize = ProjectileType.AssaultRifleBullet.VisibleEntitySize *1.5f;
            AssaultRifleBulletDeath._spriteAnimationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence("Projectiles/AssaultRifleBulletDeath", 3, 1, AssaultRifleBulletDeath._entitySize);
            AssaultRifleBulletDeath._animationDuration = 0.25f;
            AssaultRifleBulletDeath.DrawLayer = DrawLayers.UpperParticles;
            AssaultRifleBulletDeath.Lifespan = AssaultRifleBulletDeath._animationDuration;

            //Energy Rifle Bullet Death
            EnergyRifleBulletDeath._entitySize = ProjectileType.EnergyShot.VisibleEntitySize * 2f;
            EnergyRifleBulletDeath._spriteAnimationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence("Projectiles/EnergyBallDeath", 4, 1, EnergyRifleBulletDeath._entitySize);
            EnergyRifleBulletDeath._animationDuration = 0.15f;
            EnergyRifleBulletDeath.DrawLayer = DrawLayers.UpperParticles;
            EnergyRifleBulletDeath.Lifespan = EnergyRifleBulletDeath._animationDuration;

            //Swarmer Attack
            SwarmerAttack._entitySize = Swarmer.AttackRange/7f;
            SwarmerAttack._spriteAnimationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence("Projectiles/SwarmerBolt", 4, 1, SwarmerAttack._entitySize);
            SwarmerAttack._animationDuration = 0.3f;
            SwarmerAttack.DrawLayer = DrawLayers.UpperParticles;
            SwarmerAttack.Lifespan = 0.3f;
        }

        public SpriteAnimationSequence SpriteAnimationSequence {
            get { return _spriteAnimationSequence; }
        }

        public float AnimationDuration {
            get { return _animationDuration; }
        }

        public float EntitySize { get { return _entitySize; }}

        public float Lifespan { get; set; }

        public float DrawLayer { get; set; }
    }
}
