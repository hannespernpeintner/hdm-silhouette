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
    public class AudioModifyPlayback : AudioEvent
    {
        public enum Type {play, stop, pause }
        public Type EventType { get; set; }
        public Boolean looped { get; set; }

        public  AudioModifyPlayback(Rectangle rectangle, Type EventType, Boolean looped)
        {
            this.rectangle = rectangle;
            position = rectangle.Location.ToVector2();
            width = rectangle.Width;
            height = rectangle.Height;
            list = new List<LevelObject>();
            isActivated = true;
            this.EventType = EventType;
            this.looped = looped;
            
        }

        public bool OnCollision(Fixture a, Fixture b, Contact contact)
        {
            if (isActivated)
            {
                foreach (SoundObject so in this.list)
                {
                   
                    switch (EventType)
                    {
                        case (Type.play):
                            so.looped = looped;
                            so.Play();
                                break;
                        case (Type.pause):
                                so.Pause = !so.Pause;
                                break;
                        case (Type.stop):
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

       
    }
}

