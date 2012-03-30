using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaoticMind.Characters {
    class PathFinder {

        static MapManager _mapManager;

        public static void Initialize(MapManager mapManager) {
            _mapManager = mapManager;
        }

        public PathFindingResult LocationForPathTo(Vector2 worldFromLocation, Vector2 worldToLocation) {

            //Find Path, and fill a result.

            return new PathFindingResult();
        }
    }

    public class PathFindingResult {
        public Vector2 ImmediateDestination { get; set; }

        /// <summary>
        /// This will be true if the destination returned is the same as the worldToLocation this is the result for.
        /// This means there are no doors to go through or obstacles to avoid.
        /// </summary>
        public bool IsActualDestination { get; set; }
    }
}
