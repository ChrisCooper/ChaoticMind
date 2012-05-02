using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using ChaoticMind.HUD;

namespace ChaoticMind {

    public class ChaoticMindPlayable : IGameFlowComponent {

        IGameFlowComponent _gameComponent;
        IGameFlowComponent _overlayComponent;

        //map dimension
        const int MAP_SIZE = 4;

        HUDManager _hudManager;

        public ChaoticMindPlayable() {
            GameObjects gameObjects = new GameObjects();

            PainStaticMaker.Initialize();

            CharacterType.Initialize();
            ParticleType.Initialize();

            //Start the game
            gameObjects.StartNewGame(this, MAP_SIZE);

            _hudManager = new HUDManager(gameObjects);

            _gameComponent = gameObjects;
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(float deltaTime) {

            if (_overlayComponent == null) {
                _gameComponent.Update(deltaTime);
                if (_gameComponent.NextComponent != null) {
                    _overlayComponent = _gameComponent.NextComponent;
                }
            }
            else {
                _overlayComponent.Update(deltaTime);
            }
        }

        internal void Resume() {
            _overlayComponent = null;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(float deltaTime) {

            _gameComponent.Draw(deltaTime);

            if (_overlayComponent != null) {
                _overlayComponent.Draw(deltaTime);
                MouseDrawer.Draw(MouseType.POINTER);
            }
            else {
                _hudManager.Draw(deltaTime);
                MouseDrawer.Draw(MouseType.POINTER);
            }
        }

        public IGameFlowComponent NextComponent { get; set; }
    }
}
