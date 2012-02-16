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
        protected Body _body;

        static Random garbageRandom = new Random();

        public GameObject(World world, Vector2 startingPosition)
        {
        }

        public virtual void Update(float deltaTime)
        {
        }

        public virtual void Initialize()
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
