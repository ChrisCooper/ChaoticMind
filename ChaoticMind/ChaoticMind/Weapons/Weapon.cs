using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace ChaoticMind {

    class Weapon {

        WeaponType _weaponType;
        Character _weaponOwner;

        int _roundsLeftInClip;
        int _spareClipsLeft;

        //timing
        Timer _reloadTimer;
        Timer _shootTimer;

        //Spread matrices
        Matrix _spreadRotationStepMatrix;
        Matrix _halfSpreadRotationMatrix;

        public Weapon(WeaponType weaponType, int numberOfSpareClips, Character weaponOwner) {
            _weaponOwner = weaponOwner;

            _weaponType = weaponType;

            _roundsLeftInClip = _weaponType.RoundsPerClip;
            _spareClipsLeft = numberOfSpareClips;

            //timers
            _reloadTimer = new Timer(_weaponType.ReloadTime, true);
            _shootTimer = new Timer(_weaponType.FiringInterval, true);

            _spreadRotationStepMatrix = _weaponType.SpreadRotationStepMatrix;
            _halfSpreadRotationMatrix = _weaponType.HalfSpreadRotationMatrix;

        }

        public void Shoot(Vector2 location, Vector2 direction){
            if (_roundsLeftInClip > 0 && _reloadTimer.isFinished && _shootTimer.isFinished) {

                direction += Utilities.randomNormalizedVector() * _weaponType.Inaccuracy;

                direction.Normalize();

                //start shooting the particles at the left side of the spread
                Vector2 currentSpreadSweepDirection = Vector2.Transform(direction, _halfSpreadRotationMatrix);

                //initial rotation if an even number of projectiles
                if (_weaponType.FiresPerRound % 2 == 0) {
                    Vector2.Transform(ref currentSpreadSweepDirection, ref _spreadRotationStepMatrix, out currentSpreadSweepDirection);
                }

                //Special case of one particle
                if (_weaponType.FiresPerRound == 1) {
                    currentSpreadSweepDirection = direction;
                }

                for (int i = 0; i < _weaponType.FiresPerRound; i++) {
                    //shoot the projectiles

                    if (_weaponType.IsRaycasted) { //use raycasting
                        /*
                        return -1: ignore this fixture and continue
                        return 0: terminate the ray cast
                        return fraction: clip the ray to this point
                        return 1: don't clip the ray and continue
                        */
                        Vector2 pt = Vector2.Zero;
                        float minFrac = float.MaxValue;

                        //Gets the position of the closest fixture on the ray path.
                        Program.SharedGame.MainWorld.RayCast((fixture, point, normal, fraction) => {

                            //check for a valid fixture
                            if (fixture == null) return -1;

                            //check if its an enemy or a wall (and if it's closer than the previous result)
                            if ((fixture.Body.UserData as Enemy != null || fixture.Body.UserData as MapTile != null) && fraction < minFrac) {
                                pt = point;
                                minFrac = fraction;
                                return 1;
                            }
                            return -1;
                        }, location, location + currentSpreadSweepDirection * _weaponType.Range);

                        //create a particle at the place where the ray was stopped
                        if (pt != Vector2.Zero)
                            ProjectileManager.CreateProjectile(pt, Vector2.Zero, _weaponType.ProjectileType);
                    }
                    else { //use projectiles
                        ProjectileManager.CreateProjectile(location + direction * (_weaponOwner.PhysicalEntitySize/1.8f + _weaponType.ProjectileType.Radius), currentSpreadSweepDirection, _weaponType.ProjectileType);
                    }

                    //rotate the direction to shoot the next particle in
                    Vector2.Transform(ref currentSpreadSweepDirection, ref _spreadRotationStepMatrix, out currentSpreadSweepDirection);
                }
                _roundsLeftInClip--;
                _shootTimer.Reset();
            }
        }

        public void update(float deltaTime) {
            _reloadTimer.Update(deltaTime);
            _shootTimer.Update(deltaTime);
        }

        public void Reload() {
            if (_reloadTimer.isFinished && _spareClipsLeft > 0) {
                _spareClipsLeft--;
                _roundsLeftInClip = _weaponType.RoundsPerClip;
                _reloadTimer.Reset();
            }
        }

        public WeaponType WeaponType {
            get { return _weaponType; }
        }

        public int RoundsLeftInClip {
            get { return _roundsLeftInClip; }
        }

        public int SpareClipsLeft {
            get { return _spareClipsLeft; }
        }
    }
}
