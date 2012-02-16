using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;

namespace ChaoticMind
{
    class SillyBox : DrawableGameObject
    {

        public SillyBox(String imagePrefix, int numFrames, World world, Vector2 startingPosition)
            : base(imagePrefix, numFrames, ((float)Utilities.randomDouble()) + 0.1f, world, Utilities.randomVector() * 1000)
        {
            // This method creates a body (has mass, position, rotation),
            // as well as a rectangular fixture, which is just a shape stapled to the body.
            // The fixture is what collides with other objects and impacts how the body moves
            _body = BodyFactory.CreateRectangle(world, 64.0f, 64.0f, 1.0f);
            _body.BodyType = BodyType.Dynamic;
            _body.Position = startingPosition;
        }

        public override void Update(float deltaTime)
        {
            //This logic is purely for testing Farseer integration, and for fun!

            if (Utilities.randomInt() % 10 == 0)
            {
                _body.ApplyLinearImpulse(Utilities.randomVector() * deltaTime * 10000000);
            }
            if (Utilities.randomInt() % 1000 == 0)
            {
                _body.ApplyLinearImpulse(Utilities.randomVector() * deltaTime * 1000000000);
            }
            if (Utilities.randomInt() % 40 == 0)
            {
                _body.ApplyLinearImpulse((-1 * _body.Position) * deltaTime * 5000);
            }
            if (Utilities.randomInt() % 10000 == 0)
            {
                _body.ApplyLinearImpulse(Utilities.randomVector() * deltaTime * 10000000000);
            }

            base.Update(deltaTime);
        }
    }
}
