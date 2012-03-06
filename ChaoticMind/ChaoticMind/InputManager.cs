using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ChaoticMind
{
    class InputManager
    {

        static InputManager _mainManager;

        MouseState _currentMouseState;

        public static Vector2 MouseWorldPosition
        {
            get { return Program.SharedGame.MainCamera.screenPointToWorld(new Vector2(_mainManager._currentMouseState.X, _mainManager._currentMouseState.Y)); }
        }

        public static void Initialize()
        {
            _mainManager = new InputManager();
        }

        public static void Update(float deltaTime)
        {
            _mainManager.InstanceUpdate(deltaTime);
        }

            public  void InstanceUpdate(float deltaTime)
        {
            _currentMouseState = Mouse.GetState();
        }
    }
}

