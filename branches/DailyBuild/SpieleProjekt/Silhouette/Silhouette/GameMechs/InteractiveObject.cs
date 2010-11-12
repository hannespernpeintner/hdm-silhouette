using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Silhouette.Engine.Manager;

namespace Silhouette.GameMechs
{
    class InteractiveObject: DrawableLevelObject
    {
        public Texture2D texture;
        public String path;
        public List<Fixture> polygon;
        public float rotation;

        public InteractiveObject() { }

        public InteractiveObject(Vector2 position, String path, float rotation)
        {
            this.position = position;
            this.path = path;
            this.rotation = rotation;
        }

        public override void Initialise()
        {
        }

        public override void LoadContent()
        {
            GameLoop.gameInstance.Content.Load<Texture2D>(path);
            polygon = FixtureManager.TextureToPolygon(texture, BodyType.Dynamic, position, 1.0f);
        }

        public override void Update(GameTime gameTime)
        {
            position = polygon[0].Body.Position;
            rotation = polygon[0].Body.Rotation;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height), new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, Vector2.Zero, SpriteEffects.None, 0.0f);
        }
    }
}
