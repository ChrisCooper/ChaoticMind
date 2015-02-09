﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;
using ChaoticMind.HUD;

namespace ChaoticMind {
    class Camera {
        const float FollowFaithfulness = 0.4f;

        //worldPosition of the camera in game coordinates
        Vector2 _position;

        float _zoom;
        float _startingZoom;
        SpriteBatch _spriteBatch;
        GraphicsDevice _graphicsDevice;

        //A vector to the centre of the screen. Used to position drawing
        Vector2 _toCentre = Vector2.Zero;

        Body _target;
        public Body Target {
            set {
                _target = value;
                _position = _target.Position;
            }
        }

        float _shakeMagnitude = 0.0f;
        float _shakeDecay = 0.97f;
        float _shakeIncreaseAmount = 1.0f;

        public Camera(Vector2 startingPosition, float startingZoom, GraphicsDevice graphics ) {
            _position = startingPosition;
            _zoom = startingZoom;
            _startingZoom = startingZoom;
            _graphicsDevice = graphics;
            _spriteBatch = Program.SpriteBatch;
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

        internal void DrawGlow(IGlowDrawable o) {
            if (o.GlowSprite != null) {
                _spriteBatch.Draw(o.GlowSprite.Texture, WorldToScreenPos(o.Position), o.GlowSprite.CurrentTextureBounds, Color.White * o.Alpha, o.Rotation, o.GlowSprite.CurrentTextureOrigin, _zoom / o.GlowSprite.PixelsPerMeter, SpriteEffects.None, 1.0f);
            }
        }

        public void Draw(IDrawable o, float alpha) {
            if (o.Sprite != null) {
                _spriteBatch.Draw(o.Sprite.Texture, WorldToScreenPos(o.Position), o.Sprite.CurrentTextureBounds, Color.White * alpha, o.Rotation, o.Sprite.CurrentTextureOrigin, _zoom / o.Sprite.PixelsPerMeter, SpriteEffects.None, o.DrawLayer);
            }
        }

        public void DrawOverlay(MapTile tile, Color clr) {
            if (tile.Overlay != null) {
                _spriteBatch.Draw(tile.Overlay.Texture, WorldToScreenPos(tile.Position), tile.Overlay.CurrentTextureBounds, clr, tile.OverlayRotation, tile.Overlay.CurrentTextureOrigin, _zoom / tile.Overlay.PixelsPerMeter, SpriteEffects.None, tile.OverlayDrawLayer);
            }
        }

        public void Update(float deltaTime) {

            if (_target != null) {
                //to make shooting work properly
                //_position = _target.worldPosition;
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


        internal void shake() {
            _shakeMagnitude += _shakeIncreaseAmount;
        }
    }

    internal interface IDrawable {

        AnimatedSprite Sprite { get; }

        Vector2 Position { get; }

        float Rotation { get; }

        float Alpha { get; }

        float DrawLayer { get; }
    }

    internal interface IGlowDrawable {
        AnimatedSprite GlowSprite { get; }

        Vector2 Position { get; }

        float Rotation { get; }

        float Alpha { get; }
    }

    internal interface IMiniMapable {
        AnimatedSprite MapSprite { get; }

        Vector2 MapPosition { get; }

        float MapRotation { get; }

        float MapDrawLayer { get; }
    }
}
