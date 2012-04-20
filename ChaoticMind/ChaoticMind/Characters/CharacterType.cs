using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoticMind {
    enum ObjectShapes {
        CIRCLE,
        RECTANGLE,
    }

    class CharacterType {

        public static CharacterType SillyBoxType;
        public static CharacterType SwarmerType;
        public static CharacterType ParasiteType;
        public static CharacterType PlayerType;

        public static void Initialize() {

            //PlayerType
            PlayerType = new CharacterType();
            PlayerType._physicalEntitySize = 1.0f;
            PlayerType.SpriteAnimationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence("Player", 1, 1, PlayerType._physicalEntitySize * 1.3f);
            PlayerType.AnimationDuration = 1.0f;
            PlayerType.DrawLayer = DrawLayers.GameElements.Characters;
            PlayerType._objectShape = ObjectShapes.CIRCLE;
            PlayerType._density = 1.0f;
            PlayerType._maxTurningTorque = 1.0f;
            PlayerType._maxMovementForce = 80.0f;
            PlayerType._health = 150;
            PlayerType._linearDampening = 10f;
            PlayerType.MinimapSprite = new StaticSprite("Minimap/PlayerMinimap", MapTile.TileSideLength / 2, DrawLayers.HUD.Minimap_important_elements);

            //SillyBoxType
            SillyBoxType = new CharacterType();
            SillyBoxType._physicalEntitySize = 1.0f;
            SillyBoxType.SpriteAnimationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence("TestImages/Box", 17, 1, SillyBoxType._physicalEntitySize);
            SillyBoxType.AnimationDuration = 1.0f;
            SillyBoxType.DrawLayer = DrawLayers.GameElements.Characters;
            SillyBoxType._objectShape = ObjectShapes.RECTANGLE;
            SillyBoxType._density = 1.0f;
            SillyBoxType._maxTurningTorque = 1.0f;
            SillyBoxType._maxMovementForce = 300.0f;
            SillyBoxType._linearDampening = 0.0f;
            SillyBoxType._health = 10;

            //ParasiteType
            ParasiteType = new CharacterType();
            ParasiteType._physicalEntitySize = 0.8f;
            ParasiteType.VisibleEntitySize = ParasiteType._physicalEntitySize * 6f / 4f;
            ParasiteType.SpriteAnimationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence("Enemies/Parasite", 5, 1, ParasiteType._physicalEntitySize * 6f / 4f);
            ParasiteType.AnimationDuration = 0.23f;
            ParasiteType.DrawLayer = DrawLayers.GameElements.Characters;
            ParasiteType._objectShape = ObjectShapes.CIRCLE;
            ParasiteType._density = 1.0f;
            ParasiteType._maxTurningTorque = 1.0f;
            ParasiteType._maxMovementForce = 100.0f;
            ParasiteType._health = 20;
            ParasiteType.MainAttackDamage = 5.0f;
            ParasiteType._linearDampening = 10f;
            ParasiteType.MinimapSprite = new StaticSprite("Minimap/EnemyMinimap", MapTile.TileSideLength / 5, DrawLayers.HUD.Minimap_normal_elements);
            ParasiteType.DeathParticle = ParticleType.ParasiteDeath;
            ParasiteType.DeathSound = "squish";

            //SwarmerType
            SwarmerType = new CharacterType();
            SwarmerType._physicalEntitySize = 1.3f;
            SwarmerType.VisibleEntitySize = SwarmerType._physicalEntitySize * 5.0f / 4.0f;
            SwarmerType.SpriteAnimationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence("Enemies/Swarmer", 7, 1, SwarmerType.VisibleEntitySize);
            SwarmerType.AnimationDuration = 0.5f;
            SwarmerType.DrawLayer = DrawLayers.GameElements.Characters;
            SwarmerType._objectShape = ObjectShapes.CIRCLE;
            SwarmerType._density = 1.0f;
            SwarmerType._maxTurningTorque = 1.0f;
            SwarmerType._maxMovementForce = 140.0f;
            SwarmerType._health = 29;
            SwarmerType.MainAttackDamage = 8.0f;
            SwarmerType._linearDampening = 10f;
            SwarmerType.MinimapSprite = new StaticSprite("Minimap/EnemyMinimap", MapTile.TileSideLength / 4, DrawLayers.HUD.Minimap_normal_elements);
            SwarmerType.DeathParticle = ParticleType.SwarmerDeath;
            SwarmerType.DeathSound = "squish";

        }

        float _physicalEntitySize;
        ObjectShapes _objectShape;
        float _density;
        float _maxTurningTorque;
        float _maxMovementForce;
        float _linearDampening;
        float _health;

        public float PhysicalEntitySize {
            get { return _physicalEntitySize; }
        }

        public ObjectShapes ObjectShape {
            get { return _objectShape; }
        }

        public float Density {
            get { return _density; }
        }

        public float MaxTurningTorque {
            get { return _maxTurningTorque; }
        }

        public float MaxMovementForce {
            get { return _maxMovementForce; }
        }

        public float LinearDampening {
            get { return _linearDampening; }

        }
        public float Health {
            get { return _health; }
        }

        public StaticSprite MinimapSprite { get; set; }

        public ParticleType DeathParticle { get; set; }

        public String DeathSound { get; set; }

        public SpriteAnimationSequence SpriteAnimationSequence { get; set; }

        public float AnimationDuration { get; set; }

        public float VisibleEntitySize { get; set; }

        public float MainAttackDamage { get; set; }

        public float DrawLayer { get; set; }
    }
}
