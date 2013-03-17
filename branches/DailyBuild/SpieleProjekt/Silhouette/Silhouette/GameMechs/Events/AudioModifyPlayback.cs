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
    public class AudioModifyPlayback : Event
    {
        public enum Type {Play, Stop, Pause }

        private Type _EventType;
        [DisplayName("Action"), Category("Event Data")]
        [Description("Defines if the event plays, stops or pauses the SoundObjects in the list.")]
        public Type EventType { get { return _EventType; } set { _EventType = value; } }

        private Boolean _looped;
        [DisplayName("Loop"), Category("Event Data")]
        [Description("Defines if the event makes the SoundObjects is looped. Beware: You can't change this while a Sound is playing!")]
        public Boolean looped { get { return _looped; } set { _looped = value; } }
        
        public  AudioModifyPlayback(Rectangle rectangle)
        {
            this.rectangle = rectangle;
            position = rectangle.Location.ToVector2();
            width = rectangle.Width;
            height = rectangle.Height;
            list = new List<LevelObject>();
            isActivated = true;
            OnlyOnPlayerCollision = true;
            this.EventType = Type.Play;
            this.looped = false;
            
        }

        public bool OnCollision(Fixture a, Fixture b, Contact contact)
        {
            if (isActivated && ((OnlyOnPlayerCollision && b.isPlayer) || !OnlyOnPlayerCollision))
            {
                foreach (SoundObject so in this.list)
                {
                   
                    switch (EventType)
                    {
                        case (Type.Play):
                            so.looped = looped;
                            so.Play();
                                break;
                        case (Type.Pause):
                                so.Pause = !so.Pause;
                                break;
                        case (Type.Stop):
                                so.Stop();
                                break;
                    }
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
            return "AudioModifyPlaybackEvent_";
        }

        public override LevelObject clone()
        {
            AudioModifyPlayback result = (AudioModifyPlayback)this.MemberwiseClone();
            result.mouseOn = false;
            return result;
        }

        public override void ToFixture()
        {
            fixture = FixtureManager.CreateRectangle(width, height, position, BodyType.Static, 1);
            fixture.OnCollision += this.OnCollision;
            fixture.IsSensor = true;
        }

        public override void AddLevelObject(LevelObject lo)
        {

            if ((this.list != null) && (lo is SoundObject))
            {
                if (!this.list.Contains(lo))
                    this.list.Add(lo);
            }
        }
    }
}

