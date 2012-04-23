using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace ChaoticMind {
    class Projectile : DrawableGameObject, IGlowDrawable {

        ProjectileType _projectileType;
        Vector2 _direction;
        Vector2 _startingPosition;

        Timer _lifetimeTimer;

        public Projectile(Vector2 startingPosition, Vector2 direction, ProjectileType projectileType)
            : base(startingPosition, projectileType.AnimationSequence, projectileType.VisibleEntitySize, projectileType.AnimationDuration, projectileType.DrawLayer) {

            _projectileType = projectileType;

            _startingPosition = startingPosition;

            //normalizing a Vector2.Zero gives a (NaN, NaN) vector
            _direction = (direction == Vector2.Zero) ?  direction : Vector2.Normalize(direction);

            //set up the body
            _body = BodyFactory.CreateCircle(Program.DeprecatedObjects.PhysicsWorld, _projectileType.Radius, _projectileType.Density);
            _body.BodyType = BodyType.Dynamic;
            _body.LinearDamping = 0;
            _body.LinearVelocity = _direction * _projectileType.Speed;
            _body.Position = _startingPosition;
            _body.UserData = this;
            
            //collide with everything but self
            _body.CollisionCategories = Category.Cat2;
            _body.CollidesWith = Category.All & ~Category.Cat2;

            _body.OnCollision += new OnCollisionEventHandler(_body_OnCollision);

            //init the timer
            _lifetimeTimer = new Timer(_projectileType.Lifetime);

            if (projectileType.GlowSpriteSequence != null) {
                GlowSprite = new AnimatedSprite(projectileType.GlowSpriteSequence, projectileType.GlowEntitySize, projectileType.AnimationDuration, projectileType.DrawLayer);
            }
        }

        bool _body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact) {
            IDamageable hitObject = fixtureB.Body.UserData as IDamageable;

            _lifetimeTimer.Finish();

            //Check if the object is even an IDamageable, and not the player
            if (hitObject == null || hitObject == Program.DeprecatedObjects.MainPlayer) {
                return true;
            }

            hitObject.ApplyDamage(_projectileType.Damage);
            return true;
        }

        public override bool ShouldBeKilled {
            get { return _lifetimeTimer.isFinished; }
        }

        public override AnimatedSprite GlowSprite { get; set; }

        public override void WasKilled() {
            WasCleared();
            if (_projectileType.DeathParticle != null) {
                Program.DeprecatedObjects.Particles.Add(new Particle(Position, (float)Utilities.randomDouble(), _projectileType.DeathParticle));
            }
        }

        public override void Update(float deltaTime) {
            _lifetimeTimer.Update(deltaTime);
            if (GlowSprite != null) {
                GlowSprite.Update(deltaTime);
            }
            base.Update(deltaTime);
        }
    }
}
