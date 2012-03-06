using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;


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

    //Body type should be Kinematic
    class MapTile : DrawableGameObject
    {

        public const float TileSideLength = 20.0f;

        public MapTile(World world, Vector2 startingPosition, DoorDirection openDoors) : base(world, startingPosition)
        {

            _sprite = new StaticSprite(resourseStringFromDoorConfiguration(openDoors));

            _pixelsPerMeter = _sprite.CurrentTextureBounds.Width / TileSideLength;


            uint[] data = new uint[_sprite.Texture.Width * _sprite.Texture.Height];

            _sprite.Texture.GetData(data);

            //Generate a Polygon
            Vertices verts = PolygonTools.CreatePolygon(data, _sprite.Texture.Width);

            //Center
            Vector2 centeringVector = new Vector2(-_sprite.Texture.Width/2, -_sprite.Texture.Height/2);
            verts.Translate(ref centeringVector);

            //Scale
            Vector2 scale = new Vector2(1/_pixelsPerMeter, 1/_pixelsPerMeter);
            verts.Scale(ref scale);
            
   

            //Since it is a concave polygon, we need to partition it into several smaller convex polygons

            List<Vertices> list = BayazitDecomposer.ConvexPartition(verts);


            _body = new Body(world);
            List<Fixture> compund = FixtureFactory.AttachCompoundPolygon(list, 1.0f, _body);

            _body.BodyType = BodyType.Kinematic;

            _body.AngularVelocity = 0.3f;
           
            // This method creates a body (has mass, position, rotation),
            // as well as a rectangular fixture, which is just a shape stapled to the body.
            // The fixture is what collides with other objects and impacts how the body moves
            //_body = BodyFactory.CreateRectangle(world, TileSideLength, TileSideLength, 1.0f);
            //_body.BodyType = BodyType.Kinematic;
            _body.Position = startingPosition;
        }

        private String resourseStringFromDoorConfiguration(DoorDirection config)
        {
            return "TileGeometry/TileGeometry_NSEW";
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
    }
}
