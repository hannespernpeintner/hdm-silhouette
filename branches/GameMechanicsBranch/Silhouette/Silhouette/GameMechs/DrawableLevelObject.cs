using System;
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
        private Vector2 _position;
        private Fixture _fixture;

        public Vector2 position { get { return _position; } set { _position = value; } }
        public Fixture fixture { get { return _fixture; } set { _fixture = value; } }

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
