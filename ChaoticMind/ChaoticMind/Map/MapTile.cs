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
        private const float SnapThreshold = 0.9999f;
        private const float MovementSpeed = 10f;

        DoorDirections _openDoors;
        float _imageRotation;


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

            _imageRotation = _openDoors.imageRotation();

            _sprite = new StaticSprite(MapTileUtilities.appearanceStringFromDoorConfiguration(openDoors));

            _pixelsPerMeter = _sprite.CurrentTextureBounds.Width / TileSideLength;

            _body = new Body(world);
            _body.Position = startingPosition;
            _body.BodyType = BodyType.Kinematic;


            loadWallFixtures();

            loadDoorFixtures();

            
        }

        public void setTarget(Vector2 target) {
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

                if (percent > SnapThreshold) {
                    snapToTarget();
                }
            }
        }

        private void snapToTarget() {
            _body.LinearVelocity = Vector2.Zero;
            _isMoving = false;
        }

        private float movementFunction(float percent) {
            return ((-Math.Abs((percent - 0.5f) * 0.99f)) + 0.5f) * MovementSpeed;
        }

        private float travelPercent() {
            return (_travelDistance - (_targetLocation - Position).Length()) / _travelDistance;
        }

        private void loadDoorFixtures() {
            foreach (List<Vertices> list in MapTileUtilities.ClosedDoorVerticesForConfiguration(_openDoors)) {
                List<Fixture> compund = FixtureFactory.AttachCompoundPolygon(list, 1.0f, _body);
            }
        }

        private void loadWallFixtures() {
            foreach (List<Vertices> list in MapTileUtilities.WallVertices) {
                List<Fixture> compund = FixtureFactory.AttachCompoundPolygon(list, 1.0f, _body);
            }
        }

        public static Vector2 WorldPositionForGridCoordinates(int row, int col) {
            return new Vector2(-TileSideLength * col, -TileSideLength * row);
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
                return _connectedDoors.imageRotation();
            }
        }

        public override float Rotation {
            get { return _imageRotation; }
        }

        internal void go() {
            _body.LinearVelocity = 5.0f * Vector2.UnitX;
        }
    }
}
