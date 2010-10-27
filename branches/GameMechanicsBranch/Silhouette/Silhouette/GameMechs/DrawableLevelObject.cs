﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace Silhouette.GameMechs
{
    public abstract class DrawableLevelObject
    {


        public Vector2 position { get; set; }
        public Fixture fixture { get; set; }

        public void Initialise() { }

        public void LoadContent(GameLoop game) 
        {
        }

        public void Update(GameTime gameTime)
        { 
        }
         
        public void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
