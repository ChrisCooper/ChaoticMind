using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoticMind.FullMenuScreens {

    /// <summary>
    /// The controller flow component which switches between the possible menu screens.
    /// Currently starts on a simple click-through screen.
    /// </summary>
    class FullMenu : IGameFlowComponent {

        IGameFlowComponent ActiveScreen;

        public FullMenu(ChaoticMindGame parentGame) {
            ParentGame = parentGame;
            ActiveScreen = new FrontMenu(this);
        }

        public void Update(float deltaTime) {
            ActiveScreen.Update(deltaTime);
            if (ActiveScreen.NextComponent != null) {
                ActiveScreen = ActiveScreen.NextComponent;
            }
        }

        public void StartGame() {
            GameLevel testLevel = new GameLevel();
            testLevel.MapDimension = 4;
            testLevel.ObjectivesToCollect = 2;
            NextComponent = new ChaoticMindPlayable(testLevel);
        }

        public void Draw(float deltaTime) {
            ActiveScreen.Draw(deltaTime);
            MouseDrawer.Draw(MouseType.POINTER);
        }


        public IGameFlowComponent NextComponent { get; set; }

        public ChaoticMindGame ParentGame { get; set; }
    }
}
