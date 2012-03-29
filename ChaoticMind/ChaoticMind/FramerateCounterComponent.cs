using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticMind
{

    public class FrameRateCounter
    {
        private TimeSpan _elapsedTime = TimeSpan.Zero;
        private int _frameCounter;
        private int _frameRate;
        private Vector2 _position;
        private SpriteBatch _spriteBatch;
        SpriteFont _font;

        public FrameRateCounter(SpriteBatch spriteBatch, SpriteFont font)
        {
            _spriteBatch = spriteBatch;
            _font = font;
            _position = new Vector2(10, 10);
        }

        public void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime;

            if (_elapsedTime <= TimeSpan.FromSeconds(1)) return;

            _elapsedTime -= TimeSpan.FromSeconds(1);
            _frameRate = _frameCounter;
            _frameCounter = 0;
        }

        public void Draw(GameTime gameTime)
        {
            _frameCounter++;

            string fps = string.Format("{0} fps", _frameRate);

            _spriteBatch.DrawString(_font, fps, _position, Color.White);
        }
    }
}