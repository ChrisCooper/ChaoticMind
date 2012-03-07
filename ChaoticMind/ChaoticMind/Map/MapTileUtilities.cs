using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using FarseerPhysics.Common.Decomposition;

namespace ChaoticMind
{
    class MapTileUtilities
    {
        static List<List<Vertices>> _WallVertices;
        public static List<List<Vertices>> WallVertices
        {
            get { return _WallVertices; }
        }

        static List<List<Vertices>> _DoorVertices;

        static MapTileUtilities()
        {
            loadWallVertices();

            loadDoorVertices();

        }

        private static void loadDoorVertices()
        {
            _DoorVertices = new List<List<Vertices>>();

            List<StaticSprite> doorSprites = new List<StaticSprite>();
            doorSprites.Add(new StaticSprite("TileGeometry/DoorGeometryN"));
            doorSprites.Add(new StaticSprite("TileGeometry/DoorGeometryS"));
            doorSprites.Add(new StaticSprite("TileGeometry/DoorGeometryE"));
            doorSprites.Add(new StaticSprite("TileGeometry/DoorGeometryW"));


            foreach (StaticSprite sprite in doorSprites)
            {
                float pixelsPerMeter = sprite.CurrentTextureBounds.Width / MapTile.TileSideLength;

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

        public static List<Vertices> DoorVertices(DoorDirections door)
        {
            return _DoorVertices[door.toIndex()];
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
                float pixelsPerMeter = sprite.CurrentTextureBounds.Width / MapTile.TileSideLength;

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


        public static String appearanceStringFromDoorConfiguration(DoorDirections config)
        {
            return "TileAppearance/TileAppearance_"
                + (config.hasNorth ? "N" : "")
                + (config.hasSouth ? "S" : "")
                + (config.hasEast ? "E" : "")
                + (config.hasWest ? "W" : "");
        }

        internal static List<List<Vertices>> ClosedDoorVerticesForConfiguration(DoorDirections openDoors)
        {
            List<List<Vertices>> list = new List<List<Vertices>>();

            if (!openDoors.hasNorth)
            {
                list.Add(DoorVertices(DoorDirections.NORTH));
            }
            if (!openDoors.hasSouth)
            {
                list.Add(DoorVertices(DoorDirections.SOUTH));
            }
            if (!openDoors.hasEast)
            {
                list.Add(DoorVertices(DoorDirections.EAST));
            }
            if (!openDoors.hasWest)
            {
                list.Add(DoorVertices(DoorDirections.WEST));
            }

            return list;
        }
    }
}
