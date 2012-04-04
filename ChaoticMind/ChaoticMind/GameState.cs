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
            _mainInstance._level = 0;
            _mainInstance._gameMode = GameMode.PREGAME;
        }

        public static void StartNewGame(int level, int numToCollect) {
            _mainInstance._level = level;
            _mainInstance._numObjectsCollected = 0;
            _mainInstance._numToCollect = numToCollect;
            _mainInstance._bossActive = false;
            _mainInstance._exitOpen = false;

            //create the first collectable
            spawnNewObjective();
        }

        public static void ClearGame() {
            if (_mainInstance._currCollectable != null) {
                _mainInstance._currCollectable.DestroySelf();
                _mainInstance._currCollectable = null;
            }
        }

        public static void spawnNewObjective() {
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

        public static bool BossActive {
            get { return _mainInstance._bossActive; }
        }
        public static int Level {
            get { return _mainInstance._level; }
        }
        public static bool ExitOpen {
            get { return _mainInstance._exitOpen; }
        }
    }
}
