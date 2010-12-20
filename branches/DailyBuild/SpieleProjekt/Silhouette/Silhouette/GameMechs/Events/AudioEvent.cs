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
    public abstract class AudioEvent : Event
    {


       
        public abstract bool OnCollision(Fixture a, Fixture b, Contact contact);
       

        public override void AddLevelObject(LevelObject lo) {

            if (this.list != null)
            {
                if (!this.list.Contains(lo))
                    this.list.Add(lo);
            }
        }



        public override string getPrefix();

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

