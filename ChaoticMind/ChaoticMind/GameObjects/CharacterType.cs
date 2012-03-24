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

        public static CharacterType SillyBox = new CharacterType("TestImages/Box", 17, 1, 1.0f, 1.0f, ObjectShapes.RECTANGLE, 1.0f, 1.0f, 10.0f, 10);
        public static CharacterType Swarmer = new CharacterType("Enemies/Swarmer/Swarmer_Sprite_sheet", 7, 1, 1.0f, 0.75f, ObjectShapes.CIRCLE, 1.0f, 1.0f, 3.0f, 15);
        public static CharacterType Player = new CharacterType("TestImages/TestPlayer", 1, 1, 1.0f, 1.0f, ObjectShapes.CIRCLE, 5.0f, 1.0f, 2500.0f, 100);

        static CharacterType() {
            Swarmer.LinearDampening = 10f;

        }

        String _spriteName;
        int _xFrames;
        int _yFrames;
        float _entitySize;
        float _animationDuration;
        ObjectShapes _objectShape;
        float _density;
        float _maxTurningTorque;
        float _maxMovementForce;
        int _health;

        CharacterType(String spriteName, int xFrames, int yFrames, float entitySize, float animationDuration, ObjectShapes shape, float density, float maxTurningTorque, float maxMovementForce, int health) {
            _spriteName = spriteName;
            _xFrames = xFrames;
            _yFrames = yFrames;
            _entitySize = entitySize;
            _animationDuration = animationDuration;
            _objectShape = shape;
            _density = density;
            _maxTurningTorque = maxTurningTorque;
            _maxMovementForce = maxMovementForce;
            _health = health;
        }

        public String SpriteName {
            get { return _spriteName; }
        }

        public int XFrames {
            get { return _xFrames; }
        }

        public int YFrames {
            get { return _yFrames; }
        }

        public float AnimationDuration {
            get { return _animationDuration; }
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
            get;
            set;
        }
        public int Health {
            get { return _health; }
        }


        

    }
}
