using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticMind {
    class Particle : IDrawable, IGlowDrawable, IGameObject {

        private Vector2 _position;
        private float _rotation;
        private ParticleType _particleType;
        AnimatedSprite _sprite;

        Timer _fadeOutTimer;

        public Particle(Vector2 startingPosition, float rotation, ParticleType particleType) {
            _position = startingPosition;
            _rotation = rotation;
            _particleType = particleType;
            _fadeOutTimer = new Timer(particleType.Lifespan);

            if (particleType.SpriteAnimationSequence != null) {
                _sprite = new AnimatedSprite(particleType.SpriteAnimationSequence, particleType.EntitySize, particleType.AnimationDuration, particleType.DrawLayer, false);
            }

            if (particleType.GlowSpriteAnimationSequence != null) {
                GlowSprite = new AnimatedSprite(particleType.GlowSpriteAnimationSequence, particleType.EntitySize, particleType.AnimationDuration, particleType.DrawLayer, false);
            }
        }
        internal void Update(float deltaTime) {
            //shift the object
            if (MapManager.isShifting(_position)) {
                _position += MapManager.shiftAmount() * deltaTime;
            }
            if (_sprite != null) {
                _sprite.Update(deltaTime);
            }
            if (GlowSprite != null) {
                GlowSprite.Update(deltaTime);
            }
            _fadeOutTimer.Update(deltaTime);
        }

        public void WasCleared() {
        }

        public void WasKilled() {
        }

        public AnimatedSprite Sprite { get { return _sprite; } }

        public Vector2 Position { get { return _position; } }

        public float Rotation { get { return _rotation; } }

        public float Alpha {
            get {
                return 1 - _fadeOutTimer.percentComplete;
            }
        }

        public AnimatedSprite GlowSprite { get; set; }

        public bool ShouldBeKilled { get { return _fadeOutTimer.isFinished; } }

        public float DrawLayer { get { return _sprite.DrawLayer; } }
    }
}
