using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class WinScreen : IGameFlowComponent {

        StaticSprite _winSprite;

        public WinScreen() {
            _winSprite = new StaticSprite("Screens/DeathScreen", 1, DrawLayers.Menu.Backgrounds);
        }

        public void Update(float deltaTime) {
        }

        public IGameFlowComponent NextComponent { get; set; }

        public void Draw(float deltaTime) {
            Program.SpriteBatch.Draw(_winSprite.Texture, ScreenUtils.BiggestSquare, Color.White);
        }
    }
}
