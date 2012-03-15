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
    class MapTile : DrawableGameObject, IMiniMapable {

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
        bool _isVisible;

        public MapTile(World world, Vector2 startingPosition, DoorDirections openDoors, bool visible)
            : base(world, startingPosition) {
            _openDoors = openDoors;

            _connectedDoors = new DoorDirections(false, false, false, false);

            _sprite = new StaticSprite(MapTileUtilities.appearanceStringFromDoorConfiguration(openDoors), TileSideLength);

            _body = new Body(world);
            _body.Position = startingPosition;
            _body.BodyType = BodyType.Kinematic;

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
            _body.Position = _targetLocation;
            _body.LinearVelocity = Vector2.Zero;
            _isMoving = false;
        }

        private float movementFunction(float percent) {
            float part = ((percent - 0.5f) * 0.999999f);
            return ((-(part * part)) + 0.26f)*3.5f * MovementSpeed;
            //return ((-Math.Abs((percent - 0.5f) * 0.99f)) + 0.5f) * MovementSpeed;
        }

        private float travelPercent() {
            return (_travelDistance - (_targetLocation - Position).Length()) / _travelDistance;
        }

        public static Vector2 WorldPositionForGridCoordinates(int x, int y) {
            return new Vector2(TileSideLength * x, TileSideLength * y);
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
        public override Vector2 MapTileIndex {
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

        internal void go() {
            _body.LinearVelocity = 5.0f * Vector2.UnitX;
        }

        internal void shiftTo(int destX, int destY) {
            setTarget(MapTile.WorldPositionForGridCoordinates(destX, destY));
        }


        public AnimatedSprite MapSprite {
            get {
                return MapTileUtilities.getMapSprite(_openDoors, _isVisible);
            }
        }
        public Vector2 MapPosition {
            get { return Position; }
        }
        public float MapRotation {
            get { return Rotation;  }
        }

        public bool IsVisible {
            get { return _isVisible; }
            set { _isVisible = value; }
        }

        //enable/diable the collisions of the object
        public void EnableCollisions(bool b) {
            if (b){
                _body.CollidesWith = Category.All;
            }
            else{
                _body.CollidesWith = Category.None;
            }
        }
    }
}
