using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class LoseScreen {

        const float FadeInDuration = 3.0f;

        StaticSprite _deathSprite;

        Timer _fadeInTimer;

        private float _deathSpriteScale;
        Rectangle _leftSideRectangle;
        Rectangle _rightSideRectangle;

        Texture2D _blackPx;

        public LoseScreen() {
            _fadeInTimer = new Timer(FadeInDuration);
            _deathSprite = new StaticSprite("Screens/DeathScreen", 1, DrawLayers.Menu.Backgrounds);
            _deathSpriteScale = ScreenUtils.SmallestDimension / (float)_deathSprite.CurrentTextureBounds.Width;

            int barWidth = (ScreenUtils.Width - ScreenUtils.Height) / 2;
            _leftSideRectangle = new Rectangle(0, 0, barWidth, ScreenUtils.Height);
            _rightSideRectangle = new Rectangle(ScreenUtils.Width - barWidth, 0, barWidth, ScreenUtils.Height);

            //set up the black pixel used for clearing the screen
            _blackPx = new Texture2D(Program.Graphics.GraphicsDevice, 1, 1);
            uint[] px = { 0xFFFFFFFF };
            _blackPx.SetData<uint>(px);
        }

        public void Update(float deltaTime) {
            _fadeInTimer.Update(deltaTime);
        }

        public void Draw() {

            //Draw two rectangles on the sides of the lose sprite
            Program.SpriteBatch.Draw(_blackPx, _leftSideRectangle, Rectangle.Empty, Color.Black * _fadeInTimer.percentComplete, 0.0f, Vector2.Zero, SpriteEffects.None, DrawLayers.Menu.Backgrounds);
            Program.SpriteBatch.Draw(_blackPx, _rightSideRectangle, Rectangle.Empty, Color.Black * _fadeInTimer.percentComplete, 0.0f, Vector2.Zero, SpriteEffects.None, DrawLayers.Menu.Backgrounds);

            //Draw lose image
            Program.SpriteBatch.Draw(_deathSprite.Texture, ScreenUtils.Center, _deathSprite.CurrentTextureBounds, Color.White * _fadeInTimer.percentComplete, 0.0f, _deathSprite.CurrentTextureOrigin, _deathSpriteScale, SpriteEffects.None, DrawLayers.Menu.Backgrounds - 0.001f);
        }

        public bool TimerFinished() {
            return _fadeInTimer.isFinished;
        }
    }
}
