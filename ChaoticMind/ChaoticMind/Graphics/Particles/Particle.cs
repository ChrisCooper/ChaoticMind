using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticMind {
    class Particle : IDrawable {

        private Vector2 _position;
        private float _rotation;
        private ParticleType _particleType;
        AnimatedSprite _sprite;

        Timer _fadeOutTimer;

        public Particle(Vector2 startingPosition, float rotation, ParticleType particleType) {
            _position = startingPosition;
            _rotation = rotation;
            _particleType = particleType;
            _sprite = new AnimatedSprite(particleType.SpriteAnimationSequence, particleType.EntitySize, particleType.AnimationDuration, particleType.DrawLayer, false);
            _fadeOutTimer = new Timer(particleType.Lifespan);
        }
        internal void Update(float deltaTime) {
            _sprite.Update(deltaTime);
            _fadeOutTimer.Update(deltaTime);
        }

        public Texture2D Texture { get { return _sprite.Texture; } }

        public Rectangle CurrentTextureBounds { get { return _sprite.CurrentTextureBounds; } }

        public Vector2 CurrentTextureOrigin { get { return _sprite.CurrentTextureOrigin; } }

        public float PixelsPerMeter { get { return _sprite.PixelsPerMeter; } }

        public Vector2 Position { get { return _position; } }

        public float Rotation { get { return _rotation; } }

        public float Alpha {
            get {
                return 1 - _fadeOutTimer.percentComplete;
            }
        }

        public bool isDead { get { return _fadeOutTimer.isFinished; } }

        public float DrawLayer { get { return _sprite.DrawLayer; } }
    }
}
