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
    public class AudioMuteEvent : Event
    {
        public enum Type { mute, unmute }
        public Type muteType { get{return _muteType;} set{_muteType = value;} }

        private Type _muteType;



        public AudioMuteEvent(Rectangle rectangle)
        {
            this.rectangle = rectangle;
            position = rectangle.Location.ToVector2();
            width = rectangle.Width;
            height = rectangle.Height;
            list = new List<LevelObject>();
            isActivated = true;

            this.muteType = Type.mute;



        }



        public bool OnCollision(Fixture a, Fixture b, Contact contact)
        {
            if (isActivated)
            {
                foreach (SoundObject so in this.list)
                {
                    switch (muteType)
                    {
                        case (Type.mute):
                            so.mute = true;  
                            break;
                        case (Type.unmute):
                            so.mute = false;
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

