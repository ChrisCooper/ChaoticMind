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

    enum GameMode {
        NORMAL,
        PAUSED,
        SHIFTING
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 
    public class ChaoticMindGame : Microsoft.Xna.Framework.Game {

        bool _goFullscreen = false;

        //map dimension
        const int MAP_SIZE = 10;
        
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        SpriteFont _debugFont;

        StaticSprite _pauseBackground;
        Vector2 _pauseLocation;

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

        //collectable objects
        Collectable _currentCollectable;

        //Audio
        MusicController _backgroundMusic;

        MouseDrawer _mouseDrawer = new MouseDrawer();

        //Any objects in this array will have Update called on them and be drawn by the _mainCamera object
        List<DrawableGameObject> _objects = new List<DrawableGameObject>();

        MapManager _mapManager;
        ProjectileManager _projectileManager;

        ShiftInterface _shiftInterface = new ShiftInterface();
        
        //state of the game
        private GameMode _gameState = GameMode.NORMAL;

        public ChaoticMindGame() {

            _graphics = new GraphicsDeviceManager(this);

            Screen.Initialize(_graphics, _goFullscreen);

             _pauseLocation = new Vector2(Screen.Width / 2.0f, Screen.Height / 2.0f);

            Content.RootDirectory = "Content";
            SpriteAnimationSequence.SharedContentManager = Content;

            MusicController.SharedContentManager = Content;

            //Create the physics simulator object, specifying that we want no gravity (since we're top-down)
            _world = new World(Vector2.Zero);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {

            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _debugFont = Content.Load<SpriteFont>("DebugFont");
            _fpsCounter = new FrameRateCounter(_spriteBatch, _debugFont);

            _mainCamera = new Camera(Vector2.Zero, 50.0f, _graphics.GraphicsDevice, _spriteBatch);
            
            InputManager.Initialize();
            TimeDelayManager.Initilize();
            GameState.Initilize();

            //Create a bunch of fun random game objects for now
            for (int i = 0; i < 100; i++) {
                SillyBox obj = new SillyBox(CharacterType.SillyBox, Utilities.randomVector() * 100.0f + 100.0f * Vector2.UnitX);
                _objects.Add(obj);
            }

            //set up player
            _player = new Player(CharacterType.Player, Vector2.Zero);
            _objects.Add(_player);
            _mainCamera.setTarget(_player.Body);

            //set up collectable
            _currentCollectable = new Collectable("TestImages/Collectable", 5, 2, 2, new Vector2(Utilities.randomInt(0, 2), Utilities.randomInt(0, 2)) * MapTile.TileSideLength);
            _objects.Add(_currentCollectable);

            _backgroundMusic = new MusicController();
            //_backgroundMusic.Enqueue("testSound1");
            //_backgroundMusic.Enqueue("testSound2");
            //_backgroundMusic.Enqueue("testSound3");
            _backgroundMusic.Enqueue("01 Cryogenic Dreams");
            _backgroundMusic.Enqueue("05 Rapid Cognition");
            _backgroundMusic.Enqueue("10 Disappear");
            //_backgroundMusic.Play();

            _mapManager = new MapManager(MAP_SIZE);
            _mapManager.Initialize(_mainCamera, ref _objects);

            _projectileManager = new ProjectileManager();
            _projectileManager.Initilize(_mainCamera);

            _shiftInterface.Initialize(_mapManager, _spriteBatch);

            _mouseDrawer.Initialize();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            _pauseBackground = new StaticSprite("Menus/PauseScreen", 1);
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

            if (_gameState == GameMode.NORMAL) {
                normalGameUpdate(deltaTime);
            }
            else if (_gameState == GameMode.SHIFTING) {
                _shiftInterface.Update();
            }
            else if (_gameState == GameMode.PAUSED) {
                //update stuff for the pause menu
            }

            _fpsCounter.Update(gameTime);

            base.Update(gameTime);
        }

        private void normalGameUpdate(float deltaTime) {
            //Update all objects in our list. This is not where physics is evaluated,
            // it is only where object-specific actions are performed, like applying control forces

            //let time-based objects work
            TimeDelayManager.Update(deltaTime);

            for (int i = 0 ; i < _objects.Count ; i++){
                if (_objects[i].KillMe()){
                    _objects[i].Destroy();
                    _objects.RemoveAt(i);
                }
                else{
                    _objects[i].Update(deltaTime);
                }
            }

            _projectileManager.Update(deltaTime);

            _mainCamera.Update(deltaTime);

            _mapManager.Update(deltaTime);

            //Update the FarseerPhysics physics
            _world.Step(deltaTime);
        }

        private void updateGameState() {
            //Allows the game to exit
            if (InputManager.IsKeyDown(Keys.Escape)) {
                this.Exit();
            }
            //pause/unpause
            if (InputManager.IsKeyClicked(Keys.P)) {
                _gameState = _gameState == GameMode.PAUSED ? GameMode.NORMAL : GameMode.PAUSED;
            }
            //shifting interface
            if (InputManager.IsKeyClicked(Keys.Tab)) {
                _gameState = _gameState == GameMode.SHIFTING ? GameMode.NORMAL : GameMode.SHIFTING;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            drawObjects(gameTime);

            //Draw minimap
            _mapManager.DrawMap(_mainCamera);

            if (_gameState == GameMode.PAUSED) {
                drawPauseOverlay();
            }
            else if (_gameState == GameMode.SHIFTING) {
                _shiftInterface.DrawInterface(_objects);
            }

            drawDebugInfo(gameTime);

            _mouseDrawer.drawMouse(_gameState, _spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void drawPauseOverlay() {
            //_spriteBatch.DrawString(_debugFont, "Game is paused", new Vector2(600.0f, 400.0f), Color.White);
            _spriteBatch.Draw(_pauseBackground.Texture, _pauseLocation, _pauseBackground.CurrentTextureBounds, Color.White, 0.0f, _pauseBackground.CurrentTextureOrigin, 3, SpriteEffects.None, 0.0f);

        }

        private void drawDebugInfo(GameTime gameTime) {
            _fpsCounter.Draw(gameTime);
            _spriteBatch.DrawString(_debugFont, string.Format("Player: ({0:0}, {1:0})", _player.Position.X, _player.Position.Y), new Vector2(10.0f, 40.0f), Color.White);
            _spriteBatch.DrawString(_debugFont, string.Format("In Tile: ({0:0}, {1:0})", _player.MapTileIndex.X, _player.MapTileIndex.Y), new Vector2(10.0f, 65.0f), Color.White);
        }

        private void drawObjects(GameTime gameTime) {
            //Draw map tiles
            _mapManager.DrawTiles(_mainCamera, (float)gameTime.TotalGameTime.TotalMilliseconds);

            //Draw all objects in our list (and their minimap representations)
            foreach (DrawableGameObject obj in _objects) {
                _mainCamera.Draw(obj);
                _mainCamera.DrawMinimap(obj);
            }

            _projectileManager.Draw();
        }

        internal void closeShiftInterface() {
            _gameState = GameMode.NORMAL;
        }
    }
}
