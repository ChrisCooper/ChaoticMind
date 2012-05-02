using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ChaoticMind.HUD {
    class HUDManager {

        WeaponDisplay _weaponDisplay;
        HealthDisplay _healthDisplay;
        MinimapDisplay _minimapDisplay;

        public HUDManager(GameObjects objectsOwner) {
            _weaponDisplay = new WeaponDisplay(objectsOwner);
             _healthDisplay = new HealthDisplay(objectsOwner.MainPlayer);
            _minimapDisplay = new MinimapDisplay(objectsOwner);
        }

        internal void Draw(float deltaTime) {
            _healthDisplay.Draw();
            _weaponDisplay.Draw();
            _minimapDisplay.Draw();
        }
    }
}