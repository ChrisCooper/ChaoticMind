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

        public static CharacterType SillyBox = new CharacterType();
        public static CharacterType Swarmer = new CharacterType();
        public static CharacterType Parasite = new CharacterType();
        public static CharacterType Player = new CharacterType();

        static CharacterType() {

            //Player
            Player._physicalEntitySize = 1.0f;
            Player.SpriteAnimationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence("TestImages/TestPlayer", 1, 1, Player._physicalEntitySize);
            Player.AnimationDuration = 1.0f;
            Player._objectShape = ObjectShapes.CIRCLE;
            Player._density = 1.0f;
            Player._maxTurningTorque = 1.0f;
            Player._maxMovementForce = 3.0f;
            Player._health = 100;
            Player._linearDampening = 15f;
            Player.MinimapSprite = new StaticSprite("Minimap/PlayerMinimap", MapTile.TileSideLength / 2);

            //SillyBox
            SillyBox._physicalEntitySize = 1.0f;
            SillyBox.SpriteAnimationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence("TestImages/Box", 17, 1, SillyBox._physicalEntitySize);
            SillyBox.AnimationDuration = 1.0f;
            SillyBox._objectShape = ObjectShapes.RECTANGLE;
            SillyBox._density = 1.0f;
            SillyBox._maxTurningTorque = 1.0f;
            SillyBox._maxMovementForce = 10.0f;
            Parasite._linearDampening = 0.0f;
            SillyBox._health = 10;

            //Parasite
            Parasite._physicalEntitySize = 0.8f;
            Parasite.VisibleEntitySize = Parasite._physicalEntitySize * 6f / 4f;
            Parasite.SpriteAnimationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence("Enemies/Parasite", 12, 1, Parasite._physicalEntitySize * 6f / 4f);
            Parasite.AnimationDuration = 0.5f;
            Parasite._objectShape = ObjectShapes.CIRCLE;
            Parasite._density = 1.0f;
            Parasite._maxTurningTorque = 1.0f;
            Parasite._maxMovementForce = 3.0f;
            Parasite._health = 10;
            Parasite.MainAttackDamage = 3.0f;
            Parasite._linearDampening = 10f;
            Parasite.MinimapSprite = new StaticSprite("Minimap/EnemyMinimap", MapTile.TileSideLength / 5);
            Parasite.DeathParticle = ParticleType.ParasiteDeath;

            //Swarmer
            Swarmer._physicalEntitySize = 1.3f;
            Swarmer.VisibleEntitySize = Swarmer._physicalEntitySize * 5.0f / 4.0f;
            Swarmer.SpriteAnimationSequence = SpriteAnimationSequence.newOrExistingSpriteAnimationSequence("Enemies/Swarmer", 7, 1, Swarmer.VisibleEntitySize);
            Swarmer.AnimationDuration = 0.5f;
            Swarmer._objectShape = ObjectShapes.CIRCLE;
            Swarmer._density = 1.0f;
            Swarmer._maxTurningTorque = 1.0f;
            Swarmer._maxMovementForce = 3.0f;
            Swarmer._health = 25;
            Parasite.MainAttackDamage = 5.0f;
            Swarmer._linearDampening = 10f;
            Swarmer.MinimapSprite = new StaticSprite("Minimap/EnemyMinimap", MapTile.TileSideLength / 4);
            Swarmer.DeathParticle = ParticleType.SwarmerDeath;
            
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

        public SpriteAnimationSequence SpriteAnimationSequence { get; set; }

        public float AnimationDuration { get; set; }

        public float VisibleEntitySize { get; set; }

        public float MainAttackDamage { get; set; }
    }
}
