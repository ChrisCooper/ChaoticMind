using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoticMind {

    class CollectibleType {

        float _radius;
        AnimatedSprite _sprite;
        AnimatedSprite _miniMapSprite;

        public static CollectibleType ObjectiveType = new CollectibleType();

        static CollectibleType() {
            //ObjectiveType
            ObjectiveType._radius = 2f;
            ObjectiveType._sprite = new AnimatedSprite("TestImages/Box", 17, 1, ObjectiveType._radius, 3.0f);
            ObjectiveType._miniMapSprite = new AnimatedSprite("Minimap/CollectableMinimap", 1,1, MapTile.TileSideLength / 2, 1.0f);
        }

        public float Radius {
            get { return _radius; }
        }

        internal AnimatedSprite Sprite {
            get { return _sprite; }
        }

        internal AnimatedSprite MiniMapSprite {
            get { return _miniMapSprite; }
        }
    }
}
