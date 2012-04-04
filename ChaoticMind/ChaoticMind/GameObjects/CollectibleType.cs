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
            ObjectiveType._radius = 1f;
            ObjectiveType.VisibleEntitySize = ObjectiveType._radius * 8;
            ObjectiveType.AnimationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence("Collectables/Collectable", 1, 1, ObjectiveType.VisibleEntitySize);
            ObjectiveType.AnimationDuration = 3.0f;
            ObjectiveType.DrawLayer = DrawLayers.Collectibles;
            ObjectiveType._miniMapSprite = new AnimatedSprite("Minimap/CollectableMinimap", 1,1, MapTile.TileSideLength / 2, 1.0f, DrawLayers.HUD_Minimap_important_elements);
        }

        public float Radius {
            get { return _radius; }
        }

        internal SpriteAnimationSequence AnimationSequence { get; set; }

        public float AnimationDuration { get; set; }

        public float VisibleEntitySize { get; set; }

        public AnimatedSprite MiniMapSprite { get { return _miniMapSprite; } }

        public float DrawLayer {
            get;
            set;
        }
    }
}
