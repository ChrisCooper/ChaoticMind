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
            //shift the object
            if (!_body.IsDisposed && _owner.Map.isShifting(_body.Position)) {
                _body.Position += _owner.Map.shiftAmount() * deltaTime;
            }
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

        public GameObjects Owner {
            get { return _owner; }
        }

        public bool IsOutOfBounds{
            get { return _owner.Map.IsOutOfBounds(Position); }
        }

        //returns the index of the map array the object is currently in
        public virtual Vector2 GridCoordinate {
            get { return _owner.Map.GridPositionForWorldCoordinates(Position); }
        }
    }
}
