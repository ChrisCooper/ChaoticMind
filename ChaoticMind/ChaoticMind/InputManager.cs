using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ChaoticMind {
    class InputManager {

        static InputManager _mainManager;

        MouseState _oldMouseState, _curMouseState;
        KeyboardState _oldKeyState, _curKeyState;

        public static void Initialize() {
            _mainManager = new InputManager();
        }

        //update start/end pair have to be used every update loop
        //anything between them will work properly
        public static void Update(float deltaTime) {
            _mainManager._oldMouseState = _mainManager._curMouseState;
            _mainManager._curMouseState = Mouse.GetState();
            _mainManager._oldKeyState = _mainManager._curKeyState;
            _mainManager._curKeyState = Keyboard.GetState();
        }

        public static Vector2 MouseScreenPosition {
            get { return new Vector2(_mainManager._curMouseState.X, _mainManager._curMouseState.Y); }
        }

        public static Vector2 MouseWorldPosition {
            get { return Program.SharedGame.MainCamera.screenPointToWorld(new Vector2(_mainManager._curMouseState.X, _mainManager._curMouseState.Y)); }
        }

        public static bool IsKeyUp(Keys k) {
            return _mainManager._curKeyState.IsKeyUp(k);
        }
        public static bool IsKeyDown(Keys k) {
            return _mainManager._curKeyState.IsKeyDown(k);
        }
        public static bool IsKeyClicked(Keys k){
            //down this time, but not last
            return _mainManager._curKeyState.IsKeyDown(k) && !_mainManager._oldKeyState.IsKeyDown(k);
        }

        public static bool IsMouseDown() {
            return _mainManager._curMouseState.LeftButton == ButtonState.Pressed;
        }
        public static bool IsMouseClicked() {
            //up this time, but not last
            return (!(_mainManager._curMouseState.LeftButton == ButtonState.Pressed)) && (_mainManager._oldMouseState.LeftButton == ButtonState.Pressed);
        }
    }
}

