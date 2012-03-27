using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class StaticSprite : AnimatedSprite {

        public StaticSprite(String resourcePrefix, float entitySize)
            : base(resourcePrefix, 1, 1, entitySize, 1) {
        }
    }
}
