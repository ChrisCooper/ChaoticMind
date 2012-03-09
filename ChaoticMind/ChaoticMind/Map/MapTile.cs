using System;
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
    class MapTile : DrawableGameObject {

        public const float TileSideLength = 24.0f;
        public const float TileDoorPercent = 2 / 16.0f;
        public const float TileWallPercent = 1 / 16.0f;
        private const float SnapThreshold = 0.99999f;
        private const float MovementSpeed = 35f;

        DoorDirections _openDoors;


        //Todo: abstract into travel class
        Vector2 _startLocation;
        Vector2 _targetLocation;
        float _travelDistance;
        //stores the doors that are actually useful
        DoorDirections _connectedDoors;

        Vector2 _travelDirection;

        bool _isMoving;

        public MapTile(World world, Vector2 startingPosition, DoorDirections openDoors)
            : base(world, startingPosition) {
            _openDoors = openDoors;

            _connectedDoors = _openDoors;

            _sprite = new StaticSprite(MapTileUtilities.appearanceStringFromDoorConfiguration(openDoors));

            _pixelsPerMeter = _sprite.CurrentTextureBounds.Width / TileSideLength;

            _body = new Body(world);
            _body.Position = startingPosition;
            _body.BodyType = BodyType.Kinematic;


            MapTileUtilities.AttachFixtures(_body, _openDoors);


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

        public override void Update(float deltaTime) {

            if (_isMoving) {
                float percent = travelPercent();

                _body.LinearVelocity = (_travelDirection) * movementFunction(percent);

                //Are we there yet?
                if (percent > SnapThreshold || Vector2.Dot(_travelDirection, _targetLocation - Position) < 0) {
                    snapToTarget();
                }
            }
        }

        private void snapToTarget() {
            _body.LinearVelocity = Vector2.Zero;
            _isMoving = false;
        }

        private float movementFunction(float percent) {
            float part = ((percent - 0.5f) * 0.999999f);
            return ((-(part * part)) + 0.27f)*3.5f * MovementSpeed;
            //return ((-Math.Abs((percent - 0.5f) * 0.99f)) + 0.5f) * MovementSpeed;
        }

        private float travelPercent() {
            return (_travelDistance - (_targetLocation - Position).Length()) / _travelDistance;
        }

        public static Vector2 WorldPositionForGridCoordinates(int x, int y) {
            return new Vector2(TileSideLength * x, TileSideLength * y);
        }


        public void updateConnectedDoors() {
            //dummy return
            _connectedDoors = _openDoors;
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

        internal void go() {
            _body.LinearVelocity = 5.0f * Vector2.UnitX;
        }

        internal void shiftTo(int destX, int destY) {
            setTarget(MapTile.WorldPositionForGridCoordinates(destX, destY));
        }
    }
}
