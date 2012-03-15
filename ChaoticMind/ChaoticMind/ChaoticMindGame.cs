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
    public class ChaoticMindGame : Microsoft.Xna.Framework.Game {
        enum GameState {
            NORMAL,
            PAUSED,
            SHIFTING
        }
        //screen size
        const int MAX_X = 1440;
        const int MAX_Y = 800;

        //map dimension
        const int MAP_SIZE = 10;
        
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        SpriteFont _debugFont;

        StaticSprite _pauseBackground;
        Vector2 _pauseLocation = new Vector2(MAX_X / 2.0f, MAX_Y / 2.0f); //half width and height

        FrameRateCounter _fpsCounter;

        //Farseer physics simulator
        World _world;

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

        //Any objects in this array will have Update called on them and be drawn by the _mainCamera object
        List<DrawableGameObject> _objects = new List<DrawableGameObject>();

        MapManager _mapManager;
        
        //state of the game
        private GameState _gameState = GameState.NORMAL;

        public ChaoticMindGame() {

            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = MAX_X;
            _graphics.PreferredBackBufferHeight = MAX_Y;
            _graphics.IsFullScreen = false;

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

            //Create a bunch of fun random game objects for now
            for (int i = 0; i < 100; i++) {
                SillyBox obj = new SillyBox(CharacterType.SillyBox, _world, Utilities.randomVector() * 100.0f + 100.0f * Vector2.UnitX);
                _objects.Add(obj);
            }

            //set up player
            _player = new Player(CharacterType.Player, _world, Vector2.Zero);
            _objects.Add(_player);
            _mainCamera.setTarget(_player.Body);

            //set up collectable
            _currentCollectable = new Collectable("TestImages/Collectable", 5, 2, 2, _world, new Vector2(Utilities.randomInt(0, 2), Utilities.randomInt(0, 2)) * MapTile.TileSideLength);
            _objects.Add(_currentCollectable);

            //_backgroundMusic = new MusicController();
            //_backgroundMusic.Enqueue("testSound1");
            //_backgroundMusic.Enqueue("testSound2");
            //_backgroundMusic.Enqueue("testSound3");
            //_backgroundMusic.Play();


            _mapManager = new MapManager(MAP_SIZE, MAP_SIZE);
            _mapManager.Initialize(_world, _mainCamera);

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

            //must call once BEFORE any keyboard/mouse operations
            InputManager.Update(deltaTime);
            
            //Allows the game to exit
            if (InputManager.IsKeyDown(Keys.Escape)) {
                this.Exit();
            }
            //pause/unpause
            if (InputManager.IsKeyClicked(Keys.P)) {
                _gameState = _gameState == GameState.PAUSED ? GameState.NORMAL : GameState.PAUSED;
            }
            //shifting interface
            if (InputManager.IsKeyClicked(Keys.E)) {
                _gameState = _gameState == GameState.SHIFTING ? GameState.NORMAL : GameState.SHIFTING;
            }

            if (_gameState == GameState.NORMAL) {
                //Update all objects in our list. This is not where physics is evaluated,
                // it is only where object-specific actions are performed, like applying control forces
                foreach (DrawableGameObject obj in _objects) {
                    obj.Update(deltaTime);
                }

                _mainCamera.Update(deltaTime);

                _mapManager.Update(deltaTime);

                //Update the FarseerPhysics physics
                _world.Step(deltaTime);
            }
            else if (_gameState == GameState.SHIFTING) {
                //update stuff for the shifting overlay
            }
            else if (_gameState == GameState.PAUSED) {
                //update stuff for the pause menu
            }

            _fpsCounter.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            //Draw map tiles
            _mapManager.DrawTiles(_mainCamera);

            //Draw minimap
            _mapManager.DrawMap(_mainCamera);

            //Draw all objects in our list (and their minimap representations)
            foreach (DrawableGameObject obj in _objects) {
                _mainCamera.Draw(obj);
                _mainCamera.DrawMinimap(obj);
            }

            if (_gameState == GameState.PAUSED) {
                //_spriteBatch.DrawString(_debugFont, "Game is paused", new Vector2(600.0f, 400.0f), Color.White);
                _spriteBatch.Draw(_pauseBackground.Texture, _pauseLocation, _pauseBackground.CurrentTextureBounds, Color.White, 0.0f, _pauseBackground.CurrentTextureOrigin, 3, SpriteEffects.None, 0.0f);
            }
            else if (_gameState == GameState.SHIFTING) {
                _spriteBatch.DrawString(_debugFont, "Shifting interface is enabled", new Vector2(600.0f, 400.0f), Color.White);
            }

            /*Debugging writing*/
            _fpsCounter.Draw(gameTime);
            _spriteBatch.DrawString(_debugFont, string.Format("Player: ({0:0}, {1:0})", _player.Position.X, _player.Position.Y), new Vector2(10.0f, 40.0f), Color.White);
            _spriteBatch.DrawString(_debugFont, string.Format("In Tile: ({0:0}, {1:0})", _player.MapTileIndex.X, _player.MapTileIndex.Y), new Vector2(10.0f, 65.0f), Color.White);

            Vector2 mouseLocation = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            Vector2 worldMouseLocation = _mainCamera.screenPointToWorld(mouseLocation);
            _spriteBatch.DrawString(_debugFont, string.Format(".({0:0}, {1:0})", worldMouseLocation.X, worldMouseLocation.Y), mouseLocation, Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
