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

namespace ChaoticMind {

    public class ChaoticMindPlayable : IGameFlowComponent {

        GameObjects _gameObjects;
        internal GameObjects Objects {
            get { return _gameObjects; }
        }

        bool _goFullscreen = false;

        //map dimension
        const int MAP_SIZE = 4;

        StaticSprite _pauseBackground;

        Texture2D _blackPx;
        internal Texture2D BlackPx {
            get { return _blackPx; }
        }

        StaticSprite _gameoverWinScreen;

        HUD.HUDManager _hudManager = new HUD.HUDManager();

        MouseDrawer _mouseDrawer = new MouseDrawer();

        ShiftInterface _shiftInterface = new ShiftInterface();
        private GameState _deprecatedState;

        public ChaoticMindPlayable() {
            _gameObjects = new GameObjects();

            _hudManager.Initialize();

            PainStaticMaker.Initialize();

            CharacterType.Initialize();
            ParticleType.Initialize();

            LoseScreen.Initialize();

            _deprecatedState = new GameState(Objects); 

            _pauseBackground = new StaticSprite("UI/PauseScreen", 1, DrawLayers.Menu.Backgrounds);

            _gameoverWinScreen = new StaticSprite("Screens/WinScreen", 1, DrawLayers.Menu.Backgrounds);

            StartNewGame();
        }


        /// <summary>
        /// Anything that will have to be undone or redone to start a new game without restarting the program should be done in here.
        /// This includes pretty much everything except loading of resources and basic non-map-specific initialization.
        /// </summary>
        private void StartNewGame() {
            Objects.StartNewGame(MAP_SIZE);

            _shiftInterface.StartNewGame();

            //init the level
            _deprecatedState.StartNewGame(3);
        }

        /// <summary>
        /// Undoes StartNewGame() so that a new game can be created.
        /// </summary>
        private void ClearGame() {
            _deprecatedState.ClearGame();
            PainStaticMaker.ClearGame();
            LoseScreen.ClearGame();
            _shiftInterface.ClearGame();

            Objects.ClearGame();
        }

        /// <summary>
        /// This method puts the game back to a beginning state.
        /// </summary>
        private void ResetGame() {
            ClearGame();
            StartNewGame();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public  void Update(float deltaTime) {

            //must call once BEFORE any keyboard/mouse operations
            InputManager.Update(deltaTime);

            //Allows the game to exit
            if (InputManager.IsKeyDown(Keys.Escape)) {
                ClearGame();
                _deprecatedState.Mode = GameState.GameMode.EXITED;
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
                        ResetGame();
                        _deprecatedState.Mode = GameState.GameMode.PREGAME;
                    }
                    break;
                case GameState.GameMode.GAMEOVERLOSE:
                    LoseScreen.Update(deltaTime);
                    Objects.MainCamera.Update(deltaTime);
                    PainStaticMaker.Update(deltaTime);

                    if (LoseScreen.TimerFinished() && InputManager.IsMouseClicked) {
                        ResetGame();
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
                    break;
                case GameState.GameMode.PAUSED:
                    DrawGameBoard();
                    drawPauseOverlay();
                    break;
                case GameState.GameMode.SHIFTING:
                    DrawGameBoard();
                    _shiftInterface.Draw();
                    Objects.DrawOnShiftInterface(_shiftInterface);
                    break;
                case GameState.GameMode.GAMEOVERWIN:
                    drawGameoverWinOverlay();
                    break;
                case GameState.GameMode.GAMEOVERLOSE:
                    DrawGameBoard();
                    LoseScreen.Draw();
                    break;
                case GameState.GameMode.EXITED:
                    return;
                default:
                    throw new NotImplementedException("The GameState.GameMode " + _deprecatedState.Mode + " is not handled");
            }

            Program.SpriteBatch.End();
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

        private void drawGameoverWinOverlay() {
            Program.SpriteBatch.Draw(_blackPx, ScreenUtils.ScreenRect, Rectangle.Empty, Color.Black, 0.0f, Vector2.Zero, SpriteEffects.None, DrawLayers.Menu.Backgrounds);
            Program.SpriteBatch.Draw(_gameoverWinScreen.Texture, ScreenUtils.Center, _gameoverWinScreen.CurrentTextureBounds, Color.White, 0.0f, _gameoverWinScreen.CurrentTextureOrigin, ScreenUtils.SmallestDimension / (float)_gameoverWinScreen.CurrentTextureBounds.Width, SpriteEffects.None, DrawLayers.Menu.Backgrounds - 0.001f);
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
