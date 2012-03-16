using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class ShiftInterface {

        //The space around the shift interface on each of the tightest sides
        const int interfacePixelPadding = 50;

        MapManager _mapManager;
        MapTile[,] _tiles;
        SpriteBatch _spriteBatch;
        
        int _gridSideDimension;
        float _tileDimension;
        float _scalingFactor;

        internal void Initialize(MapManager mapManager, SpriteBatch spriteBatch) {
            _mapManager = mapManager;
            _spriteBatch = spriteBatch;

            _tiles = _mapManager.Tiles;

            //Calculate the size of our interface
            int interfaceSideLength = Math.Min(Screen.Width, Screen.Height) - 2 * interfacePixelPadding;

            //Add 2 for the buttons on each end
            _gridSideDimension = _tiles.GetLength(0) + 2;

            _tileDimension = interfaceSideLength / (float)_gridSideDimension;

            StaticSprite tileSprite = _tiles[0, 0].ShiftTexture;

            _scalingFactor = _tileDimension / (float)tileSprite.CurrentTextureBounds.Width;
        }

        internal void Update() {
            
        }

        internal void DrawInterface() {
            drawTiles();
            drawShiftButtons();
        }

        private void drawTiles() {
            float halfLength = (_tiles.GetLength(0) / (float)2) * _tileDimension;
            float startingX = Screen.Center.X - halfLength;
            Vector2 drawingLocation = new Vector2(startingX, Screen.Center.Y - halfLength);

            for (int y = 0; y < _tiles.GetLength(0); y++) {
                for (int x = 0; x < _tiles.GetLength(1); x++) {
                    MapTile tile = _tiles[x, y];
                    _spriteBatch.Draw(tile.ShiftTexture.Texture, drawingLocation, tile.ShiftTexture.CurrentTextureBounds, Color.White, tile.Rotation, tile.ShiftTexture.CurrentTextureOrigin, _scalingFactor, SpriteEffects.None, 1.0f);
                    drawingLocation += new Vector2(_tileDimension, 0);
                }
                drawingLocation += new Vector2(0, _tileDimension);
                drawingLocation.X = startingX;
            }
        }

        private void drawShiftButtons() {
            
        } 
    }
}
