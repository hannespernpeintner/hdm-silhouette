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
        public enum Type { Enable, Disable, Modify }

        private Type _eqType;
        public Type eqType { get { return _eqType; } set { _eqType = value; } }

        private float _fCenter;
        public float fCenter { get { return _fCenter; } set { _fCenter = value; } }

        private float _fBandwidth;
        public float fBandwidth { get { return _fBandwidth; } set { _fBandwidth = value; } }

        private float _fGain;
        public float fGain { get { return _fGain; } set { _fGain = value; } }

        public AudioEqualizerEvent(Rectangle rectangle )
        {
            this.rectangle = rectangle;
            position = rectangle.Location.ToVector2();
            width = rectangle.Width;
            height = rectangle.Height;
            list = new List<LevelObject>();
            isActivated = true;

            _fCenter = 0.0f;
            _fBandwidth = 0.0f;
            _fGain = 0.0f;
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
                    switch (eqType)
                    {
                        case (Type.Enable):
                            so.EnableEqualizer(_fCenter, _fBandwidth, _fGain);
                            break;
                        case (Type.Modify):
                            so.EnableEqualizer(_fCenter, _fBandwidth, _fGain);
                            break;
                        case (Type.Disable):
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
            AudioEqualizerEvent result = (AudioEqualizerEvent)this.MemberwiseClone();
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

