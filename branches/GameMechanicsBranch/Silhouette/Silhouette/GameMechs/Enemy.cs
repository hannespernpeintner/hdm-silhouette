using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;


namespace Silhouette.GameMechs
{
    class Enemy : DrawableLevelObject


    {
        public EnemyBehaviour Behaviour { get; set; }

        public Enemy(EnemyBehaviour Behaviour)
        {
            this.Behaviour = Behaviour;
        }
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
