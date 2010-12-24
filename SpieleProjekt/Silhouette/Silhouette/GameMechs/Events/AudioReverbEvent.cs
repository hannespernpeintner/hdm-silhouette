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
    public class AudioReverbEvent : Event
    {
        public enum Type { Enable, Disable, Update }

        private Type _reverbType;
        [DisplayName("Action"), Category("Event Data")]
        [Description("Defines what this event should do to the reverb effect")]
        public Type ReverbType { get { return _reverbType; } set { _reverbType = value; } }



        [DisplayName("Reverb input gain"), Category("Event Data")]
        [Description("Input gain of signal, in decibels (dB). Min/Max: [-96.0,0.0]")]
        public float RevInGain { get { return _RevInGain; } set { _RevInGain = value; } }
        private float _RevInGain;

        [DisplayName("Reverb mix"), Category("Event Data")]
        [Description("Reverb mix, in dB. Min/Max: [-96.0,0.0]")]
        public float RevfReverbMix { get { return _RevfReverbMix; } set { _RevfReverbMix = value; } }
        private float _RevfReverbMix;

        [DisplayName("Reverb time"), Category("Event Data")]
        [Description("Reverb time, in milliseconds. Min/Max: [0.001,3000.0]")]
        public float RevfReverbTime { get { return _RevfReverbTime; } set { _RevfReverbTime = value; } }
        private float _RevfReverbTime;

        [DisplayName("Reverb HF reverb/time ratio"), Category("Event Data")]
        [Description("High-frequency reverb time ratio. Min/Max: [0.001,0.999]")]
        public float RevfHighFreqRTRatio { get { return _RevfHighFreqRTRatio; } set { _RevfHighFreqRTRatio = value; } }
        private float _RevfHighFreqRTRatio;


        public AudioReverbEvent(Rectangle rectangle)
        {
            this.rectangle = rectangle;
            position = rectangle.Location.ToVector2();
            width = rectangle.Width;
            height = rectangle.Height;
            list = new List<LevelObject>();
            isActivated = true;

            this._reverbType = Type.Disable;
        }

        public bool OnCollision(Fixture a, Fixture b, Contact contact)
        {
            if (isActivated)
            {
                foreach (SoundObject so in this.list)
                {
                    switch (ReverbType)
                    {
                        case (Type.Enable):
                            so.EnableReverb(_RevInGain, _RevfReverbMix, _RevfReverbTime, _RevfHighFreqRTRatio);
                            break;
                        case (Type.Update):
                            
                            so.EnableReverb(_RevInGain, _RevfReverbMix, _RevfReverbTime, _RevfHighFreqRTRatio);
                            break;
                        case (Type.Disable):
                            so.DisableReverb();
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
            return "AudioMuteEvent_";
        }

        public override LevelObject clone()
        {
            AudioReverbEvent result = (AudioReverbEvent)this.MemberwiseClone();
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

