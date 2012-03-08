using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace ChaoticMind
{
    class MapManager
    {
        List<MapTile> _tiles = new List<MapTile>();

        int _gridWidth = 3;
        int _gridHeight = 3;

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
