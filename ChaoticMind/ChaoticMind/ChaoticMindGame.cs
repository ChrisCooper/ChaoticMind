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

        bool _goFullscreen = false;

        //map dimension
        const int MAP_SIZE = 4;

        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        StaticSprite _pauseBackground;
        Vector2 _centreLocation;

        
        StaticSprite _gameoverWinScreen;
        StaticSprite _startMenuScreen;
        StaticSprite _startMenuScreenOverlay;

        FrameRateCounter _fpsCounter;

        //Farseer physics simulator
        World _world;
        internal World MainWorld {
            get { return _world; }
        }

        //Draws the objects
        Camera _mainCamera;
        internal Camera MainCamera {
            get { return _mainCamera; }
        }

        //player
        Player _player;
        internal Player MainPlayer {
            get { return _player; }
        }

        //Audio
        MusicController _backgroundMusic;

        HUD.HUDManager _hudManager = new HUD.HUDManager();

        MouseDrawer _mouseDrawer = new MouseDrawer();

        MapManager _mapManager;
        internal MapManager MapManager {
            get { return _mapManager; }
        }

        ProjectileManager _projectileManager = ProjectileManager.mainInstance();
        CollectibleManager _collectibleManager = CollectibleManager.mainInstance();
        ParticleManager _particleManager = ParticleManager.mainInstance();

        ShiftInterface _shiftInterface = new ShiftInterface();

        public ChaoticMindGame() {

            _graphics = new GraphicsDeviceManager(this);

            Screen.Initialize(_graphics, _goFullscreen);

            _centreLocation = new Vector2(Screen.Width / 2.0f, Screen.Height / 2.0f);

            Content.RootDirectory = "Content";
            SpriteAnimationSequence.SharedContentManager = Content;

            MusicController.SharedContentManager = Content;
            SoundEffectManager.SharedContentManager = Content;

            //Create the physics simulator object, specifying that we want no gravity (since we're top-down)
            _world = new World(Vector2.Zero);
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

            _fpsCounter = new FrameRateCounter(_spriteBatch, FontManager.DebugFont);

            _hudManager.Initialize();

            _mapManager = new MapManager();

            _mainCamera = new Camera(Vector2.Zero, 50.0f, _graphics.GraphicsDevice, _spriteBatch);

            InputManager.Initialize();
            GameState.Initilize();
            AIDirector.Initilize();
            SoundEffectManager.Initilize();

            _backgroundMusic = new MusicController();
            //_backgroundMusic.Enqueue("testSound1");
            //_backgroundMusic.Enqueue("testSound2");
            //_backgroundMusic.Enqueue("testSound3");
            //_backgroundMusic.Enqueue("01 Cryogenic Dreams");
            //_backgroundMusic.Enqueue("05 Rapid Cognition");
            //_backgroundMusic.Enqueue("10 Disappear");
            //_backgroundMusic.Play();

            _shiftInterface.Initialize(_mapManager, _spriteBatch);

            PainStaticMaker.Initialize();

            _mouseDrawer.Initialize();

            ParticleType.Initialize();

            LoseScreen.Initialize();

            StartNewGame();

            base.Initialize();
        }

        /// <summary>
        /// Anything that will have to be undone or redone to start a new game without restarting the program should be done in here.
        /// This includes pretty much everything except loading of resources and basic non-map-specific initialization.
        /// </summary>
        private void StartNewGame() {

            _mapManager.StartNewGame(MAP_SIZE);

            _shiftInterface.StartNewGame();

            _mainCamera.StartNewGame();

            //init the level
            GameState.StartNewGame(1, 3);

            AIDirector.StartNewGame();

            _player = new Player(Vector2.Zero);
            _mainCamera.setTarget(_player.Body);

        }

        /// <summary>
        /// Undoes StartNewGame() so that a new game can be created.
        /// </summary>
        private void ClearGame() {

            _mapManager.ClearGame();
            AIDirector.ClearGame();
            GameState.ClearGame();
            PainStaticMaker.ClearGame();
            ParticleManager.ClearGame();
        }

        /// <summary>
        /// This method puts the game back to a beginning state.
        /// </summary>
        private void ResetGame() {

            _mainCamera.resetZoom();

            ClearGame();

            StartNewGame();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            _pauseBackground = new StaticSprite("UI/PauseScreen", 1, DrawLayers.MenuBackgrounds);
            
            _gameoverWinScreen = new StaticSprite("Screens/WinScreen", 1, DrawLayers.MenuBackgrounds);
            _startMenuScreen = new StaticSprite("Screens/StartMenuScreen", 1, DrawLayers.MenuBackgrounds);
            _startMenuScreenOverlay = new StaticSprite("Screens/StartMenuScreenOverlay", 1, DrawLayers.MenuBackgrounds);
            //_1pxBlack = new StaticSprite("1pxBlack", 1, DrawLayers.MenuBackgrounds);
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the _world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            float deltaTime = ((float)gameTime.ElapsedGameTime.TotalMilliseconds) * 0.001f;

            //BULLET TIME!!!! :DDDD (have to pass non-scaled time to player's update though... otherwise player gets slowed too.
            //deltaTime *= 0.2f;

            //must call once BEFORE any keyboard/mouse operations
            InputManager.Update(deltaTime);

            updateGameState();

            if (GameState.Mode == GameState.GameMode.EXITED) {
                return;
            }
            else if (GameState.Mode == GameState.GameMode.PREGAME) {
                //quack goes the duck
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
                LoseScreen.Update(deltaTime);
                _mainCamera.Update(deltaTime);
                PainStaticMaker.Update(deltaTime);
                
            }

            _fpsCounter.Update(gameTime);

            base.Update(gameTime);
        }

        private void normalGameUpdate(float deltaTime) {
            //Update all objects in our list. This is not where physics is evaluated,
            // it is only where object-specific actions are performed, like applying control forces

            _player.Update(deltaTime);

            _projectileManager.Update(deltaTime);
            _collectibleManager.Update(deltaTime);
            _particleManager.Update(deltaTime);

            _mainCamera.Update(deltaTime);

            _mapManager.Update(deltaTime);

            PainStaticMaker.Update(deltaTime);

            AIDirector.Update(deltaTime);

            //Update the FarseerPhysics physics
            _world.Step(deltaTime);
        }

        private void updateGameState() {
            //Allows the game to exit
            if (InputManager.IsKeyDown(Keys.Escape)) {
                ClearGame();
                GameState.Mode = GameState.GameMode.EXITED;
                this.Exit();

            }
            
            //debug
            /*if (InputManager.IsKeyDown(Keys.Y)) {
                ResetGame();
            }
            if (InputManager.IsKeyDown(Keys.K)) {
                GameState.Mode = GameState.GameMode.GAMEOVERLOSE;
            }
            */ 

            //menu screen progression
            if (GameState.Mode == GameState.GameMode.PREGAME && InputManager.IsMouseClicked()) {
                GameState.Mode = GameState.GameMode.NORMAL;
            }
            if ((GameState.Mode == GameState.GameMode.GAMEOVERWIN) && InputManager.IsMouseClicked()) {
                ResetGame();
                GameState.Mode = GameState.GameMode.PREGAME;
            }

            //pause/unpause
            if (InputManager.IsKeyClicked(Keys.P) && GameState.Mode == GameState.GameMode.PAUSED) {
                GameState.Mode = GameState.GameMode.NORMAL;
                SoundEffectManager.ResumeAll();
            }
            else if (InputManager.IsKeyClicked(Keys.P) && GameState.Mode == GameState.GameMode.NORMAL) {
                GameState.Mode = GameState.GameMode.PAUSED;
                SoundEffectManager.PauseAll();
            }

            //shifting interface
            if (InputManager.IsKeyClicked(Keys.Space) && GameState.Mode == GameState.GameMode.SHIFTING) {
                GameState.Mode = GameState.GameMode.NORMAL;
                SoundEffectManager.ResumeAll();
            }
            else if (InputManager.IsKeyClicked(Keys.Space) && GameState.Mode == GameState.GameMode.NORMAL) {
                GameState.Mode = GameState.GameMode.SHIFTING;
                SoundEffectManager.PauseAll();
            }

            //you died
            if (_player.ShouldDieNow()) {
                GameState.Mode = GameState.GameMode.GAMEOVERLOSE;
                SoundEffectManager.StopAll();
            }
            if (GameState.BossActive) {
                GameState.Mode = GameState.GameMode.GAMEOVERWIN;
                SoundEffectManager.StopAll();
            }
        }

        internal void AdvanceToNextGameState() {
            if (GameState.Mode == GameState.GameMode.GAMEOVERLOSE) {
                ResetGame();
                GameState.Mode = GameState.GameMode.PREGAME;
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

            //Draw Minimap
            _mapManager.DrawMap(_mainCamera);
            AIDirector.DrawMinimap(_mainCamera);
            _collectibleManager.DrawMinimap(_mainCamera);

            _mainCamera.DrawMinimap(_player);


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
                _shiftInterface.drawOnOverlay(_player);
                AIDirector.DrawOnShiftInterface(_shiftInterface);
                _collectibleManager.DrawOnShiftInterface(_shiftInterface);
            }

            drawDebugInfo(gameTime);

            _mouseDrawer.drawMouse(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void drawGameoverWinOverlay() {
            //_spriteBatch.Draw(_gameoverWinScreen.Texture, _centreLocation, _gameoverWinScreen.CurrentTextureBounds, Color.White, 0.0f, _gameoverWinScreen.CurrentTextureOrigin, , SpriteEffects.None, DrawLayers.MenuBackgrounds);
            _spriteBatch.Draw(_gameoverWinScreen.Texture, _centreLocation, _gameoverWinScreen.CurrentTextureBounds, Color.Black, 0.0f, _gameoverWinScreen.CurrentTextureOrigin, 1, SpriteEffects.None, DrawLayers.MenuBackgrounds+0.01f);

            float mapFrameSideLength = Math.Min(Screen.Width, Screen.Height);
            float mapFrameScale = mapFrameSideLength / _gameoverWinScreen.Texture.Bounds.Width;
            Rectangle mapFrameRect = new Rectangle((int)(Screen.Width - mapFrameSideLength) / 2, (int)(Screen.Height - mapFrameSideLength), (int)mapFrameSideLength, (int)mapFrameSideLength);

            float frameWidth = mapFrameSideLength;
            Rectangle mapRect = new Rectangle((int)(mapFrameRect.Left + frameWidth), (int)(mapFrameRect.Top + frameWidth), (int)(mapFrameRect.Width - 2 * frameWidth), (int)(mapFrameRect.Height - 2 * frameWidth));

            //_spriteBatch.Draw(_1pxBlack.Texture, _centreLocation, _1pxBlack.CurrentTextureBounds, Color.White, 0.0f, _1pxBlack.CurrentTextureOrigin, 10000, SpriteEffects.None, DrawLayers.MenuBackgrounds);
            _spriteBatch.Draw(_gameoverWinScreen.Texture, mapRect, _gameoverWinScreen.CurrentTextureBounds, Color.White, 0, Vector2.Zero, SpriteEffects.None, DrawLayers.MenuBackgrounds);
        }

        private void drawStartMenuOverlay(GameTime gameTime) {
            _spriteBatch.Draw(_gameoverWinScreen.Texture, _centreLocation, _gameoverWinScreen.CurrentTextureBounds, Color.Black, 0.0f, _gameoverWinScreen.CurrentTextureOrigin, 1, SpriteEffects.None, DrawLayers.MenuBackgrounds - 0.001f);

            float mapFrameSideLength = Math.Min(Screen.Width, Screen.Height);
            float mapFrameScale = mapFrameSideLength / _startMenuScreen.Texture.Bounds.Width;
            Rectangle mapFrameRect = new Rectangle((int)(Screen.Width - mapFrameSideLength) / 2, (int)(Screen.Height - mapFrameSideLength), (int)mapFrameSideLength, (int)mapFrameSideLength);

            _spriteBatch.Draw(_startMenuScreen.Texture, mapFrameRect, _startMenuScreen.CurrentTextureBounds, Color.White, 0, Vector2.Zero, SpriteEffects.None, DrawLayers.MenuBackgrounds - 0.002f);
            _spriteBatch.Draw(_startMenuScreenOverlay.Texture, mapFrameRect, _startMenuScreenOverlay.CurrentTextureBounds, Color.White * 0.5f * ((float)(Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / 500.0f) + 1.0f) + 0.5f), 0, Vector2.Zero, SpriteEffects.None, DrawLayers.MenuBackgrounds - 0.003f);
        }

        private void drawGlows(GameTime gameTime) {
            _particleManager.DrawGlows(_mainCamera);
            _projectileManager.DrawGlows(_mainCamera);
            //_collectibleManager.DrawGlows(_mainCamera);
        }

        private void drawObjects(GameTime gameTime) {
            //Draw map tiles
            _mapManager.DrawTiles(_mainCamera, (float)gameTime.TotalGameTime.TotalMilliseconds);

            _mainCamera.Draw(_player);
            AIDirector.Draw(_mainCamera);
            _particleManager.Draw(_mainCamera);
            _projectileManager.Draw(_mainCamera);
            _collectibleManager.Draw(_mainCamera);
        }

        private void drawPauseOverlay() {
            //_spriteBatch.DrawString(_debugFont, "Game is paused", new Vector2(600.0f, 400.0f), Color.White);
            _spriteBatch.Draw(_pauseBackground.Texture, _centreLocation, _pauseBackground.CurrentTextureBounds, Color.White, 0.0f, _pauseBackground.CurrentTextureOrigin, 3, SpriteEffects.None, DrawLayers.MenuBackgrounds);

        }

        private void drawDebugInfo(GameTime gameTime) {
            //_fpsCounter.Draw(gameTime);
            //_spriteBatch.DrawString(FontManager.DebugFont, string.Format("Player: ({0:0}, {1:0})", _player.Position.X, _player.Position.Y), new Vector2(10.0f, 40.0f), Color.White);
            //_spriteBatch.DrawString(FontManager.DebugFont, string.Format("In Tile: ({0:0}, {1:0})", _player.GridCoordinate.X, _player.GridCoordinate.Y), new Vector2(10.0f, 65.0f), Color.White);
        }

        internal void closeShiftInterface() {
            GameState.Mode = GameState.GameMode.NORMAL;
        }
    }
}
