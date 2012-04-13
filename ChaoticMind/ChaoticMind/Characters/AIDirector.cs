using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class AIDirector {

        List<Enemy> _enemies;

        static AIDirector _mainInstance;

        public static void Initilize() {
            _mainInstance = new AIDirector();
            _mainInstance._enemies = new List<Enemy>(50);
        }

        public static void Update(float deltaTime) {
            //update the enemies
            for (int i = 0; i < _mainInstance._enemies.Count; i++) {
                if (_mainInstance._enemies[i].ShouldDieNow()) {
                    _mainInstance._enemies[i].DestroySelf();
                    _mainInstance._enemies.RemoveAt(i);
                    i--;
                }
                else {
                    _mainInstance._enemies[i].Update(deltaTime);
                }
            }
        }

        public static void OnShift() {
            //spawn enemies around the player tile
            int tx, ty;

            for (int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {
                    if (x != 0 || y != 0) {

                        //add to player position
                        tx = (int)Program.SharedGame.MainPlayer.GridCoordinate.X + x;
                        ty = (int)Program.SharedGame.MainPlayer.GridCoordinate.Y + y;

                        if (!MapTile.isOutOfBoundsGrid(new Vector2(tx, ty))) {

                            for (int i = 0; i < Utilities.randomInt(1, 2); i++) {
                                Parasite parasite = new Parasite(MapTile.RandomPositionInTile(tx, ty));
                                _mainInstance._enemies.Add(parasite);
                            }
                            for (int i = 0; i < Utilities.randomInt(1, 2); i++) {
                                Swarmer swarmer = new Swarmer(MapTile.RandomPositionInTile(tx, ty));
                                _mainInstance._enemies.Add(swarmer);
                            }
                        }
                    }
                }
            }
        }

        public static void Draw(Camera mainCamera) {
            _mainInstance._enemies.ForEach(e => e.Draw(mainCamera));
        }
        public static void DrawMinimap(Camera mainCamera) {
            _mainInstance._enemies.ForEach(e => e.DrawMiniMap(mainCamera));
        }
        public static void DrawOnShiftInterface(ShiftInterface shiftInterface) {
            _mainInstance._enemies.ForEach(e => shiftInterface.drawOnOverlay(e));
        }

        public static void ClearGame() {
            _mainInstance._enemies.ForEach(e => e.DestroySelf());
            _mainInstance._enemies.Clear();
        }

        public static void StartNewGame() {
            //Create swarmers in the first 3x3 square
            for (int x = 0; x < Math.Min(MapManager.MainInstance.GridDimension, 3); x++) {
                for (int y = 0; y < Math.Min(MapManager.MainInstance.GridDimension, 3); y++) {
                    for (int i = 0; i < 3; i++) {
                        if ((x != 0 || y != 0) && !MapTile.isOutOfBoundsGrid(new Vector2(x, y))) { //skip starting square
                            Parasite parasite = new Parasite(MapTile.RandomPositionInTile(x, y));
                            _mainInstance._enemies.Add(parasite);

                            if (i % 2 == 0) {
                                Swarmer swarmer = new Swarmer(MapTile.RandomPositionInTile(x, y));
                                _mainInstance._enemies.Add(swarmer);
                            }
                        }
                    }
                }
            }
        }
    }
}
