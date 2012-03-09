using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace ChaoticMind {
    class MapTileUtilities {

        static List<StaticSprite> _OverlaySprites;

        static MapTileUtilities() {
            loadOverlays();
        }

        //load the overlay textures
        private static void loadOverlays() {
            _OverlaySprites = new List<StaticSprite>(3);
            _OverlaySprites.Add(new StaticSprite("TileAppearance/TileOverlay_Triple", MapTile.TileSideLength));
            _OverlaySprites.Add(new StaticSprite("TileAppearance/TileOverlay_Straight", MapTile.TileSideLength));
            _OverlaySprites.Add(new StaticSprite("TileAppearance/TileOverlay_Bent", MapTile.TileSideLength));
        }

        //attaches the physics fixtures to the map tile
        public static void AttachFixtures (Body b, DoorDirections doors){

            const float wallWidth = MapTile.TileSideLength * MapTile.TileWallPercent;
            const float doorWidth = MapTile.TileSideLength * MapTile.TileDoorPercent;
            const float halfSide = MapTile.TileSideLength / 2.0f;
            const float sideSegWidth = (MapTile.TileSideLength - doorWidth) / 2.0f;

            if (doors.Type == ComboType.TRIPLE) {
                FixtureFactory.AttachRectangle(wallWidth, MapTile.TileSideLength, 1, new Vector2(-halfSide + wallWidth/2.0f, 0), b); //left all
                FixtureFactory.AttachRectangle(sideSegWidth, wallWidth, 1, new Vector2(-(sideSegWidth + doorWidth) / 2.0f, -halfSide + wallWidth / 2.0f), b); //top left
                FixtureFactory.AttachRectangle(sideSegWidth, wallWidth, 1, new Vector2((sideSegWidth + doorWidth) / 2.0f, -halfSide + wallWidth / 2.0f), b); //top right
                FixtureFactory.AttachRectangle(sideSegWidth, wallWidth, 1, new Vector2(-(sideSegWidth + doorWidth) / 2.0f, halfSide - wallWidth / 2.0f), b); //bottom left
                FixtureFactory.AttachRectangle(sideSegWidth, wallWidth, 1, new Vector2((sideSegWidth + doorWidth) / 2.0f, halfSide - wallWidth / 2.0f), b); //bottom right
                FixtureFactory.AttachRectangle(wallWidth, sideSegWidth, 1, new Vector2(halfSide - wallWidth / 2.0f, -(sideSegWidth + doorWidth) / 2.0f), b); //right top
                FixtureFactory.AttachRectangle(wallWidth, sideSegWidth, 1, new Vector2(halfSide - wallWidth / 2.0f, (sideSegWidth + doorWidth) / 2.0f), b); //right bottom
            }
            else if (doors.Type == ComboType.STRAIGHT) {
                FixtureFactory.AttachRectangle(wallWidth, MapTile.TileSideLength, 1, new Vector2(-halfSide + wallWidth / 2.0f, 0), b); //left all
                FixtureFactory.AttachRectangle(wallWidth, MapTile.TileSideLength, 1, new Vector2(halfSide - wallWidth / 2.0f, 0), b); //right all
                FixtureFactory.AttachRectangle(sideSegWidth, wallWidth, 1, new Vector2(-(sideSegWidth + doorWidth) / 2.0f, -halfSide + wallWidth / 2.0f), b); //top left
                FixtureFactory.AttachRectangle(sideSegWidth, wallWidth, 1, new Vector2((sideSegWidth + doorWidth) / 2.0f, -halfSide + wallWidth / 2.0f), b); //top right
                FixtureFactory.AttachRectangle(sideSegWidth, wallWidth, 1, new Vector2(-(sideSegWidth + doorWidth) / 2.0f, halfSide - wallWidth / 2.0f), b); //bottom left
                FixtureFactory.AttachRectangle(sideSegWidth, wallWidth, 1, new Vector2((sideSegWidth + doorWidth) / 2.0f, halfSide - wallWidth / 2.0f), b); //bottom right
            }
            else {
                //bent
                FixtureFactory.AttachRectangle(wallWidth, MapTile.TileSideLength, 1, new Vector2(-halfSide + wallWidth / 2.0f, 0), b); //left all
                FixtureFactory.AttachRectangle(MapTile.TileSideLength, wallWidth, 1, new Vector2(0, halfSide - wallWidth / 2.0f), b); //bottom all
                FixtureFactory.AttachRectangle(sideSegWidth, wallWidth, 1, new Vector2(-(sideSegWidth + doorWidth) / 2.0f, -halfSide + wallWidth / 2.0f), b); //top left
                FixtureFactory.AttachRectangle(sideSegWidth, wallWidth, 1, new Vector2((sideSegWidth + doorWidth) / 2.0f, -halfSide + wallWidth / 2.0f), b); //top right
                FixtureFactory.AttachRectangle(wallWidth, sideSegWidth, 1, new Vector2(halfSide - wallWidth / 2.0f, -(sideSegWidth + doorWidth) / 2.0f), b); //right top
                FixtureFactory.AttachRectangle(wallWidth, sideSegWidth, 1, new Vector2(halfSide - wallWidth / 2.0f, (sideSegWidth + doorWidth) / 2.0f), b); //right bottom
            }

            //rotate the tile
            b.Rotation = doors.tileRotation();
        }

        public static String appearanceStringFromDoorConfiguration(DoorDirections config) {
            String baseName = "TileAppearance/TileAppearance_";
            if (config.Type == ComboType.TRIPLE) {
                return baseName + "Triple";
            }
            else if (config.Type == ComboType.STRAIGHT) {
                return baseName + "Straight";
            }
            else {
                return baseName + "Bent";
            }
        }

        public static StaticSprite getOverlay(DoorDirections d) {
            if (d.Type == ComboType.TRIPLE) {
                return _OverlaySprites[0];
            }
            else if (d.Type == ComboType.STRAIGHT) {
                return _OverlaySprites[1];
            }
            else {
                //bent
                return _OverlaySprites[2];
            }
        }
    }
}
