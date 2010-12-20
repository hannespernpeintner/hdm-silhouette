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
    public class AudioEqualizerEvent : AudioEvent
    {
        public enum Type { enable, disable, modify }
        public Type eqType { get; set; }

        public float fCenter { get; set; }
        public float fBandwidth { get; set; }
        public float fGain {get; set;}


        public AudioEqualizerEvent(Rectangle rectangle, Type eqType, float fCenter, float fBandwidth, float fGain )
        {
            this.rectangle = rectangle;
            position = rectangle.Location.ToVector2();
            width = rectangle.Width;
            height = rectangle.Height;
            list = new List<LevelObject>();
            isActivated = true;

            this.fCenter = fCenter;
            this.fBandwidth = fBandwidth;
            this.fGain = fGain;

        


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


    }
}

