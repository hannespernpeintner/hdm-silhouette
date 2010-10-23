using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;

namespace Silhouette.GameMechs
{
    class Player:DrawableLevelObject
    {
        private Vector2 _position;
        private Fixture _fixture;

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
