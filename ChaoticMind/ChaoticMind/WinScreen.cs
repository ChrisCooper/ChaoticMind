using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class WinScreen {
        StaticSprite _winSprite;

        public WinScreen() {
            _winSprite = new StaticSprite("Screens/DeathScreen", 1, DrawLayers.Menu.Backgrounds);
        }

        public void Update(float deltaTime) {
        }

        public void Draw() {
            //Draw win image
            Program.SpriteBatch.Draw(_winSprite.Texture, ScreenUtils.BiggestSquare, Color.White);
        }
    }
}
