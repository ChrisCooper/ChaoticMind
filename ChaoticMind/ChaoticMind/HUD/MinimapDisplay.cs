using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ChaoticMind.HUD {
    class MinimapDisplay {

        float _mapScale;

        StaticSprite _minimapFrameSprite;
        float _mapFrameScale;
        Rectangle _mapFrameRect;
        //The percentage of the full frame image that each side of the frame takes up
        float _mapFrameWidthFraction = 0.1f;
        Rectangle _mapRect;

        GameObjects _objectsOwner;

        internal MinimapDisplay(GameObjects objectsOwner) {
            _objectsOwner = objectsOwner;

            _minimapFrameSprite = new StaticSprite("HUD/Minimap_Frame", 1.0f, DrawLayers.HUD.Backgrounds);

            float mapFrameSideLength = Math.Min(ScreenUtils.Width / 4.0f, ScreenUtils.Height / 3.0f);
            _mapFrameScale = mapFrameSideLength / _minimapFrameSprite.Texture.Bounds.Width;
            _mapFrameRect = new Rectangle(0, (int)(ScreenUtils.Height - mapFrameSideLength), (int)mapFrameSideLength, (int)mapFrameSideLength);

            float frameWidth = _mapFrameWidthFraction * mapFrameSideLength;
            _mapRect = new Rectangle((int)(_mapFrameRect.Left + frameWidth), (int)(_mapFrameRect.Top + frameWidth), (int)(_mapFrameRect.Width - 2 * frameWidth), (int)(_mapFrameRect.Height - 2 * frameWidth));

            _mapScale = _mapRect.Width / _objectsOwner.Map.EdgeOfMapdimesion;
        }

        internal void Draw() {
            Program.SpriteBatch.Draw(_minimapFrameSprite.Texture, _mapFrameRect, _minimapFrameSprite.CurrentTextureBounds, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, _minimapFrameSprite.DrawLayer);

            //Draw tiles
            _objectsOwner.Map.DrawMap(this);

            //Draw objects
            _objectsOwner.Particles.ForEach(p => DrawMinimap(p));
            _objectsOwner.Projectiles.ForEach(p => DrawMinimap(p));
            _objectsOwner.Collectables.ForEach(c => DrawMinimap(c));
            _objectsOwner.Enemies.ForEach(e => DrawMinimap(e));
            DrawMinimap(_objectsOwner.MainPlayer);
        }

        public void DrawMinimap(IMiniMapable obj, float alpha) {
            if (obj.MapSprite != null) {
                Program.SpriteBatch.Draw(obj.MapSprite.Texture, WorldToMapPos(obj.MapPosition), obj.MapSprite.CurrentTextureBounds, Color.White * alpha, obj.MapRotation, obj.MapSprite.CurrentTextureOrigin, WorldToMapScale(1 / obj.MapSprite.PixelsPerMeter), SpriteEffects.None, obj.MapDrawLayer);
            }
        }


        public void DrawMinimap(IMiniMapable obj) {
            DrawMinimap(obj, 1.0f);
        }

        private float WorldToMapScale(float worldScale) {
            return worldScale * _mapScale;
        }

        private Vector2 WorldToMapPos(Vector2 worldPoint) {
            float x = _mapRect.Left + (worldPoint.X * _mapScale) + MapTile.TileSideLength / 2 * _mapScale;
            float y = _mapRect.Top + (worldPoint.Y * _mapScale) + MapTile.TileSideLength / 2 * _mapScale;
            return new Vector2(x, y);
        }
    }
}
