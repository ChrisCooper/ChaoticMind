using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace ChaoticMind {
    class Player : Character, IMiniMapable {

        AnimatedSprite _minimapSprite;

        public Player(CharacterType characterType, World world, Vector2 startingPosition)
            : base(characterType, world, startingPosition) {
            _body.LinearDamping = 30;
            _body.AngularDamping = 5;

            _minimapSprite = new StaticSprite("Minimap/PlayerMinimap", MapTile.TileSideLength / 2);

        }

        public Body Body {
            get { return _body; }
        }

        public AnimatedSprite MapSprite{
            get { return _minimapSprite; }
        }
        public Vector2 MapPosition {
            get { return Position; }
        }
        public float MapRotation {
            get { return 0; }
        }

        //Use input to decide what direction this character should try to face and move
        protected override void decideOnMovementTargets() {
            LocationToMoveToward = _body.Position;

            if (InputManager.IsKeyDown(Keys.A)) {
                LocationToMoveToward -= Vector2.UnitX;
            }
            if (InputManager.IsKeyDown(Keys.D)) {
                LocationToMoveToward += Vector2.UnitX;
            }
            if (InputManager.IsKeyDown(Keys.W)) {
                LocationToMoveToward -= Vector2.UnitY;
            }
            if (InputManager.IsKeyDown(Keys.S)) {
                LocationToMoveToward += Vector2.UnitY;
            }

            //Vector2 mouseLocation = InputManager.MouseWorldPosition;
            LocationToFace = InputManager.MouseWorldPosition;
        }
    }
}
