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
            for (int i = 0 ; i < _collectibles.Count ; i++){
                if (!_collectibles[i].ShouldDieNow())
                    _collectibles[i].Update(deltaTime);
                else{
                    _collectibles[i].Destroy();
                    _collectibles.RemoveAt(i);
                }
            }
        }

        public void Draw(Camera camera) {
            foreach (Collectable c in _collectibles) {
                camera.Draw(c);
            }
        }

        public static void CreateCollectible(Vector2 startingPosition,  CollectibleType collectibleType) {
            _mainInstance._collectibles.Add(new Collectable(collectibleType, startingPosition));
        }

    }
}
