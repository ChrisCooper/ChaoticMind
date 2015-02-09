﻿using System;
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
            //ParasiteType Death
            ParasiteDeath._spriteAnimationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence("Enemies/ParasiteDeath", 4, 1, CharacterType.ParasiteType.VisibleEntitySize);
            ParasiteDeath._animationDuration = 0.3f;
            ParasiteDeath.DrawLayer = DrawLayers.GameElements.LowerParticles;
            ParasiteDeath._entitySize = CharacterType.ParasiteType.VisibleEntitySize;
            ParasiteDeath.Lifespan = 4.0f;

            //SwarmerType Death
            SwarmerDeath._spriteAnimationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence("Enemies/SwarmerDeath", 4, 1, CharacterType.SwarmerType.VisibleEntitySize);
            SwarmerDeath._animationDuration = 0.3f;
            SwarmerDeath.DrawLayer = DrawLayers.GameElements.LowerParticles;
            SwarmerDeath._entitySize = CharacterType.SwarmerType.VisibleEntitySize;
            SwarmerDeath.Lifespan = 4.0f;

            //Assault Rifle Bullet Death
            AssaultRifleBulletDeath._entitySize = ProjectileType.AssaultRifleBullet.VisibleEntitySize *1.5f;
            AssaultRifleBulletDeath._spriteAnimationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence("Projectiles/AssaultRifleBulletDeath", 3, 1, AssaultRifleBulletDeath._entitySize);
            AssaultRifleBulletDeath._animationDuration = 0.25f;
            AssaultRifleBulletDeath.DrawLayer = DrawLayers.GameElements.UpperParticles;
            AssaultRifleBulletDeath.Lifespan = AssaultRifleBulletDeath._animationDuration;

            //Energy Rifle Bullet Death
            EnergyRifleBulletDeath._entitySize = ProjectileType.EnergyShot.VisibleEntitySize * 2f;
            EnergyRifleBulletDeath._spriteAnimationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence("Projectiles/EnergyBallDeath", 4, 1, EnergyRifleBulletDeath._entitySize);
            EnergyRifleBulletDeath._animationDuration = 0.15f;
            EnergyRifleBulletDeath.GlowSpriteAnimationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence("Projectiles/EnergyBallDeathGlow", 4, 1, EnergyRifleBulletDeath._entitySize);
            EnergyRifleBulletDeath.DrawLayer = DrawLayers.GameElements.UpperParticles;
            EnergyRifleBulletDeath.Lifespan = EnergyRifleBulletDeath._animationDuration;

            //SwarmerType Attack
            SwarmerAttack._entitySize = Swarmer.AttackRange/7f;
            SwarmerAttack._spriteAnimationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence("Projectiles/SwarmerBolt", 4, 1, SwarmerAttack._entitySize);
            SwarmerAttack.GlowSpriteAnimationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence("Projectiles/SwarmerBoltGlow", 4, 1, SwarmerAttack._entitySize);
            SwarmerAttack._animationDuration = 0.3f;
            SwarmerAttack.DrawLayer = DrawLayers.GameElements.UpperParticles;
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

        public ChaoticMind.SpriteAnimationSequence GlowSpriteAnimationSequence { get; set; }
    }
}
