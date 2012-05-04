using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class ObjectiveManager {

        int ObjectivesRemaining { get; set; }
        int ObjectivesCollected { get; set; }

        List<Collectable> _objectiveCollectables;

        GameObjects _objectsOwner;

        public ObjectiveManager(GameObjects objectsOwner, GameLevel level) {
            _objectsOwner = objectsOwner;

            _objectiveCollectables = new List<Collectable>();

            ObjectivesRemaining = level.ObjectivesToCollect;

            SpawnNewObjective();
        }

        public void SpawnNewObjective() {
            Collectable objective = new Collectable(_objectsOwner, CollectibleType.ObjectiveType, _objectsOwner.Map.RandomPositionOnMap());
            _objectiveCollectables.Add(objective);
            _objectsOwner.Collectables.Add(objective);
        }

        internal void ObjectiveWasCollected() {

            ObjectivesCollected++;
            ObjectivesRemaining--;

            _objectsOwner.MainPlayer.GoToFullHealth();

            if (ObjectivesRemaining > 0) {
                SpawnNewObjective();
            }
        }
    }
}
