using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ChaoticMind.HUD {
    class HUDManager {

        static HUDManager _mainInstance;


        WeaponDisplay _weaponDisplay;
        HealthDisplay _healthDisplay;
        MinimapDisplay _minimapDisplay;

        public HUDManager(GameObjects objectsOwner) {
            _mainInstance = this;
            _weaponDisplay = new WeaponDisplay(objectsOwner);
             _healthDisplay = new HealthDisplay(objectsOwner.MainPlayer);
            _minimapDisplay = new MinimapDisplay();
        }

        internal void Draw() {
            _healthDisplay.Draw();
            _weaponDisplay.Draw();
            _minimapDisplay.Draw();
        }

        internal Rectangle MinimapRect {
            get { return _minimapDisplay.MinimapRect; }
        }
    }
}