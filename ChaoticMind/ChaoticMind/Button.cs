using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class ShiftButton {

        enum ButtonState {
            NORMAL,
            HOVER,
            PRESSED,
        }

        static StaticSprite arrowButton = new StaticSprite("Shifting/ShiftArrow", 1);
        static StaticSprite arrowButtonPressed = new StaticSprite("Shifting/ShiftArrowPressed", 1);

        ShiftInterface _interface;

        ButtonState _state;

        Rectangle _frame;
        float _rotation;
        float _scalingFactor;

        bool _isToggledDown = false;
        int _index;
        ShiftDirection _direction;

        public ShiftButton(ShiftInterface inter, Vector2 center, float sideLength, float rotation, int index, ShiftDirection direction) {
            _frame = new Rectangle((int)(center.X - sideLength/2.0f), (int)(center.Y - sideLength/2.0f), (int)sideLength, (int)sideLength);
            _direction = direction;
            switch (_direction) {
                case ShiftDirection.UP:
                    _rotation = 0.0f;
                    break;
                case ShiftDirection.DOWN:
                    _rotation = (float)Math.PI;
                    break;
                case ShiftDirection.LEFT:
                    _rotation = (float)Math.PI/2.0f;
                    break;
                case ShiftDirection.RIGHT:
                    _rotation = -(float)Math.PI/2.0f;
                    break;
            }
            _rotation = rotation;
            _scalingFactor = sideLength/(float)arrowButton.CurrentTextureBounds.Width;
            _index = index;
            _interface = inter;
        }

        internal void Update() {
            if (mouseIsOverSelf()) {
                if (InputManager.IsMouseClicked()) {
                    if (!_isToggledDown) {
                        pressDownButton();
                    }
                    else {
                        toggleOffButton();
                    }
                    pressDownButton();
                } else if (InputManager.IsMouseDown()) {
                    _state = ButtonState.HOVER;
                }
            } else {
                if (_state != ButtonState.PRESSED) {
                    _state = ButtonState.NORMAL;
                }
            }
        }

        private void toggleOffButton() {
            _isToggledDown = false;
            _state = ButtonState.NORMAL;
            _interface.ButtonWasToggledOff();
        }

        private void pressDownButton() {
            _state = ButtonState.PRESSED;
            _isToggledDown = true;
            _interface.ButtonWasPressed(this);
        }

        private bool mouseIsOverSelf() {
            Vector2 mouse = InputManager.MouseScreenPosition;

            return (mouse.X > _frame.Left && mouse.X < _frame.Right &&
                    mouse.Y > _frame.Top && mouse.Y < _frame.Bottom);
        }

        internal void reset() {
            _isToggledDown = false;
            _state = ButtonState.NORMAL;
        }

        internal StaticSprite Sprite {
            get { return (_state == ButtonState.NORMAL) ? arrowButton : arrowButtonPressed; }
        }

        public Vector2 Center {
            get {
                return new Vector2(_frame.Center.X, _frame.Center.Y);
            }
        }

        public float Rotation {
            get {
                return _rotation;
            }
        }

        public float ScalingFactor {
            get {
                return _scalingFactor;
            }
        }

        public int Index {
            get {
                return _index;
            }
        }

        public ShiftDirection Direction {
            get {
                return _direction;
            }
        }
    }
}
