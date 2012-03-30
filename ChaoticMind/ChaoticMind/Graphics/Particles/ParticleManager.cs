using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class ParticleManager {
        static ParticleManager _mainInstance;

        List<Particle> _particles = new List<Particle>();
        
        static ParticleManager() {
            _mainInstance = new ParticleManager();
        }

        internal static ParticleManager mainInstance() {
            return _mainInstance;
        }

        //No one can make one but us
        private ParticleManager() {
        }

        public void Update(float deltaTime) {
            foreach (Particle p in _particles){
                    p.Update(deltaTime);
            }
            for (int i = 0; i < _particles.Count; i++) {
                Particle p = _particles[i];
                if (p.isDead) {
                    _particles.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Draw(Camera camera) {
            foreach (Particle p in _particles) {
                camera.Draw(p);
            }
        }

        internal void DrawGlows(Camera camera) {
            foreach (Particle p in _particles) {
                camera.DrawGlow(p);
            }
        }

        public static void CreateParticle(Vector2 startingPosition, float rotation, ParticleType particleType) {
            _mainInstance._particles.Add(new Particle(startingPosition, rotation, particleType));
        }

        internal static void Remove(Particle particle) {
            _mainInstance._particles.Remove(particle);
        }
    }
}
