using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace ChaoticMind {
    enum ShiftDirection {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    //store shift data
    struct MapShift {
        public MapShift(int index, ShiftDirection dir, DoorDirections newDoors) {
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
        int _gridDimension;
        float _edgeOfMapdimesion;

        MapTile[,] _tiles;
        LinkedList<MapShift> _shiftQueue;

        private const float NERVE_PULSE_SPEED = 0.003f;
        private const float NERVE_MAX_ALPHA = 0.9f;
        private const float NERVE_MIN_ALPHA = 0.3f;
        public float NervePulseTimeCount;

        private MapTile _shiftedOutTile;

        bool _isShifting = false;
        int _currShiftIndex;
        ShiftDirection _currShiftDir;

        GameObjects _objectsOwner;

        //constructor
        public MapManager(GameObjects objectsOwner, int gridDimension) {
            _objectsOwner = objectsOwner;
            _shiftQueue = new LinkedList<MapShift>();
        }

        //Make the tiles
        public void StartNewGame(int gridDimension) {
            _gridDimension = gridDimension;

            _edgeOfMapdimesion = _gridDimension * MapTile.TileSideLength;

            _tiles = new MapTile[_gridDimension, _gridDimension];

            for (int y = 0; y < _gridDimension; y++) {
                for (int x = 0; x < _gridDimension; x++) {
                    _tiles[x, y] = new MapTile(_objectsOwner, MapTile.WorldPositionForGridCoordinates(x, y), DoorDirections.RandomDoors(), false);
                }
            }
            //initially set the overlays
            UpdateOverlays();
            UpdateVisibilities();
        }

        private void UpdateVisibilities() {
            for (int x = 0; x < _tiles.GetLength(0); x++) {
                for (int y = 0; y < _tiles.GetLength(1); y++) {
                    if (_tiles[x, y] != null) {
                        _tiles[x, y].updateVisibility();
                        _tiles[x, y].updateConnectedDoors(getTileDoors(x, y - 1), getTileDoors(x, y + 1), getTileDoors(x + 1, y), getTileDoors(x - 1, y));
                    }
                }
            }
        }

        public void Update(float deltaTime) {

            foreach (MapTile tile in _tiles) {
                tile.Update(deltaTime);
            }
            if (_shiftedOutTile != null) {
                _shiftedOutTile.Update(deltaTime);
            }

            UpdateVisibilities();

            //process queue
            if (_shiftQueue.Count > 0) {
                if (!_isShifting) {
                    MapShift temp = _shiftQueue.First.Value;
                    _shiftQueue.RemoveFirst();
                    shiftTiles(temp.Index, temp.Direction, temp.TileDoors);
                }
            }

            NervePulseTimeCount += deltaTime*1000;
        }

        //TODO: probably not the most efficient way of doing it
        //better way might be to have each tile analyze and set the tiles to the north and west of them
        private void UpdateOverlays() {
            for (int x = 0; x < _tiles.GetLength(0); x++) {
                for (int y = 0; y < _tiles.GetLength(1); y++) {
                    if (_tiles[x, y] != null) {
                        _tiles[x, y].updateConnectedDoors(getTileDoors(x, y - 1), getTileDoors(x, y + 1), getTileDoors(x + 1, y), getTileDoors(x - 1, y));
                    }
                }
            }
        }

        //add a shift to the queue
        public void queueShift(int index, ShiftDirection dir, DoorDirections tileDoors, bool pritority) {
            if (pritority)
                _shiftQueue.AddFirst(new MapShift(index, dir, tileDoors));
            else
                _shiftQueue.AddLast(new MapShift(index, dir, tileDoors));
        }

        //shift the row/col of tiles
        private void shiftTiles(int index, ShiftDirection dir, DoorDirections newTileDoors) {

            if (_isShifting) {
                //should never happen with the queueing now in place, but still check it to be safe
                return;
            }

            //check if the index is valid
            if (index < 0 || index >= _gridDimension) {
                throw new Exception("Invalid grid index passed to shiftTiles()");
            }

            //set current shift direction
            _currShiftDir = dir;
            _currShiftIndex = index;

            _objectsOwner.MainCamera.shake();

            //spawn extra enemies
            _objectsOwner.EnemyDirector.OnShift();

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
                MapTile pushingTile = new MapTile(_objectsOwner, MapTile.WorldPositionForGridCoordinates(shiftEnd + shiftInc, index), newTileDoors, false);
                _tiles[shiftEnd, index] = pushingTile;
                shiftTile(pushingTile, shiftEnd, index);
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
                MapTile pushingTile = new MapTile(_objectsOwner, MapTile.WorldPositionForGridCoordinates(index, shiftEnd + shiftInc), newTileDoors, false);
                _tiles[index, shiftEnd] = pushingTile;
                shiftTile(pushingTile, index, shiftEnd);
            }

            _shiftedOutTile.flagForDestruction(tileFinishedShifting);

            //update the overlays
            UpdateOverlays();

            _isShifting = true;
        }

        internal void tileFinishedShifting(MapTile finishedTile) {
            _shiftedOutTile.WasKilled();
            _shiftedOutTile = null;
            _isShifting = false;
        }

        private void shiftTile(int tileX, int tileY, int destX, int destY) {
            shiftTile(_tiles[tileX, tileY], destX, destY);
        }
        private void shiftTile(MapTile tile, int destX, int destY) {
            tile.shiftTo(destX, destY);
        }

        public void DrawTiles(Camera camera) {
            float alpha = NERVE_MIN_ALPHA + (NERVE_MAX_ALPHA - NERVE_MIN_ALPHA) * (float)(Math.Sin(NervePulseTimeCount * NERVE_PULSE_SPEED) + 1) / 2.0f;
            for (int y = 0; y < _gridDimension; y++) {
                for (int x = 0; x < _gridDimension; x++) {
                    MapTile tile = _tiles[x, y];
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

        internal void DrawGlows(Camera camera) {
        }

        //Minimap
        public void DrawMap(HUD.MinimapDisplay display) {
            for (int y = 0; y < _gridDimension; y++) {
                for (int x = 0; x < _gridDimension; x++) {
                    MapTile tile = _tiles[x, y];
                    display.DrawMinimap(tile);
                }
            }
            if (_shiftedOutTile != null) {
                display.DrawMinimap(_shiftedOutTile, 1.0f - _shiftedOutTile.travelPercent());
            }
        }

        public DoorDirections getTileDoors(int x, int y) {
            if (x >= 0 && x < _gridDimension && y >= 0 && y < _gridDimension) {
                return _tiles[x, y].OpenDoors;
            }
            return null;
        }

        public Vector2 GridPositionForWorldCoordinates(Vector2 worldPosition) {
            return new Vector2((float)Math.Floor((worldPosition.X + MapTile.TileSideLength / 2.0f) / MapTile.TileSideLength), (float)Math.Floor((worldPosition.Y + MapTile.TileSideLength / 2.0f) / MapTile.TileSideLength));
        }

        public int GridDimension {
            get { return _gridDimension; }
        }

        public MapTile[,] Tiles {
            get { return _tiles; }
        }

        //The fartherst point still on the map
        public float EdgeOfMapDimesion {
            get { return _edgeOfMapdimesion; }
        }

        //objects call these to shift themselves
        public bool isShifting(Vector2 position) {
            Vector2 tempGridCoord = GridPositionForWorldCoordinates(position);
            if (_isShifting) {
                if (((_currShiftDir == ShiftDirection.LEFT || _currShiftDir == ShiftDirection.RIGHT) && tempGridCoord.Y == _currShiftIndex) ||
                    ((_currShiftDir == ShiftDirection.UP || _currShiftDir == ShiftDirection.DOWN) && tempGridCoord.X == _currShiftIndex)) {
                    return true;
                }
            }
            return false;
        }
        public Vector2 shiftAmount() {
            return _tiles[_currShiftIndex, _currShiftIndex].Velocity;
        }

        internal  Vector2 RandomPositionOnMap() {
            return MapTile.RandomPositionInTile(Utilities.randomInt(0, _objectsOwner.Map.GridDimension), Utilities.randomInt(0, _objectsOwner.Map.GridDimension));
        }

        public bool IsOutOfGridBounds(Vector2 gridCoord) {
            return gridCoord.X < 0 || gridCoord.X >= GridDimension || gridCoord.Y < 0 || gridCoord.Y >= GridDimension;
        }

        internal bool IsOutOfBounds(Vector2 worldPosition) {
            return IsOutOfGridBounds(GridPositionForWorldCoordinates(worldPosition));
        }
    }
}
