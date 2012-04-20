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
        internal Player MainPlayer;

        internal MapManager Map { get; set; }

        internal World PhysicsWorld { get; set; }

        internal void StartNewGame() {
            //Create the physics simulator object, specifying that we want no gravity (since we're top-down)
            PhysicsWorld = new World(Vector2.Zero);
            
            Particles = new List<IGameObject>();
            Projectiles = new List<IGameObject>();
            Collectables = new List<IGameObject>();
            Map = new MapManager();

            MainPlayer = new Player(Vector2.Zero);
        }

        internal void ClearGame() {
            Projectiles.ForEach(p => p.WasCleared());
            Projectiles.Clear();

            Particles.ForEach(p => p.WasCleared());
            Particles.Clear();

            Collectables.ForEach(c => c.WasCleared());
            Collectables.Clear();

            Map.ClearGame();

            MainPlayer.WasCleared();
            MainPlayer = null;
        }

        internal void Update(float deltaTime) {
            //Update the FarseerPhysics physics
            PhysicsWorld.Step(deltaTime);

            Projectiles.ForEach(p => p.Update(deltaTime));
            Particles.ForEach(p => p.Update(deltaTime));
            Collectables.ForEach(c => c.Update(deltaTime));
            MainPlayer.Update(deltaTime);
            Map.Update(deltaTime);

            Cull(Projectiles);
            Cull(Particles);
            Cull(Collectables);
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
            camera.Draw(MainPlayer);
        }

        internal void DrawGlows(Camera camera) {
            Map.DrawGlows(camera);
            Particles.ForEach(p => camera.DrawGlow(p));
            Projectiles.ForEach(p => camera.DrawGlow(p));
            Collectables.ForEach(c => camera.DrawGlow(c));
            camera.DrawGlow(MainPlayer);
        }

        internal void DrawMinimap(Camera camera) {
            Map.DrawMap(camera);
            Particles.ForEach(p => camera.DrawMinimap(p));
            Projectiles.ForEach(p => camera.DrawMinimap(p));
            Collectables.ForEach(c => camera.DrawMinimap(c));
            camera.DrawMinimap(MainPlayer);
        }

        internal void DrawOnShiftInterface(ShiftInterface shiftInterface) {
            Particles.ForEach(p => shiftInterface.drawOnOverlay(p));
            Projectiles.ForEach(p => shiftInterface.drawOnOverlay(p));
            Collectables.ForEach(c => shiftInterface.drawOnOverlay(c));
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


