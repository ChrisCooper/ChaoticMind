using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace ChaoticMind
{
    class ControllableSillyBox : SillyBox
    {

        public ControllableSillyBox(CharacterType characterType, World world, Vector2 startingPosition)
            : base(characterType, world, startingPosition)
        {
        }

        //Use input to decide what direction this character should try to face and move
        protected override void decideOnMovementTargets()
        {
            KeyboardState keyState = Keyboard.GetState();

            LocationToMoveToward = _body.Position;

            if (keyState.IsKeyDown(Keys.A))
            {
                LocationToMoveToward -= Vector2.UnitX;
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                LocationToMoveToward += Vector2.UnitX;
            }
            if (keyState.IsKeyDown(Keys.W))
            {
                LocationToMoveToward -= Vector2.UnitY;
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                LocationToMoveToward += Vector2.UnitY;
            }

            //Vector2 mouseLocation = _mainCamera.screenPointToWorld(new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
        }
    }
}
