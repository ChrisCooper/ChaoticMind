using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class LoseScreen {

        const float FadeInDuration = 3.0f;

        static StaticSprite _deathSprite = new StaticSprite("Screens/DeathScreen", 1, DrawLayers.Menu.Backgrounds);

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

        public static void ClearGame() {
            _fadeInTimer.Reset();
        }

        public static void Update(float deltaTime) {
            _fadeInTimer.Update(deltaTime);
        }

        public static void Draw(SpriteBatch spriteBatch) {

            //Draw two rectangles on the sides of the lose sprite
            spriteBatch.Draw(Program.SharedGame.BlackPx, _leftSideRectangle, Rectangle.Empty, Color.Black * _fadeInTimer.percentComplete, 0.0f, Vector2.Zero, SpriteEffects.None, DrawLayers.Menu.Backgrounds);
            spriteBatch.Draw(Program.SharedGame.BlackPx, _rightSideRectangle, Rectangle.Empty, Color.Black * _fadeInTimer.percentComplete, 0.0f, Vector2.Zero, SpriteEffects.None, DrawLayers.Menu.Backgrounds);

            //Draw lose image
            spriteBatch.Draw(_deathSprite.Texture, Screen.Center, _deathSprite.CurrentTextureBounds, Color.White * _fadeInTimer.percentComplete, 0.0f, _deathSprite.CurrentTextureOrigin, _deathSpriteScale, SpriteEffects.None, DrawLayers.Menu.Backgrounds - 0.001f);
        }

        public static bool TimerFinished() {
            return _fadeInTimer.isFinished;
        }
    }
}
