using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoticMind {
    class GameState {

        public enum GameMode {
            NORMAL,
            PAUSED,
            SHIFTING
        }

        static GameState _self;

        int _level;
        int _numObjectsCollected;
        int _numToCollect;
        bool _bossActive;
        bool _exitOpen;
        GameMode _gameMode;

        Collectable _currCollectable;

        public static void Initilize() {
            _self = new GameState();
            _self._gameMode = GameMode.NORMAL;
        }

        public static void StartLevel(int level, int numToCollect) {
            _self._level = level;
            _self._numObjectsCollected = 0;
            _self._numToCollect = numToCollect;
            _self._bossActive = false;
            _self._exitOpen = false;
            //create the first collectable
        }

        public static void CollectObject() {
            _self._numObjectsCollected++;
            if (_self._numObjectsCollected >= _self._numToCollect) {
                _self._bossActive = true;
            }
            else {
                //create another collectable
            }
        }

        public static void KillBoss() {
            _self._bossActive = false;
            _self._exitOpen = true;
        }

        public static GameMode Mode {
            get { return _self._gameMode; }
            set { _self._gameMode = value; }
        }
    }
}
