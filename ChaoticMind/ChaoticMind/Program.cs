using System;
using Microsoft.Xna.Framework.Graphics;
using ChaoticMind.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ChaoticMind {
#if WINDOWS || XBOX
    static class Program {

        public static GraphicsUtilities Graphics { get { return OverallGame.GraphicsUtils; } }
        public static SpriteBatch SpriteBatch { get { return OverallGame.GraphicsUtils.MainSpriteBatch; } }
        public static ContentManager Content { get { return OverallGame.Content; } }

        static ChaoticMindGame OverallGame;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args) {
            using (OverallGame = new ChaoticMindGame()) {
                OverallGame.Run();
            }
        }
    }
#endif
}

