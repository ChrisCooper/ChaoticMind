using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ChaoticMind.HUD {

    class WeaponDisplay {

        Vector2 _ammoLocation;
        Vector2 _spareRoundsLocation;

        StaticSprite _weaponFrameSprite;
        Rectangle _weaponFrameRectangle;

        Vector2 singleDigitOffset = new Vector2(6, 0.0f);

        public void Initialize() {
            _weaponFrameSprite = new StaticSprite("HUD/WeaponFrame", 1.0f, DrawLayers.HUD_Backgrounds);

            float weaponFrameSideLength = Math.Min(Screen.Width / 4.0f, Screen.Height / 3.0f);
            _weaponFrameRectangle = new Rectangle((int)(Screen.Width - weaponFrameSideLength), (int)(Screen.Height - weaponFrameSideLength), (int)(weaponFrameSideLength), (int)(weaponFrameSideLength));

            _ammoLocation = new Vector2(Screen.Width - 0.824f * weaponFrameSideLength, Screen.Height - 0.411f * weaponFrameSideLength);
            _spareRoundsLocation = new Vector2(Screen.Width - 0.824f * weaponFrameSideLength, Screen.Height - 0.201f * weaponFrameSideLength);
        
        }

        internal void DrawDisplay(SpriteBatch spriteBatch) {

            spriteBatch.Draw(_weaponFrameSprite.Texture, _weaponFrameRectangle, _weaponFrameSprite.CurrentTextureBounds, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, _weaponFrameSprite.DrawLayer);

            if (Player.Instance.CurrentWeapon.RoundsLeftInClip < 10) {
                spriteBatch.DrawString(FontManager.DebugFont, string.Format("{0:0}", Player.Instance.CurrentWeapon.RoundsLeftInClip), _ammoLocation + singleDigitOffset, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, DrawLayers.HUD_Minimap_normal_elements);
            }
            else {
                spriteBatch.DrawString(FontManager.DebugFont, string.Format("{0:0}", Player.Instance.CurrentWeapon.RoundsLeftInClip), _ammoLocation, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, DrawLayers.HUD_Minimap_normal_elements);
            }

            if (Player.Instance.CurrentWeapon.SpareClipsLeft < 10) {

                spriteBatch.DrawString(FontManager.DebugFont, string.Format("{0:0}", Player.Instance.CurrentWeapon.SpareClipsLeft), _spareRoundsLocation + singleDigitOffset, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, DrawLayers.HUD_Minimap_normal_elements);
            }
            else {
                spriteBatch.DrawString(FontManager.DebugFont, string.Format("{0:0}", Player.Instance.CurrentWeapon.SpareClipsLeft), _spareRoundsLocation, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, DrawLayers.HUD_Minimap_normal_elements);
            
            }
        }
    }
}
