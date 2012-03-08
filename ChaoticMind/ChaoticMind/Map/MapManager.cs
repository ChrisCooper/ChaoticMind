using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ChaoticMind
{
    class MapManager
    {
        List<MapTile> _tiles = new List<MapTile>();

        int _gridWidth = 3;
        int _gridHeight = 3;
        private bool hasntMovedTemp = true;

        public MapManager(int mapWidth, int mapHeight)
        {
            _gridWidth = mapWidth;
            _gridHeight = mapHeight;
        }

        //Make the tiles
        public void Initialize(World world)
        {
            for (int row = 0; row < _gridHeight; row++)
            {
                for (int col = 0; col < _gridWidth; col++)
                {
                    _tiles.Add(new MapTile(world, MapTile.WorldPositionForGridCoordinates(row, col), DoorDirections.RandomDoors()));
                }
            }
        }


        public void Update(float deltaTime)
        {
            foreach (MapTile tile in _tiles)
            {
                tile.Update(deltaTime);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.P) && hasntMovedTemp) {
                for (int i = 0; i < _gridWidth; i++) {

                    //_tiles[i].setTarget(MapTile.WorldPositionForGridCoordinates(0, i + 1));
                    _tiles[i].go();
                }
                hasntMovedTemp = false;
            }
        }

        public void DrawMap(Camera camera)
        {
            foreach (MapTile tile in _tiles)
            {
                camera.Draw(tile);
                camera.DrawOverlay(tile, Color.White * 0.3f);
            }
        }
    }
}
