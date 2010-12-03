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
using Silhouette.Engine;

namespace Silhouette.GameMechs
{
    [Serializable]
    public partial class InteractiveObject: DrawableLevelObject
    {
        [NonSerialized]
        public Texture2D texture;
        [NonSerialized]
        public Fixture fixture;

        private string _assetName;
        [DisplayName("Filename"), Category("Texture Data")]
        [Description("The filename of the attached texture.")]
        public string assetName { get { return _assetName; } set { _assetName = value; } }

        private string _fullPath;
        [DisplayName("Path"), Category("Texture Data")]
        [Description("The full path of the texture.")]
        public string fullPath { get { return _fullPath; } set { _fullPath = value; } }

        private Vector2 _scale;
        [DisplayName("Scale"), Category("Texture Data")]
        [Description("The scale factor of the object.")]
        public Vector2 scale { get { return _scale; } set { _scale = value; transformed(); } }

        private float _rotation;
        [DisplayName("Rotation"), Category("Texture Data")]
        [Description("The rotation factor of the object.")]
        public float rotation { get { return _rotation; } set { _rotation = value; transformed(); } }

        private Vector2 _origin;
        [DisplayName("Origin"), Category("Texture Data")]
        [Description("The sprite origin. Default is (0,0), which is the upper left corner.")]
        public Vector2 origin { get { return _origin; } set { _origin = value; transformed(); } } 

        private float _density;
        [DisplayName("Mass"), Category("Physical Behavior")]
        [Description("The mass of the object to calculate physical interaction.")]
        public float density { get { return _density; } set { _density = value; } }

        Matrix transform;
        Rectangle boundingBox;
        Vector2[] polygon;

        public InteractiveObject(String path)
        {
            this.fullPath = path;
            this.assetName = Path.GetFileNameWithoutExtension(path);
            this.scale = Vector2.One;
            this.rotation = 0f;
            this.origin = Vector2.Zero;
            this.polygon = new Vector2[4];
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

        public override void drawInEditor(SpriteBatch spriteBatch)
        {
            Color color = Color.White;
            if (mouseOn) color = Constants.onHover;
            spriteBatch.Draw(texture, position, null, color, rotation, origin, scale, SpriteEffects.None, 1);
        }

        public override void loadContentInEditor(GraphicsDevice graphics)
        {
            if (texture == null)
            {
                FileStream file = FileManager.LoadConfigFile(fullPath);
                texture = Texture2D.FromStream(graphics, file);
            }

            transformed();
        }

        public override string getPrefix()
        {
            return "InteractiveObject_";
        }

        public override LevelObject clone()
        {
            throw new NotImplementedException();
        }

        public override void transformed()
        {
            if (texture == null)
                return;

            transform =
                Matrix.CreateTranslation(new Vector3(origin.X, origin.Y, 0.0f)) *
                Matrix.CreateScale(scale.X, scale.Y, 1) *
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateTranslation(new Vector3(position, 0.0f));

            Vector2 leftTop = new Vector2(0, 0);
            Vector2 rightTop = new Vector2(texture.Width, 0);
            Vector2 leftBottom = new Vector2(0, texture.Height);
            Vector2 rightBottom = new Vector2(texture.Width, texture.Height);

            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            polygon[0] = leftTop;
            polygon[1] = rightTop;
            polygon[3] = leftBottom;
            polygon[2] = rightBottom;

            Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop),
                                      Vector2.Min(leftBottom, rightBottom));
            Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop),
                                      Vector2.Max(leftBottom, rightBottom));

            boundingBox = new Rectangle((int)min.X, (int)min.Y,
                                 (int)(max.X - min.X), (int)(max.Y - min.Y));
        }

        public override bool contains(Vector2 worldPosition)
        {
            return boundingBox.Contains((int)worldPosition.X, (int)worldPosition.Y);
        }

        public override void drawSelectionFrame(SpriteBatch spriteBatch, Matrix matrix)
        {
            Primitives.Instance.drawPolygon(spriteBatch, polygon, Color.Yellow, 2);
            foreach (Vector2 p in polygon)
            {
                Primitives.Instance.drawCircleFilled(spriteBatch, p, 4, Color.Yellow);
            }
        }
    }
}
