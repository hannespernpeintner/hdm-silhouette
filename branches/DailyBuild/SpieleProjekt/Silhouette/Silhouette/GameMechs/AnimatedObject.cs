using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silhouette.Engine;
using FarseerPhysics.Dynamics;
using Silhouette.Engine.Manager;
using FarseerPhysics.Dynamics.Contacts;

using System.ComponentModel;

namespace Silhouette.GameMechs
{
    [Serializable]
    public class AnimatedObject: DrawableLevelObject
    {
        [NonSerialized]
        public Texture2D texture;

        private String _fullPath;
        [DisplayName("Path"), Category("Animation Data")]
        [Description("The absolute path to the sound file.")]
        public String fullPath { get { return _fullPath; } set { _fullPath = value; } }

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

        private float _speed;
        [DisplayName("Speed"), Category("Animation Data")]
        [Description("The animation speed in fps.")]
        public float speed { get { return _speed; } set { _speed = value; } }

        private bool _looped;
        [DisplayName("Looped"), Category("Animation Data")]
        [Description("Animations loop config")]
        public bool looped { get { return _looped; } set { _looped = value; } }

        private bool _pingpong;
        [DisplayName("Pingpong"), Category("Animation Data")]
        [Description("Animations pingpong config")]
        public bool pingpong { get { return _pingpong; } set { _pingpong = value; } }

        private bool _backwards;
        [DisplayName("Backwards"), Category("Animation Data")]
        [Description("Play backwards?")]
        public bool backwards { get { return _backwards; } set { _backwards = value; } }


        private float _density;
        [DisplayName("Mass"), Category("Physical Behavior")]
        [Description("The mass of the object to calculate physical interaction.")]
        public float density { get { return _density; } set { _density = value; } }

        private BodyType _bodyType;
        [DisplayName("BodyType"), Category("Physical Behavior")]
        [Description("The BodyType defines the behavior of an object. A static object never changes position or rotation, like the dynamic ones do.")]
        public BodyType bodyType { get { return _bodyType; } set { _bodyType = value; } }

        private bool _isSensor;
        [DisplayName("Sensor(Collision)"), Category("Physical Behavior")]
        [Description("Defines if the player or any other physical object can collide with this object.")]
        public bool isSensor { get { return _isSensor; } set { _isSensor = value; } }

        private int _startInMiliseonds;
        public int StartInMiliseonds
        {
            get { return _startInMiliseonds; }
            set { _startInMiliseonds = value; }
        }

        public Layer layer;

        [NonSerialized]
        public Fixture fixture;
        [NonSerialized]
        public Animation animation;
        Vector2[] polygon;
        Rectangle boundingBox;
        Matrix transform;

        public AnimatedObject(Vector2 position, int amount, String path, float speed)
        {
            this.position = position;
            this.origin = Vector2.Zero;
            this.scale = new Vector2(1, 1);
            this.rotation = 0;

            this.fullPath = path;
            this.speed = speed;
            this.looped = true;
            this.pingpong = false;
            this.backwards = false;
            this.polygon = new Vector2[4];
            animation = new Animation(path, 0, speed, null, 0);
            //animation.Fullpath = path;
            //animation.Speed = speed;
            animation.position = position;
            animation.Looped = looped;
            animation.Pingpong = pingpong;
            animation.Backwards = backwards;
            //StartInMiliseonds = 0;
        }

        public override void Initialise() { }

        public override void LoadContent()
        {
            animation = new Animation();
            animation.Speed = speed;
            animation.Looped = looped;
            animation.Pingpong = pingpong;
            animation.Backwards = backwards;
            animation.StartInMiliseconds = StartInMiliseonds;

            animation.Fullpath = fullPath;
            animation.Load();
            texture = animation.activeTexture;
            //animation.Load(amount, path, speed, false);
            ToFixture();
            if (texture != null)
                origin = new Vector2((float)(texture.Width / 2), (float)(texture.Height / 2));
            transformed();
            if (isSensor)
            {
                animation.stop();
            }
            else
            {
                animation.start();
            }
        }
        public void LoadContent2()
        {
            if (animation == null)
            {
                animation = new Animation();
            }
            animation.Load();

            //ToFixture();
        }

        public override void Update(GameTime gameTime)
        {
            animation.Update(gameTime);
            animation.UpdateTransformation(position, rotation, scale);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
        }

        public override void drawInEditor(SpriteBatch spriteBatch)
        {
            Color color = Color.White;
            if (mouseOn) color = Constants.onHover;

            if (texture == null)
            {
                return;
            }
            origin = new Vector2((float)(texture.Width / 2), (float)(texture.Height / 2));
            spriteBatch.Draw(texture, position, null, color, rotation, origin, getScale(), SpriteEffects.None, 1);
        }

        public override string getPrefix()
        {
            return "AnimatedObject_";
        }

        public override bool canScale() { return true; }
        public override Vector2 getScale() { return scale; }
        public override void setScale(Vector2 scale) { this.scale = scale; }
        public override bool canRotate() { return true; }
        public override float getRotation() { return rotation; }
        public override void setRotation(float rotate) { this.rotation = rotate; }

        public void ToFixture()
        {
            fixture = FixtureManager.CreateRectangle(animation.activeTexture.Width, animation.activeTexture.Height, position, bodyType, density);
            fixture.IsSensor = true;
            fixture.OnCollision += this.OnCollision;
            fixture.OnSeparation += this.OnSeperation;
        }

        public override void drawSelectionFrame(SpriteBatch spriteBatch, Matrix matrix)
        {
            Primitives.Instance.drawPolygon(spriteBatch, polygon, Color.Yellow, 2);
            foreach (Vector2 p in polygon)
            {
                Primitives.Instance.drawCircleFilled(spriteBatch, p, 4, Color.Yellow);
            }
        }

        public override LevelObject clone()
        {
            AnimatedObject result = (AnimatedObject)this.MemberwiseClone();
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
                return true;
            }

            return false;
        }

        public bool OnCollision(Fixture f1, Fixture f2, Contact contact) 
        {
            if (isSensor)
            {
                this.animation.start();
            }
            return true;
        }

        public void OnSeperation(Fixture f1, Fixture f2)
        {
            if (isSensor)
            {
                this.animation.PlayedOnce = false;
            }
        }
    }
}
