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

    //store shift data
    struct Shift {
        public Shift(int index, ShiftDirection dir, DoorDirections newDoors) {
            _index = index;
            _dir = dir;
            _newDoors = newDoors;
        }
        int _index;
        ShiftDirection _dir;
        DoorDirections _newDoors;
        public int Index {
            get { return _index; }
        }
        public ShiftDirection Direction {
            get { return _dir; }
        }
        public DoorDirections TileDoors {
            get { return _newDoors; }
        }
    }

    class MapManager {
        int _gridDimension = 3;

        MapTile[,] _tiles;
        LinkedList<Shift> _shiftQueue; //note: doubly linked

        //timing
        private int _timerId;

        Camera _camera;

        private MapTile _shiftedOutTile;

        //constructor
        public MapManager(int gridDimension) {
            _gridDimension = gridDimension;
            _tiles = new MapTile[_gridDimension, _gridDimension];
            _shiftQueue = new LinkedList<Shift>();
        }

        //Make the tiles
        public void Initialize(Camera camera) {
            for (int y = 0; y < _gridDimension; y++) {
                for (int x = 0; x < _gridDimension; x++) {
                    //TODO: visible = false on start
                    _tiles[x,y] = new MapTile(MapTile.WorldPositionForGridCoordinates(x, y), DoorDirections.RandomDoors(), true);
                }
            }
            //initially set the overlays
            UpdateOverlays();

            //set up the timer (make sure to update if the shift takes longer than 3 sec or it'll run into problems)
            _timerId = TimeDelayManager.InitTimer(3);

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

            //process queue
            if (_shiftQueue.Count > 0 && TimeDelayManager.Finished(_timerId)) {
                Shift temp = _shiftQueue.First.Value;
                _shiftQueue.RemoveFirst();
                shiftTiles(temp.Index, temp.Direction, temp.TileDoors);
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

        //add a shift to the queue
        public void queueShift(int index, ShiftDirection dir, DoorDirections tileDoors, bool pritority) {
            if (pritority)
                _shiftQueue.AddFirst(new Shift(index, dir, tileDoors));
            else
                _shiftQueue.AddLast(new Shift(index, dir, tileDoors));
        }

        //shift the row/col of tiles
        private void shiftTiles(int index, ShiftDirection dir, DoorDirections newTileDoors) {

            if (!TimeDelayManager.Finished(_timerId)) {
                //should never happen with the queueing now in place, but still check it to be safe
                return;
            }

            //check if the index is valid
            if (index < 0 || index >= _gridDimension) {
                throw new Exception("Invalid grid index passed to shiftTiles()");
            }

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
                MapTile pushingTile = new MapTile(MapTile.WorldPositionForGridCoordinates(shiftEnd + shiftInc, index), newTileDoors, true);
                _tiles[shiftEnd, index] = pushingTile;
                pushingTile.shiftTo(shiftEnd, index);
            }
            else { //UP or DOWN
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
                MapTile pushingTile = new MapTile(MapTile.WorldPositionForGridCoordinates(index, shiftEnd + shiftInc), newTileDoors, true);
                _tiles[index, shiftEnd] = pushingTile;
                pushingTile.shiftTo(index, shiftEnd);
            }

            _shiftedOutTile.flagForDestruction(tileFinishedShifting);

            //update the overlays
            UpdateOverlays();

            //reset the timer
            TimeDelayManager.RestartTimer(_timerId);
        }

        internal void tileFinishedShifting(MapTile finishedTile) {
            finishedTile.destroySelf();
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
