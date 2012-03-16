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
        int _gridDimension = 3;

        MapTile[,] _tiles;

        //shift timing
        //make sure to update if the shifting speed is altered
        public const float MinShiftTime = 3000; //3 seconds
        private DateTimeOffset _lastShiftTime = DateTimeOffset.Now.AddMilliseconds(-MinShiftTime);

        World _world;
        Camera _camera;

        private MapTile _shiftedOutTile;

        public MapManager(int gridDimension) {
            _gridDimension = gridDimension;
            _tiles = new MapTile[_gridDimension, _gridDimension];
        }

        //Make the tiles
        public void Initialize(World world, Camera camera) {
            _world = world;
            for (int y = 0; y < _gridDimension; y++) {
                for (int x = 0; x < _gridDimension; x++) {
                    //TODO: visible = false on start
                    _tiles[x,y] = new MapTile(_world, MapTile.WorldPositionForGridCoordinates(x, y), DoorDirections.RandomDoors(), true);
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
            if (_shiftedOutTile != null) {
                _shiftedOutTile.Update(deltaTime);
            }

            //temp shifting logic
            if (InputManager.IsKeyClicked(Keys.Up)) {
                shiftTiles(0, ShiftDirection.UP, DoorDirections.RandomDoors());
            }
            else if (InputManager.IsKeyClicked(Keys.Down)) {
                shiftTiles(0, ShiftDirection.DOWN, DoorDirections.RandomDoors());
            }
            else if (InputManager.IsKeyClicked(Keys.Left)) {
                shiftTiles(0, ShiftDirection.LEFT, DoorDirections.RandomDoors());
            }
            else if (InputManager.IsKeyClicked(Keys.Right)) {
                shiftTiles(0, ShiftDirection.RIGHT, DoorDirections.RandomDoors());
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
        public void shiftTiles(int index, ShiftDirection dir, DoorDirections newTileDoors) {

            if (shiftTimePercent() < 1){
                return;
            }

            //check if the index is valid
            if (index < 0 || index >= _gridDimension) {
                throw new Exception("Invalid grid index passed to shiftTiles()");
            }

            _lastShiftTime = DateTimeOffset.Now;
            _camera.shake();


            bool isPositiveShift = (dir == ShiftDirection.RIGHT || dir == ShiftDirection.DOWN);

            int shiftStart = isPositiveShift ? _gridDimension - 1 : 0;
            int shiftEnd = isPositiveShift ? 0 : _gridDimension - 1;
            int shiftInc = isPositiveShift ? -1 : 1;

            if (dir == ShiftDirection.LEFT || dir == ShiftDirection.RIGHT) {

                //Set all the tiles to start moving
                for (int x = 0; x < _gridDimension; x++) {
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
                //Set all the tiles to start moving
                for (int y = 0; y < _gridDimension; y++) {
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

            _shiftedOutTile.flagForDestruction(tileFinishedShifting);

            //update the overlays
            UpdateOverlays();
        }

        internal void tileFinishedShifting(MapTile finishedTile) {
            finishedTile.destroySelf(_world);
            _shiftedOutTile = null;
        }

        private void shiftTile(int tileX, int tileY, int destX, int destY) {
            _tiles[tileX, tileY].shiftTo(destX, destY);
        }

        public void DrawTiles(Camera camera) {
            for (int y = 0; y < _gridDimension; y++) {
                for (int x = 0; x < _gridDimension; x++) {
                    MapTile tile = _tiles[x,y];
                    camera.Draw(tile);
                    camera.DrawOverlay(tile, Color.White * 0.3f);
                }
            }
            if (_shiftedOutTile != null) {
                camera.Draw(_shiftedOutTile);
            }
        }

        //Minimap
        public void DrawMap(Camera camera) {
            for (int y = 0; y < _gridDimension; y++) {
                for (int x = 0; x < _gridDimension; x++) {
                    MapTile tile = _tiles[x, y];
                    camera.DrawMinimap(tile);
                }
            }
            if (_shiftedOutTile != null) {
                camera.DrawMinimap(_shiftedOutTile);
            }
        }

        public DoorDirections getTileDoors(int x, int y) {
            if (x >= 0 && x < _gridDimension && y >= 0 && y < _gridDimension) {
                return _tiles[x, y].OpenDoors;
            }
            return null;
        }


        public MapTile[,] Tiles {
            get { return _tiles; }
        }
    }
}
