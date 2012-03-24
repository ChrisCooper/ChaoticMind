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

        static GameState _self;

        int _level;
        int _numObjectsCollected;
        int _numToCollect;
        bool _bossActive;
        bool _exitOpen;
        GameMode _gameMode;

        int _collectTimer;

        int _mapDimensions;

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

            _self._mapDimensions = Program.SharedGame.MapManager.GridDimension;

            //create the first collectable
            _self._currCollectable = new Collectable("TestImages/Collectable", 5, 2, 2, new Vector2(Utilities.randomInt(0, _self._mapDimensions), Utilities.randomInt(0, _self._mapDimensions)) * MapTile.TileSideLength);
            
            _self._collectTimer = TimeDelayManager.InitTimer(0.5f);
        }

        public static void CollectObject() {
            if (TimeDelayManager.Finished(_self._collectTimer)){
                _self._numObjectsCollected++;
                if (_self._numObjectsCollected >= _self._numToCollect) {
                    _self._bossActive = true;
                    _self._currCollectable.MarkForDeath();
                }
                else {
                    _self._currCollectable.SetPosition(new Vector2(Utilities.randomInt(0, _self._mapDimensions), Utilities.randomInt(0, _self._mapDimensions)) * MapTile.TileSideLength);
                }
                TimeDelayManager.RestartTimer(_self._collectTimer);
            }
        }

        public static void KillBoss() {
            _self._bossActive = false;
            _self._exitOpen = true;
        }

        public static Collectable GetCurrCollectable() {
            return _self._currCollectable;
        }

        public static GameMode Mode {
            get { return _self._gameMode; }
            set { _self._gameMode = value; }
        }
    }
}
