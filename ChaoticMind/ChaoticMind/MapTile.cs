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
    public enum DoorDirection
    {
        NONE = 0,
        NORTH = 1 << 0,
        SOUTH = 1 << 1,
        EAST = 1 << 2,
        WEST = 1 << 3
    };

    //This is only used to index the door array internally. All other accesses are through theDoorDirection enum
    enum DoorIndex
    {
        NORTH = 0,
        SOUTH = 1,
        EAST = 2,
        WEST = 3
    };

    class MapTile : DrawableGameObject
    {

        public const float TileSideLength = 15.0f;

        static List<List<Vertices>> _WallVertices;
        static List<List<Vertices>> _DoorVertices;

        DoorDirection _openDoors;

        public MapTile(World world, Vector2 startingPosition, DoorDirection openDoors) : base(world, startingPosition)
        {
            _openDoors = openDoors;

            _sprite = new StaticSprite(resourseStringFromDoorConfiguration(openDoors));

            _pixelsPerMeter = _sprite.CurrentTextureBounds.Width / TileSideLength;

            _body = new Body(world);

            loadWallFixtures();

            _body.BodyType = BodyType.Kinematic;

            _body.Position = startingPosition;
        }

        
        private static List<Vertices> DoorVertices(DoorDirection door) {
            return _DoorVertices[(int)DoorDiretionToIndex(door)];
        }

        private static DoorIndex DoorDiretionToIndex(DoorDirection direction)
        {
            switch (direction) {
                case DoorDirection.NORTH:
                    return DoorIndex.NORTH;
                case DoorDirection.SOUTH:
                    return DoorIndex.SOUTH;
                case DoorDirection.EAST:
                    return DoorIndex.EAST;
                case DoorDirection.WEST:
                    return DoorIndex.WEST;
                default:
                    return (DoorIndex)(-1);
            }
        }

        private void loadWallFixtures()
        {
            foreach (List<Vertices> list in _WallVertices)
            {
                List<Fixture> compund = FixtureFactory.AttachCompoundPolygon(list, 1.0f, _body);
            }
        }

        private void loadDoorFixture(DoorDirection door)
        {
            List<Fixture> compund = FixtureFactory.AttachCompoundPolygon(DoorVertices(door), 1.0f, _body);
         }

        private String resourseStringFromDoorConfiguration(DoorDirection config)
        {
            return "TileAppearance/TileAppearance_NSEW";
            return "MapTileGeo"
                + ((config & DoorDirection.NORTH) != 0 ? "N" : "")
                + ((config & DoorDirection.SOUTH) != 0 ? "S" : "")
                + ((config & DoorDirection.EAST) != 0 ? "E" : "")
                + ((config & DoorDirection.WEST) != 0 ? "W" : "") + "0";
        }

        public static DoorDirection randomDoorConfiguration()
        {
            return (DoorDirection) (Utilities.randomInt() % ((int)DoorDirection.WEST) + 1);
        }

        static MapTile()
        {
            loadWallVertices();

            _DoorVertices = new List<List<Vertices>>();

            List<StaticSprite> doorSprites = new List<StaticSprite>();
            doorSprites.Add(new StaticSprite("TileGeometry/DoorGeometryN"));
            doorSprites.Add(new StaticSprite("TileGeometry/DoorGeometryS"));
            doorSprites.Add(new StaticSprite("TileGeometry/DoorGeometryE"));
            doorSprites.Add(new StaticSprite("TileGeometry/DoorGeometryW"));


            foreach (StaticSprite sprite in doorSprites)
            {
                float pixelsPerMeter = sprite.CurrentTextureBounds.Width / TileSideLength;

                uint[] data = new uint[sprite.Texture.Width * sprite.Texture.Height];

                sprite.Texture.GetData(data);

                //Generate a Polygon
                Vertices verts = PolygonTools.CreatePolygon(data, sprite.Texture.Width);

                //Center
                Vector2 centeringVector = new Vector2(-sprite.Texture.Width / 2, -sprite.Texture.Height / 2);
                verts.Translate(ref centeringVector);

                //Scale
                Vector2 scale = new Vector2(1 / pixelsPerMeter, 1 / pixelsPerMeter);
                verts.Scale(ref scale);

                //Since it is a concave polygon, we need to partition it into several smaller convex polygons

                List<Vertices> list = BayazitDecomposer.ConvexPartition(verts);

                _DoorVertices.Add(list);
            }

        }

        private static void loadWallVertices()
        {
            _WallVertices = new List<List<Vertices>>();

            List<StaticSprite> wallSprites = new List<StaticSprite>();
            wallSprites.Add(new StaticSprite("TileGeometry/BoxWallGeometryNW"));
            wallSprites.Add(new StaticSprite("TileGeometry/BoxWallGeometryNE"));
            wallSprites.Add(new StaticSprite("TileGeometry/BoxWallGeometrySE"));
            wallSprites.Add(new StaticSprite("TileGeometry/BoxWallGeometrySW"));

            foreach (StaticSprite sprite in wallSprites)
            {
                float pixelsPerMeter = sprite.CurrentTextureBounds.Width / TileSideLength;

                uint[] data = new uint[sprite.Texture.Width * sprite.Texture.Height];

                sprite.Texture.GetData(data);

                //Generate a Polygon
                Vertices verts = PolygonTools.CreatePolygon(data, sprite.Texture.Width);

                //Center
                Vector2 centeringVector = new Vector2(-sprite.Texture.Width / 2, -sprite.Texture.Height / 2);
                verts.Translate(ref centeringVector);

                //Scale
                Vector2 scale = new Vector2(1 / pixelsPerMeter, 1 / pixelsPerMeter);
                verts.Scale(ref scale);

                //Since it is a concave polygon, we need to partition it into several smaller convex polygons

                List<Vertices> list = BayazitDecomposer.ConvexPartition(verts);

                _WallVertices.Add(list);

            }
        }

        public static Vector2 WorldPositionForGridCoordinates(int row, int col)
        {
            return new Vector2(-TileSideLength * col, -TileSideLength * row);
        }
    }
}
