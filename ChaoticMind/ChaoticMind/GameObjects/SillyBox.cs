using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;

namespace ChaoticMind
{
    class SillyBox : Character
    {

        public SillyBox(CharacterType characterType, World world, Vector2 startingPosition)
            : base(characterType, world, Utilities.randomVector() * 1000)
        {
            // This method creates a body (has mass, position, rotation),
            // as well as a rectangular fixture, which is just a shape stapled to the body.
            // The fixture is what collides with other objects and impacts how the body moves
            _body = BodyFactory.CreateRectangle(world, 64.0f, 64.0f, 1.0f);
            _body.BodyType = BodyType.Dynamic;
            _body.Position = startingPosition;
        }

        //Use input (in the case of a controllable character)
        // or an AI routine to decide what direction this character should try to face, and move
        protected override void decideOnMovementTargets()
        {
            LocationToMoveToward = _body.Position + Utilities.randomNormalizedVector();
            LocationToMoveToward += -1 * _body.Position * 0.00001f;
        }
    }
}
