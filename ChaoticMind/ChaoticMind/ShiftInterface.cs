using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ChaoticMind {
    class ShiftInterface {

        //The space around the shift interface on each of the tightest sides
        const int interfacePixelPadding = 50;

        MapManager _mapManager;
        MapTile[,] _tiles;
        SpriteBatch _spriteBatch;

        float _tileDimension;
        float _scalingFactor;
        List<ShiftButton> _buttons = new List<ShiftButton>();
        private ShiftButton _pressedButton;

        internal void Initialize(MapManager mapManager, SpriteBatch spriteBatch) {
            _mapManager = mapManager;
            _spriteBatch = spriteBatch;

            _tiles = _mapManager.Tiles;

            //Calculate the size of our interface
            int interfaceSideLength = Math.Min(Screen.Width, Screen.Height) - 2 * interfacePixelPadding;

            //Add 2 for the buttons on each end
            int gridSideDimension = _tiles.GetLength(0) + 2;

            _tileDimension = interfaceSideLength / (float)gridSideDimension;

            StaticSprite tileSprite = _tiles[0, 0].ShiftTexture;

            _scalingFactor = _tileDimension / (float)tileSprite.CurrentTextureBounds.Width;



            //Create buttons
            float longLength = (gridSideDimension / (float)2) * _tileDimension;
            float shortLength = (_tiles.GetLength(0) / (float)2) * _tileDimension;
            float startX = Screen.Center.X - shortLength;
            float startY = Screen.Center.Y - shortLength;
            for (int i = 0; i < _tiles.GetLength(0); i++) {
                //Top buttons
                _buttons.Add(new ShiftButton(this, new Vector2(startX + _tileDimension * i, Screen.Center.Y - longLength), _tileDimension, i, ShiftDirection.UP));
                //Bottom buttons
                _buttons.Add(new ShiftButton(this, new Vector2(startX + _tileDimension * i, Screen.Center.Y + longLength - _tileDimension), _tileDimension, i, ShiftDirection.DOWN));
                //Left buttons
                _buttons.Add(new ShiftButton(this, new Vector2(Screen.Center.X - longLength, startY + _tileDimension * i), _tileDimension, i, ShiftDirection.LEFT));
                //Right buttons
                _buttons.Add(new ShiftButton(this, new Vector2(Screen.Center.X + longLength - _tileDimension, startY + _tileDimension * i), _tileDimension, i, ShiftDirection.RIGHT));
            }
        }

        internal void Update() {
            foreach (ShiftButton button in _buttons) {
                button.Update();
            }

            if (InputManager.IsKeyClicked(Keys.Enter)) {
                if (_pressedButton != null) {
                    _mapManager.queueShift(_pressedButton.Index, _pressedButton.Direction, DoorDirections.RandomDoors(), false);
                    _pressedButton.reset();
                    _pressedButton = null;
                    //Program.SharedGame.closeShiftInterface();
                }
            }
        }

        internal void DrawInterface(List<DrawableGameObject> minimapObjects) {
            //setup constants
            float halfLength = (_tiles.GetLength(0) / (float)2) * _tileDimension;
            Vector2 startCoord = new Vector2(Screen.Center.X - halfLength, Screen.Center.Y - halfLength);

            //draw tiles
            Vector2 drawingLocation = new Vector2(startCoord.X, startCoord.Y);
            for (int y = 0; y < _tiles.GetLength(0); y++) {
                for (int x = 0; x < _tiles.GetLength(1); x++) {
                    MapTile tile = _tiles[x, y];
                    _spriteBatch.Draw(tile.ShiftTexture.Texture, drawingLocation, tile.ShiftTexture.CurrentTextureBounds, Color.White, tile.Rotation, tile.ShiftTexture.CurrentTextureOrigin, _scalingFactor, SpriteEffects.None, DrawLayers.MenuElements);
                    drawingLocation.X += _tileDimension;
                }
                drawingLocation.Y += _tileDimension;
                drawingLocation.X = startCoord.X;
            }

            //draw minimap objects
            foreach (DrawableGameObject mm in minimapObjects) {
                drawOnOverlay(startCoord, mm);
            }

            foreach(Collectable c in CollectibleManager.Collectables) {
                startCoord = drawOnOverlay(startCoord, c);
            }   


            //draw shift buttons
            foreach (ShiftButton button in _buttons) {
                _spriteBatch.Draw(button.Sprite.Texture, button.Center, button.Sprite.CurrentTextureBounds, Color.White, button.Rotation, button.Sprite.CurrentTextureOrigin, button.ScalingFactor, SpriteEffects.None, DrawLayers.MenuElements);
            }
        }

        private Vector2 drawOnOverlay(Vector2 startCoord, IMiniMapable c) {
            _spriteBatch.Draw(c.MapSprite.Texture, c.MapPosition / (float)MapTile.TileSideLength * _tileDimension + startCoord, c.MapSprite.CurrentTextureBounds, Color.White, c.MapRotation, c.MapSprite.CurrentTextureOrigin, 1 / c.MapSprite.PixelsPerMeter * 2, SpriteEffects.None, DrawLayers.MenuHighlightElements);
            return startCoord;
        }

        internal void ButtonWasPressed(ShiftButton pressedButton) {
            if (_pressedButton != null){
                _pressedButton.reset();
            }
            _pressedButton = pressedButton;
        }

        internal void ButtonWasToggledOff() {
            _pressedButton = null;
        }
    }
}
