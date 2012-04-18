using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace ChaoticMind {
    class GameObject {
        protected Body _body;
        protected bool _shouldBeKilledFlag = false;

        public GameObject(Vector2 startingPosition) {
        }

        public virtual void Update(float deltaTime) {
        }

        public Vector2 Position {
            get { return _body.Position; }
        }

        public virtual float Rotation {
            get { return _body.Rotation; }
        }

        public Body Body {
            get { return _body; }
        }

        public virtual bool ShouldBeKilled {
            get { return _shouldBeKilledFlag; }
        }

        public virtual void WasKilled() {
            WasCleared();
        }

        public virtual void WasCleared(){
            _body.Dispose();
        }
    }
}
