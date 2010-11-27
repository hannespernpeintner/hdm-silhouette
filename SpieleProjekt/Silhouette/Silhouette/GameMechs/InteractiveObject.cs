using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Silhouette.Engine.Manager;
using System.ComponentModel;

namespace Silhouette.GameMechs
{
    [Serializable]
    public partial class InteractiveObject: DrawableLevelObject
    {
        [NonSerialized]
        public Texture2D texture;
        [NonSerialized]
        public Fixture fixture;

        private float _density;
        [DisplayName("Mass"), Category("General")]
        [Description("The mass of the object to calculate physical interaction.")]
        public float density { get { return _density; } set { _density = value; } }

        public InteractiveObject(String path)
        {
            this.fullPath = path;
            this.assetName = Path.GetFileNameWithoutExtension(path);
            this.scale = 1f;
            this.rotation = 0f;
            this.origin = Vector2.Zero;
            density = 1;
        }

        public override void Initialise() {}

        public override void LoadContent()
        {
            texture = GameLoop.gameInstance.Content.Load<Texture2D>("Sprites/" + assetName);
            origin = new Vector2((float)(texture.Width / 2), (float)(texture.Height / 2));
            this.ToFixture();
        }

        public override void Update(GameTime gameTime)
        {
            position = FixtureManager.ToPixel(fixture.Body.Position);
            rotation = fixture.Body.Rotation;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, rotation, origin, scale, SpriteEffects.None, 1);
        }

        public void ToFixture()
        {
            fixture = FixtureManager.CreateRectangle(texture.Width, texture.Height, position, BodyType.Dynamic, density);
        }

        public override string getPrefix()
        {
            return "InteractiveObject_";
        }

        public override bool contains(Vector2 worldPosition)
        {
            Rectangle r = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            return r.Contains((int)worldPosition.X, (int)worldPosition.Y);
        }

        public override void drawSelectionFrame()
        {

        }
    }
}
