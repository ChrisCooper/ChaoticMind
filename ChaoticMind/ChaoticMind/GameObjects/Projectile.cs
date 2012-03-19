﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace ChaoticMind {
    class Projectile : DrawableGameObject{

        float _range;
        int _damage;
        Vector2 _direction;
        Vector2 _startingPosition;

        private int _timerId;

        //default projectile
        public Projectile(Vector2 startingPosition, Vector2 direction, float range, int damage, float speed)
            : this(startingPosition, direction, range, damage, speed, new AnimatedSprite("TestImages/Projectile", 1, 1, 0.1f, 1.0f)) {
        }

        //create a new sprite
        public Projectile(Vector2 startingPosition, Vector2 direction, float range, int damage, float speed, String spriteResource, int xFrames, int yFrames, float diameter, float animationDuration)
            : this(startingPosition, direction, range, damage, speed, new AnimatedSprite(spriteResource, xFrames, yFrames, diameter, animationDuration)) {
        }

        //use existing sprite
        public Projectile(Vector2 startingPosition, Vector2 direction, float range, int damage, float speed, AnimatedSprite sprite)
            : base (startingPosition, sprite){
            //set 
            _range = range;
            _startingPosition = startingPosition;
            _damage = damage;

            //for some reason, normalizing a Vector2.Zero gives a (NaN, NaN) vector
            if (direction != Vector2.Zero) _direction = Vector2.Normalize(direction);
            else _direction = direction;

            //set up the body
            _body = BodyFactory.CreateCircle(Program.SharedGame.MainWorld, EntitySize, 1);
            _body.BodyType = BodyType.Dynamic;
            _body.LinearDamping = 0;
            _body.LinearVelocity = _direction * speed;
            _body.Position = _startingPosition;
            
            //collide with everything but self
            _body.CollisionCategories = Category.Cat2;
            _body.CollidesWith = Category.All & ~Category.Cat2;

            //init the timer
            if (speed <= 0) throw new Exception("Speed cannot be negative or 0 (but direction can, hint hint)");
            _timerId = TimeDelayManager.InitTimer(_range / speed, 0);
        }

        public bool Timeout (){
            return TimeDelayManager.Finished(_timerId);
        }

        public void Destroy() {
            TimeDelayManager.DeleteTimer(_timerId);
            _body.Dispose();
        }

        public override void Update(float deltaTime) {
            base.Update(deltaTime);
        }
    }
}
