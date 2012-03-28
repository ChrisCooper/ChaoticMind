﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticMind {
    class MouseDrawer {

        StaticSprite _retical;
        StaticSprite _mouse;
        StaticSprite _mouseClicked;

        internal void Initialize() {
            _retical = new StaticSprite("UI/Retical", 1, DrawLayers.Mouse);
            _mouse = new StaticSprite("UI/Mouse", 1, DrawLayers.Mouse);
            _mouseClicked = new StaticSprite("UI/Mouse_Clicked", 1, DrawLayers.Mouse);
        }

        internal void drawMouse(SpriteBatch spriteBatch) {
            Vector2 mouseLocation = InputManager.MouseScreenPosition;
            if (GameState.Mode == GameState.GameMode.NORMAL) {
                spriteBatch.Draw(_retical.Texture, mouseLocation, _retical.CurrentTextureBounds, Color.White, 0.0f, _retical.CurrentTextureOrigin, 1, SpriteEffects.None, 0.0f);
            }
            else {
                if (!InputManager.IsMouseDown()) {
                    spriteBatch.Draw(_mouse.Texture, mouseLocation, _mouse.CurrentTextureBounds, Color.White, 0.0f, Vector2.Zero, 1, SpriteEffects.None, 0.0f);
                }
                else {
                    spriteBatch.Draw(_mouseClicked.Texture, mouseLocation, _mouseClicked.CurrentTextureBounds, Color.White, 0.0f, Vector2.Zero, 1, SpriteEffects.None, 0.0f);
                }
            }
        }

    }
}
