using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoticMind {
    class GameObjects {

        internal List<IGameObject> Particles { get; set; }
        internal List<IGameObject> Projectiles { get; set; }
        internal List<IGameObject> Collectables { get; set; }

        internal MapManager Map { get; set; }


        internal GameObjects() {
            Particles = new List<IGameObject>();
            Projectiles = new List<IGameObject>();
            Collectables = new List<IGameObject>();
            Map = new MapManager();
        }

        internal void Clear() {
            Projectiles.ForEach(p => p.WasCleared());
            Projectiles.Clear();

            Particles.ForEach(p => p.WasCleared());
            Particles.Clear();

            Collectables.ForEach(c => c.WasCleared());
            Collectables.Clear();

            Map.ClearGame();
        }

        internal void Update(float deltaTime) {
            Projectiles.ForEach(p => p.Update(deltaTime));
            Particles.ForEach(p => p.Update(deltaTime));
            Collectables.ForEach(c => c.Update(deltaTime));
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
        }

        internal void DrawGlows(Camera camera) {
            Map.DrawGlows(camera);
            Particles.ForEach(p => camera.DrawGlow(p));
            Projectiles.ForEach(p => camera.DrawGlow(p));
            Collectables.ForEach(c => camera.DrawGlow(c));
        }

        internal void DrawMinimap(Camera camera) {
            Map.DrawMap(camera);
            Particles.ForEach(p => camera.DrawMinimap(p));
            Projectiles.ForEach(p => camera.DrawMinimap(p));
            Collectables.ForEach(c => camera.DrawMinimap(c));
        }

        internal void DrawOnShiftInterface(ShiftInterface shiftInterface) {
            Particles.ForEach(p => shiftInterface.drawOnOverlay(p));
            Projectiles.ForEach(p => shiftInterface.drawOnOverlay(p));
            Collectables.ForEach(c => shiftInterface.drawOnOverlay(c));
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


