﻿using System;
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
