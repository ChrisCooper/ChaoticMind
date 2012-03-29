
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;

namespace ChaoticMind {
    class Camera {
        const float FollowFaithfulness = 0.4f;

        //Position of the camera in game coordinates
        Vector2 _position;

        float _zoom;
        SpriteBatch _spriteBatch;
        GraphicsDevice _graphicsDevice;

        //A vector to the centre of the screen. Used to position drawing
        Vector2 _toCentre = Vector2.Zero;

        Body _target;

        float _shakeMagnitude = 0.0f;
        float _shakeDecay = 0.95f;
        float _shakeIncreaseAmount = 1.0f;

        Rectangle _minimapRect;
        private float _mapWidthScale;
        private float _mapHeightScale;

        public Camera(Vector2 startingPosition, float startingZoom, GraphicsDevice graphics, SpriteBatch spriteBatch) {
            _position = startingPosition;
            _zoom = startingZoom;
            _graphicsDevice = graphics;
            _spriteBatch = spriteBatch;
            _minimapRect = HUD.HUDManager.MinimapRect;
            _mapWidthScale = _minimapRect.Width / MapManager.MainInstance.EdgeOfMapdimesion;
            _mapHeightScale = _minimapRect.Height / MapManager.MainInstance.EdgeOfMapdimesion;
        }

        public void setTarget(Body target) {
            _target = target;
        }

        public Vector2 screenPointToWorld(Vector2 screenPoint) {
            return ((screenPoint - _toCentre) / _zoom) + _position;
        }

        private Vector2 WorldToScreenPos(Vector2 worldPoint) {
            return (worldPoint - _position) * _zoom + _toCentre;
        }

        public void Draw(IDrawable o) {
            Draw(o, o.Alpha);
        }

        public void Draw(IDrawable o, float alpha) {
            if (o.Texture != null) {
                _spriteBatch.Draw(o.Texture, WorldToScreenPos(o.Position), o.CurrentTextureBounds, Color.White * alpha, o.Rotation, o.CurrentTextureOrigin, _zoom / o.PixelsPerMeter, SpriteEffects.None, o.DrawLayer);
            }
        }

        public void DrawOverlay(MapTile tile, Color clr) {
            if (tile.Overlay != null) {
                _spriteBatch.Draw(tile.Overlay.Texture, WorldToScreenPos(tile.Position), tile.Overlay.CurrentTextureBounds, clr, tile.OverlayRotation, tile.Overlay.CurrentTextureOrigin, _zoom / tile.PixelsPerMeter, SpriteEffects.None, tile.OverlayDrawLayer);
            }
        }

        public void DrawMinimap(IMiniMapable obj) {
            DrawMinimap(obj, 1.0f);
        }

        public void DrawMinimap(IMiniMapable obj, float alpha) {
            if (obj.MapSprite != null) {
                _spriteBatch.Draw(obj.MapSprite.Texture, WorldToMapPos(obj.MapPosition), obj.MapSprite.CurrentTextureBounds, Color.White * alpha, obj.MapRotation, obj.MapSprite.CurrentTextureOrigin, WorldToMapScale(1 / obj.MapSprite.PixelsPerMeter), SpriteEffects.None, obj.MapDrawLayer);
            }
        }

        private float WorldToMapScale(float worldScale) {
            return worldScale * _mapWidthScale;
        }

        private Vector2 WorldToMapPos(Vector2 worldPoint) {

            float x = _minimapRect.Left + (worldPoint.X * _mapWidthScale) + MapTile.TileSideLength / 2 * _mapWidthScale;
            float y = _minimapRect.Top + (worldPoint.Y * _mapHeightScale) + MapTile.TileSideLength / 2 * _mapHeightScale;
            return new Vector2(x, y);
        }

        public void Update(float deltaTime) {
            updateFromInput(deltaTime);

            if (_target != null) {
                //to make shooting work properly
                //_position = _target.Position;
                _position += FollowFaithfulness * (_target.Position - _position);
            }

            _position += _shakeMagnitude * Utilities.randomVector();

            _shakeMagnitude *= _shakeDecay;

            updateFrame();
        }

        private void updateFrame() {
            _toCentre.X = ((float)_graphicsDevice.Viewport.Width) / 2.0f;
            _toCentre.Y = ((float)_graphicsDevice.Viewport.Height) / 2.0f;
        }

        //Move the camera according to input. Should probably use
        //some sort of input manger in the future, but we need to make that first.
        private void updateFromInput(float deltaTime) {
            if (InputManager.IsKeyDown(Keys.OemPlus)) {
                _zoom *= 1 + (1f * deltaTime);
            }
            if (InputManager.IsKeyDown(Keys.OemMinus)) {
                _zoom *= 1 - (1f * deltaTime);
            }
        }

        internal void shake() {
            _shakeMagnitude += _shakeIncreaseAmount;
        }
    }

    internal interface IDrawable {

        Texture2D Texture { get; }

        Rectangle CurrentTextureBounds { get; }

        Vector2 CurrentTextureOrigin { get; }

        float PixelsPerMeter { get; }

        Vector2 Position { get; }

        float Rotation { get; }

        float Alpha { get; }

        float DrawLayer { get; }
    }

    internal interface IMiniMapable {
        AnimatedSprite MapSprite { get; }

        Vector2 MapPosition { get; }

        float MapRotation { get; }

        float MapDrawLayer { get; }
    }
}
