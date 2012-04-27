using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ChaoticMind.HUD {
    class HUDManager {

        static HUDManager _mainInstance;


        WeaponDisplay _weaponDisplay = new WeaponDisplay();
        HealthDisplay _healthDisplay = new HealthDisplay();
        MinimapDisplay _minimapDisplay = new MinimapDisplay();

        internal void Initialize() {
            _mainInstance = this;
            _healthDisplay.Initialize();
            _weaponDisplay.Initialize();
            _minimapDisplay.Initialize();
        }

        private void setupMinimapInfo() {
        }

        private void setupHealthInfo() {
        }

        internal void Draw() {
            _healthDisplay.Draw();
            _weaponDisplay.Draw();
            _minimapDisplay.Draw();
        }

        internal static Rectangle MinimapRect {
            get { return _mainInstance._minimapDisplay.MinimapRect; }
        }
    }
}