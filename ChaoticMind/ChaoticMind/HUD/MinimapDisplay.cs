using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ChaoticMind.HUD {
    class MinimapDisplay {

        StaticSprite _minimapFrame;
        float _mapFrameScale;
        Rectangle _mapFrameRect;
        //The percentage of the full frame image that each side of the frame takes up
        float _mapFrameWidthFraction = 0.1f;
        Rectangle _mapRect;

        internal MinimapDisplay() {
            _minimapFrame = new StaticSprite("HUD/Minimap_Frame", 1.0f, DrawLayers.HUD.Backgrounds);

            float mapFrameSideLength = Math.Min(ScreenUtils.Width / 4.0f, ScreenUtils.Height / 3.0f);
            _mapFrameScale = mapFrameSideLength / _minimapFrame.Texture.Bounds.Width;
            _mapFrameRect = new Rectangle(0, (int)(ScreenUtils.Height - mapFrameSideLength), (int)mapFrameSideLength, (int)mapFrameSideLength);

            float frameWidth = _mapFrameWidthFraction * mapFrameSideLength;
            _mapRect = new Rectangle((int)(_mapFrameRect.Left + frameWidth), (int)(_mapFrameRect.Top + frameWidth), (int)(_mapFrameRect.Width - 2 * frameWidth), (int)(_mapFrameRect.Height - 2 * frameWidth));
        }

        internal void Draw() {
            Program.SpriteBatch.Draw(_minimapFrame.Texture, _mapFrameRect, _minimapFrame.CurrentTextureBounds, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, _minimapFrame.DrawLayer);

        }

        internal Rectangle MinimapRect {
            get { return _mapRect; }
        }
    }
}
