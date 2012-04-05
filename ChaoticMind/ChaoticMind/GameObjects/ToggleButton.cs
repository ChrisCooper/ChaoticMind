using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class ToggleButton : UIButton {

        private bool _isToggledDown;

        ButtonWasClickedListener _signalButtonToggleDown;
        ButtonWasClickedListener _signalButtonToggleUp;

        public ToggleButton(Rectangle rectangle, float rotation, AnimatedSprite normalSprite, AnimatedSprite pressedSprite)
            : base(rectangle, rotation, normalSprite, pressedSprite) {

            base.setTarget(toggleState);
        }

        public void setTargets(ButtonWasClickedListener buttonToggleDownSignal, ButtonWasClickedListener buttonToggleUpSignal) {
            _signalButtonToggleDown = buttonToggleDownSignal;
            _signalButtonToggleUp = buttonToggleUpSignal;
        }

        public void toggleState(UIButton clickedButton) {
            _isToggledDown = !_isToggledDown;

            if (_isToggledDown) {
                _signalButtonToggleDown(this);
            }
            else {
                _signalButtonToggleUp(this);
            }
        }

        internal void reset() {
            _isToggledDown = false;
            _state = UIButtonState.NORMAL;
        }

        public override AnimatedSprite Sprite {
            get {
                if (_isToggledDown) {
                    return _pressedSprite;
                }
                
                return _state == UIButtonState.NORMAL ? _normalSprite : _pressedSprite; }
        }

    }
}