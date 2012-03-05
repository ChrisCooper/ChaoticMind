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

        public static CharacterType SillyBox = new CharacterType("TestImages/Box", 64, 64, 1.0f, ObjectShapes.RECTANGLE, 1.0f, 10.0f);
        public static CharacterType CountingBox = new CharacterType("TestImages/TestImage", 64, 64, 1.0f, ObjectShapes.RECTANGLE, 1.0f, 10.0f);
        public static CharacterType ControllableBox = new CharacterType("TestImages/TestPlayer", 64, 64, 1.0f, 20.0f, ObjectShapes.CIRCLE, 1.0f, 50.0f);

        String _spriteName;
        int _xSize;
        int _ySize; 
        float _animationDuration;
        float _pixelsPerMeter;
        ObjectShapes _objectShape;
        float _maxTurningTorque;
        float _maxMovementForce;

        CharacterType(String spriteName, int xSize, int ySize, float animationDuration, ObjectShapes shape, float maxTurningTorque, float maxMovementForce) : this(spriteName, xSize, ySize, animationDuration, defaultPixelsPerMeter, shape, maxTurningTorque, maxMovementForce)
        {
        }


        CharacterType(String spriteName, int xSize, int ySize, float animationDuration, float pixelsPerMeter, ObjectShapes shape, float maxTurningTorque, float maxMovementForce)
        {
            _spriteName = spriteName;
            _xSize = xSize;
            _ySize = ySize;
            _animationDuration = animationDuration;
            _pixelsPerMeter = pixelsPerMeter;
            _objectShape = shape;
            _maxTurningTorque = maxTurningTorque;
            _maxMovementForce = maxMovementForce;
        }

        public String SpriteName
        {
            get { return _spriteName; }
        }

        public int XSize
        {
            get { return _xSize; }
        }

        public int YSize
        {
            get { return _ySize; }
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
