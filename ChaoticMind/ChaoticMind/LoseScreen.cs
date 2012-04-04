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

        private static float _deathSpriteScale;
        static Rectangle _leftSideRectangle;
        static Rectangle _rightSideRectangle;

        public static void Initialize() {
            _fadeInTimer = new Timer(FadeInDuration);
            _deathSpriteScale = Screen.SmallestDimension / (float)_deathSprite.CurrentTextureBounds.Width;

            int barWidth = (Screen.Width - Screen.Height) / 2;
            _leftSideRectangle = new Rectangle(0, 0, barWidth, Screen.Height);
            _rightSideRectangle = new Rectangle(Screen.Width - barWidth, 0, barWidth, Screen.Height);
        }

        public static void Update(float deltaTime) {
            _fadeInTimer.Update(deltaTime);

            if (_fadeInTimer.isFinished) {
                if (InputManager.IsMouseClicked()) {
                    Program.SharedGame.AdvanceToNextGameState();
                    _fadeInTimer.Reset();
                }
            }
        }

        public static void Draw(SpriteBatch spriteBatch) {

            //Draw two rectangles on the sides of the lose sprite
            spriteBatch.Draw(_deathSprite.Texture, _leftSideRectangle, _deathSprite.CurrentTextureBounds, Color.Black * _fadeInTimer.percentComplete, 0.0f, Vector2.Zero, SpriteEffects.None, DrawLayers.MenuBackgrounds);
            spriteBatch.Draw(_deathSprite.Texture, _rightSideRectangle, _deathSprite.CurrentTextureBounds, Color.Black * _fadeInTimer.percentComplete, 0.0f, Vector2.Zero, SpriteEffects.None, DrawLayers.MenuBackgrounds);




            //Draw lose image
            spriteBatch.Draw(_deathSprite.Texture, Screen.Center, _deathSprite.CurrentTextureBounds, Color.White * _fadeInTimer.percentComplete, 0.0f, _deathSprite.CurrentTextureOrigin, _deathSpriteScale, SpriteEffects.None, DrawLayers.MenuBackgrounds - 0.001f);
        }
    }
}
