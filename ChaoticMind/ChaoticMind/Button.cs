using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class Button {

        enum ButtonState {
            NORMAL,
            HOVER,
            PRESSED,
        }

        static StaticSprite arrowButton = new StaticSprite("Shifting/ShiftArrow", 1);
        static StaticSprite arrowButtonPressed = new StaticSprite("Shifting/ShiftArrowPressed", 1);

        ButtonState _state;

        Rectangle _frame;

        public Button(Vector2 center, float sideLength) {
            _frame = new Rectangle((int)center.X, (int)center.Y, (int)sideLength, (int)sideLength);
        }

        internal void Update() {
            if (mouseIsOverSelf()) {
                if (InputManager.IsMouseClicked()) {
                    _state = ButtonState.PRESSED;
                } else if (InputManager.IsMouseDown()) {
                    _state = ButtonState.HOVER;
                } else {
                    _state = ButtonState.NORMAL;
                }
            } else {
                if (_state != ButtonState.PRESSED) {
                    _state = ButtonState.NORMAL;
                }
            }
        }

        private bool mouseIsOverSelf() {
            Vector2 mouse = InputManager.MouseScreenPosition;

            return (mouse.X > _frame.Left && mouse.Y < _frame.Right &&
                    mouse.Y > _frame.Top && mouse.Y < _frame.Bottom);
        }


        internal StaticSprite Texture {
            get { return (_state == ButtonState.NORMAL) ? arrowButton : arrowButtonPressed; }
        }
    }
}
