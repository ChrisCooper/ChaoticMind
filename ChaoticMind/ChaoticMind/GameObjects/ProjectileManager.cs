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
                    _self._projectiles[i].DestroyBody();
                    _self._projectiles.RemoveAt(i);
                }
            }
        }

        public void Draw() {
            foreach (Projectile p in _self._projectiles) {
                _self._camera.Draw(p);
            }
        }

        public static void CreateProjectile(Vector2 startingPosition, Vector2 direction, float maxTime, float diameter, float speed, World world) {
            _self._projectiles.Add(new Projectile(startingPosition, direction, maxTime, diameter, speed, world));
        }
        public static void CreateProjectile(Vector2 startingPosition, Vector2 direction, float maxTime, float diameter, float speed, String spriteResource, int xFrames, int yFrames, float animationDuration, World world) {
            _self._projectiles.Add(new Projectile(startingPosition, direction, maxTime, diameter, speed, spriteResource, xFrames, yFrames, animationDuration, world));
        }
    }
}
