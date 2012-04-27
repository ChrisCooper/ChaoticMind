using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticMind {
    class FontManager {


        public static SpriteFont DebugFont;

        static FontManager() {

            DebugFont = Program.Content.Load<SpriteFont>("DebugFont");

        }
    }
}
