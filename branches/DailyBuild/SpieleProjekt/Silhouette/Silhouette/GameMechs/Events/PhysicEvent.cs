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
    public class PhysicEvent : Event
    {
        // Hannes: Aktiviert einfach bei Kollision alle in der Liste befindlichen InteractiveObjects

        private List<LevelObject> _list;
        [DisplayName("Object List"), Category("Event Data")]
        [Description("The list of Objects which are affected by the event.")]
        public List<LevelObject> list { get { return _list; } set { _list = value; } }

        public PhysicEvent(Rectangle rectangle)
        {
            this.rectangle = rectangle;
            position = rectangle.Location.ToVector2();
            width = rectangle.Width;
            height = rectangle.Height;
            list = new List<LevelObject>();
            isActivated = true;
        }

        public bool OnCollision(Fixture a, Fixture b, Contact contact)
        {
            if (isActivated)
            {
                foreach (InteractiveObject io in this.list)
                {
                    if (io.fixture.Body.BodyType == BodyType.Static)
                        io.fixture.Body.BodyType = BodyType.Dynamic;
                    else
                        io.fixture.Body.BodyType = BodyType.Static;
                }
                foreach (RectangleFixtureItem r in this.list)
                {
                    if (r.fixture.Body.BodyType == BodyType.Static)
                        r.fixture.Body.BodyType = BodyType.Dynamic;
                    else
                        r.fixture.Body.BodyType = BodyType.Static;
                }
                foreach (CircleFixtureItem c in this.list)
                {
                    if (c.fixture.Body.BodyType == BodyType.Static)
                        c.fixture.Body.BodyType = BodyType.Dynamic;
                    else
                        c.fixture.Body.BodyType = BodyType.Static;
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
            if (this.list != null)
            {
                if(!this.list.Contains(lo))
                    this.list.Add(lo);
            }
        }

        public override string getPrefix()
        {
            return "PhysicEvent_";
        }

        public override LevelObject clone()
        {
            PhysicEvent result = (PhysicEvent)this.MemberwiseClone();
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
