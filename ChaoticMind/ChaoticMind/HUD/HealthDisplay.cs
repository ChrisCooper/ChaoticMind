using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticMind.HUD {
    class HealthDisplay {

        StaticSprite _healthSprite;
        Rectangle _healthFrameRect;
        float _healthFrameWidthFraction = 1 / 10.0f;
        float _healthFrameHeightFraction = 1 / 5.0f;
        Rectangle _healthRect;
        StaticSprite _healthFrameSprite;
        Vector2 _healthFrameScale;

        internal void Initialize() {
            _healthSprite = new StaticSprite("HUD/HealthBar", 1.0f, DrawLayers.HUD.Dynamic_Info);
            _healthFrameSprite = new StaticSprite("HUD/Health_Frame", 1.0f, DrawLayers.HUD.Backgrounds);

            float healthFrameSpriteWidth = Screen.Width / 2.0f;
            float healthFrameSpriteHeight = _healthFrameSprite.Texture.Height / 3;

            //To be used with the spriteBatch.Draw() call
            _healthFrameScale = new Vector2(healthFrameSpriteWidth / _healthFrameSprite.Texture.Width, healthFrameSpriteHeight / _healthFrameSprite.Texture.Height);

            float healthFrameWidth = healthFrameSpriteWidth * _healthFrameWidthFraction;
            float healthFrameHeight = healthFrameSpriteHeight * _healthFrameHeightFraction;

            _healthFrameRect = new Rectangle((int)(Screen.Width / 4.0f), 10, (int)healthFrameSpriteWidth, (int)healthFrameSpriteHeight);

            _healthRect = new Rectangle((int)(_healthFrameRect.Left + healthFrameWidth), (int)(_healthFrameRect.Top + healthFrameHeight), (int)(_healthFrameRect.Width - 2 * healthFrameWidth), (int)(_healthFrameRect.Height - 2 * healthFrameHeight));

        }

        internal void DrawDisplay(SpriteBatch spriteBatch) {
            //Health frame
            spriteBatch.Draw(_healthFrameSprite.Texture, _healthFrameRect, _healthFrameSprite.CurrentTextureBounds, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, _healthFrameSprite.DrawLayer);
            //Health bar
            spriteBatch.Draw(_healthSprite.Texture, new Rectangle(_healthRect.Left, _healthRect.Top, (int)(Player.Instance.PercentHealth * _healthRect.Width), _healthRect.Height), _healthSprite.CurrentTextureBounds, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, _healthSprite.DrawLayer);

        }
    }
}
