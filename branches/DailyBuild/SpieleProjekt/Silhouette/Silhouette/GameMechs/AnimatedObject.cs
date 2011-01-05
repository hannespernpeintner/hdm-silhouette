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
    public class AnimatedObject: DrawableLevelObject
    {
        private String _path;
        [DisplayName("Path"), Category("Animation Data")]
        [Description("The absolute path to the sound file.")]
        public String path { get { return _path; } set { _path = value; } }

        private int _amount;
        [Browsable(false)]
        public int amount { get { return _amount; } set { _amount = value; } }

        private float _speed;
        [DisplayName("Speed"), Category("Animation Data")]
        [Description("The animation speed.")]
        public float speed { get { return _speed; } set { _speed = value; } }


        private float _density;
        [DisplayName("Mass"), Category("Physical Behavior")]
        [Description("The mass of the object to calculate physical interaction.")]
        public float density { get { return _density; } set { _density = value; } }

        private BodyType _bodyType;
        [DisplayName("BodyType"), Category("Physical Behavior")]
        [Description("The BodyType defines the behavior of an object. A static object never changes position or rotation, like the dynamic ones do.")]
        public BodyType bodyType { get { return _bodyType; } set { _bodyType = value; } }

        private bool _isSensor;
        [DisplayName("Collision"), Category("Physical Behavior")]
        [Description("Defines if the player or any other physical object can collide with this object.")]
        public bool isSensor { get { return _isSensor; } set { _isSensor = value; } }

        [NonSerialized]
        public Fixture fixture;
        [NonSerialized]
        public Animation animation;     

        public AnimatedObject(Vector2 position, int amount, String path, float speed)
        {
            this.position = position;
            this.amount = amount;

            this.path = path;
            this.speed = speed;
            animation = new Animation();
        }

        public override void Initialise() { }

        public override void LoadContent()
        {
            animation.Load(amount, path, speed, false);
            ToFixture();
        }

        public override void Update(GameTime gameTime)
        {
            animation.Update(gameTime, position);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
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
            this.animation.playedOnce = false;
        }
    }
}
