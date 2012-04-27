using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticMind {

    enum UIButtonState {
        NORMAL,
        PRESSED,
    }


    delegate void ButtonWasClickedListener(UIButton clickedButton);

    class UIButton {

        protected UIButtonState _state;

        protected AnimatedSprite _normalSprite;
        protected AnimatedSprite _pressedSprite;

        Rectangle _frame;
        float _rotation;

        private event ButtonWasClickedListener _signalButtonClick;

        public UIButton(Rectangle rectangle, float rotation, AnimatedSprite normalSprite, AnimatedSprite pressedSprite) {
            _frame = rectangle;
            Center = new Vector2(_frame.Center.X, _frame.Center.Y);
            _normalSprite = normalSprite;
            _pressedSprite = pressedSprite;
            _rotation = rotation;
            ScalingFactor = new Vector2(rectangle.Width / (float)_normalSprite.CurrentTextureBounds.Width, rectangle.Height / (float)_normalSprite.CurrentTextureBounds.Height);
        }

        public void setTarget(ButtonWasClickedListener buttonClickSignal) {
            _signalButtonClick = buttonClickSignal;
        }

        internal void Update(float deltaTime) {
            if (mouseIsOverSelf()) {
                if (InputManager.IsMouseClicked) {
                    if (_signalButtonClick != null) {
                        _signalButtonClick(this);
                    }
                }
                else if (InputManager.IsMouseDown) {
                    _state = UIButtonState.PRESSED;
                }
                else {
                    _state = UIButtonState.NORMAL;
                }
            }
            else {
                _state = UIButtonState.NORMAL;
            }
        }

        public void DrawSelf(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Sprite.Texture, Center, Sprite.CurrentTextureBounds, Color.White, Rotation, Sprite.CurrentTextureOrigin, ScalingFactor, SpriteEffects.None, DrawLayers.Menu.Elements);
        }

        private bool mouseIsOverSelf() {
            Vector2 mouse = InputManager.MouseScreenPosition;

            return (mouse.X > _frame.Left && mouse.X < _frame.Right &&
                    mouse.Y > _frame.Top && mouse.Y < _frame.Bottom);
        }

        public virtual AnimatedSprite Sprite {
            get { return _state == UIButtonState.NORMAL ? _normalSprite : _pressedSprite; }
        }

        public Vector2 Center { get; set; }

        public Rectangle Rectangle {
            get { return _frame; }
        }

        public float Rotation {
            get {
                return _rotation;
            }
        }

        public Vector2 ScalingFactor { get; set; }
    }
}