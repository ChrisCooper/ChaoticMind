using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoticMind {
    class AIDirector {

        List<Enemy> _enemies;

        static AIDirector _mainInstance;

        public static void Initilize() {
            _mainInstance = new AIDirector();
            _mainInstance._enemies = new List<Enemy>(50);
        }

        public static void Update(float deltaTime) {
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

        public static void Draw(Camera mainCamera) {
            foreach(Enemy e in _mainInstance._enemies){
                e.Draw(mainCamera);
            }
        }
        public static void DrawMinimap(Camera mainCamera) {
            foreach (Enemy e in _mainInstance._enemies) {
                e.DrawMiniMap(mainCamera);
            }
        }
        public static void DrawOnShiftInterface(Camera mainCamera) {
            foreach (Enemy e in _mainInstance._enemies) {
                e.DrawOnShiftInterface(mainCamera);
            }
        }

        public static void ClearGame() {
            for (int i = 0; i < _mainInstance._enemies.Count; i++) {
                _mainInstance._enemies[i].DestroySelf();
            }
            _mainInstance._enemies.Clear();
        }

        public static void StartNewGame() {
            //Create swarmers in the first 3x3 square
            for (int x = 0; x < Math.Min(MapManager.MainInstance.GridDimension, 3); x++) {
                for (int y = 0; y < Math.Min(MapManager.MainInstance.GridDimension, 3); y++) {
                    for (int i = 0; i < 5; i++) {
                        if (x == 0 && y == 0) {
                            //Skip the starting square for fairness
                            continue;
                        }
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
