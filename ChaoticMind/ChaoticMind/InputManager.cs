using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ChaoticMind {
    class InputManager {

        static InputManager _mainManager;

        MouseState _currentMouseState;
        KeyboardState _oldState, _curState;

        public static Vector2 MouseWorldPosition {
            get { return Program.SharedGame.MainCamera.screenPointToWorld(new Vector2(_mainManager._currentMouseState.X, _mainManager._currentMouseState.Y)); }
        }

        public static void Initialize() {
            _mainManager = new InputManager();
        }

        //update start/end pair have to be used every update loop
        //anything between them will work properly
        public static void UpdateStart(float deltaTime) {
            _mainManager.InstanceUpdate(deltaTime);
            _mainManager._curState = Keyboard.GetState();
        }
        public static void UpdateEnd() {
            _mainManager._oldState = _mainManager._curState;
        }

        public void InstanceUpdate(float deltaTime) {
            _currentMouseState = Mouse.GetState();
        }

        public static bool IsKeyUp(Keys k) {
            return _mainManager._curState.IsKeyUp(k);
        }
        public static bool IsKeyDown(Keys k) {
            return _mainManager._curState.IsKeyDown(k);
        }
        public static bool IsKeyClicked(Keys k){
            //down this time, but not last
            return _mainManager._curState.IsKeyDown(k) && !_mainManager._oldState.IsKeyDown(k);
        }
    }
}

