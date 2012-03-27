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
            Player._entitySize = 1.0f;
            Player._sprite = new AnimatedSprite("TestImages/TestPlayer", 1, 1, Player._entitySize, 1.0f);
            Player._objectShape = ObjectShapes.CIRCLE;
            Player._density = 5.0f;
            Player._maxTurningTorque = 1.0f;
            Player._maxMovementForce = 2500.0f;
            Player._health = 100;
            Player._linearDampening = 30f;
            Player.MinimapSprite = new StaticSprite("Minimap/PlayerMinimap", MapTile.TileSideLength / 2);

            //SillyBox
            SillyBox._entitySize = 1.0f;
            SillyBox._sprite = new AnimatedSprite("TestImages/Box", 17, 1, SillyBox._entitySize, 1.0f);
            SillyBox._objectShape = ObjectShapes.RECTANGLE;
            SillyBox._density = 1.0f;
            SillyBox._maxTurningTorque = 1.0f;
            SillyBox._maxMovementForce = 10.0f;
            SillyBox._health = 10;

            //Parasite
            Parasite._entitySize = 0.8f;
            Parasite._sprite = new AnimatedSprite("Enemies/Parasite", 12, 1, Parasite._entitySize * 6f / 4f, 15.0f);
            Parasite._objectShape = ObjectShapes.CIRCLE;
            Parasite._density = 1.0f;
            Parasite._maxTurningTorque = 1.0f;
            Parasite._maxMovementForce = 3.0f;
            Parasite._health = 10;
            Parasite._linearDampening = 10f;
            Parasite.MinimapSprite = new StaticSprite("Minimap/EnemyMinimap", MapTile.TileSideLength / 5);

            //Swarmer
            Swarmer._entitySize = 1.3f;
            Swarmer._sprite = new AnimatedSprite("Enemies/Swarmer", 7, 1, Swarmer._entitySize * 5.0f / 4.0f, 15f);
            Swarmer._objectShape = ObjectShapes.CIRCLE;
            Swarmer._density = 1.0f;
            Swarmer._maxTurningTorque = 1.0f;
            Swarmer._maxMovementForce = 3.0f;
            Swarmer._health = 25;
            Swarmer._linearDampening = 10f;
            Swarmer.MinimapSprite = new StaticSprite("Minimap/EnemyMinimap", MapTile.TileSideLength / 4);
        }

        AnimatedSprite _sprite;
        float _entitySize;
        ObjectShapes _objectShape;
        float _density;
        float _maxTurningTorque;
        float _maxMovementForce;
        float _linearDampening;
        int _health;

        public AnimatedSprite Sprite {
            get { return _sprite; }
        }

        public float EntitySize {
            get { return _entitySize; }
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
        public int Health {
            get { return _health; }
        }

        public StaticSprite MinimapSprite { get; set; }
    }
}
