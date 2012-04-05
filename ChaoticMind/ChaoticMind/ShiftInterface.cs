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

        const int addShiftButtonXDist = 50;

        MapManager _mapManager;
        MapTile[,] _tiles;
        SpriteBatch _spriteBatch;

        float _tileSideLength;
        float _scalingFactor;
        List<ShiftButton> _buttons = new List<ShiftButton>();
        private ShiftButton _pressedButton;
        private Vector2 _topLeftTilePosition;

        UIButton _addShiftToQueueButton;

        internal void Initialize(MapManager mapManager, SpriteBatch spriteBatch) {
            _mapManager = mapManager;
            _spriteBatch = spriteBatch;
        }

        internal void StartNewGame() {

            _tiles = _mapManager.Tiles;

            //Calculate the size of our interface
            int interfaceSideLength = Math.Min(Screen.Width, Screen.Height) - (2 * interfacePixelPadding);

            //Add 2 for the buttons on each end
            int gridSideIndexDimension = _tiles.GetLength(0) + 2;

            _tileSideLength = interfaceSideLength / (float)gridSideIndexDimension;

            StaticSprite tileSprite = _tiles[0, 0].ShiftTexture;
            _scalingFactor = _tileSideLength / tileSprite.CurrentTextureBounds.Width;

            //Create buttons

            //Length from centre to the very edge of the farthest button
            float longLength = interfaceSideLength / 2f;

            //Length to the edge of the map tiles
            float shortLength = longLength - _tileSideLength;

            float leftX = Screen.Center.X - longLength;
            float topY = Screen.Center.Y - longLength;
            float leftOfRightX = leftX + (gridSideIndexDimension - 1) * _tileSideLength;
            float topOfBottomY = topY + (gridSideIndexDimension - 1) * _tileSideLength;

            for (int i = 0; i < _tiles.GetLength(0); i++) {

                //Top buttons
                Rectangle buttonRect = new Rectangle((int)(leftX + _tileSideLength * (i + 1)), (int)topY, (int)_tileSideLength, (int)_tileSideLength);
                _buttons.Add(new ShiftButton(this, buttonRect, i, ShiftDirection.UP));

                //Bottom buttons
                buttonRect = new Rectangle((int)(leftX + _tileSideLength * (i + 1)), (int)topOfBottomY, (int)_tileSideLength, (int)_tileSideLength);
                _buttons.Add(new ShiftButton(this, buttonRect, i, ShiftDirection.DOWN));

                //Left buttons
                buttonRect = new Rectangle((int)leftX, (int)(topY + _tileSideLength + (_tileSideLength * i)), (int)_tileSideLength, (int)_tileSideLength);
                _buttons.Add(new ShiftButton(this, buttonRect, i, ShiftDirection.LEFT));

                //Right buttons
                buttonRect = new Rectangle((int)leftOfRightX, (int)(topY + _tileSideLength + (_tileSideLength * i)), (int)_tileSideLength, (int)_tileSideLength);
                _buttons.Add(new ShiftButton(this, buttonRect, i, ShiftDirection.RIGHT));
            }

            float halfLength = (_tiles.GetLength(0) / (float)2) * _tileSideLength;
            _topLeftTilePosition = new Vector2(Screen.Center.X - halfLength + 0.5f * _tileSideLength , Screen.Center.Y - halfLength + 0.5f*_tileSideLength);


            //Create the shift button
            Rectangle shiftButtonRect = new Rectangle((int)(Screen.ScreenRect.Right - 300), 200, 250, 125);
            StaticSprite normalSprite = new StaticSprite("Shifting/AddShiftButton", 1.0f, DrawLayers.MenuElements);
            StaticSprite pressedSprite = new StaticSprite("Shifting/AddShiftButtonPressed", 1.0f, DrawLayers.MenuElements);
            _addShiftToQueueButton = new UIButton(shiftButtonRect, 0.0f, normalSprite, pressedSprite);
            _addShiftToQueueButton.setTarget(enqueueShift);
        }

        public void enqueueShift(UIButton button) {
            if (_pressedButton != null) {
                _mapManager.queueShift(_pressedButton.Index, _pressedButton.Direction, DoorDirections.RandomDoors(), false);
                _pressedButton.reset();
                _pressedButton = null;
            }
        }

        internal void ClearGame() {
            _pressedButton = null;
            _buttons.Clear();
        }

        internal void Update(float deltaTime) {
            foreach (ShiftButton button in _buttons) {
                button.Update(deltaTime);
            }

            _addShiftToQueueButton.Update(deltaTime);

        }

        internal void Draw() {

            //draw tiles
            Vector2 drawingLocation = new Vector2(_topLeftTilePosition.X, _topLeftTilePosition.Y);
            for (int y = 0; y < _tiles.GetLength(0); y++) {
                for (int x = 0; x < _tiles.GetLength(1); x++) {
                    MapTile tile = _tiles[x, y];
                    _spriteBatch.Draw(tile.ShiftTexture.Texture, drawingLocation, tile.ShiftTexture.CurrentTextureBounds, Color.White, tile.Rotation, tile.ShiftTexture.CurrentTextureOrigin, _scalingFactor, SpriteEffects.None, DrawLayers.MenuElements);
                    drawingLocation.X += _tileSideLength;
                }
                drawingLocation.Y += _tileSideLength;
                drawingLocation.X = _topLeftTilePosition.X;
            }

            //draw shift buttons
            foreach (ShiftButton button in _buttons) {
                button.DrawSelf(_spriteBatch);
            }
            _addShiftToQueueButton.DrawSelf(_spriteBatch);
        }

        public void drawOnOverlay(IMiniMapable c) {
            _spriteBatch.Draw(c.MapSprite.Texture, c.MapPosition / (float)MapTile.TileSideLength * _tileSideLength + _topLeftTilePosition, c.MapSprite.CurrentTextureBounds, Color.White, c.MapRotation, c.MapSprite.CurrentTextureOrigin, 1 / c.MapSprite.PixelsPerMeter * 2, SpriteEffects.None, DrawLayers.MenuHighlightElements);
        }

        internal void ButtonWasPressed(ShiftButton pressedButton) {
            if (_pressedButton != null) {
                _pressedButton.reset();
            }
            _pressedButton = pressedButton;
        }

        internal void ButtonWasToggledOff() {
            _pressedButton = null;
        }
    }
}
