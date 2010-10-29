using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Factories;
using Silhouette.Engine;

namespace Silhouette.GameMechs
{
    class Player:DrawableLevelObject
    {
       

        public Vector2 Position { get; set; }
        public Fixture Fixture { get; set; }
        public AnimatedSprite Sprite { get; set; }

        public void Initialise() {
            Sprite = new AnimatedSprite();
        }

        public void LoadContent(GameLoop game)
        {
          Texture2D temp = game.Content.Load<Texture2D>("Sprites/jumptest");
          Sprite.LoadGraphic(temp,1,8,temp.Width,temp.Height,4);
          Fixture = FixtureFactory.CreateCircle(Level.Physics, 4.0f, 1.0f);

        }

        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                //Sprung mit W-Taste
                Fixture.Body.ApplyForce(new Vector2(0.0f, gameTime.ElapsedGameTime.Milliseconds * 0.001f));
                //animateJump();
            
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Fixture.Body.ApplyForce(new Vector2(gameTime.ElapsedGameTime.Milliseconds * 0.001f, 0.0f));

            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                Fixture.Body.ApplyForce(new Vector2(-gameTime.ElapsedGameTime.Milliseconds * 0.001f, 0.0f));

            }

            this.Sprite.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds);
           
            
        }

        public void animateJump() 
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, Camera.matrix);
            spriteBatch.Draw(Sprite.Texture, position, Color.White);
            spriteBatch.End();
        }
    }
}
