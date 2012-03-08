using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Input;

namespace ChaoticMind {
    enum ShiftDirection {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    class MapManager {
        int _gridWidth = 3;
        int _gridHeight = 3;

        MapTile[,] _tiles;

        private bool hasntMovedTemp = true;

        public MapManager(int mapWidth, int mapHeight) {
            _gridWidth = mapWidth;
            _gridHeight = mapHeight;
            _tiles = new MapTile[_gridHeight + 2, _gridWidth + 2];
        }

        //Make the tiles
        public void Initialize(World world) {
            for (int y = 0; y < _gridHeight; y++) {
                for (int x = 0; x < _gridWidth; x++) {
                    setTile(x, y, new MapTile(world, MapTile.WorldPositionForGridCoordinates(x, y), DoorDirections.RandomDoors()));
                }
            }
        }

        private void setTile(int x, int y, MapTile mapTile) {
            _tiles[y + 1, x + 1] = mapTile;
        }


        public void Update(float deltaTime) {
            foreach (MapTile tile in _tiles) {
                if (tile == null) {
                    continue;
                }
                tile.Update(deltaTime);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.P) && hasntMovedTemp) {
                shiftRow(0, ShiftDirection.RIGHT);
                hasntMovedTemp = false;
            }
        }

        private void shiftRow(int yIndex, ShiftDirection direction) {
            for (int x = 0; x < _gridWidth; x++) {
                shiftTile(x, yIndex, x + (direction == ShiftDirection.RIGHT ? 1 : -1), yIndex);
            }
            
            //remove the now-empty tile
            //This will be changed to be the new tile in the future
            setTile(direction == ShiftDirection.RIGHT ? -1 : _gridWidth, yIndex, null);

            for (int x = _gridWidth; x > 0; x--) {
                setTile(x , yIndex, getTile(x + direction == ShiftDirection.RIGHT ? -1 : 1, yIndex));
                ///shiftTile(0, x, 0, x + (direction == ShiftDirection.RIGHT ? 1 : -1));
            }
        }

        private void shiftTile(int tileX, int tileY, int destX, int destY) {
            MapTile tile = getTile(tileX, tileY);
            tile.shiftTo(destX, destY);
        }

        private MapTile getTile(int tileX, int tileY) {
            return _tiles[tileY + 1, tileX + 1];
        }

        public void DrawMap(Camera camera) {
            for (int y = 0; y < _gridHeight; y++) {
                for (int x = 0; x < _gridWidth; x++) {
                    MapTile tile = getTile(x, y);
                    camera.Draw(tile);
                    camera.DrawOverlay(tile, Color.White * 0.3f);
                }
            }
        }
    }
}
