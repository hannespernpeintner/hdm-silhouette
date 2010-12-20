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
        public enum Type {FadeUp, FadeDown}
        public  Type fadeType { get; set; }

        public float fadeTime { get; set; }
        public float gainOrLoss { get; set; }
        

        public AudioFadeEvent(Rectangle rectangle)
        {
            this.rectangle = rectangle;
            position = rectangle.Location.ToVector2();
            width = rectangle.Width;
            height = rectangle.Height;
            list = new List<LevelObject>();
            isActivated = true;

            this.fadeType = 0;
            this.fadeTime = 0;
            gainOrLoss = 0;
            


        }

       

        public bool OnCollision(Fixture a, Fixture b, Contact contact)
        {
            if (isActivated)
            {
                foreach (SoundObject so in this.list)
                {
                    if (gainOrLoss != 0)
                    {
                        if (fadeType == Type.FadeDown)
                        {
                            so.fadeDown(fadeTime, gainOrLoss);
                        }
                        else
                        {
                            so.fadeUp(fadeTime, gainOrLoss);
                        }
                    }
                    else
                    {
                        if (fadeType == Type.FadeDown)
                        {
                            so.fadeDown(fadeTime);
                        }
                        else
                        {
                            so.fadeUp(fadeTime);
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


        public void AddLevelObject(SoundObject so)
        {

            if (this.list != null)
            {
                if (!this.list.Contains(so))
                    this.list.Add(so);
            }
        }

        public override string getPrefix()
        {
            return "AudioFadeEvent_";
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
            throw new NotImplementedException();
        }
    }
}

