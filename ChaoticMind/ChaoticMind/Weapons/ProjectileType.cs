using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoticMind {

    class ProjectileType {

        float _damage;
        float _speed;
        float _range;
        float _radius;
        float _density;

        public static ProjectileType AssaultRifleBullet = new ProjectileType();
        public static ProjectileType EnergyShot = new ProjectileType();

        static ProjectileType() {
            //AssaultRifle
            AssaultRifleBullet._damage = 2.0f;
            AssaultRifleBullet._speed = 20.0f;
            AssaultRifleBullet._range = 50.0f;
            AssaultRifleBullet._radius = 0.075f;
            AssaultRifleBullet.VisibleEntitySize = AssaultRifleBullet._radius * 2;
            AssaultRifleBullet._density = 10.0f;
            AssaultRifleBullet.AnimationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence("Projectiles/AssaultRifleBullet", 1, 1, AssaultRifleBullet.VisibleEntitySize);
            AssaultRifleBullet.AnimationDuration = 1.0f;
            AssaultRifleBullet.DrawLayer = DrawLayers.UpperParticles;
            AssaultRifleBullet.DeathParticle = ParticleType.AssaultRifleBulletDeath;

            //EnergyShot
            EnergyShot._damage = 15.0f;
            EnergyShot._speed = 12.0f;
            EnergyShot._range = 25.0f;
            EnergyShot._radius = 0.5f;
            EnergyShot.VisibleEntitySize = EnergyShot._radius * 2;
            EnergyShot._density = 100.0f;
            EnergyShot.AnimationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence("Projectiles/EnergyBall", 4, 1, EnergyShot.VisibleEntitySize);
            EnergyShot.AnimationDuration = 0.15f;
            EnergyShot.GlowEntitySize = EnergyShot.VisibleEntitySize * 2;
            EnergyShot.GlowSpriteSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence("Projectiles/EnergyBallGlow", 4, 1, EnergyShot.GlowEntitySize);
            EnergyShot.DrawLayer = DrawLayers.UpperParticles;
            EnergyShot.DeathParticle = ParticleType.EnergyRifleBulletDeath;
        }

        public float Lifetime {
            get { return _range / _speed; }
        }

        public float Damage {
            get { return _damage; }
        }

        public float Range {
            get { return _range; }
        }

        public float Speed {
            get { return _speed; }
        }

        public float Radius {
            get { return _radius; }
        }

        public float Density {
            get { return _density; }
        }

        public SpriteAnimationSequence AnimationSequence { get; set; }

        public float AnimationDuration { get; set; }

        public float VisibleEntitySize { get; set; }

        public ParticleType DeathParticle { get; set; }

        public float DrawLayer { get; set; }

        public SpriteAnimationSequence GlowSpriteSequence { get; set; }

        public float GlowEntitySize { get; set; }
    }
}
