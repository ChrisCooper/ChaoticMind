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

namespace ChaoticMind
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ChaoticMindGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        //Farseer physics simulator
        World _world;

        //Draws the objects
        Camera _mainCamera;

        //Any objects in this array will have Update called on them and be drawn by the _mainCamera object
        List<DrawableGameObject> _objects = new List<DrawableGameObject>();

        public ChaoticMindGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            SpriteAnimationSequence.SharedContentManager = Content;

            //Create the physics simulator object, specifying that we want no gravity (since we're top-down)
            _world = new World(Vector2.Zero);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _mainCamera = new Camera(Vector2.Zero, 1.0f, _graphics.GraphicsDevice, _spriteBatch);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            //Create a bunch of random game objects for now
            for (int i = 0; i < 1; i++)
            {
                DrawableGameObject obj = new DrawableGameObject("TestImage", 5, _world, Vector2.Zero);
                _objects.Add(obj);
            }

            _objects.Add(new DrawableGameObject("TestImage", 5, _world, new Vector2(100.0f, 100.0f)));
            _objects.Add(new DrawableGameObject("TestImage", 5, _world, new Vector2(151.0f, 114.0f)));
        
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the _world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            float deltaTime = ((float)gameTime.ElapsedGameTime.TotalMilliseconds) * 0.001f;

            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                this.Exit();
            }

            //Update all objects in ourlist. This is not where physics is evaluated,
            // it is only where object-specific actions are performed, like applying control forces
            foreach (DrawableGameObject obj in _objects)
            {
                obj.Update(deltaTime);
            }

            _mainCamera.Update(deltaTime);

            //Update the FarseerPhysics physics
            _world.Step(deltaTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            //Draw all objects in our list
            foreach (DrawableGameObject obj in _objects)
            {
                _mainCamera.Draw(obj);
            }
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
