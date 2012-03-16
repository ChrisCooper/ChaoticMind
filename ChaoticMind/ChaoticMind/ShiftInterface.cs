using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticMind {
    class ShiftInterface {

        MapManager _mapManager;
        SpriteBatch _spriteBatch;



        internal void Initialize(MapManager mapManager, SpriteBatch spriteBatch) {
            _mapManager = mapManager;
            _spriteBatch = spriteBatch;
        }

        internal void Update() {
        }

        internal void DrawInterface() {
            MapTile[,] tiles = _mapManager.Tiles;



            //drawShiftButtons();
        }

        
    }
}
