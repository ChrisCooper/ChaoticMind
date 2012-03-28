using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoticMind {
    class DrawLayers {

        /*** GAME ELEMENTS ***/

        /// <summary>
        /// Tile bottom
        /// </summary>
        public static float MapGround = 0.47f;

        public static float MapOverlay = 0.46f;

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


        /*** HUD ***/
        /// <summary>
        /// Such as the black back of minimap area, or elements that have no upper frame
        /// </summary>
        public static float HUD_Backgrounds = 0.34f;

        /// <summary>
        /// Actual updating interface info, like the gun image, ammo (but not minimap. See minimap DrawLevels)
        /// </summary>
        public static float HUD_Dynamic_Info = 0.33f;

        /*Minimap*/

        /// <summary>
        /// The maze tile shapes
        /// </summary>
        public static float HUD_Minimap_Maze = 0.323f;

        /// <summary>
        /// Elements like enemies which are not as important as the player and collectibles
        /// </summary>
        public static float HUD_Minimap_normal_elements = 0.322f;

        /// <summary>
        /// Player, collectibles, and other important elements
        /// </summary>
        public static float HUD_Minimap_important_elements = 0.321f;

        public static float HUD_Upper_Frames = 0.31f;

        /*** MENUS ***/

        /// <summary>
        /// Menu elements which should be obscured by buttons, text, etc
        /// </summary>
        public static float MenuBackgrounds = 0.22f;

        /// <summary>
        /// Buttons, images, not text
        /// </summary>
        public static float MenuElements = 0.212f;

        /// <summary>
        /// Such as character location on shift interface
        /// </summary>
        public static float MenuHighlightElements = 0.211f;

        /// <summary>
        /// Text for buttons, titles, etc.
        /// </summary>
        public static float MenuText = 0.12f;

        public static float DebugText = 0.11f;

        /// <summary>
        /// The... uh... mouse!
        /// </summary>
        public static float Mouse = 0.01f;

    }
}
