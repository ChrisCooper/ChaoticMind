using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace ChaoticMind {
    class Player : Character {

        AnimatedSprite _minimapSprite;

        public Player(CharacterType characterType, Vector2 startingPosition)
            : base(characterType, startingPosition) {
            _body.LinearDamping = 30;
            _body.AngularDamping = 5;
            _body.UserData = Utilities.BodyTypes.PLAYER;

            _minimapSprite = new StaticSprite("Minimap/PlayerMinimap", MapTile.TileSideLength / 2);

            _curWeapon = new Weapon(new StaticSprite("Weapons/AssaultRife", 1), 2, 0.150f, 10, 80, 3, 10, new StaticSprite("TestImages/Projectile", 0.1f), 20, 1, 30);
        }

        public Body Body {
            get { return _body; }
        }

        //minimap stuff
        public override AnimatedSprite MapSprite{
            get { return _minimapSprite; }
        }

        //Use input to decide what direction this character should try to face and move
        protected override void decideOnMovementTargets() {

            //movement
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

            //shooting
            LocationToFace = InputManager.MouseWorldPosition;
            if (InputManager.IsMouseDown()) {
                Shoot();
            }

            //reload
            if (InputManager.IsKeyClicked(Keys.E)){
                Reload();
            }
        }
    }
}
