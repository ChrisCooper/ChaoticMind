using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticMind {
    class PauseMenu : IGameFlowComponent {

        StaticSprite _pauseBackground;

        UIButton _exitGameButton;

        private ChaoticMindPlayable _playble;

        public PauseMenu(ChaoticMindPlayable playable) {
            _playble = playable; 
            _pauseBackground = new StaticSprite("UI/PauseScreen", 1, DrawLayers.Menu.Backgrounds);


            _exitGameButton = new UIButton(new Vector2(0.5f, 0.7f), 0.5f, "PauseMenu/ExitGameButton", DrawLayers.Menu.Elements);
            //_exitGameButton = new UIButton(ScreenUtils.VertCenteredRect(400, 200, 600), 0, new StaticSprite("PauseMenu/ExitGameButton", 1, DrawLayers.Menu.HighlightElements), new StaticSprite("PauseMenu/ExitGameButtonPressed", 1, DrawLayers.Menu.Elements));
            _exitGameButton.setTarget(ExitGame);
        }


        public void Update(float deltaTime) {
            if (InputManager.IsKeyClicked(KeyInput.TOGGLE_PAUSE_MENU)) {
                _playble.Resume();
            }

            _exitGameButton.Update(deltaTime);
        }

        public IGameFlowComponent NextComponent { get;  set; }

        public void Draw(float deltaTime) {
            Program.SpriteBatch.Draw(_pauseBackground.Texture, ScreenUtils.BiggestSquare, _pauseBackground.CurrentTextureBounds, Color.White, 0, Vector2.Zero, SpriteEffects.None, DrawLayers.Menu.Backgrounds);
            _exitGameButton.DrawSelf(Program.SpriteBatch);
        }

        public void ExitGame(UIButton exitButton) {
            Program.OverallGame.Exit();
        }
    }
}
