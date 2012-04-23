﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticMind.FullMenuScreens {

    /// <summary>
    /// The main menu screen you hit when you first open the game.
    /// </summary>
    class FrontMenu : IGameFlowComponent {

        StaticSprite _background;
        StaticSprite _overlay;

        float _pulseTime = 0f;


        public FrontMenu() {
            _background = new StaticSprite("Screens/StartMenuScreen", 1, DrawLayers.Menu.Backgrounds);
            _overlay = new StaticSprite("Screens/StartMenuScreenOverlay", 1, DrawLayers.Menu.Backgrounds);
        }

        public void Update(float deltaTime) {
            _pulseTime += deltaTime * 1000f;
        }

        public void Draw(float deltaTime) {

            float mapFrameSideLength = Math.Min(ScreenUtils.Width, ScreenUtils.Height);
            float mapFrameScale = mapFrameSideLength / _background.Texture.Bounds.Width;
            Rectangle mapFrameRect = new Rectangle((int)(ScreenUtils.Width - mapFrameSideLength) / 2, (int)(ScreenUtils.Height - mapFrameSideLength), (int)mapFrameSideLength, (int)mapFrameSideLength);

            Program.SpriteBatch.Draw(_background.Texture, mapFrameRect, _background.CurrentTextureBounds, Color.White, 0, Vector2.Zero, SpriteEffects.None, DrawLayers.Menu.Backgrounds - 0.002f);
            Program.SpriteBatch.Draw(_overlay.Texture, mapFrameRect, _overlay.CurrentTextureBounds, Color.White * 0.5f * ((float)(Math.Sin(_pulseTime / 500.0f) + 1.0f) + 0.5f), 0, Vector2.Zero, SpriteEffects.None, DrawLayers.Menu.Backgrounds - 0.003f);
        
        }

        public IGameFlowComponent NextComponent { get; set; }
    }
}
