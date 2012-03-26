using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace ChaoticMind {
    class Projectile : DrawableGameObject{

        ProjectileType _projectileType;
        Vector2 _direction;
        Vector2 _startingPosition;

        Timer _lifetimeTimer;

        public Projectile(Vector2 startingPosition, Vector2 direction, ProjectileType projectileType)
            : base(startingPosition, projectileType.Animation) {

            _projectileType = projectileType;

            _startingPosition = startingPosition;

            //normalizing a Vector2.Zero gives a (NaN, NaN) vector
            _direction = (direction == Vector2.Zero) ?  direction : Vector2.Normalize(direction);

            //set up the body
            _body = BodyFactory.CreateCircle(Program.SharedGame.MainWorld, _projectileType.Radius, _projectileType.Density);
            _body.BodyType = BodyType.Dynamic;
            _body.LinearDamping = 0;
            _body.LinearVelocity = _direction * _projectileType.Speed;
            _body.Position = _startingPosition;
            _body.UserData = this;
            
            //collide with everything but self
            _body.CollisionCategories = Category.Cat2;
            _body.CollidesWith = Category.All & ~Category.Cat2;

            //init the timer
            _lifetimeTimer = new Timer(_projectileType.Lifetime);
        }

        public override bool ShouldDieNow(){
            return _lifetimeTimer.isFinished;
        }

        public override void Destroy() {
            _body.Dispose();
        }

        public override void Update(float deltaTime) {
            _lifetimeTimer.Update(deltaTime);
            base.Update(deltaTime);
        }
    }
}
