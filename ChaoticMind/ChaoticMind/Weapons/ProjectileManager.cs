using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class ProjectileManager {

        static ProjectileManager _mainInstance;

        List<Projectile> _projectiles = new List<Projectile>();
        
        static ProjectileManager() {
            _mainInstance = new ProjectileManager();
        }

        internal static ProjectileManager mainInstance() {
            return _mainInstance;
        }

        //No one can make one but us
        private ProjectileManager() {
        }

        public void Update(float deltaTime) {
            foreach (Projectile p in _projectiles){
                    p.Update(deltaTime);
            }
        }

        public void Draw(Camera camera) {
            foreach (Projectile p in _projectiles) {
                camera.Draw(p);
            }
        }

        internal void DrawGlows(Camera camera) {
            foreach (Projectile p in _projectiles) {
                camera.DrawGlow(p);
            }
        }

        public static void CreateProjectile(Vector2 startingPosition, Vector2 gunDirection, ProjectileType projectileType) {
            _mainInstance._projectiles.Add(new Projectile(startingPosition, gunDirection, projectileType));
        }

        internal static void Remove(Projectile projectile) {
            _mainInstance._projectiles.Remove(projectile);
        }


    }
}
