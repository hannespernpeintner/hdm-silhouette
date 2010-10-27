using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Factories;

namespace Silhouette.GameMechs
{
    class Player:DrawableLevelObject
    {
       

        public Vector2 position { get; set; }
        public Fixture fixture { get; set; }
        public AnimatedSprite Sprite { get; set; }

        public void Initialise() {
            Sprite = new AnimatedSprite();
        }

        public void LoadContent(GameLoop game)
        {
          Sprite.Texture = game.Content.Load<Texture2D>("Sprites/link");
          fixture = FixtureFactory.CreateCircle(Engine.Level.Physics, 4.0f, 1.0f);

        }

        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
            {
                fixture.Body.ApplyForce(new Vector2(0.0f, gameTime.ElapsedGameTime.Milliseconds * 0.001f));
            
            }
           
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite.Texture, position, Color.White);
        }
    }
}
