using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class CollectibleManager {

        static CollectibleManager _mainInstance;

        List<Collectable> _collectibles = new List<Collectable>();

        static CollectibleManager() {
            _mainInstance = new CollectibleManager();
        }

        internal static CollectibleManager mainInstance() {
            return _mainInstance;
        }

        //No one can make one but us
        private CollectibleManager() {
        }

        public void Update(float deltaTime) {
            for (int i = 0; i < _collectibles.Count; i++) {
                if (!_collectibles[i].ShouldDieNow()) {
                    _collectibles[i].Update(deltaTime);
                    //OOB, reset and damage the player (idiot....)
                    if (MapTile.isOutOfBounds(_collectibles[i].Position)){
                        Player.Instance.ApplyDamage(50);
                        _collectibles[i].DestroySelf();
                        //done by the collectable
                        //_collectibles.RemoveAt(i);
                        i--;
                        GameState.spawnNewObjective();
                    }
                }
                else {
                    _collectibles[i].DestroySelf();
                    //done by the collectable
                    //_collectibles.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Draw(Camera mainCamera) {
            foreach (Collectable c in _mainInstance._collectibles) {
                c.Draw(mainCamera);
            }
        }
        public void DrawMinimap(Camera mainCamera) {
            foreach (Collectable c in _mainInstance._collectibles) {
                c.DrawMiniMap(mainCamera);
            }
        }
        public void DrawOnShiftInterface(Camera mainCamera) {
            foreach (Collectable c in _mainInstance._collectibles) {
                c.DrawOnShiftInterface(mainCamera);
            }
        }

        public void ClearGame() {
            for (int i = 0; i < _mainInstance._collectibles.Count; i++) {
                _mainInstance._collectibles[i].DestroySelf();
            }
            _mainInstance._collectibles.Clear();
        }

        public static Collectable CreateCollectible(Vector2 startingPosition, CollectibleType collectibleType) {
            Collectable newCollectible = new Collectable(collectibleType, startingPosition);
            _mainInstance._collectibles.Add(newCollectible);
            return newCollectible;
        }

        public static void removeCollectible(Collectable collectable) {
            _mainInstance._collectibles.Remove(collectable);
        }
    }
}
