using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class PainStaticMaker {
        static float _painMagnitude = 0.0f;
        static float _painFalloff = 0.98f;

        //This is NOT a lame variable name. This is "static" as in TV static. ;P
        static AnimatedSprite _staticSprite;

        private static float _opacity;

        public static void Initialize() {
            _staticSprite = new AnimatedSprite("Screens/PainStatic", 12, 3, 1, 1.0f, DrawLayers.VeryTop.FullOverlay);
        }

        public static void AddDamage(float magnitude) {
            magnitude *= 0.03f;
            _painMagnitude += magnitude;
        }

        public static void Update(float deltaTime) {
            _painMagnitude *= _painFalloff;
            _staticSprite.Update(deltaTime);

            _opacity = Math.Min(0.5f, _painMagnitude);
        }

        public static void DrawStatic() {
            Program.SpriteBatch.Draw(_staticSprite.Texture, ScreenUtils.ScreenRect, _staticSprite.CurrentTextureBounds, Color.White * _opacity);
        }
    }
}
