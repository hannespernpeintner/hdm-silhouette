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
    public class SaveStateEvent : Event
    {
        public SaveStateEvent(Rectangle rectangle)
        {
            this.rectangle = rectangle;
            position = rectangle.Location.ToVector2();
            width = rectangle.Width;
            height = rectangle.Height;
            list = new List<LevelObject>();
            isActivated = true;
            OnlyOnPlayerCollision = true;
        }

        public override string getPrefix()
        {
            return "SaveState_";
        }

        public override LevelObject clone()
        {
            SaveStateEvent result = (SaveStateEvent)this.MemberwiseClone();
            result.mouseOn = false;
            return result;
        }

        public bool OnCollision(Fixture a, Fixture b, Contact contact)
        {
            if (b.isEvent)
            {
                if (isActivated)
                {
                    GameStateManager.Default.currentLevel.startPosition = this.position;
                }
            }

            return true;
        }

        public override void AddLevelObject(LevelObject lo) { }

        public override void ToFixture()
        {
            fixture = FixtureManager.CreateRectangle(width, height, position, BodyType.Static, 1);
            fixture.OnCollision += this.OnCollision;
            fixture.IsSensor = true;
        }
    }
}
