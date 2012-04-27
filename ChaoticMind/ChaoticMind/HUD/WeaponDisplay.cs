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

        private Rectangle _weaponImageRectangle;

        private GameObjects _objectsOwner;

        public WeaponDisplay(GameObjects objectsOwner) {
            _objectsOwner = objectsOwner;

            _weaponFrameSprite = new StaticSprite("HUD/WeaponFrame", 1.0f, DrawLayers.HUD.Backgrounds);

            float weaponFrameSideLength = Math.Min(ScreenUtils.Width / 4.0f, ScreenUtils.Height / 3.0f);
            _weaponFrameRectangle = new Rectangle((int)(ScreenUtils.Width - weaponFrameSideLength), (int)(ScreenUtils.Height - weaponFrameSideLength), (int)(weaponFrameSideLength), (int)(weaponFrameSideLength));

            _ammoLocation = new Vector2(ScreenUtils.Width - 0.824f * weaponFrameSideLength, ScreenUtils.Height - 0.411f * weaponFrameSideLength);
            _spareRoundsLocation = new Vector2(ScreenUtils.Width - 0.824f * weaponFrameSideLength, ScreenUtils.Height - 0.201f * weaponFrameSideLength);

            _weaponImageRectangle = new Rectangle((int)(_weaponFrameRectangle.Left + (0.357f * weaponFrameSideLength)), (int)(_weaponFrameRectangle.Top + (0.150f * weaponFrameSideLength)), (int)(0.547f * _weaponFrameRectangle.Width), (int)(0.547f * _weaponFrameRectangle.Height));
        }

        internal void Draw() {

            Program.SpriteBatch.Draw(_weaponFrameSprite.Texture, _weaponFrameRectangle, _weaponFrameSprite.CurrentTextureBounds, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, _weaponFrameSprite.DrawLayer);

            //Ammo in clip
            Vector2 ammoPosition = (_objectsOwner.MainPlayer.CurrentWeapon.RoundsLeftInClip < 10) ? _ammoLocation + singleDigitOffset : _ammoLocation;
            Program.SpriteBatch.DrawString(FontManager.DebugFont, string.Format("{0:0}", _objectsOwner.MainPlayer.CurrentWeapon.RoundsLeftInClip), ammoPosition, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, DrawLayers.HUD.Minimap_normal_elements);

            //Spare clips
            Vector2 spareClipsPosition = (_objectsOwner.MainPlayer.CurrentWeapon.SpareClipsLeft < 10) ? _spareRoundsLocation + singleDigitOffset : _spareRoundsLocation;
            Program.SpriteBatch.DrawString(FontManager.DebugFont, string.Format("{0:0}", _objectsOwner.MainPlayer.CurrentWeapon.SpareClipsLeft), spareClipsPosition, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, DrawLayers.HUD.Minimap_normal_elements);

            //Weapon picture
            AnimatedSprite weaponSprite = _objectsOwner.MainPlayer.CurrentWeapon.WeaponType.HUD_Image;
            Program.SpriteBatch.Draw(weaponSprite.Texture, _weaponImageRectangle, weaponSprite.CurrentTextureBounds, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, weaponSprite.DrawLayer);
        }
    }
}
