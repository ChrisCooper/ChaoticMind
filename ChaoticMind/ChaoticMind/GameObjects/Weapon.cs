using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaoticMind {

    class Weapon {

        //weapon properties
        AnimatedSprite _weaponSprite;
        float _reloadCooldown;
        float _shootCooldown;
        int _numRounds;
        int _firesPerRound;
        int _numProjectiles;
        float _spread;
        Matrix _rotMat;

        //projectile properties
        AnimatedSprite _projectileSprite;
        float _projectileSpeed;
        int _projectileDamage;
        float _projectileTTL;

        //ammo
        int _curRounds;
        int _curFires;

        //timing
        private int _reloadTimerId;
        private int _shootTimerId;

        public Weapon(AnimatedSprite weaponSprite, float reloadCooldown, float shootCooldown, int numRounds, int firesPerRound, int numProjectiles, float spread, AnimatedSprite projectileSprite, float projectileSpeed, int projectileDamage, float projectileTTL) {
            //weapon properties
            _weaponSprite = weaponSprite;
            _reloadCooldown = reloadCooldown;
            _shootCooldown = shootCooldown;
            _numRounds = _curRounds = numRounds;
            _firesPerRound = _curFires = firesPerRound;
            _numProjectiles = numProjectiles;
            //convert to farseer-friendly radians and generate rotation matrix
            _spread = MathHelper.ToRadians(spread);
            _rotMat = Matrix.CreateRotationZ(_spread / _numProjectiles + (_numProjectiles % 2 == 0 ? 1 : -1));

            //projectile properties
            _projectileSprite = projectileSprite;
            _projectileSpeed = projectileSpeed;
            _projectileDamage = projectileDamage;
            _projectileTTL = projectileTTL;

            //timers
            _reloadTimerId = TimeDelayManager.InitTimer(_reloadCooldown);
            _shootTimerId = TimeDelayManager.InitTimer(_shootCooldown);
            
        }

        public void Shoot(Vector2 location, Vector2 direction){
            if (_curFires > 0 && TimeDelayManager.Finished(_reloadTimerId) && TimeDelayManager.Finished(_shootTimerId)) {

                //TODO: Still has issues with spreading
                //possible:
                //non-zero origin (i think it is, but maybe not)
                //degree/radian tomfoolery

                Vector2 temp = Vector2.Transform(direction, Matrix.CreateRotationZ(-_spread/2.0f));

                //initial rotation if an even number of projectiles
                if (_numProjectiles % 2 == 0) {
                    temp = Vector2.Transform(temp, _rotMat);
                }

                //spawn the projectiles
                for (int i = 0; i < _numProjectiles; i++) {
                    ProjectileManager.CreateProjectile(location, temp, _projectileTTL, _projectileDamage, _projectileSpeed, _projectileSprite);
                    temp = Vector2.Transform(temp, _rotMat);
                }
                _curFires--;
                TimeDelayManager.RestartTimer(_shootTimerId);
            }
        }

        public void Reload() {
            if (TimeDelayManager.Finished(_reloadTimerId) && _curRounds > 0) {
                _curRounds--;
                _curFires = _firesPerRound;
                TimeDelayManager.RestartTimer(_reloadTimerId);
            }
        }

        //get weapon info (may need in HUD)
        public AnimatedSprite WeaponSprite {
            get { return _weaponSprite; }
        }
        public float ReloadCooldown {
            get { return _reloadCooldown; }
        }
        public float ShootCooldown {
            get { return _shootCooldown; }
        }
        public int NumRounds {
            get { return _numRounds; }
        }
        public int FiresPerRound {
            get { return _firesPerRound; }
        }
        public int NumProjectiles {
            get { return _numProjectiles; }
        }
        public float ProjectileSpeed {
            get { return _projectileSpeed; }
        }
        public int ProjectileDamage {
            get { return _projectileDamage; }
        }
    }
}
