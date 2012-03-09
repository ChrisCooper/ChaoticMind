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

        World _world;
        private MapTile _shiftedOutTile;

        public MapManager(int mapWidth, int mapHeight) {
            _gridWidth = mapWidth;
            _gridHeight = mapHeight;
            _tiles = new MapTile[_gridWidth, _gridHeight];
        }

        //Make the tiles
        public void Initialize(World world) {
            _world = world;
            for (int y = 0; y < _gridHeight; y++) {
                for (int x = 0; x < _gridWidth; x++) {
                    _tiles[x,y] = new MapTile(_world, MapTile.WorldPositionForGridCoordinates(x, y), DoorDirections.RandomDoors());
                }
            }
        }

        public void Update(float deltaTime) {
            foreach (MapTile tile in _tiles) {
                if (tile == null) {
                    continue;
                }
                tile.Update(deltaTime);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.P) && hasntMovedTemp) {
                shiftRow(0, ShiftDirection.RIGHT, DoorDirections.RandomDoors());
                hasntMovedTemp = false;
            }
        }

        private void shiftRow(int yIndex, ShiftDirection direction, DoorDirections newTileDoors) {
            //Set all the tiles to start moving
            for (int x = 0; x < _gridWidth; x++) {
                shiftTile(x, yIndex, x + (direction == ShiftDirection.RIGHT ? 1 : -1), yIndex);
            }
            
            //Shift all the tiles in the array to their new positions 
            if (direction == ShiftDirection.RIGHT) {
                _shiftedOutTile = _tiles[_gridWidth - 1, yIndex];

                //shift the tiles in our array to represent the new arrangement
                for (int x = _gridWidth - 1; x > 0; x--) {
                    _tiles[x, yIndex] = _tiles[x-1, yIndex];
                }

                //TODO: don't reassign this. pass it in.
                MapTile pushingTile = new MapTile(_world, MapTile.WorldPositionForGridCoordinates(-1, yIndex), newTileDoors);
                _tiles[0, yIndex] = pushingTile;
                pushingTile.shiftTo(0, yIndex);
            }
            else {
                _shiftedOutTile = _tiles[0, yIndex];
                //shift the tiles in our array to represent the new arrangement
                for (int x = 0; x < _gridWidth-2; x++) {
                    _tiles[x, yIndex] = _tiles[x + 1, yIndex];
                }

                //TODO: don't reassign this. pass it in.
                int newX = _gridHeight-1;
                MapTile pushingTile = new MapTile(_world, MapTile.WorldPositionForGridCoordinates(newX + 1, yIndex), newTileDoors);
                _tiles[newX, yIndex] = pushingTile;
            }
        }

        private void shiftTile(int tileX, int tileY, int destX, int destY) {
            _tiles[tileX, tileY].shiftTo(destX, destY);
        }

        public void DrawMap(Camera camera) {
            for (int y = 0; y < _gridHeight; y++) {
                for (int x = 0; x < _gridWidth; x++) {
                    MapTile tile = _tiles[x,y];
                    camera.Draw(tile);
                    camera.DrawOverlay(tile, Color.White * 0.3f);
                }
            }
        }
    }
}
