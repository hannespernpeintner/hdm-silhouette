using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Silhouette.Engine.Manager;
using System.ComponentModel;
using Silhouette.Engine;

//Physik-Engine Klassen
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics.Contacts;

namespace Silhouette.GameMechs
{
    public enum PhysicAccuracy
    {
        Low,
        High
    }

    [Serializable]
    public partial class InteractiveObject : DrawableLevelObject
    {
        /* Sascha:
         * InteractiveObjects sind Texturen mit physikalischem Verhalten. Der Spieler kann so mit ihnen direkt interagieren, 
         * sie z.B. zum Stopfen eines Lochs verwenden oder aber von Ihnen erschlagen werden.
        */

        //Sascha: Texturen und Fixtures können nicht serialisiert werden, zudem würde das zu viel Speicherplatz fressen.
        [NonSerialized]
        public Texture2D texture;
        [NonSerialized]
        public Fixture fixture;
        [NonSerialized]
        public List<Fixture> fixtures;

        //Sascha: AssetNames ist einfach der Name der Datei. Wird verwendet um die Bilddatei aus der ContentPipeline oder dem Content-Ordner zu laden.
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

        private BodyType _bodyType;
        [DisplayName("BodyType"), Category("Physical Behavior")]
        [Description("The BodyType defines the behavior of an object. A static object never changes position or rotation, like the dynamic ones do.")]
        public BodyType bodyType { get { return _bodyType; } set { _bodyType = value; } }

        private PhysicAccuracy _accuracy;
        [DisplayName("Accuracy"), Category("Physical Behavior")]
        [Description("The accuracy defines how accurate the collision will be. High cost much more performance then low.")]
        public PhysicAccuracy accuracy { get { return _accuracy; } set { _accuracy = value; } }

        private bool _isDeadly;
        [DisplayName("Deadly"), Category("Physical Behavior")]
        [Description("Defines if the object can kill the player if it hits him with an defined force.")]
        public bool isDeadly { get { return _isDeadly; } set { _isDeadly = value; } }

        private bool _isClimbable;
        [DisplayName("Climbable"), Category("Fixture Data")]
        [Description("Defines if the player can climb up the collision object.")]
        public bool isClimbable { get { return _isClimbable; } set { _isClimbable = value; } }

        Matrix transform;       //Sascha: Das Objekt hat eine eigene Transformationsmatrix, damit man die Transformation auf das Selection Frame übertragen kann.
        Rectangle boundingBox;  //Sascha: Bounding Box für das Objekt, benutzt die Methode Contains zur ersten Collisionserkennung im Editor.
        Vector2[] polygon;
        [NonSerialized]
        Color[] collisionData;  //Sascha: Speichert die Bilddaten der Textur ab, damit man später transparente Pixel nicht selektieren kann.

        public InteractiveObject(String path)
        {
            this.fullPath = path;
            this.assetName = Path.GetFileNameWithoutExtension(path);
            this.scale = Vector2.One;
            this.rotation = 0f;
            this.origin = Vector2.Zero;
            this.polygon = new Vector2[4];
            density = 1;
            bodyType = BodyType.Dynamic;
            accuracy = PhysicAccuracy.Low;
        }

        public override void Initialise() { }

        public override void LoadContent()
        {
            try
            {
                texture = GameLoop.gameInstance.Content.Load<Texture2D>("Sprites/" + assetName);
            }
            catch (Exception e1)
            {

                try
                {
                    texture = GameLoop.gameInstance.Content.Load<Texture2D>(fullPath);
                }
                catch (Exception e2)
                {
                    try
                    {
                        string p = Path.Combine(layer.level.contentPath, Path.GetFileName(fullPath));
                        texture = TextureManager.Instance.LoadFromFile(p);
                        if (texture == null)
                            throw new Exception();
                    }
                    catch (Exception e3)
                    {
                        texture = TextureManager.Instance.LoadFromFile(fullPath);
                    }
                }
            }

            if (texture != null)
            {
                origin = new Vector2((float)(texture.Width / 2), (float)(texture.Height / 2));
            }
            else
            {
                DebugLogManager.writeToLogFile(@"Unable to load texture: " + assetName + @" . Using fallback!");
                texture = GameLoop.gameInstance.Content.Load<Texture2D>(@"Sprites/fallback");
            }
            this.ToFixture();

            if (fixture != null)
                fixture.Body.Rotation = rotation;
            if (fixtures != null)
                fixtures[0].Body.Rotation = rotation;
        }

        public override void Update(GameTime gameTime)
        {
            if (fixture != null && bodyType == BodyType.Dynamic)
            {
                if (accuracy == PhysicAccuracy.Low)
                {
                    position = FixtureManager.ToPixel(fixture.Body.Position);
                    rotation = fixture.Body.Rotation;
                }
            }
            if (fixtures != null && bodyType == BodyType.Dynamic)
            {
                if (accuracy == PhysicAccuracy.High)
                {
                    position = FixtureManager.ToPixel(fixtures[0].Body.Position);
                    rotation = fixtures[0].Body.Rotation;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, rotation, origin, scale, SpriteEffects.None, 1);
        }

        public void ToFixture()
        {
            try
            {
                if (accuracy == PhysicAccuracy.Low)
                {
                    fixture = FixtureManager.CreatePolygon(texture, scale, bodyType, position, density);
                    fixture.isDeadly = isDeadly;
                    fixture.isClimbable = isClimbable;
                    fixture.OnCollision += this.InteractiveOnCollision;
                }
                else if (accuracy == PhysicAccuracy.High)
                {
                    fixtures = FixtureManager.TextureToPolygon(texture, scale, bodyType, position, density);
                    fixtures[0].isDeadly = isDeadly;
                    fixtures[0].isClimbable = isClimbable;
                    fixtures[0].OnCollision += this.InteractiveOnCollision;
                }
            }
            catch (Exception e)
            {
                fixture = FixtureFactory.CreateRectangle(Level.Physics, (texture.Width * scale.X) / Level.PixelPerMeter, (texture.Height * scale.Y) / Level.PixelPerMeter, density);
                fixture.Body.BodyType = bodyType;
                fixture.Body.Position = FixtureManager.ToMeter(position);
                fixture.isDeadly = isDeadly;
                fixture.isClimbable = isClimbable;

                fixture.OnCollision += this.InteractiveOnCollision;
            }
        }

        public override void drawInEditor(SpriteBatch spriteBatch)
        {
            Color color = Color.White;
            if (mouseOn) color = Constants.onHover;
            if (texture != null)
            {
                origin = new Vector2((float)(texture.Width / 2), (float)(texture.Height / 2));
                spriteBatch.Draw(texture, position, null, color, rotation, origin, scale, SpriteEffects.None, 1);
            }
        }

        public override void loadContentInEditor(GraphicsDevice graphics)
        {
            if (texture == null)
            {
                try
                {
                    string p = Path.Combine(layer.level.contentPath, Path.GetFileName(fullPath));
                    texture = TextureManager.Instance.LoadFromFile(p, graphics);
                    this.fullPath = p;
                }
                catch (Exception e)
                {
                    texture = TextureManager.Instance.LoadFromFile(fullPath, graphics);
                }
            }

            if (texture.Width != 1280 && texture.Height != 768)
            {
                collisionData = TextureManager.Instance.GetCollisionData(fullPath);
            }
            transformed();
        }

        public override string getPrefix()
        {
            return "InteractiveObject_";
        }

        public override bool canScale() { return true; }
        public override Vector2 getScale() { return scale; }
        public override void setScale(Vector2 scale) { this.scale = scale; }
        public override bool canRotate() { return true; }
        public override float getRotation() { return rotation; }
        public override void setRotation(float rotate) { this.rotation = rotate; }

        public override LevelObject clone()
        {
            InteractiveObject result = (InteractiveObject)this.MemberwiseClone();
            result.polygon = (Vector2[])this.polygon.Clone();
            result.mouseOn = false;
            return result;
        }

        public override void transformed()
        {
            if (texture == null)
                return;

            transform =
                Matrix.CreateTranslation(new Vector3(-origin.X, -origin.Y, 0.0f)) *
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
            if (boundingBox.Contains(new Point((int)worldPosition.X, (int)worldPosition.Y)))
            {
                if (collisionData != null)
                    return intersectPixels(worldPosition);
                else
                    return true;
            }

            return false;
        }

        public override void drawSelectionFrame(SpriteBatch spriteBatch, Matrix matrix)
        {
            Primitives.Instance.drawPolygon(spriteBatch, polygon, Color.Yellow, 2);
            foreach (Vector2 p in polygon)
            {
                Primitives.Instance.drawCircleFilled(spriteBatch, p, 4, Color.Yellow);
            }
        }

        public bool intersectPixels(Vector2 worldPosition)
        {
            Vector2 positionInB = Vector2.Transform(worldPosition, Matrix.Invert(transform));
            int xB = (int)Math.Round(positionInB.X);
            int yB = (int)Math.Round(positionInB.Y);

            if (0 <= xB && xB < texture.Width && 0 <= yB && yB < texture.Height)
            {
                Color colorB = collisionData[xB + yB * texture.Width];
                if (colorB.A != 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool InteractiveOnCollision(Fixture a, Fixture b, Contact contact)
        {
            if (!b.isPlayer)
            {
                isDeadly = false;
                a.isDeadly = false;
            }

            return true;
        }
    }
}
