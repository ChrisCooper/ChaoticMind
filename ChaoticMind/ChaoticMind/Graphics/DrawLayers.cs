using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoticMind {
    class DrawLayers {


        /// <summary>
        /// The draw layer group for the map tiles, characters, collectibles, projectiles, etc.
        /// </summary>
        public class GameElements {

            /// <summary>
            /// Tile bottom
            /// </summary>
            public static float TileGround = 0.47f;

            public static float TileOverlay = 0.46f;

            /// <summary>
            /// Shadows from characters, projectiles, etc
            /// </summary>
            public static float Shadows = 0.45f;

            /// <summary>
            /// Particles that should appear on the ground, such as dead enemies
            /// </summary>
            public static float LowerParticles = 0.44f;

            /// <summary>
            /// Collectibles
            /// </summary>
            public static float Collectibles = 0.43f;

            /// <summary>
            /// Characters such as the player and enemies
            /// </summary>
            public static float Characters = 0.42f;

            /// <summary>
            /// Particles to be displayed on top, such as the swamer's lightning
            /// </summary>
            public static float UpperParticles = 0.41f;

        }

        /// <summary>
        /// The DrawLayer group for HUD elements such as the health bar, minimap, and weapon display
        /// </summary>
        public class HUD {

            /// <summary>
            /// Such as the black back of minimap area, or elements that have no upper frame
            /// </summary>
            public static float Backgrounds = 0.34f;

            /// <summary>
            /// Actual updating interface info, like the gun image, ammo (but not minimap. See minimap DrawLevels)
            /// </summary>
            public static float Dynamic_Info = 0.33f;

            /// <summary>
            /// The maze tile shapes
            /// </summary>
            public static float Minimap_Maze = 0.323f;

            /// <summary>
            /// Elements like enemies which are not as important as the player and collectibles
            /// </summary>
            public static float Minimap_normal_elements = 0.322f;

            /// <summary>
            /// PlayerType, collectibles, and other important elements
            /// </summary>
            public static float Minimap_important_elements = 0.321f;

            public static float Upper_Frames = 0.31f;

        }

        /// <summary>
        /// Draw Layer group for menus like the shifting interface and pause menu
        /// </summary>
        public class Menu {

            /// <summary>
            /// Menu elements which should be obscured by buttons, text, etc
            /// </summary>
            public static float Backgrounds = 0.22f;

            /// <summary>
            /// Buttons, images, not text
            /// </summary>
            public static float Elements = 0.212f;

            /// <summary>
            /// Such as character location on shift interface
            /// </summary>
            public static float HighlightElements = 0.211f;

            /// <summary>
            /// Text for buttons, titles, etc.
            /// </summary>
            public static float Text = 0.12f;

            public static float DebugText = 0.11f;
        }

        /// <summary>
        /// The draw layer group for very upper elements like the mouse
        /// </summary>
        public class VeryTop {
            /// <summary>
            /// The... uh... mouse!
            /// </summary>
            public static float Mouse = 0.01f;

            /// <summary>
            /// For Pain static, and the like
            /// </summary>
            public static float FullOverlay = 0.001f;
        }
    }
}
