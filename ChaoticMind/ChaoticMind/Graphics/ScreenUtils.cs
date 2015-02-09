﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class ScreenUtils {

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

            ScreenRect = new Rectangle(0, 0, Width, Height);

            float squareRadius = Math.Min(Height, Width) / (float)2;
            BiggestSquare = new Rectangle((int)(Center.X - squareRadius), (int)(Center.Y - squareRadius), (int)(squareRadius * 2), (int)(squareRadius * 2));
        }

        internal static int Width { get; set; }
        internal static int Height { get; set; }

        public static Vector2 Center { get { return new Vector2(Width / (float)2, Height / (float)2); } }

        public static Rectangle ScreenRect { get; set; }

        /// <summary>
        /// A rectanlge taking up the biggest possible square (in the middle of the screen rectangle)
        /// </summary>
        public static Rectangle BiggestSquare { get; set; }

        public static int SmallestDimension { get { return Math.Min(Width, Height); } }
    }
}
