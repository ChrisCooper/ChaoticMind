using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

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
        Matrix _halfRot;

        //projectile properties
        AnimatedSprite _projectileSprite;
        float _projectileSpeed;
        int _projectileDamage;
        float _projectileRange;

        //ammo
        int _curRounds;
        int _curFires;

        //timing
        private int _reloadTimerId;
        private int _shootTimerId;

        public Weapon(AnimatedSprite weaponSprite, float reloadCooldown, float shootCooldown, int numRounds, int firesPerRound, int numProjectiles, float spread, AnimatedSprite projectileSprite, float projectileSpeed, int projectileDamage, float projectileRange) {
            //weapon properties
            _weaponSprite = weaponSprite;
            _reloadCooldown = reloadCooldown;
            _shootCooldown = shootCooldown;
            _numRounds = _curRounds = numRounds;
            _firesPerRound = _curFires = firesPerRound;
            _numProjectiles = numProjectiles;
            //convert to farseer-friendly radians and generate rotation matrix
            _spread = MathHelper.ToRadians(spread);
            //generate matricies for rotation
            _rotMat = Matrix.CreateRotationZ(_spread / (_numProjectiles + (_numProjectiles % 2 == 0 ? 1 : -1)));
            _halfRot = Matrix.CreateRotationZ(-_spread / 2.0f);

            //projectile properties
            _projectileSprite = projectileSprite;
            _projectileSpeed = projectileSpeed;
            _projectileDamage = projectileDamage;
            _projectileRange = projectileRange;

            //timers
            _reloadTimerId = TimeDelayManager.InitTimer(_reloadCooldown);
            _shootTimerId = TimeDelayManager.InitTimer(_shootCooldown);
            
        }

        public void Shoot(Vector2 location, Vector2 direction){
            if (_curFires > 0 && TimeDelayManager.Finished(_reloadTimerId) && TimeDelayManager.Finished(_shootTimerId)) {

                //start shooting the particles at the left side of the spread
                Vector2 temp = Vector2.Transform(direction, _halfRot);

                //initial rotation if an even number of projectiles
                if (_numProjectiles % 2 == 0) {
                    Vector2.Transform(ref temp, ref _rotMat, out temp);
                }

                for (int i = 0; i < _numProjectiles; i++) {
                    //shoot the projectiles
                    if (_projectileSpeed <= 0) { //use raycasting
                        /*
                        return -1: ignore this fixture and continue
                        return 0: terminate the ray cast
                        return fraction: clip the ray to this point
                        return 1: don't clip the ray and continue
                        */
                        Fixture hit = null;
                        Vector2 pt = Vector2.Zero;
                        float minFrac = float.MaxValue;

                        //BUG: the returns are probably not right (I guessed), but you can sometimes hit sillyboxes through the walls
                        //also only goes to the mouse, but I'll refactor this into the weapons class with a range property at some point
                        Program.SharedGame.MainWorld.RayCast((fixture, point, normal, fraction) => {
                            if (fixture != null && fraction < minFrac) {
                                hit = fixture;
                                pt = point;
                                minFrac = fraction;
                                return 1;
                            }
                            return -1;
                        }, location, temp * _projectileRange);

                        //DOES NOTHING SO FAR, JUST PRINTS TO CONSOLE THE HIT OBJECT
                        Console.WriteLine("Detected a hit at " + pt);
                        ProjectileManager.CreateProjectile(pt, Vector2.Zero, 1, 0, 1);
                    }
                    else { //use projectiles
                        ProjectileManager.CreateProjectile(location, temp, _projectileRange, _projectileDamage, _projectileSpeed, _projectileSprite);
                    }

                    //rotate the direction to shoot the next projectile in
                    Vector2.Transform(ref temp, ref _rotMat, out temp);
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
        public float ProjectileRange {
            get { return _projectileRange; }
        }
    }
}
