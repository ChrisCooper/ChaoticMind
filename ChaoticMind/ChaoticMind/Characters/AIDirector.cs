using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class AIDirector {

        public void StartNewGame() {
            //Create swarmers in the first 3x3 square
            for (int x = 0; x < Math.Min(Program.DeprecatedObjects.Map.GridDimension, 3); x++) {
                for (int y = 0; y < Math.Min(Program.DeprecatedObjects.Map.GridDimension, 3); y++) {
                    //Skip the player's starting tile
                    if (x == 0 && y == 0) {
                        continue;
                    }

                    for (int i = 0; i < 2; i++) {
                        SpawnEnemiesInTile(x, y);
                    }
                }
            }
        }

        internal void ClearGame() {
        }

        public void Update(float deltaTime) {
        }

        public void OnShift() {

            for (int xAddition = -1; xAddition <= 1; xAddition++) {
                for (int yAddition = -1; yAddition <= 1; yAddition++) {
                    //add to player position
                    int resultX = (int)Program.DeprecatedObjects.MainPlayer.GridCoordinate.X + xAddition;
                    int resultY = (int)Program.DeprecatedObjects.MainPlayer.GridCoordinate.Y + yAddition;

                    if (MapTile.isOutOfBoundsGrid(new Vector2(resultX, resultY))) {
                        continue;
                    }

                    SpawnEnemiesInTile(resultX, resultY);

                }
            }
        }

        void SpawnEnemiesInTile(int x, int y) {
            for (int i = 0; i < Utilities.randomInt(1, 2); i++) {
                Parasite parasite = new Parasite(MapTile.RandomPositionInTile(x, y));
                Program.DeprecatedObjects.Enemies.Add(parasite);
            }
            for (int i = 0; i < Utilities.randomInt(1, 2); i++) {
                Swarmer swarmer = new Swarmer(MapTile.RandomPositionInTile(x, y));
                Program.DeprecatedObjects.Enemies.Add(swarmer);
            }
        }
    }
}
