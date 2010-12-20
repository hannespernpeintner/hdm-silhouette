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
    public class AudioEqualizerEvent : Event
    {
        public enum Type { enable, disable, modify }
        public Type eqType { get { return _eqType; } set { _eqType = value; } }
        private Type _eqType;

        public float fCenter { get; set; }
        private float _fCenter;
        public float fBandwidth { get; set; }
        public float fGain {get; set;}


        public AudioEqualizerEvent(Rectangle rectangle )
        {
            this.rectangle = rectangle;
            position = rectangle.Location.ToVector2();
            width = rectangle.Width;
            height = rectangle.Height;
            list = new List<LevelObject>();
            isActivated = true;

           // this.fCenter = 0.0f;
            this.fBandwidth = 0.0f;
            this.fGain = 0.0f;

            


        }


        public override void AddLevelObject(LevelObject lo)
        {

            if (this.list != null)
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
                    switch (eqType)
                    {
                        case (Type.enable):
                            so.EnableEqualizer(fCenter, fBandwidth, fGain);
                            break;
                        case (Type.modify):
                            so.EnableEqualizer(fCenter, fBandwidth, fGain);
                            break;
                        case (Type.disable):
                            so.DisableEqualizer();
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
            return "AudioEqualizerEvent_";
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

    }
}

