using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoticMind {
    class GameObjects {

        internal List<Particle> Particles { get; set; }
        internal List<Projectile> Projectiles { get; set; }

        internal GameObjects() {
            Particles = new List<Particle>();
            Projectiles = new List<Projectile>();
        }

        internal void Clear() {
            Projectiles.ForEach(p => p.WasCleared());
            Projectiles.Clear();

            Particles.ForEach(p => p.WasCleared());
            Particles.Clear();
        }

        internal void Update(float deltaTime) {
            Projectiles.ForEach(p => p.Update(deltaTime));
            Particles.ForEach(p => p.Update(deltaTime));


            CullObjects();
            
        }

        void CullObjects() {
            //Projectiles
            for (int i = 0; i < Projectiles.Count; i++) {
                Projectile obj = Projectiles[i];
                if (obj.ShouldBeKilled) {
                    obj.WasKilled();
                    Projectiles.Remove(obj);
                    i--;
                }
            }

            //Particles
            for (int i = 0; i < Particles.Count; i++) {
                Particle obj = Particles[i];
                if (obj.ShouldBeKilled) {
                    obj.WasKilled();
                    Particles.Remove(obj);
                    i--;
                }
            }
        }

        internal void DrawObjects(Camera camera) {
            Particles.ForEach(p => camera.Draw(p));
            Projectiles.ForEach(p => camera.Draw(p));
        }

        internal void DrawGlows(Camera _mainCamera) {
            Particles.ForEach(p => _mainCamera.DrawGlow(p));
            Projectiles.ForEach(p => _mainCamera.DrawGlow(p));
        }
    }
}

internal interface IGameObject {

    //Management
    bool ShouldBeKilled { get; }
    void WasKilled();
    void WasCleared();

}
