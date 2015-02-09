﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using Microsoft.Xna.Framework.Graphics;


namespace ChaoticMind {

    delegate void ShiftFinishListener(MapTile finishedTile);

    class MapTile : DrawableGameObject, IMiniMapable {

        public const float TileSideLength = 20.0f;
        public const float TileDoorPercent = 2 / 16.0f;
        public const float TileWallPercent = 1 / 16.0f;
        private const float SnapThreshold = 0.99999f;
        private const float MovementSpeed = 35f;

        private event ShiftFinishListener signalEndOfShift;

        DoorDirections _openDoors;

        //Todo: abstract into travel class
        Vector2 _startLocation;
        Vector2 _targetLocation;
        float _travelDistance;

        //stores the doors that are actually useful
        DoorDirections _connectedDoors;

        Vector2 _travelDirection;

        bool _isMoving;
        bool _isVisible;

        public MapTile(Vector2 startingPosition, DoorDirections openDoors, bool visible)
            : base(startingPosition) {
            _openDoors = openDoors;

            _connectedDoors = new DoorDirections(false, false, false, false);

            _sprite = new StaticSprite(MapTileUtilities.appearanceStringFromDoorConfiguration(openDoors), TileSideLength, DrawLayers.GameElements.TileGround);

            _body = new Body(Program.DeprecatedObjects.PhysicsWorld);
            _body.Position = startingPosition;
            _body.BodyType = BodyType.Kinematic;
            _body.UserData = this;

            MapTileUtilities.AttachFixtures(_body, _openDoors);

            _isVisible = visible;
        }

        private void setTarget(Vector2 target) {
            if (_isMoving) {
                throw new Exception("Can't set tile target while it's already moving.");
            }

            _startLocation = Position;
            _targetLocation = target;
            _travelDistance = (_targetLocation - _startLocation).Length();
            _travelDirection = (_targetLocation - _startLocation);
            _travelDirection.Normalize();

            _isMoving = true;
        }

        internal void flagForDestruction(ShiftFinishListener listener) {
            signalEndOfShift = listener;
        }

        public override void Update(float deltaTime) {

            if (_isMoving) {
                float percent = travelPercent();
                Vector2 vel = (_travelDirection) * movementFunction(percent);

                _body.LinearVelocity = vel;

                //Are we there yet?
                if (percent > SnapThreshold || Vector2.Dot(_travelDirection, _targetLocation - Position) < 0) {
                    snapToTarget();
                }
            }

            if (Math.Abs(GridCoordinate.X - Program.DeprecatedObjects.MainPlayer.GridCoordinate.X) <= Program.DeprecatedObjects.MainPlayer.SightGridDistance &&
                Math.Abs(GridCoordinate.Y - Program.DeprecatedObjects.MainPlayer.GridCoordinate.Y) <= Program.DeprecatedObjects.MainPlayer.SightGridDistance) {
                IsVisible = true;
            }
        }

        private void snapToTarget() {
            _body.Position = _targetLocation;
            _body.LinearVelocity = Vector2.Zero;
            _isMoving = false;

            finishShift();
        }

        private void finishShift() {
            if (signalEndOfShift != null) {
                signalEndOfShift(this);
            }
        }

        private float movementFunction(float percent) {
            float part = ((percent - 0.5f) * 0.999999f);
            return ((-(part * part)) + 0.26f)*3.5f * MovementSpeed;
            //return ((-Math.Abs((percent - 0.5f) * 0.99f)) + 0.5f) * MovementSpeed;
        }

        public float travelPercent() {
            return (_travelDistance - (_targetLocation - Position).Length()) / _travelDistance;
        }

        public static Vector2 WorldPositionForGridCoordinates(int x, int y) {
            return new Vector2(TileSideLength * x, TileSideLength * y);
        }
    
        public static Vector2 GridPositionForWorldCoordinates(Vector2 worldPosition) {
            return new Vector2((float)Math.Floor((worldPosition.X + MapTile.TileSideLength / 2.0f) / MapTile.TileSideLength), (float)Math.Floor((worldPosition.Y + MapTile.TileSideLength / 2.0f) / MapTile.TileSideLength)); 
        }
        public static bool isOutOfBounds(Vector2 position) {
            return isOutOfBoundsGrid(GridPositionForWorldCoordinates(position));
        }
        public static bool isOutOfBoundsGrid(Vector2 gridCoord) {
            return gridCoord.X < 0 || gridCoord.X >= Program.DeprecatedObjects.Map.GridDimension || gridCoord.Y < 0 || gridCoord.Y >= Program.DeprecatedObjects.Map.GridDimension;
        }


        //updates the connected doors
        public void updateConnectedDoors(DoorDirections n, DoorDirections s, DoorDirections e, DoorDirections w) {
            _connectedDoors.hasNorth = _openDoors.hasNorth && n != null && n.hasSouth;
            _connectedDoors.hasSouth = _openDoors.hasSouth && s != null && s.hasNorth;
            _connectedDoors.hasEast = _openDoors.hasEast && e != null && e.hasWest;
            _connectedDoors.hasWest = _openDoors.hasWest && w != null && w.hasEast;
        }

        //override to deal with shifting
        //will get an int back every time even though it's floating point division
        public override Vector2 GridCoordinate {
            get {
                if (_isMoving) {
                    return _targetLocation / TileSideLength;
                }
                else {
                    return _body.Position / TileSideLength; ;
                }
            }
        }

        public DoorDirections OpenDoors {
            get { return _openDoors; }
        }

        public StaticSprite Overlay {

            get {
                return MapTileUtilities.getOverlay(_connectedDoors);
            }
        }

        public float OverlayRotation {
            get {
                return _connectedDoors.tileRotation();
            }
        }

        public StaticSprite ShiftTexture {
            get {
                return MapTileUtilities.getShiftSprite(_openDoors, _isVisible);
            }
        }

        internal void shiftTo(int destX, int destY) {
            //go
            setTarget(MapTile.WorldPositionForGridCoordinates(destX, destY));
        }


        public override AnimatedSprite MapSprite {
            get {
                return MapTileUtilities.getMapSprite(_openDoors, _isVisible);
            }
        }

        public override Vector2 MapPosition {
            get { return Position; }
        }

        public override float MapRotation {
            get { return Rotation;  }
        }

        public override float MapDrawLayer {
            get { return DrawLayers.HUD.Minimap_Maze; }
        }

        public bool IsVisible {
            get { return _isVisible; }
            set { _isVisible = value; }
        }

        internal static Vector2 RandomPositionInTile(int x, int y) {
            return MapTile.WorldPositionForGridCoordinates(x, y) + Utilities.randomVector() * 0.7f * MapTile.TileSideLength;
        }

        public float OverlayDrawLayer { get { return DrawLayers.GameElements.TileOverlay; } }

        public Vector2 Velocity {
            get { return _body.LinearVelocity; }
        }
    }
}
