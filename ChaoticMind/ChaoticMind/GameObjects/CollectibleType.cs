using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoticMind {

    class CollectibleType {

        float _radius;
        
        AnimatedSprite _miniMapSprite;

        public static CollectibleType ObjectiveType = new CollectibleType();

        static CollectibleType() {
            //ObjectiveType
            ObjectiveType._radius = 2f;
            ObjectiveType.VisibleEntitySize = ObjectiveType._radius;
            ObjectiveType.AnimationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence("TestImages/Box", 17, 1, ObjectiveType.VisibleEntitySize);
            ObjectiveType.AnimationDuration = 3.0f;
            ObjectiveType._miniMapSprite = new AnimatedSprite("Minimap/CollectableMinimap", 1,1, MapTile.TileSideLength / 2, 1.0f);
        }

        public float Radius {
            get { return _radius; }
        }

        internal SpriteAnimationSequence AnimationSequence { get; set; }

        public float AnimationDuration { get; set; }

        public float VisibleEntitySize { get; set; }

        public AnimatedSprite MiniMapSprite { get { return _miniMapSprite; } }
    }
}
