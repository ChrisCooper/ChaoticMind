using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace ChaoticMind
{
    class GameObject
    {
        Body _body;

        public GameObject(World world, Vector2 startingPosition)
        {
            // This method creates a body (has mass, position, rotation),
            // as well as a rectangular fixture, which is just a shape stapled to the body.
            // The fixture is what collides with other objects and impacts how the body moves
            _body = BodyFactory.CreateRectangle(world,64.0f, 64.0f, 1.0f);
            _body.BodyType = BodyType.Dynamic;
            _body.Position = startingPosition;
        }

        public void Update(float deltaTime)
        {
        }

        public Vector2 Position
        {
            get
            {
                return _body.Position;
            }
        }

        public float Rotation
        {
            get
            {
                return _body.Rotation;
            }
        }
    }
}
