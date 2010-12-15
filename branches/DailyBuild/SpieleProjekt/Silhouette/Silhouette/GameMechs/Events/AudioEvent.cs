using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Silhouette;
using Silhouette.Engine;
using Silhouette.Engine.Manager;
using Silhouette.GameMechs;
using System.IO;
using System.ComponentModel;

//Physik-Engine Klassen
using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics.Contacts;

namespace Silhouette.GameMechs.Events
{
    public class AudioEvent : Event
    {
        // Julius: Spielt einfach bei Kollision alle in der List befindlichen AudioObjects

        private List<SoundObject> _list;

        [DisplayName("Object List"), Category("Event Data")]
        [Description("The list of Objects which are affected by the event.")]
        public List<SoundObject> list { get { return _list; } set { _list = value; } }

        public AudioEvent(Rectangle rectangle)
        {
            this.rectangle = rectangle;
            position = rectangle.Location.ToVector2();
            width = rectangle.Width;
            height = rectangle.Height;
            list = new List<SoundObject>();
            isActivated = true;
        }

        public bool OnCollision(Fixture a, Fixture b, Contact contact)
        {
            if (isActivated)
            {
                foreach (SoundObject so in this.list)
                {
                    so.Play();
                }
                isActivated = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void addInteractiveObject(SoundObject so)
        {
            if (this.list != null)
            {
                if (!this.list.Contains(so))
                    this.list.Add(so);
            }
        }

        public override string getPrefix()
        {
            return "AudioEvent_";
        }

        public override LevelObject clone()
        {
            AudioEvent result = (AudioEvent)this.MemberwiseClone();
            result.mouseOn = false;
            return result;
        }

        public override void ToFixture()
        {
            fixture = FixtureManager.CreateRectangle(width, height, position, BodyType.Static, 1);
            fixture.OnCollision += this.OnCollision;
            fixture.IsSensor = true;
        }
    }
}
