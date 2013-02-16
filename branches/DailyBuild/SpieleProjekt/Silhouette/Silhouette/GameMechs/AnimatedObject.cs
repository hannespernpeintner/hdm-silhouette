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

        private String _path;
        [DisplayName("Path"), Category("Animation Data")]
        [Description("The absolute path to the sound file.")]
        public String path { get { return _path; } set { _path = value; } }

        private int _amount;
        [Browsable(false)]
        public int amount { get { return _amount; } set { _amount = value; } }

        private int _speed;
        [DisplayName("Speed"), Category("Animation Data")]
        [Description("The animation speed.")]
        public int speed { get { return _speed; } set { _speed = value; } }

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

        [NonSerialized]
        public Fixture fixture;
        [NonSerialized]
        public Animation animation;     

        public AnimatedObject(Vector2 position, int amount, String path, int speed)
        {
            this.position = position;
            this.amount = amount;

            this.path = path;
            this.speed = speed;
            this.looped = true;
            this.pingpong = false;
            this.backwards = false;
            animation = new Animation();
            animation.Fullpath = path;
            animation.speed = speed;
            animation.position = position;
            animation.Looped = looped;
            animation.Pingpong = pingpong;
            animation.Backwards = backwards;
        }

        public override void Initialise() { }

        public override void LoadContent()
        {
            animation = new Animation();
            if(isSensor)
            {
                animation.stop();
            }
            animation.Fullpath = path;
            animation.Load();
            //animation.Load(amount, path, speed, false);
            ToFixture();
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
            animation.Update(gameTime, position);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
        }

        public override void drawInEditor(SpriteBatch spriteBatch)
        {
            if (texture == null)
            {
                LoadContent2();
            }
            spriteBatch.Draw(texture, new Vector2((position.X-texture.Width/2), (position.Y-texture.Height/2)), Color.White);
        }

        public override string getPrefix()
        {
            return "AnimatedObject_";
        }

        public void ToFixture()
        {
            fixture = FixtureManager.CreateRectangle(animation.activeTexture.Width, animation.activeTexture.Height, position, bodyType, density);
            fixture.IsSensor = true;
            fixture.OnCollision += this.OnCollision;
            fixture.OnSeparation += this.OnSeperation;
        }

        public bool OnCollision(Fixture f1, Fixture f2, Contact contact) 
        {
            this.animation.start();
            return true;
        }

        public void OnSeperation(Fixture f1, Fixture f2)
        {
            this.animation.PlayedOnce = false;
        }
    }
}
