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
    public class AudioCrossfaderEvent : Event
    {

        private LevelObject _Sound2;
        [DisplayName("Channel 2 AudioObject"), Category("Event Data")]
        [Description("Audio Object which will be faded up. This one shall always be quieter than the SoundObject which has been added to the Event")]
        public LevelObject Sound2 { get { return _Sound2; } set { _Sound2 = value; } }
        
        private float _fadeTime;
        [DisplayName("Fade Time"), Category("Event Data")]
        [Description("Defines the time needed till the fade is complete.")]
        public float fadeTime { get { return _fadeTime; } set { _fadeTime = value; } }

        private float _Channel1Loss;
        [DisplayName("Channel 1 Loss"), Category("Event Data")]
        [Description("Decreases the volume by a factor of this Value. E.g. : The current volume is 0.5, selecting a loss of 0.5 will fade the volume to 0")]
        public float Channel1Loss { get { return _Channel1Loss; } set { _Channel1Loss = value; } }

        private float _Channel2Gain;
        [DisplayName("Channel 2 Gain"), Category("Event Data")]
        [Description("Increases the volume by a factor of this Value. E.g. : The current volume is 0.5, selecting a gain of 0.5 will fade the volume to 1.0")]
        public float Channel2Gain { get { return _Channel2Gain; } set { _Channel2Gain = value; } }



        public AudioCrossfaderEvent(Rectangle rectangle)
        {
            this.rectangle = rectangle;
            position = rectangle.Location.ToVector2();
            width = rectangle.Width;
            height = rectangle.Height;
            list = new List<LevelObject>();
            isActivated = true;

           
            _fadeTime = 0;
            _Channel1Loss = 0;
        }

        public bool OnCollision(Fixture a, Fixture b, Contact contact)
        {
            if (isActivated && b.isPlayer == true)
            {
                foreach (SoundObject so in this.list)
                {
                    if ((Sound2 is SoundObject)  )
                    { 
                        SoundObject so2 = (SoundObject) Sound2;
                        if ( so2.volume < so.volume)
                            so.Crossfade(so2, _Channel2Gain, _Channel1Loss, _fadeTime);
                        else
                            so2.Crossfade(so, _Channel2Gain, _Channel1Loss, _fadeTime);
                    }
                }
                //isActivated = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void AddLevelObject(LevelObject lo)
        {

            if ((this.list != null) && (lo is SoundObject))
            {
                if (!this.list.Contains(lo))
                    this.list.Add(lo);
            }
        }

        public override string getPrefix()
        {
            return "AudioCrossfaderEvent_";
        }

        public override LevelObject clone()
        {
            AudioCrossfaderEvent result = (AudioCrossfaderEvent)this.MemberwiseClone();
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
