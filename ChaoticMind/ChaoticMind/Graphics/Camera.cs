
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;

namespace ChaoticMind
{
    class Camera
    {
        const float FollowFaithfulness = 0.1f;

        //Position of the camera in game coordinates
        Vector2 _position;

        float _zoom;
        SpriteBatch _spriteBatch;
        GraphicsDevice _graphicsDevice;
        
        //A vector to the centre of the screen. Used to position drawing
        Vector2 _toCentre = Vector2.Zero;

        Body _target;

        public Camera(Vector2 startingPosition, float startingZoom, GraphicsDevice graphics, SpriteBatch spriteBatch)
        {
            _position = startingPosition;
            _zoom = startingZoom;
            _graphicsDevice = graphics;
            _spriteBatch = spriteBatch;
        }

        public void setTarget(Body target) {
            _target = target;
        }

        public Vector2 screenPointToWorld(Vector2 screenPoint)
        {
            return ((screenPoint - _toCentre) / _zoom) + _position;

            //(o.Position - _position) * _zoom + _toCentre;
        }

        public void Draw(DrawableGameObject o)
        {
            _spriteBatch.Draw(o.Texture, (o.Position - _position) * _zoom + _toCentre, o.CurrentTextureBounds, Color.White, o.Rotation, o.CurrentTextureOrigin, _zoom / o.PixelsPerMeter, SpriteEffects.None, 1.0f);
        }


        public void Update(float deltaTime)
        {
            
            updateFromInput(deltaTime);

            if (_target != null)
            {
                _position += FollowFaithfulness * (_target.Position - _position);
            }

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
