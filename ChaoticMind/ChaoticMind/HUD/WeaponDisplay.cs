using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ChaoticMind.HUD {

    class WeaponDisplay {

        Vector2 _ammoLocation;

        public void Initialize() {
            _ammoLocation = new Vector2(Screen.Width - 150, Screen.Height - 50);
        }

        internal void DrawDisplay(SpriteBatch spriteBatch) {

            spriteBatch.DrawString(FontManager.DebugFont, string.Format("Ammo: {0:0}", Player.Instance.CurrentWeapon.RoundsLeftInClip), _ammoLocation, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, DrawLayers.HUD_Minimap_normal_elements);
        
        }
    }
}
