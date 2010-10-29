using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Factories;
using Silhouette.Engine;

namespace Silhouette.GameMechs
{
    class Ground : DrawableLevelObject
    {
        public Vector2 position { get; set; }
        public Fixture fixture { get; set; }
        public Texture2D sprite { get; set; }

        public void Initialise() { }

        public void LoadContent(GameLoop game)
        {
            //Hannes: Ground verfügt erstmal nur über TestTextur, wird ausgetauscht gegen AnimatedSprite. Position ist auch
            //nur vorläufig. Fixture ist unbeweglich aber dient als Kollisionsdomäne.
            sprite = game.Content.Load<Texture2D>("Sprites/TestGround");
            position = new Vector2(0, game.GraphicsDevice.Viewport.Height-sprite.Height);
            fixture = FixtureFactory.CreateRectangle(Level.Physics, sprite.Width, sprite.Height,1.0f);
            fixture.Body.IsStatic = true;
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Einfacher Draw-Vorgang, statische Camera-Klasse gibt matrix für Transformationen
            spriteBatch.Draw(sprite, position, Color.White);
            spriteBatch.End();
        }

    }
}
