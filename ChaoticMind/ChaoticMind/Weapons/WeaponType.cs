using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaoticMind {

        class WeaponType {

            String _weaponName;
            float _spread;
            int _firesPerRound;
            float _firingInterval;
            float _reloadTime;
            StaticSprite _HUD_Image;
            int _roundsPerClip; 
            ProjectileType _projectileType;
            //Otherwise, this will be set to something
            float _range;
            bool _isRayCasted;

            //For spread:
            Matrix _spreadRotationStepMatrix;
            Matrix _halfSpreadRotationMatrix;


            public static WeaponType AssaultRifle = new WeaponType();
            public static WeaponType EnergyRifle = new WeaponType();

            static WeaponType() {
                //Assault Rifle
                AssaultRifle._weaponName = "Assault Rifle";
                AssaultRifle._firesPerRound = 4;
                AssaultRifle._firingInterval = 0.1f;
                AssaultRifle._roundsPerClip = 50;
                AssaultRifle._reloadTime = 2.0f;
                AssaultRifle.setSpread(15.0f);
                AssaultRifle.Inaccuracy = 0.4f;
                AssaultRifle._HUD_Image = new StaticSprite("Weapons/AssaultRifle", 1, DrawLayers.HUD_Dynamic_Info);
                AssaultRifle._projectileType = ProjectileType.AssaultRifleBullet;
                AssaultRifle._isRayCasted = false;

                //Energy Rifle
                EnergyRifle._weaponName = "Energy Rifle";
                EnergyRifle._firesPerRound = 1;
                EnergyRifle._firingInterval = 0.5f;
                EnergyRifle._roundsPerClip = 40;
                EnergyRifle._reloadTime = 2.5f;
                EnergyRifle.setSpread(0.0f);
                EnergyRifle.Inaccuracy = 0.1f;
                EnergyRifle._HUD_Image = new StaticSprite("Weapons/EnergyRifle", 1, DrawLayers.HUD_Dynamic_Info);
                EnergyRifle._projectileType = ProjectileType.EnergyShot;
                EnergyRifle._isRayCasted = false;
            }

            private void setSpread(float spread_degrees) {
                //convert to farseer-friendly radians and generate rotation matrices
                _spread = MathHelper.ToRadians(spread_degrees);
                //generate matricies for rotation
                _spreadRotationStepMatrix = Matrix.CreateRotationZ(_spread / (_firesPerRound + (_firesPerRound % 2 == 0 ? 1 : -1)));
                _halfSpreadRotationMatrix = Matrix.CreateRotationZ(-_spread / 2.0f);
            }

            public String WeaponName {
                get { return _weaponName; }
            }

            public float Spread {
                get { return _spread; }
            }

            public int FiresPerRound {
                get { return _firesPerRound; }
            }

            public float FiringInterval {
                get { return _firingInterval; }
            }

            internal StaticSprite HUD_Image {
                get { return _HUD_Image; }
            }

            public float ReloadTime {
                get { return _reloadTime; }
            }

            public int RoundsPerClip {
                get { return _roundsPerClip; }
            }

            public ProjectileType ProjectileType {
                get { return _projectileType; }
            }

            public float Range {
                get {
                    if (_projectileType != null) {
                        return _range;
                    }
                    else {
                        throw new Exception("Don't use \"" + _weaponName + "\"'s range property if it has an associated ProjectileType. Use the projectile's range property instead.");
                    }
                }
            }

            public Matrix SpreadRotationStepMatrix {
                get { return _spreadRotationStepMatrix; }
            }

            public Matrix HalfSpreadRotationMatrix {
                get { return _halfSpreadRotationMatrix; }
            }

            public bool IsRaycasted {
                get {
                    return _isRayCasted;
                }
            }

            public float Inaccuracy { get; set; }
        }
}
