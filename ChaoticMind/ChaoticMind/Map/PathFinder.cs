using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class PathFinder {

        static MapManager _mapManager;


        public static void Initialize(MapManager mapManager) {
            _mapManager = mapManager;
        }

        public static PathFindingResult NextLocationForPathToPlayer(Vector2 worldFromLocation, bool searchToFutureLocation) {

            //Find Path, and fill a result.

            return new PathFindingResult(searchToFutureLocation ? Program.Objects.MainPlayer.FuturePosition : Program.Objects.MainPlayer.Position, true);
        }
    }

    public class PathFindingResult {
        public PathFindingResult(Vector2 immediateDestination, bool isActualDestination) {
            ImmediateDestination = immediateDestination;
            IsActualDestination = isActualDestination;
        }

        public Vector2 ImmediateDestination { get; set; }

        /// <summary>
        /// This will be true if the destination returned is the same as the worldToLocation this is the result for.
        /// This means there are no doors to go through or obstacles to avoid.
        /// </summary>
        public bool IsActualDestination { get; set; }
    }
}
