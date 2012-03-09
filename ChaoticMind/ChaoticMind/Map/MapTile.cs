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
        public const float TileDoorPercent = 2/16.0f;
        public const float TileWallPercent = 1/16.0f;

        DoorDirections _openDoors;

        //stores the doors that are actually useful
        DoorDirections _connectedDoors;

        public MapTile(World world, Vector2 startingPosition, DoorDirections openDoors)
            : base(world, startingPosition) {
            _openDoors = openDoors;

            _connectedDoors = _openDoors;

            _sprite = new StaticSprite(MapTileUtilities.appearanceStringFromDoorConfiguration(openDoors), TileSideLength);

            _body = new Body(world);

            MapTileUtilities.AttachFixtures(_body, _openDoors);

            _body.BodyType = BodyType.Kinematic;

            _body.Position = startingPosition;
        }

        public static Vector2 WorldPositionForGridCoordinates(int row, int col) {
            return new Vector2(-TileSideLength * col, -TileSideLength * row);
        }

        //TODO
        //compute connected doors
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
    }
}
