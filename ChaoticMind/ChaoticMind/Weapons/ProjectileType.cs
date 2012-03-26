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
        AnimatedSprite _animation;

        public static ProjectileType AssaultRifleBullet = new ProjectileType();

        static ProjectileType() {
            //AssaultRifle
            AssaultRifleBullet._damage = 3.0f;
            AssaultRifleBullet._speed = 20.0f;
            AssaultRifleBullet._range = 50.0f;
            AssaultRifleBullet._radius = 0.125f;
            AssaultRifleBullet._animation = new AnimatedSprite("Projectiles/AssaultRifleBullet", 1, 1, AssaultRifleBullet._radius * 2, 1.0f);
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

        internal AnimatedSprite Animation {
            get { return _animation; }
        }
    }
}
