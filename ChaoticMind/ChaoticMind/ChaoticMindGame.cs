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
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        SpriteFont _debugFont;

        FrameRateCounter _fpsCounter;

        //Farseer physics simulator
        World _world;

        //Draws the objects
        Camera _mainCamera;

        internal Camera MainCamera {
            get { return _mainCamera; }
        }

        ControllableSillyBox _player;

        //Audio
        MusicController _backgroundMusic;

        //Any objects in this array will have Update called on them and be drawn by the _mainCamera object
        List<DrawableGameObject> _objects = new List<DrawableGameObject>();

        MapManager _mapManager;

        public ChaoticMindGame() {

            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1400; //_graphics.GraphicsDevice.DisplayMode.Width;
            _graphics.PreferredBackBufferHeight = 800; //_graphics.GraphicsDevice.DisplayMode.Height;
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
            for (int i = 0; i < 50; i++) {
                float distance = 100.0f;

                SillyBox obj = new SillyBox(CharacterType.SillyBox, _world, Utilities.randomVector() * distance + distance * Vector2.UnitX);
                _objects.Add(obj);

                SillyBox obj2 = new SillyBox(CharacterType.CountingBox, _world, Utilities.randomVector() * distance + distance * Vector2.UnitX);
                _objects.Add(obj2);
            }

            _player = new ControllableSillyBox(CharacterType.ControllableBox, _world, Vector2.Zero);
            _objects.Add(_player);
            _mainCamera.setTarget(_player.Body);


            _backgroundMusic = new MusicController();
            //_backgroundMusic.Enqueue(0);  //Load first wav from Music content to play
            //_backgroundMusic.Play();    //Start playing the music from the current queue


            _mapManager = new MapManager(10, 10);
            _mapManager.Initialize(_world);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
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

            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                this.Exit();
            }

            _fpsCounter.Update(gameTime);

            InputManager.Update(deltaTime);

            //Update all objects in our list. This is not where physics is evaluated,
            // it is only where object-specific actions are performed, like applying control forces
            foreach (DrawableGameObject obj in _objects) {
                obj.Update(deltaTime);
            }

            _mainCamera.Update(deltaTime);

            _mapManager.Update(deltaTime);

            //Update the FarseerPhysics physics
            _world.Step(deltaTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            _mapManager.DrawMap(_mainCamera);

            //Draw all objects in our list
            foreach (DrawableGameObject obj in _objects) {
                _mainCamera.Draw(obj);
            }

            /*Debugging writing*/
            _fpsCounter.Draw(gameTime);
            _spriteBatch.DrawString(_debugFont, string.Format("Player: ({0:0}, {1:0})", _player.Position.X, _player.Position.Y), new Vector2(10.0f, 40.0f), Color.White);
            Vector2 mouseLocation = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            Vector2 worldMouseLocation = _mainCamera.screenPointToWorld(mouseLocation);
            _spriteBatch.DrawString(_debugFont, string.Format(".({0:0}, {1:0})", worldMouseLocation.X, worldMouseLocation.Y), mouseLocation, Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
