using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class ProjectileManager {

        static ProjectileManager _self;

        List<Projectile> _projectiles;
        Camera _camera;

        public void Initilize(Camera camera) {
            _self = new ProjectileManager();
            _self._camera = camera;
            _self._projectiles = new List<Projectile>();
        }

        public void Update(float deltaTime) {
            for (int i = 0 ; i < _self._projectiles.Count ; i++){
                if (!_self._projectiles[i].ShouldDieNow())
                    _self._projectiles[i].Update(deltaTime);
                else{
                    _self._projectiles[i].Destroy();
                    _self._projectiles.RemoveAt(i);
                }
            }
        }

        public void Draw() {
            foreach (Projectile p in _self._projectiles) {
                _self._camera.Draw(p);
            }
        }

        //default projectile
        public static void CreateProjectile(Vector2 startingPosition, Vector2 direction, float range, int damage, float speed) {
            _self._projectiles.Add(new Projectile(startingPosition, direction, range, damage, speed));
        }
        //new custom projectile
        public static void CreateProjectile(Vector2 startingPosition, Vector2 direction, float range, int damage, float speed, String spriteResource, int xFrames, int yFrames, float diameter, float animationDuration) {
            _self._projectiles.Add(new Projectile(startingPosition, direction, range, damage, speed, spriteResource, xFrames, yFrames, diameter, animationDuration));
        }
        //reuse a projectile
        public static void CreateProjectile(Vector2 startingPosition, Vector2 direction, float range, int damage, float speed, AnimatedSprite sprite) {
            _self._projectiles.Add(new Projectile(startingPosition, direction, range, damage, speed, sprite));
        }
    }
}
