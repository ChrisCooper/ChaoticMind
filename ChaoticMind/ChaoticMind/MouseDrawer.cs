using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticMind {
    public enum MouseType {
        POINTER,
        REDICAL
    }

    class MouseDrawer {

        static StaticSprite _retical;
        static StaticSprite _mouse;
        static StaticSprite _mouseClicked;

        public static void Initialize() {
            _retical = new StaticSprite("UI/Retical", 1, DrawLayers.VeryTop.Mouse);
            _mouse = new StaticSprite("UI/Mouse", 1, DrawLayers.VeryTop.Mouse);
            _mouseClicked = new StaticSprite("UI/Mouse_Clicked", 1, DrawLayers.VeryTop.Mouse);
        }

        //internal

        public static void Draw(MouseType type) {
            Vector2 mouseLocation = InputManager.MouseScreenPosition;

            StaticSprite sprite;
            Vector2 origin;

            switch (type) {
                case MouseType.POINTER:
                    sprite = InputManager.IsMouseDown ? _mouseClicked : _mouse;
                    origin = Vector2.Zero;
                    break;
                case MouseType.REDICAL:
                    sprite = _retical;
                    origin = sprite.CurrentTextureOrigin;
                    break;
                default:
                    throw new Exception("Unhandled MouseType");
            }

            Program.SpriteBatch.Draw(sprite.Texture, mouseLocation, sprite.CurrentTextureBounds, Color.White, 0.0f, origin, 1, SpriteEffects.None, 0.0f);
        }

    }
}
