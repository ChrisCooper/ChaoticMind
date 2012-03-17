﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;

namespace ChaoticMind {
    class Collectable : DrawableGameObject {

        const float ENTITY_SIZE = 2f;

        AnimatedSprite _minimapSprite;

        public Collectable(String resource, int xFrames, int yFrames, float animationDuration, Vector2 startingPosition)
            : base(resource, xFrames, yFrames, ENTITY_SIZE, animationDuration, startingPosition) {

            //set up the body
                _body = BodyFactory.CreateRectangle(Program.SharedGame.MainWorld, ENTITY_SIZE, ENTITY_SIZE, 1);
            _body.BodyType = BodyType.Kinematic;
            _body.AngularDamping = 0;
            _body.AngularVelocity = 5;
            _body.Position = startingPosition;
            _body.UserData = Utilities.BodyTypes.COLLECTABLE;

            _minimapSprite = new StaticSprite("Minimap/CollectableMinimap", MapTile.TileSideLength / 2);   
        }

        //minimap stuff
        public override AnimatedSprite MapSprite{
            get { return _minimapSprite; }
        }
    }
}
