using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoticMind
{
    enum ObjectShapes {
        CIRCLE,
        RECTANGLE,
    }


    //Could this be a struct? Probably......... Dang.
    //I will look into that and change it perhaps when I have an ineternet connection.
    class CharacterType
    {
        static float defaultPixelsPerMeter = 64.0f;

        public static CharacterType SillyBox = new CharacterType("TestImages/Box", 16, 1.0f, ObjectShapes.RECTANGLE, 1.0f, 1.0f, 10.0f);
        public static CharacterType CountingBox = new CharacterType("TestImages/TestImage", 5, 1.0f, ObjectShapes.RECTANGLE, 1.0f, 1.0f, 10.0f);
        public static CharacterType ControllableBox = new CharacterType("TestImages/TestPlayer", 1, 1.0f, ObjectShapes.CIRCLE, 5.0f, 1.0f, 30.0f);

        String _imagePrefix;
        int _numFrames;
        float _animationDuration;
        float _pixelsPerMeter;
        ObjectShapes _objectShape;
        float _density;
        float _maxTurningTorque;
        float _maxMovementForce;

        CharacterType(String imagePrefix, int numFrames, float animationDuration, ObjectShapes shape, float density, float maxTurningTorque, float maxMovementForce)
            : this(imagePrefix, numFrames, animationDuration, defaultPixelsPerMeter, shape, density, maxTurningTorque, maxMovementForce)
        {
        }


        CharacterType(String imagePrefix, int numFrames, float animationDuration, float pixelsPerMeter, ObjectShapes shape, float density, float maxTurningTorque, float maxMovementForce)
        {
            _imagePrefix = imagePrefix;
            _numFrames = numFrames;
            _animationDuration = animationDuration;
            _pixelsPerMeter = pixelsPerMeter;
            _objectShape = shape;
            _density = density;
            _maxTurningTorque = maxTurningTorque;
            _maxMovementForce = maxMovementForce;
        }

        public String ImagePrefix
        {
            get { return _imagePrefix; }
        }

        public int NumFrames
        {
            get { return _numFrames; }
        }

        public float AnimationDuration
        {
            get { return _animationDuration; }
        }

        public float PixelsPerMeter
        {
            get { return _pixelsPerMeter; }
        }

        public ObjectShapes ObjectShape
        {
            get { return _objectShape; }
        }

        public float Density
        {
            get { return _density; }
        }

        public float MaxTurningTorque
        {
            get { return _maxTurningTorque; }
        }

        public float MaxMovementForce
        {
            get { return _maxMovementForce; }
        }


    }
}
