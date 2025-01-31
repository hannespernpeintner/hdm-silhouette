﻿using System;
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
    public class AudioFadeEvent : Event
    {
        public enum Type { FadeUp, FadeDown }

        private Type _fadeType;
        [DisplayName("Fade"), Category("Event Data")]
        [Description("Defines if the event fades up or fades down the SoundObjects in the list.")]
        public Type fadeType { get { return _fadeType; } set { _fadeType = value; } }

        private float _fadeTime;
        [DisplayName("Fade Time"), Category("Event Data")]
        [Description("Defines the time needed till the fade is complete.")]
        public float fadeTime { get { return _fadeTime; } set { _fadeTime = value; } }

        private float _gainOrLoss;
        [DisplayName("Signal Gain / Loss"), Category("Event Data")]
        [Description("Increases or decreases the volume by a factor of this Value. E.g. : The current volume is 0.5, the selected FadeType is FadeDown. Selecting a loss of 0.5 will fade the volume to 0")]
        public float gainOrLoss { get { return _gainOrLoss; } set { _gainOrLoss = value; } }

        public AudioFadeEvent(Rectangle rectangle)
        {
            this.rectangle = rectangle;
            position = rectangle.Location.ToVector2();
            width = rectangle.Width;
            height = rectangle.Height;
            list = new List<LevelObject>();
            isActivated = true;
            OnlyOnPlayerCollision = true;

            _fadeType = 0;
            _fadeTime = 0;
            _gainOrLoss = 0;
        }

        public bool OnCollision(Fixture a, Fixture b, Contact contact)
        {
            if (isActivated && ((OnlyOnPlayerCollision && b.isPlayer) || !OnlyOnPlayerCollision))
            {
                foreach (SoundObject so in this.list)
                {
                    if (_gainOrLoss != 0)
                    {
                        if (fadeType == Type.FadeDown)
                        {
                            so.fadeDown(_fadeTime, _gainOrLoss);
                        }
                        else
                        {
                            so.fadeUp(_fadeTime, _gainOrLoss);
                        }
                    }
                    else
                    {
                        if (fadeType == Type.FadeDown)
                        {
                            so.fadeDown(_fadeTime);
                        }
                        else
                        {
                            so.fadeUp(_fadeTime);
                        }
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
            return "AudioFadeEvent_";
        }

        public override LevelObject clone()
        {
            AudioFadeEvent result = (AudioFadeEvent)this.MemberwiseClone();
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
