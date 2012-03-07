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


namespace ChaoticMind
{
    class MapTile : DrawableGameObject
    {

        public const float TileSideLength = 10.0f;

        DoorDirections _openDoors;

        float _imageRotation;

        public MapTile(World world, Vector2 startingPosition, DoorDirections openDoors) : base(world, startingPosition)
        {
            _openDoors = openDoors;

            _imageRotation = _openDoors.imageRotation();

            _sprite = new StaticSprite(MapTileUtilities.appearanceStringFromDoorConfiguration(openDoors));

            _pixelsPerMeter = _sprite.CurrentTextureBounds.Width / TileSideLength;

            _body = new Body(world);

            loadWallFixtures();

            loadDoorFixtures();

            _body.BodyType = BodyType.Kinematic;

            _body.Position = startingPosition;
        }

        private void loadDoorFixtures()
        {
            foreach (List<Vertices> list in MapTileUtilities.ClosedDoorVerticesForConfiguration(_openDoors))
            {
                List<Fixture> compund = FixtureFactory.AttachCompoundPolygon(list, 1.0f, _body);
            }
        }

        private void loadWallFixtures()
        {
            foreach (List<Vertices> list in MapTileUtilities.WallVertices)
            {
                List<Fixture> compund = FixtureFactory.AttachCompoundPolygon(list, 1.0f, _body);
            }
        }

        public static Vector2 WorldPositionForGridCoordinates(int row, int col)
        {
            return new Vector2(-TileSideLength * col, -TileSideLength * row);
        }

        public override float Rotation
        {
            get { return _imageRotation; }
        }
    }
}
