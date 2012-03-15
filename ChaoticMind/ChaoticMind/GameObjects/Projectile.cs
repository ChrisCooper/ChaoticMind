using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace ChaoticMind {
    class Projectile : DrawableGameObject{

        float _maxTime;
        Vector2 _direction;
        Vector2 _startingPosition;

        private DateTimeOffset _createTime;

        public Projectile(Vector2 startingPosition, Vector2 direction, float maxTime, float diameter, float speed, World world)
            : this(startingPosition, direction, maxTime, diameter, speed, "TestImages/Projectile", 1, 1, 1.0f, world) {
        }

        public Projectile(Vector2 startingPosition, Vector2 direction, float maxTime, float diameter, float speed, String spriteResource, int xFrames, int yFrames, float animationDuration, World world)
            : base(spriteResource, xFrames, yFrames, diameter, animationDuration, world, startingPosition) {
            //set 
            _maxTime = maxTime;
            _startingPosition = startingPosition;
            _direction = Vector2.Normalize(direction);

            //set up the body
            _body = BodyFactory.CreateCircle(world, diameter, 1);
            _body.BodyType = BodyType.Kinematic;
            _body.LinearDamping = 0;
            _body.LinearVelocity = _direction * speed;
            _body.Position = _startingPosition;

            _createTime = DateTimeOffset.Now;
        }

        public bool Timeout (){
            return _createTime.AddMilliseconds(_maxTime) < DateTimeOffset.Now;
        }

        public void DestroyBody() {
            _body.Dispose();
        }

        public override void Update(float deltaTime) {
            base.Update(deltaTime);
        }
    }
}
