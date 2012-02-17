
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ChaoticMind
{
    class Camera
    {
        //Position of the camera in game coordinates
        Vector2 _position;

        float _zoom;
        SpriteBatch _spriteBatch;
        GraphicsDevice _graphicsDevice;
        
        //A vector to the centre of the screen. Used to position drawing
        Vector2 _toCentre = Vector2.Zero;

        public Camera(Vector2 startingPosition, float startingZoom, GraphicsDevice graphics, SpriteBatch spriteBatch)
        {
            _position = startingPosition;
            _zoom = startingZoom;
            _graphicsDevice = graphics;
            _spriteBatch = spriteBatch;
        }

        public void Draw(DrawableGameObject o)
        {
            _spriteBatch.Draw(o.CurrentTexture, (o.Position - _position) * _zoom + _toCentre, null, Color.White, o.Rotation, o.CurrentTextureOrigin, _zoom, SpriteEffects.None, 1.0f);
        }


        public void Update(float deltaTime)
        {
            updateFromInput(deltaTime);

            updateFrame();
        }

        private void updateFrame()
        {
            _toCentre.X = ((float)_graphicsDevice.Viewport.Width) / 2.0f;
            _toCentre.Y = ((float)_graphicsDevice.Viewport.Height) / 2.0f;
        }

        //Move the camera according to input. Should probably use
        //some sort of input manger in the future, but we need to make that first.
        private void updateFromInput(float deltaTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            float forcePower = 10.0f;

            //Apply force in the arrow key direction
            Vector2 force = Vector2.Zero;
            if (keyState.IsKeyDown(Keys.Left))
            {
                force.X -= forcePower;
            }
            if (keyState.IsKeyDown(Keys.Right))
            {
                force.X += forcePower;
            }
            if (keyState.IsKeyDown(Keys.Up))
            {
                force.Y -= forcePower;
            }
            if (keyState.IsKeyDown(Keys.Down))
            {
                force.Y += forcePower;
            }
            if (keyState.IsKeyDown(Keys.OemPlus))
            {
                _zoom *= 1 + (1f * deltaTime);
            }
            if (keyState.IsKeyDown(Keys.OemMinus))
            {
                _zoom *= 1 - (1f * deltaTime);
            }

            _position += force * (1/_zoom);

        }
    }
}
