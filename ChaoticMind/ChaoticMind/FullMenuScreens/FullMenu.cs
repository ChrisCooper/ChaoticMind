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

        public FullMenu() {
            ActiveScreen = new FrontMenu();
        }

        public void Update(float deltaTime) {
            ActiveScreen.Update(deltaTime);
            if (ActiveScreen.NextComponent != null) {
                ActiveScreen = ActiveScreen.NextComponent;
            }
        }

        public void Draw(float deltaTime) {
            ActiveScreen.Draw(deltaTime);
        }


        public IGameFlowComponent NextComponent { get; set; }
    }
}
