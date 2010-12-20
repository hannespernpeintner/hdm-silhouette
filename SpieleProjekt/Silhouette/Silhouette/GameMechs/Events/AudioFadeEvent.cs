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
    public class AudioFadeEvent : AudioEvent
    {
        public enum Type {FadeUp, FadeDown}
        public  Type fadeType { get; set; }

        public float fadeTime { get; set; }
        public float gainOrLoss { get; set; }
        

        public AudioFadeEvent(Rectangle rectangle, Type fadeType, float fadeTime)
        {
            this.rectangle = rectangle;
            position = rectangle.Location.ToVector2();
            width = rectangle.Width;
            height = rectangle.Height;
            list = new List<LevelObject>();
            isActivated = true;

            this.fadeType = fadeType;
            this.fadeTime = fadeTime;
            gainOrLoss = 0;
            


        }

        public AudioFadeEvent(Rectangle rectangle, Type fadeType, float fadeTime, float gainOrLoss)
        {
            this.rectangle = rectangle;
            position = rectangle.Location.ToVector2();
            width = rectangle.Width;
            height = rectangle.Height;
            list = new List<LevelObject>();
            isActivated = true;

            this.fadeType = fadeType;
            this.fadeTime = fadeTime;
            this.gainOrLoss = gainOrLoss;


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



        public override string getPrefix()
        {
            return "AudioFadeEvent_";
        }


    }
}

