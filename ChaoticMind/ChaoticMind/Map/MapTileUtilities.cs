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
        static List<StaticSprite> _MapSprites;
        static List<StaticSprite> _ShiftSprites;

        static MapTileUtilities() {
            loadOverlays();
            loadMapSprites();
            loadShiftSprites();
        }

        //load the overlay textures
        private static void loadOverlays() {
            _OverlaySprites = new List<StaticSprite>(4);
            _OverlaySprites.Add(new StaticSprite("TileAppearance/TileOverlay_Triple", MapTile.TileSideLength, DrawLayers.MapOverlay));
            _OverlaySprites.Add(new StaticSprite("TileAppearance/TileOverlay_Straight", MapTile.TileSideLength, DrawLayers.MapOverlay));
            _OverlaySprites.Add(new StaticSprite("TileAppearance/TileOverlay_Bent", MapTile.TileSideLength, DrawLayers.MapOverlay));
            _OverlaySprites.Add(new StaticSprite("TileAppearance/TileOverlay_Single", MapTile.TileSideLength, DrawLayers.MapOverlay));
        }

        //load the overlay textures
        private static void loadMapSprites() {
            _MapSprites = new List<StaticSprite>(4);
            _MapSprites.Add(new StaticSprite("Minimap/TileMap_Triple", MapTile.TileSideLength, DrawLayers.HUD_Minimap_Maze));
            _MapSprites.Add(new StaticSprite("Minimap/TileMap_Straight", MapTile.TileSideLength, DrawLayers.HUD_Minimap_Maze));
            _MapSprites.Add(new StaticSprite("Minimap/TileMap_Bent", MapTile.TileSideLength, DrawLayers.HUD_Minimap_Maze));
            _MapSprites.Add(new StaticSprite("Minimap/TileMap_Hidden", MapTile.TileSideLength, DrawLayers.HUD_Minimap_Maze));
        }

        //load the shift interface textures
        private static void loadShiftSprites() {
            _ShiftSprites = new List<StaticSprite>(4);
            _ShiftSprites.Add(new StaticSprite("Shifting/TileShift_Triple", MapTile.TileSideLength, DrawLayers.MenuElements));
            _ShiftSprites.Add(new StaticSprite("Shifting/TileShift_Straight", MapTile.TileSideLength, DrawLayers.MenuElements));
            _ShiftSprites.Add(new StaticSprite("Shifting/TileShift_Bent", MapTile.TileSideLength, DrawLayers.MenuElements));
            _ShiftSprites.Add(new StaticSprite("Shifting/TileShift_Hidden", MapTile.TileSideLength, DrawLayers.MenuElements));
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
            else if (config.Type == ComboType.BENT) {
                return baseName + "Bent";
            }
            else{
                return null;
            }
        }
        public static StaticSprite getOverlay(DoorDirections d) {
            if (d.Type == ComboType.TRIPLE) {
                return _OverlaySprites[0];
            }
            else if (d.Type == ComboType.STRAIGHT) {
                return _OverlaySprites[1];
            }
            else if (d.Type == ComboType.BENT){
                return _OverlaySprites[2];
            }
            else if (d.Type == ComboType.SINGLE) {
                return _OverlaySprites[3];
            }
            else if (d.Type == ComboType.NONE) {
                return null;
            }
            return null;
        }
        public static StaticSprite getMapSprite(DoorDirections d, bool visible) {
            if (!visible) {
                return _MapSprites[3];
            }
            if (d.Type == ComboType.TRIPLE) {
                return _MapSprites[0];
            }
            else if (d.Type == ComboType.STRAIGHT) {
                return _MapSprites[1];
            }
            else if (d.Type == ComboType.BENT) {
                return _MapSprites[2];
            }
            return null;
        }
        public static StaticSprite getShiftSprite(DoorDirections d, bool visible) {
            if (!visible) {
                return _ShiftSprites[3];
            }
            if (d.Type == ComboType.TRIPLE) {
                return _ShiftSprites[0];
            }
            else if (d.Type == ComboType.STRAIGHT) {
                return _ShiftSprites[1];
            }
            else if (d.Type == ComboType.BENT) {
                return _ShiftSprites[2];
            }
            return null;
        }
    }
}
