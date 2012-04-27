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
        protected GameObjects _owner;

        public GameObject(GameObjects owner, Vector2 startingPosition) {
            _owner = owner;
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

        public bool isOutOfGridBounds(Vector2 gridCoord) {
            return gridCoord.X < 0 || gridCoord.X >= _owner.Map.GridDimension || gridCoord.Y < 0 || gridCoord.Y >= _owner.Map.GridDimension;
        }
    }
}
