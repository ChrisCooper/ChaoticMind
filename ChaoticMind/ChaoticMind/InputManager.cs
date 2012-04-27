using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ChaoticMind {
    class InputManager {

        static MouseState _oldMouseState, _curMouseState;
        static KeyboardState _oldKeyState, _curKeyState;

        //update start/end pair have to be used every update loop
        //anything between them will work properly
        public static void Update(float deltaTime) {
            _oldMouseState = _curMouseState;
            _curMouseState = Mouse.GetState();
            _oldKeyState = _curKeyState;
            _curKeyState = Keyboard.GetState();
        }

        public static Vector2 MouseScreenPosition {
            get { return new Vector2(_curMouseState.X, _curMouseState.Y); }
        }

        //public static Vector2 MouseWorldPosition {
        //    get { return Program.DeprecatedObjects.MainCamera.screenPointToWorld(new Vector2(_curMouseState.X, _curMouseState.Y)); }
        //}

        public static bool IsKeyUp(Keys k) {
            return _curKeyState.IsKeyUp(k);
        }
        public static bool IsKeyDown(Keys k) {
            return _curKeyState.IsKeyDown(k);
        }
        public static bool IsKeyClicked(Keys k){
            //down this time, but not last
            return _curKeyState.IsKeyDown(k) && !_oldKeyState.IsKeyDown(k);
        }

        public static bool IsMouseDown {
            get { return _curMouseState.LeftButton == ButtonState.Pressed; }
        }
        public static bool IsMouseClicked {
            get { //up this time, but not last
                return (!(_curMouseState.LeftButton == ButtonState.Pressed)) && (_oldMouseState.LeftButton == ButtonState.Pressed);
            }
        }
    }
}

