using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class HUDManager {

        static HUDManager _mainInstance;

        //MINIMAP
        StaticSprite _minimapFrame;
        float _mapFrameScale;
        Rectangle _mapFrameRect;
        //The percentage of the full frame image that each side of the frame takes up
        float _mapFrameWidthFraction = 0.1f;
        Rectangle _mapRect;

        //HEALTH BAR
        StaticSprite _healthSprite;
        Rectangle _healthFrameRect;
        float _healthFrameWidthFraction = 1 / 10.0f;
        float _healthFrameHeightFraction = 1 / 5.0f;
        Rectangle _healthRect;
        StaticSprite _healthFrameSprite;
        Vector2 _healthFrameScale;

        internal void Initialize() {
            _mainInstance = this;
            setupMinimapInfo();
            setupHealthInfo();
        }

        private void setupMinimapInfo() {
            _minimapFrame = new StaticSprite("HUD/Minimap_Frame", 1.0f, DrawLayers.HUD_Backgrounds);

            float mapFrameSideLength = Math.Min(Screen.Width / 4.0f, Screen.Height / 3.0f);
            _mapFrameScale = mapFrameSideLength / _minimapFrame.Texture.Bounds.Width;
            _mapFrameRect = new Rectangle(0, (int)(Screen.Height - mapFrameSideLength), (int)mapFrameSideLength, (int)mapFrameSideLength);

            float frameWidth = _mapFrameWidthFraction * mapFrameSideLength;
            _mapRect = new Rectangle((int)(_mapFrameRect.Left + frameWidth), (int)(_mapFrameRect.Top + frameWidth), (int)(_mapFrameRect.Width - 2 * frameWidth), (int)(_mapFrameRect.Height - 2 * frameWidth));
        }

        private void setupHealthInfo() {
            _healthSprite = new StaticSprite("HUD/HealthBar", 1.0f, DrawLayers.HUD_Dynamic_Info);
            _healthFrameSprite = new StaticSprite("HUD/Health_Frame", 1.0f, DrawLayers.HUD_Backgrounds);

            float healthFrameSpriteWidth = Screen.Width / 2.0f;
            float healthFrameSpriteHeight = _healthFrameSprite.Texture.Height / 3;

            //To be used with the spriteBatch.Draw() call
            _healthFrameScale = new Vector2(healthFrameSpriteWidth / _healthFrameSprite.Texture.Width, healthFrameSpriteHeight / _healthFrameSprite.Texture.Height);

            float healthFrameWidth = healthFrameSpriteWidth * _healthFrameWidthFraction;
            float healthFrameHeight = healthFrameSpriteHeight * _healthFrameHeightFraction;

            _healthFrameRect = new Rectangle((int)(Screen.Width / 4.0f), 10, (int)healthFrameSpriteWidth, (int)healthFrameSpriteHeight);

            _healthRect = new Rectangle((int)(_healthFrameRect.Left + healthFrameWidth), (int)(_healthFrameRect.Top + healthFrameHeight), (int)(_healthFrameRect.Width - 2 * healthFrameWidth), (int)(_healthFrameRect.Height - 2 * healthFrameHeight));
        }

        internal void Draw_HUD(SpriteBatch spriteBatch) {
            //Minimap frame
            spriteBatch.Draw(_minimapFrame.Texture, _mapFrameRect, _minimapFrame.CurrentTextureBounds, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, _minimapFrame.DrawLayer);


            //Health frame
            spriteBatch.Draw(_healthFrameSprite.Texture, _healthFrameRect, _healthFrameSprite.CurrentTextureBounds, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, _healthFrameSprite.DrawLayer);
            //Health bar
            spriteBatch.Draw(_healthSprite.Texture, new Rectangle(_healthRect.Left, _healthRect.Top, (int)(Player.Instance.PercentHealth * _healthRect.Width), _healthRect.Height), _healthSprite.CurrentTextureBounds, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, _healthSprite.DrawLayer);
        }

        internal static Rectangle MinimapRect {
            get { return _mainInstance._mapRect; }
        }
    }
}