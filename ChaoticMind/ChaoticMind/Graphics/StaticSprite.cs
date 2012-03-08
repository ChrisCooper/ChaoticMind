using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoticMind {
    class StaticSprite : AnimatedSprite {
        public StaticSprite(String resourcePrefix)
            : base(resourcePrefix, 1, 1, 1) {
        }
    }
}
