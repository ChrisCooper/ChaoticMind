using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ChaoticMind.FullMenuScreens;
using ChaoticMind.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticMind {

    class ChaoticMindGame : Microsoft.Xna.Framework.Game {

        //Parameters
        bool _goFullscreen = false;

        //Publics
        public IGameFlowComponent ActiveComponent {get; set; }
        public GraphicsUtilities GraphicsUtils { get; set; } 

        public ChaoticMindGame() {
            GraphicsUtils = new GraphicsUtilities(this, _goFullscreen);
        }

        protected override void LoadContent() {
            GraphicsUtils.LoadContent();

            ActiveComponent = new FullMenu();

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime) {
            float deltaTime = ((float)gameTime.ElapsedGameTime.TotalMilliseconds) * 0.001f;

            //Must call once BEFORE any keyboard/mouse operations
            InputManager.Update(deltaTime);

            ActiveComponent.Update(deltaTime);
            if (ActiveComponent.NextComponent != null) {
                ActiveComponent = ActiveComponent.NextComponent;
            }
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            float deltaTime = ((float)gameTime.ElapsedGameTime.TotalMilliseconds) * 0.001f;

            GraphicsDevice.Clear(Color.Black);
            GraphicsUtils.MainSpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            ActiveComponent.Draw(deltaTime);

            GraphicsUtils.MainSpriteBatch.End();

            base.Draw(gameTime);
        }
    }

    public interface IGameFlowComponent {

        /// <summary>
        /// Tells the component to do any logic and input checking required for it to function.
        /// </summary>
        /// <param name="deltaTime"></param>
        void Update(float deltaTime);

        /// <summary>
        /// Checks if the component is ready to pass control to another component.
        /// This will usually be checked after an update call to see if the controller should switch in another component
        /// </summary>
        IGameFlowComponent NextComponent { get; set; }

        /// <summary>
        /// Tells the component to draw itself to the screen.
        /// </summary>
        /// <param name="deltaTime"></param>
        void Draw(float deltaTime);
    }
}
