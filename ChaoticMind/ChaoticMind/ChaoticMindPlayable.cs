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

        GameObjects _gameObjects;
        internal GameObjects Objects {
            get { return _gameObjects; }
        }

        //map dimension
        const int MAP_SIZE = 4;
         
        StaticSprite _pauseBackground;

        StaticSprite _gameoverWinScreen;
        WinScreen _winScreen;
        LoseScreen _loseScreen;

        HUDManager _hudManager;

        ShiftInterface _shiftInterface;
        private GameState _deprecatedState;

        public ChaoticMindPlayable() {
            _gameObjects = new GameObjects();

            PainStaticMaker.Initialize();

            CharacterType.Initialize();
            ParticleType.Initialize();

            _winScreen = new WinScreen();
            _loseScreen = new LoseScreen();

            _deprecatedState = new GameState(Objects); 

            _pauseBackground = new StaticSprite("UI/PauseScreen", 1, DrawLayers.Menu.Backgrounds);

            _gameoverWinScreen = new StaticSprite("Screens/WinScreen", 1, DrawLayers.Menu.Backgrounds);

            //Start the game
            Objects.StartNewGame(MAP_SIZE);
            _deprecatedState.StartNewGame(3);

            _hudManager = new HUDManager(_gameObjects);
            _shiftInterface = new ShiftInterface(_gameObjects);
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public  void Update(float deltaTime) {

            //Allows the game to exit
            if (InputManager.IsKeyDown(Keys.Escape)) {
                return;
            }

            switch (_deprecatedState.Mode) {
                case GameState.GameMode.NORMAL:
                    normalGameUpdate(deltaTime);
                    break;
                case GameState.GameMode.PAUSED:
                    if (InputManager.IsKeyClicked(Keys.P)) {
                        _deprecatedState.Mode = GameState.GameMode.NORMAL;
                    }
                    break;
                case GameState.GameMode.SHIFTING:
                     _shiftInterface.Update(deltaTime);

                     if (InputManager.IsKeyClicked(Keys.Space)) {
                         _deprecatedState.Mode = GameState.GameMode.NORMAL;
                     }
                    break;
                case GameState.GameMode.GAMEOVERWIN:
                    if (InputManager.IsMouseClicked) {
                        _deprecatedState.Mode = GameState.GameMode.PREGAME;
                    }
                    break;
                case GameState.GameMode.GAMEOVERLOSE:
                    _loseScreen.Update(deltaTime);
                    Objects.MainCamera.Update(deltaTime);
                    PainStaticMaker.Update(deltaTime);

                    if (_loseScreen.TimerFinished() && InputManager.IsMouseClicked) {
                        _deprecatedState.Mode = GameState.GameMode.PREGAME;
                    }
                    break;
                case GameState.GameMode.EXITED:
                    return;
                default:
                    throw new NotImplementedException("The GameState.GameMode " + _deprecatedState.Mode + " is not handled");
            }
        }

        private void normalGameUpdate(float deltaTime) {
            if (Objects.MainPlayer.ShouldBeKilled) {
                _deprecatedState.Mode = GameState.GameMode.GAMEOVERLOSE;
            } else if (_deprecatedState.AllObjectivesCollected) {
                _deprecatedState.Mode = GameState.GameMode.GAMEOVERWIN;
            } else if (InputManager.IsKeyClicked(Keys.P)) {
                _deprecatedState.Mode = GameState.GameMode.PAUSED;
            } else if (InputManager.IsKeyClicked(Keys.Space)) {
                _deprecatedState.Mode = GameState.GameMode.SHIFTING;
            }

            Objects.Update(deltaTime);

            PainStaticMaker.Update(deltaTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(float deltaTime) {

            switch (_deprecatedState.Mode) {
                case GameState.GameMode.NORMAL:
                    DrawGameBoard();
                    DrawHUD();
                    MouseDrawer.Draw(MouseType.REDICAL);
                    break;
                case GameState.GameMode.PAUSED:
                    DrawGameBoard();
                    drawPauseOverlay();
                    MouseDrawer.Draw(MouseType.POINTER);
                    break;
                case GameState.GameMode.SHIFTING:
                    DrawGameBoard();
                    _shiftInterface.Draw();
                    Objects.DrawOnShiftInterface(_shiftInterface);
                    MouseDrawer.Draw(MouseType.POINTER);
                    break;
                case GameState.GameMode.GAMEOVERWIN:
                    _winScreen.Draw();
                    break;
                case GameState.GameMode.GAMEOVERLOSE:
                    DrawGameBoard();
                    _loseScreen.Draw();
                    break;
                case GameState.GameMode.EXITED:
                    return;
                default:
                    throw new NotImplementedException("The GameState.GameMode " + _deprecatedState.Mode + " is not handled");
            }
        }

        private void DrawHUD() {
            _hudManager.Draw();
            Objects.DrawMinimap(Objects.MainCamera);
        }

        private void DrawGameBoard() {
            Objects.DrawObjects(Objects.MainCamera);
            Program.SpriteBatch.End();

            /**** Draw Glow Effects ****/
            //Using BlendState.Additive will make things drawn in this section only brighten, never darken.
            //This means colors will be intensified, and look like glow
            Program.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);

            Objects.DrawGlows(Objects.MainCamera);
            PainStaticMaker.DrawStatic();

            Program.SpriteBatch.End();

            //Prep for others' drawing
            Program.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
        }

        private void drawPauseOverlay() {
            Program.SpriteBatch.Draw(_pauseBackground.Texture, ScreenUtils.Center, _pauseBackground.CurrentTextureBounds, Color.White, 0.0f, _pauseBackground.CurrentTextureOrigin, (ScreenUtils.SmallestDimension - 200) / (float)_pauseBackground.CurrentTextureBounds.Width, SpriteEffects.None, DrawLayers.Menu.Backgrounds);
        }

        internal void closeShiftInterface() {
            _deprecatedState.Mode = GameState.GameMode.NORMAL;
        }

        public IGameFlowComponent NextComponent { get; set; }
    }
}
