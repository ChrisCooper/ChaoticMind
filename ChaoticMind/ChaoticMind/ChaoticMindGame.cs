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

        StaticSprite _deathScreen;

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
        SoundEffects _soundEffects;

        HUD.HUDManager _hudManager = new HUD.HUDManager();

        MouseDrawer _mouseDrawer = new MouseDrawer();

        //Any objects in this array will have Update called on them and be drawn by the _mainCamera object
        List<DrawableGameObject> _objects = new List<DrawableGameObject>();

        MapManager _mapManager;
        internal MapManager MapManager {
            get { return _mapManager; }
        }

        ProjectileManager _projectileManager = ProjectileManager.mainInstance();
        CollectibleManager _collectibleManager = CollectibleManager.mainInstance();
        ParticleManager _particleManager = ParticleManager.mainInstance();

        ShiftInterface _shiftInterface = new ShiftInterface();

        //OutcomeScreen _outcomeScreen = new OutcomeScreen();

        public ChaoticMindGame() {

            _graphics = new GraphicsDeviceManager(this);

            Screen.Initialize(_graphics, _goFullscreen);

             _centreLocation = new Vector2(Screen.Width / 2.0f, Screen.Height / 2.0f);

            Content.RootDirectory = "Content";
            SpriteAnimationSequence.SharedContentManager = Content;

            MusicController.SharedContentManager = Content;
            SoundEffects.SharedContentManager = Content;

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
            
            _fpsCounter = new FrameRateCounter(_spriteBatch, FontManager.DebugFont);

            _hudManager.Initialize();

            _mapManager = new MapManager(MAP_SIZE);
            _mapManager.Initialize(ref _objects);

            _mainCamera = new Camera(Vector2.Zero, 50.0f, _graphics.GraphicsDevice, _spriteBatch);

            InputManager.Initialize();
            GameState.Initilize();
             
            //Create swarmers in the first 3x3 square
            for (int x = 0; x < Math.Min(MAP_SIZE, 3); x++) {
                for (int y = 0; y < Math.Min(MAP_SIZE, 3); y++) {
                    for (int i = 0; i < 5; i++) {
                        if (x == 0 && y == 0) {
                            //Skip the starting square for fairness
                            continue;
                        }
                        Parasite parasite = new Parasite(MapTile.RandomPositionInTile(x, y));
                        _objects.Add(parasite);
                        
                        if (i % 2 == 0) {
                            Swarmer swarmer = new Swarmer(MapTile.RandomPositionInTile(x, y));
                            _objects.Add(swarmer);
                        }
                    }
                }
            }
            

            //set up player
            _player = new Player(Vector2.Zero);
            _objects.Add(_player);
            _mainCamera.setTarget(_player.Body);

            _backgroundMusic = new MusicController();
            //_backgroundMusic.Enqueue("testSound1");
            //_backgroundMusic.Enqueue("testSound2");
            //_backgroundMusic.Enqueue("testSound3");
            _backgroundMusic.Enqueue("01 Cryogenic Dreams");
            _backgroundMusic.Enqueue("05 Rapid Cognition");
            _backgroundMusic.Enqueue("10 Disappear");
            //_backgroundMusic.Play();

            //_soundEffects = new SoundEffects();

            _shiftInterface.Initialize(_mapManager, _spriteBatch);

            //_outcomeScreen.Initialize();

            _mouseDrawer.Initialize();

            ParticleType.Initialize();

            //init the level
            GameState.StartLevel(1, 3);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            _pauseBackground = new StaticSprite("UI/PauseScreen", 1, DrawLayers.MenuBackgrounds);
            _deathScreen = new StaticSprite("Screens/DeathScreen", 1, DrawLayers.MenuBackgrounds);
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

            if (GameState.Mode == GameState.GameMode.NORMAL) {
                normalGameUpdate(deltaTime);
            }
            else if (GameState.Mode == GameState.GameMode.SHIFTING) {
                _shiftInterface.Update();
            }
            else if (GameState.Mode == GameState.GameMode.PAUSED) {
                //update stuff for the pause menu
            }
            //else if (GameState.Mode == GameState.GameMode.GAMEOVER) {
            //    _outcomeScreen.Update();
            //}

            _fpsCounter.Update(gameTime);

            base.Update(gameTime);
        }

        private void normalGameUpdate(float deltaTime) {
            //Update all objects in our list. This is not where physics is evaluated,
            // it is only where object-specific actions are performed, like applying control forces

            for (int i = 0 ; i < _objects.Count ; i++){
                if (_objects[i].ShouldDieNow()){
                    _objects[i].DestroySelf();
                    _objects.RemoveAt(i);
                    i--;
                }
                else{
                    _objects[i].Update(deltaTime);
                }
            }

            _projectileManager.Update(deltaTime);
            _collectibleManager.Update(deltaTime);
            _particleManager.Update(deltaTime);

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
                GameState.Mode = GameState.Mode == GameState.GameMode.PAUSED ? GameState.GameMode.NORMAL : GameState.GameMode.PAUSED;
            }
            //shifting interface
            if (InputManager.IsKeyClicked(Keys.Tab)) {
                GameState.Mode = GameState.Mode == GameState.GameMode.SHIFTING ? GameState.GameMode.NORMAL : GameState.GameMode.SHIFTING;
            }
            //you died
            if (_player.ShouldDieNow()) {
                GameState.Mode = GameState.GameMode.GAMEOVERLOSE;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            if (GameState.Mode == GameState.GameMode.GAMEOVERLOSE) {
                drawDeathScreen();
                //base.Draw(gameTime);
            }
            

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

            _spriteBatch.End();



            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            /**** Draw HUD ****/
            _hudManager.Draw_HUD(_spriteBatch);

            //Draw Minimap
            _mapManager.DrawMap(_mainCamera);
            drawObjectsOnMinimap(gameTime);


            /**** Draw Special State Objects ****/

            if (GameState.Mode == GameState.GameMode.PAUSED) {
                drawPauseOverlay();
            }
            else if (GameState.Mode == GameState.GameMode.SHIFTING) {
                _shiftInterface.DrawInterface(_objects);
            }

            drawDebugInfo(gameTime);

            _mouseDrawer.drawMouse(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void drawDeathScreen() {
            //_spriteBatch.Draw(_deathScreen.Texture, _centreLocation, _deathScreen.CurrentTextureBounds, Color.White, 0.0f, _deathScreen.CurrentTextureOrigin, , SpriteEffects.None, DrawLayers.MenuBackgrounds);

        }

        private void drawGlows(GameTime gameTime) {
            _particleManager.DrawGlows(_mainCamera);
            _projectileManager.DrawGlows(_mainCamera);
            //_collectibleManager.DrawGlows(_mainCamera);
        }

        private void drawObjects(GameTime gameTime) {
            //Draw map tiles
            _mapManager.DrawTiles(_mainCamera, (float)gameTime.TotalGameTime.TotalMilliseconds);

            _particleManager.Draw(_mainCamera);

            //Draw all objects in our list (and their minimap representations)
            foreach (DrawableGameObject obj in _objects) {
                _mainCamera.Draw((IDrawable)obj);
                _mainCamera.DrawMinimap((IMiniMapable) obj);
            }

            _projectileManager.Draw(_mainCamera);
            _collectibleManager.Draw(_mainCamera);
            _collectibleManager.DrawOnMinimap(_mainCamera);
        }

        private void drawObjectsOnMinimap(GameTime gameTime) {

            //Draw all objects in our list (and their minimap representations)
            foreach (DrawableGameObject obj in _objects) {
                _mainCamera.DrawMinimap(obj);
            }
            _collectibleManager.DrawOnMinimap(_mainCamera);
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
