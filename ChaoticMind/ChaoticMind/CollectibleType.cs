using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoticMind {

    class CollectibleType {

        float _radius;
        AnimatedSprite _animation;

        public static CollectibleType Objective = new CollectibleType();

        static CollectibleType() {
            //Objective
            Objective._radius = 1.5f;
            Objective._animation = new AnimatedSprite("Projectiles/AssaultRifleBullet", 1, 1, Objective._radius * 2, 1.0f);
        }

        public float Radius {
            get { return _radius; }
        }

        internal AnimatedSprite Animation {
            get { return _animation; }
        }
    }
}
