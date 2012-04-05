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
            String _fireSound;
            String _reloadSound;
            ProjectileType _projectileType;
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
                AssaultRifle._roundsPerClip = 80;
                AssaultRifle._reloadTime = 1.5f;
                AssaultRifle.setSpread(15.0f);
                AssaultRifle.Inaccuracy = 0.4f;
                AssaultRifle._fireSound = "pistol";
                AssaultRifle._reloadSound = "reload";
                AssaultRifle._HUD_Image = new StaticSprite("Weapons/AssaultRifle", 1, DrawLayers.HUD_Dynamic_Info);
                AssaultRifle._projectileType = ProjectileType.AssaultRifleBullet;
                AssaultRifle._isRayCasted = false;

                //Energy Rifle
                EnergyRifle._weaponName = "Energy Rifle";
                EnergyRifle._firesPerRound = 1;
                EnergyRifle._firingInterval = 0.5f;
                EnergyRifle._roundsPerClip = 40;
                EnergyRifle._reloadTime = 1.5f;
                EnergyRifle.setSpread(0.0f);
                EnergyRifle.Inaccuracy = 0.1f;
                EnergyRifle._fireSound = "grenade";
                EnergyRifle._reloadSound = "reload";
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

            public String FireSound {
                get { return _fireSound; }
            }

            public String ReloadSound {
                get { return _reloadSound; }
            }

            public ProjectileType ProjectileType {
                get { return _projectileType; }
            }

            public float Range {
                get { return _projectileType.Range; }
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
