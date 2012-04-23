using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class GameObjects {

        internal List<IGameObject> Particles { get; set; }
        internal List<IGameObject> Projectiles { get; set; }
        internal List<IGameObject> Collectables { get; set; }
        internal List<IGameObject> Enemies { get; set; }

        internal Player MainPlayer;

        internal AIDirector EnemyDirector { get; set; }

        internal MapManager Map { get; set; }

        internal World PhysicsWorld { get; set; }

        internal Camera MainCamera { get; set; }

        internal void StartNewGame(int mapDimension) {
            //Create the physics simulator object, specifying that we want no gravity (since we're top-down)
            PhysicsWorld = new World(Vector2.Zero);
            
            Particles = new List<IGameObject>();
            Projectiles = new List<IGameObject>();
            Collectables = new List<IGameObject>();
            Enemies = new List<IGameObject>();

            Map = new MapManager();
            Map.StartNewGame(mapDimension);

            MainPlayer = new Player(Vector2.Zero);

            EnemyDirector = new AIDirector();
            EnemyDirector.StartNewGame();

            MainCamera = new Camera(Vector2.Zero, 35.0f, Program.DeprecatedGame.GraphicsDevice, Program.DeprecatedGame.SpriteBatch);
            MainCamera.setTarget(MainPlayer.Body);
            MainCamera.StartNewGame();
        }

        internal void ClearGame() {
            Projectiles.ForEach(p => p.WasCleared());
            Projectiles.Clear();

            Particles.ForEach(p => p.WasCleared());
            Particles.Clear();

            Collectables.ForEach(c => c.WasCleared());
            Collectables.Clear();

            Enemies.ForEach(e => e.WasCleared());
            Enemies.Clear();

            Map.ClearGame();

            EnemyDirector.ClearGame();

            MainPlayer.WasCleared();
            MainPlayer = null;

            MainCamera = null;
        }

        internal void Update(float deltaTime) {
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

            Cull(Projectiles);
            Cull(Particles);
            Cull(Collectables);
            Cull(Enemies);
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

        internal void DrawOnShiftInterface(ShiftInterface shiftInterface) {
            Particles.ForEach(p => shiftInterface.drawOnOverlay(p));
            Projectiles.ForEach(p => shiftInterface.drawOnOverlay(p));
            Collectables.ForEach(c => shiftInterface.drawOnOverlay(c));
            Enemies.ForEach(e => shiftInterface.drawOnOverlay(e));
            shiftInterface.drawOnOverlay(MainPlayer);
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


