using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class GameState {

        public enum GameMode {
            PREGAME,
            NORMAL,
            PAUSED,
            SHIFTING,
            GAMEOVERWIN,
            GAMEOVERLOSE,
            EXITED
        }

        int _numObjectsCollected;
        int _numToCollect;
        bool _bossActive;
        bool _exitOpen;
        GameMode _gameMode;

        Collectable _currCollectable;

        GameObjects _objectsOwner;

        public GameState(GameObjects objectsOwner) {
            _objectsOwner = objectsOwner;
            _gameMode = GameMode.NORMAL;
        }

        public  void StartNewGame(int numToCollect) {
            _numObjectsCollected = 0;
            _numToCollect = numToCollect;
            _bossActive = false;
            _exitOpen = false;

            //create the first collectable
            spawnNewObjective();
        }

        public void spawnNewObjective() {
            Collectable collectable = new Collectable(_objectsOwner, CollectibleType.ObjectiveType, _objectsOwner.Map.RandomPositionOnMap());
            _currCollectable = collectable;
            _objectsOwner.Collectables.Add(collectable);
        }

        public void KillBoss() {
            _bossActive = false;
            _exitOpen = true;
        }

        public GameMode Mode {
            get { return _gameMode; }
            set { _gameMode = value; }
        }

        internal void ObjectiveWasCollected() {
            
            _numObjectsCollected++;
            _objectsOwner.MainPlayer.GoToFullHealth();
            if (_numObjectsCollected >= _numToCollect) {
                _bossActive = true;
            }
            else {
                spawnNewObjective();
            }
        }

        public bool AllObjectivesCollected {
            get { return _bossActive; }
        }

        public bool ExitOpen {
            get { return _exitOpen; }
        }
    }
}
