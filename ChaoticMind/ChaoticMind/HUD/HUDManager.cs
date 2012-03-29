using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ChaoticMind.HUD {
    class HUDManager {

        static HUDManager _mainInstance;


        WeaponDisplay _weaponDisplay = new WeaponDisplay();
        HealthDisplay _healthDisplay = new HealthDisplay();

        //MINIMAP
        StaticSprite _minimapFrame;
        float _mapFrameScale;
        Rectangle _mapFrameRect;
        //The percentage of the full frame image that each side of the frame takes up
        float _mapFrameWidthFraction = 0.1f;
        Rectangle _mapRect;

        internal void Initialize() {
            _mainInstance = this;
            setupMinimapInfo();
            _healthDisplay.Initialize();
            _weaponDisplay.Initialize();
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
           }

        internal void Draw_HUD(SpriteBatch spriteBatch) {
            //Minimap frame
            spriteBatch.Draw(_minimapFrame.Texture, _mapFrameRect, _minimapFrame.CurrentTextureBounds, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, _minimapFrame.DrawLayer);


            _healthDisplay.DrawDisplay(spriteBatch);
            _weaponDisplay.DrawDisplay(spriteBatch);

            }

        internal static Rectangle MinimapRect {
            get { return _mainInstance._mapRect; }
        }
    }
}