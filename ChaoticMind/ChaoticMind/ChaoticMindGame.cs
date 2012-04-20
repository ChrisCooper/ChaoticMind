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

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 
    public class ChaoticMindGame : Microsoft.Xna.Framework.Game {

        GameObjects _gameObjects;
        internal GameObjects Objects {
            get { return _gameObjects; }
        }

        bool _goFullscreen = false;

        //map dimension
        const int MAP_SIZE = 4;

        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        StaticSprite _pauseBackground;
        Vector2 _centreLocation;

        Texture2D _blackPx;
        internal Texture2D BlackPx {
            get { return _blackPx; }
        }
        
        StaticSprite _gameoverWinScreen;
        StaticSprite _startMenuScreen;
        StaticSprite _startMenuScreenOverlay;
        //Audio
        MusicController _backgroundMusic;

        HUD.HUDManager _hudManager = new HUD.HUDManager();

        MouseDrawer _mouseDrawer = new MouseDrawer();

        ShiftInterface _shiftInterface = new ShiftInterface();

        public ChaoticMindGame() {

            _graphics = new GraphicsDeviceManager(this);

            Screen.Initialize(_graphics, _goFullscreen);

            _centreLocation = new Vector2(Screen.Width / 2.0f, Screen.Height / 2.0f);

            Content.RootDirectory = "Content";
            SpriteAnimationSequence.SharedContentManager = Content;

            MusicController.SharedContentManager = Content;
            SoundEffectManager.SharedContentManager = Content;

            _gameObjects = new GameObjects();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.StartNewGame will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {

            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _hudManager.Initialize();

            //set up the black pixel used for clearing the screen
            _blackPx = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            uint[] px = { 0xFFFFFFFF };
            _blackPx.SetData<uint>(px);

            InputManager.Initialize();

            SoundEffectManager.Initilize();

            //initial menu music
            _backgroundMusic = new MusicController();
            _backgroundMusic.Enqueue("Predatory Instincts_ElevatorRemix");
            _backgroundMusic.Play();

            PainStaticMaker.Initialize();

            _mouseDrawer.Initialize();

            CharacterType.Initialize();
            ParticleType.Initialize();

            LoseScreen.Initialize();

            GameState.Initilize();
            StartNewGame();

            _shiftInterface.Initialize(_spriteBatch);

            base.Initialize();
        }

        /// <summary>
        /// Anything that will have to be undone or redone to start a new game without restarting the program should be done in here.
        /// This includes pretty much everything except loading of resources and basic non-map-specific initialization.
        /// </summary>
        private void StartNewGame() {
            Objects.StartNewGame(MAP_SIZE);

            _shiftInterface.StartNewGame();

            //init the level
            GameState.StartNewGame(1, 3);
        }

        /// <summary>
        /// Undoes StartNewGame() so that a new game can be created.
        /// </summary>
        private void ClearGame() {
            GameState.ClearGame();
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
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            _pauseBackground = new StaticSprite("UI/PauseScreen", 1, DrawLayers.Menu.Backgrounds);
            
            _gameoverWinScreen = new StaticSprite("Screens/WinScreen", 1, DrawLayers.Menu.Backgrounds);
            _startMenuScreen = new StaticSprite("Screens/StartMenuScreen", 1, DrawLayers.Menu.Backgrounds);
            _startMenuScreenOverlay = new StaticSprite("Screens/StartMenuScreenOverlay", 1, DrawLayers.Menu.Backgrounds);
            //_1pxBlack = new StaticSprite("1pxBlack", 1, DrawLayers.MenuBackgrounds);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            float deltaTime = ((float)gameTime.ElapsedGameTime.TotalMilliseconds) * 0.001f;

            //must call once BEFORE any keyboard/mouse operations
            InputManager.Update(deltaTime);

            updateGameState();

            if (GameState.Mode == GameState.GameMode.EXITED) {
                return;
            }
            else if (GameState.Mode == GameState.GameMode.PREGAME) {

            }
            else if (GameState.Mode == GameState.GameMode.NORMAL) {
                normalGameUpdate(deltaTime);
            }
            else if (GameState.Mode == GameState.GameMode.SHIFTING) {
                _shiftInterface.Update(deltaTime);
            }
            else if (GameState.Mode == GameState.GameMode.PAUSED) {
                //update stuff for the pause menu
            }
            else if (GameState.Mode == GameState.GameMode.GAMEOVERWIN) {
                //quack goes the duck
            }
            else if (GameState.Mode == GameState.GameMode.GAMEOVERLOSE) {
                _backgroundMusic.Stop();
                LoseScreen.Update(deltaTime);
                Objects.MainCamera.Update(deltaTime);
                PainStaticMaker.Update(deltaTime);
            }

            base.Update(gameTime);
        }

        private void normalGameUpdate(float deltaTime) {
            //Update all objects in our list. This is not where physics is evaluated,
            // it is only where object-specific actions are performed, like applying control forces

            Objects.Update(deltaTime);

            PainStaticMaker.Update(deltaTime);
        }

        private void updateGameState() {
            //Allows the game to exit
            if (InputManager.IsKeyDown(Keys.Escape)) {
                ClearGame();
                GameState.Mode = GameState.GameMode.EXITED;
                this.Exit();
                return;
            }
           

            //menu screen progression
            if (GameState.Mode == GameState.GameMode.PREGAME && InputManager.IsMouseClicked()) {
                //game music
                _backgroundMusic.Stop();
                _backgroundMusic.ClearQueue();
                _backgroundMusic.Enqueue("01 Cryogenic Dreams");
                _backgroundMusic.Enqueue("05 Rapid Cognition");
                _backgroundMusic.Enqueue("10 Disappear");
                _backgroundMusic.Play();
                GameState.Mode = GameState.GameMode.NORMAL;
            }
            if ((GameState.Mode == GameState.GameMode.GAMEOVERWIN || (GameState.Mode == GameState.GameMode.GAMEOVERLOSE && LoseScreen.TimerFinished())) && InputManager.IsMouseClicked()) {
                ResetGame();
                _backgroundMusic.Stop();
                _backgroundMusic.ClearQueue();
                _backgroundMusic.Enqueue("Predatory Instincts_ElevatorRemix");
                _backgroundMusic.Play();
                GameState.Mode = GameState.GameMode.PREGAME;
            }
            
            //pause/unpause
            if (InputManager.IsKeyClicked(Keys.P) && GameState.Mode == GameState.GameMode.PAUSED) {
                GameState.Mode = GameState.GameMode.NORMAL;
                SoundEffectManager.ResumeSound("shift");
            }
            else if (InputManager.IsKeyClicked(Keys.P) && GameState.Mode == GameState.GameMode.NORMAL) {
                GameState.Mode = GameState.GameMode.PAUSED;
                SoundEffectManager.PauseSound("shift");
            }

            //shifting interface
            if (InputManager.IsKeyClicked(Keys.Space) && GameState.Mode == GameState.GameMode.SHIFTING) {
                GameState.Mode = GameState.GameMode.NORMAL;
                SoundEffectManager.ResumeSound("shift");
            }
            else if (InputManager.IsKeyClicked(Keys.Space) && GameState.Mode == GameState.GameMode.NORMAL) {
                GameState.Mode = GameState.GameMode.SHIFTING;
                SoundEffectManager.PauseSound("shift");
            }

            //you died
            if (Objects.MainPlayer == null || Objects.MainPlayer.ShouldBeKilled) {
                GameState.Mode = GameState.GameMode.GAMEOVERLOSE;
                SoundEffectManager.StopInstances();
            }
            if (GameState.AllObjectivesCollected) {
                GameState.Mode = GameState.GameMode.GAMEOVERWIN;
                SoundEffectManager.StopInstances();
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            /**** Draw Game Objects ****/
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            //Draw real game
            drawObjects(gameTime);

            _spriteBatch.End();


            /**** Draw Glow Effects ****/
            //Using BlendState.Additive will make things drawn in this section only brighten, never darken.
            //This means colors will be intensified, and look like glow

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);

            //Draw real game
            drawGlows(gameTime);

            PainStaticMaker.DrawStatic(_spriteBatch);

            _spriteBatch.End();



            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            /**** Draw HUD ****/
            _hudManager.Draw_HUD(_spriteBatch);

            Objects.DrawMinimap(Objects.MainCamera);


            /**** Draw Special State Objects ****/

            if (GameState.Mode == GameState.GameMode.PAUSED) {
                drawPauseOverlay();
            }
            else if (GameState.Mode == GameState.GameMode.GAMEOVERWIN) {
                drawGameoverWinOverlay();
            }
            else if (GameState.Mode == GameState.GameMode.GAMEOVERLOSE) {
                LoseScreen.Draw(_spriteBatch);
            }
            else if (GameState.Mode == GameState.GameMode.PREGAME) {
                drawStartMenuOverlay(gameTime);
            }
            else if (GameState.Mode == GameState.GameMode.SHIFTING) {
                _shiftInterface.Draw();
                Objects.DrawOnShiftInterface(_shiftInterface);
            }

            _mouseDrawer.drawMouse(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void drawGameoverWinOverlay() {
            _spriteBatch.Draw(_blackPx, Screen.ScreenRect, Rectangle.Empty, Color.Black, 0.0f, Vector2.Zero, SpriteEffects.None, DrawLayers.Menu.Backgrounds);
            _spriteBatch.Draw(_gameoverWinScreen.Texture, Screen.Center, _gameoverWinScreen.CurrentTextureBounds, Color.White, 0.0f, _gameoverWinScreen.CurrentTextureOrigin, Screen.SmallestDimension / (float)_gameoverWinScreen.CurrentTextureBounds.Width, SpriteEffects.None, DrawLayers.Menu.Backgrounds - 0.001f);
        }

        private void drawStartMenuOverlay(GameTime gameTime) {
            _spriteBatch.Draw(_gameoverWinScreen.Texture, _centreLocation, _gameoverWinScreen.CurrentTextureBounds, Color.Black, 0.0f, _gameoverWinScreen.CurrentTextureOrigin, 1, SpriteEffects.None, DrawLayers.Menu.Backgrounds - 0.001f);

            float mapFrameSideLength = Math.Min(Screen.Width, Screen.Height);
            float mapFrameScale = mapFrameSideLength / _startMenuScreen.Texture.Bounds.Width;
            Rectangle mapFrameRect = new Rectangle((int)(Screen.Width - mapFrameSideLength) / 2, (int)(Screen.Height - mapFrameSideLength), (int)mapFrameSideLength, (int)mapFrameSideLength);

            _spriteBatch.Draw(_startMenuScreen.Texture, mapFrameRect, _startMenuScreen.CurrentTextureBounds, Color.White, 0, Vector2.Zero, SpriteEffects.None, DrawLayers.Menu.Backgrounds - 0.002f);
            _spriteBatch.Draw(_startMenuScreenOverlay.Texture, mapFrameRect, _startMenuScreenOverlay.CurrentTextureBounds, Color.White * 0.5f * ((float)(Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / 500.0f) + 1.0f) + 0.5f), 0, Vector2.Zero, SpriteEffects.None, DrawLayers.Menu.Backgrounds - 0.003f);
        }

        private void drawGlows(GameTime gameTime) {
            Objects.DrawGlows(Objects.MainCamera);
        }

        private void drawObjects(GameTime gameTime) {
            Objects.DrawObjects(Objects.MainCamera);    
        }

        private void drawPauseOverlay() {
            _spriteBatch.Draw(_pauseBackground.Texture, _centreLocation, _pauseBackground.CurrentTextureBounds, Color.White, 0.0f, _pauseBackground.CurrentTextureOrigin, (Screen.SmallestDimension - 200) / (float)_pauseBackground.CurrentTextureBounds.Width, SpriteEffects.None, DrawLayers.Menu.Backgrounds);

        }

        internal void closeShiftInterface() {
            GameState.Mode = GameState.GameMode.NORMAL;
        }

        public SpriteBatch SpriteBatch { get { return _spriteBatch; } }
    }
}
