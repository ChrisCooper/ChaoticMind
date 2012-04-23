using System;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticMind {
#if WINDOWS || XBOX
    static class Program {

        public static ChaoticMindPlayable DeprecatedGame { get; set; }

        public static SpriteBatch SpriteBatch { get { return OverallGame.GraphicsUtils.MainSpriteBatch; } }

        public static GameObjects DeprecatedObjects {
            get { return DeprecatedGame.Objects; }
        }

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

