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
            GAMEOVER
        }

        static GameState _mainInstance;

        int _level;
        int _numObjectsCollected;
        int _numToCollect;
        bool _bossActive;
        bool _exitOpen;
        GameMode _gameMode;

        Timer _collectTimer;

        int _mapDimensions;

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

            _mainInstance._mapDimensions = Program.SharedGame.MapManager.GridDimension;

            //create the first collectable
            _mainInstance._currCollectable = new Collectable("TestImages/Collectable", 5, 2, 2, new Vector2(Utilities.randomInt(0, _mainInstance._mapDimensions), Utilities.randomInt(0, _mainInstance._mapDimensions)) * MapTile.TileSideLength);
            
            _mainInstance._collectTimer = new Timer(0.5f);
        }

        public static void CollectObject() {
            if (_mainInstance._collectTimer.isFinished){
                _mainInstance._numObjectsCollected++;
                if (_mainInstance._numObjectsCollected >= _mainInstance._numToCollect) {
                    _mainInstance._bossActive = true;
                    _mainInstance._currCollectable.MarkForDeath();
                }
                else {
                    _mainInstance._currCollectable.SetPosition(new Vector2(Utilities.randomInt(0, _mainInstance._mapDimensions), Utilities.randomInt(0, _mainInstance._mapDimensions)) * MapTile.TileSideLength);
                }
                _mainInstance._collectTimer.Reset();
            }
        }

        //SHOULD NOT BE HERE. This is just to get around the collectable bug for now
        public static void Update(float deltaTime) {
            _mainInstance._collectTimer.Update(deltaTime);
        }

        public static void KillBoss() {
            _mainInstance._bossActive = false;
            _mainInstance._exitOpen = true;
        }

        public static Collectable GetCurrCollectable() {
            return _mainInstance._currCollectable;
        }

        public static GameMode Mode {
            get { return _mainInstance._gameMode; }
            set { _mainInstance._gameMode = value; }
        }
    }
}
