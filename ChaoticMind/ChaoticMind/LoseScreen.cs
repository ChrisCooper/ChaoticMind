using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class LoseScreen {

        const float FadeInDuration = 3.0f;

        static StaticSprite _deathSprite = new StaticSprite("Screens/DeathScreen", 1, DrawLayers.MenuBackgrounds);

        static Timer _fadeInTimer;

        static bool _playedSound;

        private static float _deathSpriteScale;
        static Rectangle _leftSideRectangle;
        static Rectangle _rightSideRectangle;

        public static void Initialize() {
            _playedSound = false;
            _fadeInTimer = new Timer(FadeInDuration);
            _deathSpriteScale = Screen.SmallestDimension / (float)_deathSprite.CurrentTextureBounds.Width;

            int barWidth = (Screen.Width - Screen.Height) / 2;
            _leftSideRectangle = new Rectangle(0, 0, barWidth, Screen.Height);
            _rightSideRectangle = new Rectangle(Screen.Width - barWidth, 0, barWidth, Screen.Height);
        }

        public static void Update(float deltaTime) {
            _fadeInTimer.Update(deltaTime);

            if (!_playedSound) {
                SoundEffectManager.PlayEffect("cinematicboom", 1.0f);
                _playedSound = true;
            }

            if (_fadeInTimer.isFinished) {
                if (InputManager.IsMouseClicked()) {
                    _playedSound = false;
                    _fadeInTimer.Reset();
                }
            }
        }

        public static void Draw(SpriteBatch spriteBatch) {

            //Draw two rectangles on the sides of the lose sprite
            spriteBatch.Draw(Program.SharedGame.BlackPx, _leftSideRectangle, Rectangle.Empty, Color.Black * _fadeInTimer.percentComplete, 0.0f, Vector2.Zero, SpriteEffects.None, DrawLayers.MenuBackgrounds);
            spriteBatch.Draw(Program.SharedGame.BlackPx, _rightSideRectangle, Rectangle.Empty, Color.Black * _fadeInTimer.percentComplete, 0.0f, Vector2.Zero, SpriteEffects.None, DrawLayers.MenuBackgrounds);

            //Draw lose image
            spriteBatch.Draw(_deathSprite.Texture, Screen.Center, _deathSprite.CurrentTextureBounds, Color.White * _fadeInTimer.percentComplete, 0.0f, _deathSprite.CurrentTextureOrigin, _deathSpriteScale, SpriteEffects.None, DrawLayers.MenuBackgrounds - 0.001f);
        }

        public static bool TimerFinished() {
            return _fadeInTimer.isFinished;
        }
    }
}
