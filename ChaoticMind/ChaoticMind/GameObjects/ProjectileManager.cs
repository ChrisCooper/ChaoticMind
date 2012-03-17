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
        World _world;

        public void Initilize(World world, Camera camera) {
            _self = new ProjectileManager();
            _self._world = world;
            _self._camera = camera;
            _self._projectiles = new List<Projectile>();
        }

        public void Update(float deltaTime) {
            for (int i = 0 ; i < _self._projectiles.Count ; i++){
                if (!_self._projectiles[i].Timeout())
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
        public static void CreateProjectile(Vector2 startingPosition, Vector2 direction, float maxTime, int damage, float speed) {
            _self._projectiles.Add(new Projectile(startingPosition, direction, maxTime, damage, speed, _self._world));
        }
        //new custom projectile
        public static void CreateProjectile(Vector2 startingPosition, Vector2 direction, float maxTime, int damage, float speed, String spriteResource, int xFrames, int yFrames, float diameter, float animationDuration) {
            _self._projectiles.Add(new Projectile(startingPosition, direction, maxTime, damage, speed, spriteResource, xFrames, yFrames, diameter, animationDuration, _self._world));
        }
        //reuse a projectile
        public static void CreateProjectile(Vector2 startingPosition, Vector2 direction, float maxTime, int damage, float speed, AnimatedSprite sprite) {
            _self._projectiles.Add(new Projectile(startingPosition, direction, maxTime, damage, speed, sprite, _self._world));
        }
    }
}
