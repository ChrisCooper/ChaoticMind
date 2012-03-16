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

        //projectile properties
        AnimatedSprite _projectileSprite;
        float _projectileSpeed;
        int _projectileDamage;
        float _projectileTTL;

        //ammo
        int _curRounds;
        int _curFires;

        //timing
        DateTimeOffset _lastShotTime;
        DateTimeOffset _lastReloadTime;

        public Weapon(AnimatedSprite weaponSprite, float reloadCooldown, float shootCooldown, int numRounds, int firesPerRound, int numProjectiles, float spread, AnimatedSprite projectileSprite, float projectileSpeed, int projectileDamage, float projectileTTL) {
            //weapon properties
            _weaponSprite = weaponSprite;
            _reloadCooldown = reloadCooldown;
            _shootCooldown = shootCooldown;
            _numRounds = _curRounds = numRounds;
            _firesPerRound = _curFires = firesPerRound;
            _numProjectiles = numProjectiles;
            _spread = spread;

            //projectile properties
            _projectileSprite = projectileSprite;
            _projectileSpeed = projectileSpeed;
            _projectileDamage = projectileDamage;
            _projectileTTL = projectileTTL;
        }

        public void Shoot(Vector2 location, Vector2 direction){
            if (_curFires > 0 && ReloadPercent() == 1 && ShootPercent() == 1) {
                //TODO: Multiple and spread
                ProjectileManager.CreateProjectile(location, direction, _projectileTTL, _projectileDamage, _projectileSpeed, _projectileSprite);
                _curFires--;
                _lastShotTime = DateTimeOffset.Now;
            }
        }

        public void Reload() {
            if (ReloadPercent() == 1 && _curRounds > 0) {
                _curRounds--;
                _curFires = _firesPerRound;
                _lastReloadTime = DateTimeOffset.Now;
            }
        }

        //is the player waiting for a shoot/reload cooldown?
        public float ShootPercent() {
            double temp = (DateTimeOffset.Now - _lastShotTime).TotalMilliseconds / _shootCooldown;
            return temp > 1 ? 1 : (float)temp;
        }
        public float ReloadPercent() {
            double temp = (DateTimeOffset.Now - _lastReloadTime).TotalMilliseconds / _reloadCooldown;
            return temp > 1 ? 1 : (float)temp;
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
