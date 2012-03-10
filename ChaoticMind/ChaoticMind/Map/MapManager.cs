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

        //shift timing
        //make sure to update if the shifting speed is altered
        public const float MinShiftTime = 5000; //5 seconds
        private DateTimeOffset _lastShiftTime = DateTimeOffset.Now.AddMilliseconds(-MinShiftTime);

        World _world;
        Camera _camera;

        private MapTile _shiftedOutTile;

        public MapManager(int mapWidth, int mapHeight) {
            _gridWidth = mapWidth;
            _gridHeight = mapHeight;
            _tiles = new MapTile[_gridWidth, _gridHeight];
        }

        //Make the tiles
        public void Initialize(World world, Camera camera) {
            _world = world;
            for (int y = 0; y < _gridHeight; y++) {
                for (int x = 0; x < _gridWidth; x++) {
                    _tiles[x,y] = new MapTile(_world, MapTile.WorldPositionForGridCoordinates(x, y), DoorDirections.RandomDoors(), true); //TODO: visible = false on start
                }
            }
            //initially set the overlays
            UpdateOverlays();

            _camera = camera;
        }

        public void Update(float deltaTime) {
            foreach (MapTile tile in _tiles) {
                if (tile != null) {
                    tile.Update(deltaTime);
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.P) && shiftTimePercent() > 0.999f) {
                shiftTiles(0, ShiftDirection.DOWN, DoorDirections.RandomDoors());
            }
        }

        //TODO: probably not the most effecient way of doing it
        //better way might be to have each tile analyze and set the tiles to the north and west of them
        private void UpdateOverlays() {
            for (int x = 0; x < _tiles.GetLength(0); x++) {
                for (int y = 0 ; y < _tiles.GetLength(1) ; y++){
                    if (_tiles[x, y] != null) {
                        _tiles[x, y].updateConnectedDoors(getTileDoors(x, y-1), getTileDoors(x, y+1), getTileDoors(x+1, y), getTileDoors(x-1, y));
                    }
                }
            }
        }

        //returns the percent of the shift time that has elapsed since the last shift
        public float shiftTimePercent() {
            double temp = (DateTimeOffset.Now - _lastShiftTime).TotalMilliseconds / MinShiftTime;
            return temp > 1 ? 1 : (float)temp;
        }

        //shift the row/col of tiles
        //TODO: KNOWN BUG: Fixtures of old tiles are not removed from the tile that gets shifted out
        private void shiftTiles(int index, ShiftDirection dir, DoorDirections newTileDoors) {

            _lastShiftTime = DateTimeOffset.Now;

            int gridLimit = dir == ShiftDirection.LEFT || dir == ShiftDirection.RIGHT ? _gridWidth : _gridHeight;

            //check if the index is valid
            if (index < 0 || index >= gridLimit) {
                return;
            }

            _camera.shake();

            if (dir == ShiftDirection.LEFT || dir == ShiftDirection.RIGHT) {
                int shiftStart = dir == ShiftDirection.RIGHT ? gridLimit - 1 : 0;
                int shiftEnd = dir == ShiftDirection.RIGHT ? 0 : gridLimit - 1;
                int shiftInc = dir == ShiftDirection.RIGHT ? -1 : 1;

                //Set all the tiles to start moving
                for (int x = 0; x < gridLimit; x++) {
                    shiftTile(x, index, x - shiftInc, index);
                }

                //store the tile that is getting shifted out
                _shiftedOutTile = _tiles[shiftStart, index];

                //shift the tiles in our array to represent the new arrangement
                for (int x = shiftStart; x != shiftEnd; x += shiftInc) {
                    _tiles[x, index] = _tiles[x + shiftInc, index];
                }

                //set the location of the tile that was pushed in a move it into place
                //TODO: don't reassign this. pass it in.
                MapTile pushingTile = new MapTile(_world, MapTile.WorldPositionForGridCoordinates(shiftEnd + shiftInc, index), newTileDoors, true);
                _tiles[shiftEnd, index] = pushingTile;
                pushingTile.shiftTo(shiftEnd, index);
            }
            else {
                int shiftStart = dir == ShiftDirection.DOWN ? gridLimit - 1 : 0;
                int shiftEnd = dir == ShiftDirection.DOWN ? 0 : gridLimit - 1;
                int shiftInc = dir == ShiftDirection.DOWN ? -1 : 1;

                //Set all the tiles to start moving
                for (int y = 0; y < gridLimit; y++) {
                    shiftTile(index, y, index, y - shiftInc);
                }

                //store the tile that is getting shifted out
                _shiftedOutTile = _tiles[index, shiftStart];

                //shift the tiles in our array to represent the new arrangement
                for (int y = shiftStart; y != shiftEnd; y += shiftInc) {
                    _tiles[index, y] = _tiles[index, y + shiftInc];
                }

                //set the location of the tile that was pushed in a move it into place
                //TODO: don't reassign this. pass it in.
                MapTile pushingTile = new MapTile(_world, MapTile.WorldPositionForGridCoordinates(index, shiftEnd + shiftInc), newTileDoors, true);
                _tiles[index, shiftEnd] = pushingTile;
                pushingTile.shiftTo(index, shiftEnd);
            }

            //update the overlays
            UpdateOverlays();
        }

        private void shiftTile(int tileX, int tileY, int destX, int destY) {
            _tiles[tileX, tileY].shiftTo(destX, destY);
        }

        public void DrawTiles(Camera camera) {
            for (int y = 0; y < _gridHeight; y++) {
                for (int x = 0; x < _gridWidth; x++) {
                    MapTile tile = _tiles[x,y];
                    camera.Draw(tile);
                    camera.DrawOverlay(tile, Color.White * 0.3f);
                }
            }
        }

        //Minimap
        public void DrawMap(Camera camera) {
            for (int y = 0; y < _gridHeight; y++) {
                for (int x = 0; x < _gridWidth; x++) {
                    MapTile tile = _tiles[x, y];
                    camera.DrawMinimap(tile);
                }
            }
        }

        private DoorDirections getTileDoors(int x, int y) {
            if (x >= 0 && x < _gridWidth && y >= 0 && y < _gridHeight) {
                return _tiles[x, y].Doors;
            }
            return null;
        }
    }
}
