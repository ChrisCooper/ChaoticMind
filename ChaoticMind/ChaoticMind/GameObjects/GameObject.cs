﻿using System;
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

        //destroy flag
        public virtual bool ShouldDieNow() {
            return false;
        }

        //destroy method
        public virtual void DestroySelf(){
            _body.Dispose();
        }
    }
}
