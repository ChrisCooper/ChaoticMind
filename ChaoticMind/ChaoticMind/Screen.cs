using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticMind {
    class Screen {

        internal static void Initialize(Microsoft.Xna.Framework.GraphicsDeviceManager graphics, bool goFullscreen) {
            Width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            Height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            if (!goFullscreen) {
                Width -= 20;
                Height -= 80;
            }

            graphics.PreferredBackBufferWidth = Width;
            graphics.PreferredBackBufferHeight = Height;
            graphics.IsFullScreen = goFullscreen;
        }

        internal static int Width {
            get;
            set;
        }
        internal static int Height {
            get;
            set;
        }

        
    }
}
