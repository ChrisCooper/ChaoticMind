using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class ShiftButton : ToggleButton {

        static StaticSprite arrowButton = new StaticSprite("Shifting/ShiftArrow", 1, DrawLayers.MenuElements);
        static StaticSprite arrowButtonPressed = new StaticSprite("Shifting/ShiftArrowPressed", 1, DrawLayers.MenuElements);

        ShiftInterface _interface;

        int _index;
        ShiftDirection _direction;

        public ShiftButton(ShiftInterface inter, Rectangle rectangle, int index, ShiftDirection direction)
            : base(rectangle, rotationForShiftDirection(direction), arrowButton, arrowButtonPressed) {
            
            _direction = direction;
            _index = index;
            _interface = inter;

            setTargets(ButtonWasToggledDown, buttonWasToggledUp);
        }

        private static float rotationForShiftDirection(ShiftDirection shiftDirection) {
            switch (shiftDirection) {
                case ShiftDirection.UP:
                    return 0.0f;
                case ShiftDirection.DOWN:
                    return (float)Math.PI;
                case ShiftDirection.LEFT:
                    return -(float)Math.PI / 2.0f;
                case ShiftDirection.RIGHT:
                    return (float)Math.PI / 2.0f;
                default:
                    throw new Exception("Invalid ShiftDirection passed to rotationForShiftDirection");
            }
        }


        private void buttonWasToggledUp(UIButton button) {
            _interface.ButtonWasToggledOff();
        }

        private void ButtonWasToggledDown(UIButton button) {
            _interface.ButtonWasPressed(this);
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
