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
        AnimatedSprite _animation;

        public static ProjectileType AssaultRifleBullet = new ProjectileType();

        static ProjectileType() {
            //AssaultRifle
            AssaultRifleBullet._damage = 3.0f;
            AssaultRifleBullet._speed = 20.0f;
            AssaultRifleBullet._range = 50.0f;
            AssaultRifleBullet._radius = 0.075f;
            AssaultRifleBullet.VisibleEntitySize = AssaultRifleBullet._radius * 2;
            AssaultRifleBullet._density = 10.0f;
            AssaultRifleBullet.AnimationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence("Projectiles/AssaultRifleBullet", 1, 1, AssaultRifleBullet.VisibleEntitySize);
            AssaultRifleBullet.AnimationDuration = 1.0f;
            AssaultRifleBullet.DeathParticle = ParticleType.AssaultRifleButtleDeath;
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
    }
}
