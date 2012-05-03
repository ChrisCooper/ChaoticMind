using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class PauseMenu : IGameFlowComponent {

        StaticSprite _pauseBackground;

        private ChaoticMindPlayable _playble;

        public PauseMenu(ChaoticMindPlayable playable) {
            _playble = playable; 
            _pauseBackground = new StaticSprite("UI/PauseScreen", 1, DrawLayers.Menu.Backgrounds);
        }


        public void Update(float deltaTime) {
            if (InputManager.IsKeyClicked(KeyInput.TOGGLE_PAUSE_MENU)) {
                _playble.Resume();
            }
        }

        public IGameFlowComponent NextComponent { get;  set; }

        public void Draw(float deltaTime) {
            Program.SpriteBatch.Draw(_pauseBackground.Texture, ScreenUtils.BiggestSquare, Color.White);
        }
    }
}
