using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ChaoticMind {

    enum KeyInput {
        MOVE_FORWARD,
        MOVE_BACKWARD,
        MOVE_LEFT,
        MOVE_RIGHT,
        RELOAD,
        CHANGE_WEAPON,
        TOGGLE_SHIFT_MENU,
        TOGGLE_PAUSE_MENU,
    }


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

        public static bool IsKeyUp(KeyInput input) {
            return _curKeyState.IsKeyUp(KeyForInput(input));
        }

        public static bool IsKeyDown(KeyInput input) {
            return _curKeyState.IsKeyDown(KeyForInput(input));
        }
        public static bool IsKeyClicked(KeyInput input) {
            //down this time, but not last
            return _curKeyState.IsKeyDown(KeyForInput(input)) && !_oldKeyState.IsKeyDown(KeyForInput(input));
        }

        private static Keys KeyForInput(KeyInput input) {
            switch (input) {
                case KeyInput.MOVE_FORWARD:
                    return Keys.W;
                case KeyInput.MOVE_BACKWARD:
                    return Keys.S;
                case KeyInput.MOVE_LEFT:
                    return Keys.A;
                case KeyInput.MOVE_RIGHT:
                    return Keys.D;
                case KeyInput.RELOAD:
                    return Keys.R;
                case KeyInput.CHANGE_WEAPON:
                    return Keys.Tab;
                case KeyInput.TOGGLE_SHIFT_MENU:
                    return Keys.Space;
                case KeyInput.TOGGLE_PAUSE_MENU:
                    return Keys.Escape;
                default:
                    throw new Exception("Unhandled KeyInput");
            }
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

