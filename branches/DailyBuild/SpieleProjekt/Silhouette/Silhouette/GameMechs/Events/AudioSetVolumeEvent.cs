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
    [Serializable]
    public class AudioSetVolumeEvent : Event
    {
        private float _Volume;
        [DisplayName("Volume"), Category("Event Data")]
        [Description("The sound volume which should be applied to the SoundObjects in the list.")]
        public float Volume { get { return _Volume; } set { _Volume = value; } }

        public AudioSetVolumeEvent(Rectangle rectangle)
        {
            this.rectangle = rectangle;
            position = rectangle.Location.ToVector2();
            width = rectangle.Width;
            height = rectangle.Height;
            list = new List<LevelObject>();
            isActivated = true;
            _Volume = 1.0f;
        }

        public override void AddLevelObject(LevelObject lo)
        {
            if ((this.list != null) && (lo is SoundObject))
            {
                if (!this.list.Contains(lo))
                    this.list.Add(lo);
            }
        }

        public bool OnCollision(Fixture a, Fixture b, Contact contact)
        {
            if (isActivated)
            {
                foreach (SoundObject so in this.list)
                {
                    so.volume = _Volume;
                }
                isActivated = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override string getPrefix()
        {
            return "AudioSetVolumeEvent_";
        }

        public override LevelObject clone()
        {
            AudioSetVolumeEvent result = (AudioSetVolumeEvent)this.MemberwiseClone();
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

