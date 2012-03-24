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

        float nerve_pulse_speed = 0.001f;
        float nerve_pulse_alpha = 0.6f;

        //for keeping the objects relative to tiles as they shift
        private List<DrawableGameObject> _objects;

        Camera _camera;

        private MapTile _shiftedOutTile;

        //constructor
        public MapManager(int gridDimension) {
            _gridDimension = gridDimension;
            _tiles = new MapTile[_gridDimension, _gridDimension];
            _shiftQueue = new LinkedList<Shift>();
        }

        //Make the tiles
        public void Initialize(Camera camera, ref List<DrawableGameObject> objects) {
            for (int y = 0; y < _gridDimension; y++) {
                for (int x = 0; x < _gridDimension; x++) {
                    _tiles[x,y] = new MapTile(MapTile.WorldPositionForGridCoordinates(x, y), DoorDirections.RandomDoors(), false);
                }
            }
            //initially set the overlays
            UpdateOverlays();

            //set up the timer (make sure to update if the shift takes longer than 3 sec or it'll run into problems)
            _timerId = TimeDelayManager.InitTimer(3);

            _objects = objects;
            _camera = camera;
        }

        public void Update(float deltaTime) {
            //set visibility based on player position
            Vector2 playerPos = Program.SharedGame.MainPlayer.MapTileIndex;
            for (int x = (int)playerPos.X - 1; x < (int)playerPos.X + 2; x++) {
                for (int y = (int)playerPos.Y - 1; y < (int)playerPos.Y + 2; y++) {
                    if (x >= 0 && x < _gridDimension && y >= 0 && y < _gridDimension && !_tiles[x, y].IsVisible) {
                        _tiles[x, y].IsVisible = true;
                    }
                }
            }

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

            //get all objects that are in the shifted row
            List<DrawableGameObject> shiftingObjects = new List<DrawableGameObject>(20);
            foreach (DrawableGameObject o in _objects) {
                if (((dir == ShiftDirection.LEFT || dir == ShiftDirection.RIGHT) && o.MapTileIndex.Y == index) ||
                    ((dir == ShiftDirection.UP || dir == ShiftDirection.DOWN) && o.MapTileIndex.X == index)){
                    shiftingObjects.Add(o);
                }
            }

            _camera.shake();

            bool isPositiveShift = (dir == ShiftDirection.RIGHT || dir == ShiftDirection.DOWN);

            int shiftStart = isPositiveShift ? _gridDimension - 1 : 0;
            int shiftEnd = isPositiveShift ? 0 : _gridDimension - 1;
            int shiftInc = isPositiveShift ? -1 : 1;

            if (dir == ShiftDirection.LEFT || dir == ShiftDirection.RIGHT) {
                //Set all the tiles to start moving
                for (int x = 0; x < _gridDimension; x++) {
                    shiftTile(x, index, x - shiftInc, index, shiftingObjects);
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
                shiftTile(pushingTile, shiftEnd, index, shiftingObjects);
            }
            else { //UP or DOWN
                //Set all the tiles to start moving
                for (int y = 0; y < _gridDimension; y++) {
                    shiftTile(index, y, index, y - shiftInc, shiftingObjects);
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
                shiftTile(pushingTile, index, shiftEnd, shiftingObjects);
            }

            _shiftedOutTile.flagForDestruction(tileFinishedShifting);

            //update the overlays
            UpdateOverlays();

            //reset the timer
            TimeDelayManager.RestartTimer(_timerId);
        }

        internal void tileFinishedShifting(MapTile finishedTile) {
            finishedTile.Destroy();
            _shiftedOutTile = null;
        }

        private void shiftTile(int tileX, int tileY, int destX, int destY, List<DrawableGameObject> objects) {
            shiftTile(_tiles[tileX, tileY], destX, destY, objects);
        }
        private void shiftTile(MapTile tile, int destX, int destY, List<DrawableGameObject> objects) {
            tile.shiftTo(destX, destY, objects);
        }

        public void DrawTiles(Camera camera, float deltaTime) {
            float alpha = nerve_pulse_alpha * (float)(Math.Sin(deltaTime * nerve_pulse_speed) + 1)/2.0f;
            for (int y = 0; y < _gridDimension; y++) {
                for (int x = 0; x < _gridDimension; x++) {
                    MapTile tile = _tiles[x,y];
                    if (tile.IsVisible) {
                        camera.Draw(tile);
                        camera.DrawOverlay(tile, Color.White * alpha);
                    }
                }
            }
            if (_shiftedOutTile != null) {
                camera.Draw(_shiftedOutTile, 1.0f - _shiftedOutTile.travelPercent());
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
                camera.DrawMinimap(_shiftedOutTile, 1.0f - _shiftedOutTile.travelPercent());
            }
        }

        public DoorDirections getTileDoors(int x, int y) {
            if (x >= 0 && x < _gridDimension && y >= 0 && y < _gridDimension) {
                return _tiles[x, y].OpenDoors;
            }
            return null;
        }

        public int GridDimension {
            get { return _gridDimension; }
        }

        public MapTile[,] Tiles {
            get { return _tiles; }
        }
    }
}
