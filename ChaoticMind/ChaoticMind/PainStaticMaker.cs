using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class PainStaticMaker {
        static float painMagnitude = 0.0f;
        static float painFalloff = 0.99f;

        //This is NOT a lame variable name. This is "static" as in TV static. ;P
        static AnimatedSprite _staticSprite;

        public static void Initialize() {
            _staticSprite = new AnimatedSprite("Screens/PainStatic", 6, 2, 1, 0.5f, DrawLayers.Mouse);
        }

        public static void AddDamage(float magnitude) {
            magnitude *= 0.03f;
            painMagnitude += magnitude;
        }

        public static void Update(float deltaTime) {
            painMagnitude *= painFalloff;
            _staticSprite.Update(deltaTime);
        }

        public static void DrawStatic(SpriteBatch spriteBatch) {
            spriteBatch.Draw(_staticSprite.Texture, Screen.ScreenRect, _staticSprite.CurrentTextureBounds, Color.White * painMagnitude);
        }
    }
}
