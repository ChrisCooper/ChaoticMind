using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class PainStaticMaker {
        static float painMagnitude = 0.0f;
        static float painFalloff = 0.98f;

        //This is NOT a lame variable name. This is "static" as in TV static. ;P
        static AnimatedSprite _staticSprite;

        private static float _opacity;
        private static float _deathFadeInSpeed = 0.005f;

        public static void Initialize() {
            _staticSprite = new AnimatedSprite("Screens/PainStatic", 12, 3, 1, 1.0f, DrawLayers.Mouse);
        }

        public static void AddDamage(float magnitude) {
            magnitude *= 0.03f;
            painMagnitude += magnitude;
        }

        public static void Update(float deltaTime) {
            painMagnitude *= painFalloff;
            _staticSprite.Update(deltaTime);


            if (GameState.Mode == GameState.GameMode.GAMEOVERLOSE) {
                _opacity = Math.Min(1.0f, _opacity + _deathFadeInSpeed);
            } else {
               _opacity = Math.Min(0.5f,(painMagnitude / Player.Instance.PercentHealth / 5));
            }
        }

        public static void DrawStatic(SpriteBatch spriteBatch) {
            spriteBatch.Draw(_staticSprite.Texture, Screen.ScreenRect, _staticSprite.CurrentTextureBounds, Color.White * _opacity);
        }
    }
}
