using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class GameState {

        public enum GameMode {
            NORMAL,
            PAUSED,
            SHIFTING,
            GAMEOVERWIN,
            GAMEOVERLOSE
        }

        static GameState _mainInstance;

        int _level;
        int _numObjectsCollected;
        int _numToCollect;
        bool _bossActive;
        bool _exitOpen;
        GameMode _gameMode;

        Collectable _currCollectable;

        public static void Initilize() {
            _mainInstance = new GameState();
            _mainInstance._gameMode = GameMode.NORMAL;
        }

        public static void StartLevel(int level, int numToCollect) {
            _mainInstance._level = level;
            _mainInstance._numObjectsCollected = 0;
            _mainInstance._numToCollect = numToCollect;
            _mainInstance._bossActive = false;
            _mainInstance._exitOpen = false;

            //create the first collectable
            spawnNewObjective();
        }

        private static void spawnNewObjective() {
            _mainInstance._currCollectable = CollectibleManager.CreateCollectible(MapTile.RandomPositionInTile(Utilities.randomInt(0, Program.SharedGame.MapManager.GridDimension), Utilities.randomInt(0, Program.SharedGame.MapManager.GridDimension)), CollectibleType.ObjectiveType);
        }

        public static void KillBoss() {
            _mainInstance._bossActive = false;
            _mainInstance._exitOpen = true;
        }

        public static GameMode Mode {
            get { return _mainInstance._gameMode; }
            set { _mainInstance._gameMode = value; }
        }

        internal static void ObjectiveWasCollected() {
            _mainInstance._numObjectsCollected++;
            Player.Instance.GoToFullHealth();
            if (_mainInstance._numObjectsCollected >= _mainInstance._numToCollect) {
                _mainInstance._bossActive = true;
            }
            else {
                spawnNewObjective();
            }
        }
    }
}
