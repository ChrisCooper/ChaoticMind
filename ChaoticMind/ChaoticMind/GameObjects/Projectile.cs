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
        int _damage;
        Vector2 _direction;
        Vector2 _startingPosition;

        private DateTimeOffset _createTime;

        //default projectile
        public Projectile(Vector2 startingPosition, Vector2 direction, float maxTime, int damage, float speed, World world)
            : this(startingPosition, direction, maxTime, damage, speed, new AnimatedSprite("TestImages/Projectile", 1, 1, 0.1f, 1.0f), world) {
        }

        //create a new sprite
        public Projectile(Vector2 startingPosition, Vector2 direction, float maxTime, int damage, float speed, String spriteResource, int xFrames, int yFrames, float diameter, float animationDuration, World world)
            : this(startingPosition, direction, maxTime, damage, speed, new AnimatedSprite(spriteResource, xFrames, yFrames, diameter, animationDuration), world) {
        }

        //use existing sprite
        public Projectile(Vector2 startingPosition, Vector2 direction, float maxTime, int damage, float speed, AnimatedSprite sprite, World world)
            : base (world, startingPosition, sprite){
            //set 
            _maxTime = maxTime;
            _startingPosition = startingPosition;
            _direction = Vector2.Normalize(direction);
            _damage = damage;

            //set up the body
            _body = BodyFactory.CreateCircle(world, sprite.EntitySize, 1);
            _body.BodyType = BodyType.Dynamic;
            _body.LinearDamping = 0;
            _body.LinearVelocity = _direction * speed;
            _body.Position = _startingPosition;
            
            //collide with everything but self
            _body.CollisionCategories = Category.Cat2;
            _body.CollidesWith = Category.All & ~Category.Cat2;

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
