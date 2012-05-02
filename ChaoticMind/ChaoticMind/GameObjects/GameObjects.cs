using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ChaoticMind {
    class GameObjects : IGameFlowComponent {

        internal List<IGameObject> Particles { get; set; }
        internal List<IGameObject> Projectiles { get; set; }
        internal List<IGameObject> Collectables { get; set; }
        internal List<IGameObject> Enemies { get; set; }

        internal Player MainPlayer;

        internal AIDirector EnemyDirector { get; set; }

        internal MapManager Map { get; set; }

        internal World PhysicsWorld { get; set; }

        internal Camera MainCamera { get; set; }


        ShiftInterface _shiftInterface;

        ChaoticMindPlayable _playable;

        internal void StartNewGame(ChaoticMindPlayable playable, int mapDimension) {
            _playable = playable;

            //Create the physics simulator object, specifying that we want no gravity (since we're top-down)
            PhysicsWorld = new World(Vector2.Zero);

            Particles = new List<IGameObject>();
            Projectiles = new List<IGameObject>();
            Collectables = new List<IGameObject>();
            Enemies = new List<IGameObject>();

            Map = new MapManager(this);
            Map.StartNewGame(mapDimension);

            MainPlayer = new Player(this, Vector2.Zero);

            EnemyDirector = new AIDirector(this);
            EnemyDirector.StartNewGame();

            MainCamera = new Camera(Vector2.Zero, 35.0f, Program.Graphics.GraphicsDevice);
            MainCamera.setTarget(MainPlayer.Body);
            MainCamera.StartNewGame(mapDimension, mapDimension);

            _shiftInterface = new ShiftInterface(_playable, this);
        }

        public void Update(float deltaTime) {
            //Update the FarseerPhysics physics
            PhysicsWorld.Step(deltaTime);

            Projectiles.ForEach(p => p.Update(deltaTime));
            Particles.ForEach(p => p.Update(deltaTime));
            Collectables.ForEach(c => c.Update(deltaTime));
            Enemies.ForEach(e => e.Update(deltaTime));
            MainPlayer.Update(deltaTime);

            Map.Update(deltaTime);
            EnemyDirector.Update(deltaTime);

            MainCamera.Update(deltaTime);
            PainStaticMaker.Update(deltaTime);

            Cull(Projectiles);
            Cull(Particles);
            Cull(Collectables);
            Cull(Enemies);

            CheckGameState();
        }

        private void CheckGameState() {
            NextComponent = null;
            /*if (Objects.MainPlayer.ShouldBeKilled) {
                _deprecatedState.Mode = GameState.GameMode.GAMEOVERLOSE;
            } else if (_deprecatedState.AllObjectivesCollected) {
                _deprecatedState.Mode = GameState.GameMode.GAMEOVERWIN;
            } else */
            if (InputManager.IsKeyClicked(Keys.P)) {
                NextComponent = new PauseMenu(_playable);
                //_deprecatedState.Mode = GameState.GameMode.PAUSED;
            } else if (InputManager.IsKeyClicked(Keys.Space)) {
                NextComponent = _shiftInterface;
            }
        }

        void Cull(List<IGameObject> objects) {
            for (int i = 0; i < objects.Count; i++) {
                IGameObject obj = objects[i];
                if (obj.ShouldBeKilled) {
                    obj.WasKilled();
                    objects.Remove(obj);
                    i--;
                }
            }
        }

        internal void DrawObjects(Camera camera) {
            Map.DrawTiles(camera);
            Particles.ForEach(p => camera.Draw(p));
            Projectiles.ForEach(p => camera.Draw(p));
            Collectables.ForEach(c => camera.Draw(c));
            Enemies.ForEach(e => camera.Draw(e));
            camera.Draw(MainPlayer);
        }

        internal void DrawGlows(Camera camera) {
            Map.DrawGlows(camera);
            Particles.ForEach(p => camera.DrawGlow(p));
            Projectiles.ForEach(p => camera.DrawGlow(p));
            Collectables.ForEach(c => camera.DrawGlow(c));
            Enemies.ForEach(e => camera.DrawGlow(e));
            camera.DrawGlow(MainPlayer);
        }

        internal void DrawMinimap(Camera camera) {
            Map.DrawMap(camera);
            Particles.ForEach(p => camera.DrawMinimap(p));
            Projectiles.ForEach(p => camera.DrawMinimap(p));
            Collectables.ForEach(c => camera.DrawMinimap(c));
            Enemies.ForEach(e => camera.DrawMinimap(e));
            camera.DrawMinimap(MainPlayer);
        }

        public IGameFlowComponent NextComponent { get; set; }

        public void Draw(float deltaTime) {
            DrawObjects(MainCamera);
            Program.SpriteBatch.End();

            /**** Draw Glow Effects ****/
            //Using BlendState.Additive will make things drawn in this section only brighten, never darken.
            //This means colors will be intensified, and look like glow
            Program.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);

            DrawGlows(MainCamera);
            PainStaticMaker.DrawStatic();

            Program.SpriteBatch.End();

            //Prep for others' drawing
            Program.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
        }
    }


    internal interface IGameObject : IMiniMapable, IDrawable, IGlowDrawable {
        //Management
        bool ShouldBeKilled { get; }
        void WasKilled();
        void WasCleared();

        void Update(float deltaTime);
    }
}


