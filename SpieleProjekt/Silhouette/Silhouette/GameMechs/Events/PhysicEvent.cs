using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Silhouette.Engine.Manager;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

namespace Silhouette.GameMechs.Events
{
    public class PhysicEvent
    {
        // Hannes: Aktiviert einfach bei Kollision alle in der Liste befindlichen InteractiveObjects
        List<InteractiveObject> list;
        RectangleFixtureItem rectFixItem;
        public bool activated;

        public PhysicEvent() { }
        public PhysicEvent(Vector2 position, int width, int height, List<InteractiveObject> list)
        {
            activated = false;
            rectFixItem.fixture = FixtureManager.CreateRectangle(width, height, position, BodyType.Static, 1);
            rectFixItem.fixture.IsSensor = true;
            rectFixItem.fixture.OnCollision += this.OnCollision;
        }

        public bool OnCollision(Fixture a, Fixture b, Contact contact)
        {
            activated = true;
            foreach (InteractiveObject io in this.list)
            {
                try { io.fixture.Body.BodyType = BodyType.Dynamic; }
                catch (Exception e)
                {
                    foreach (Fixture fix in io.fixtures)
                    {
                        fix.Body.BodyType = BodyType.Dynamic;
                    }
                }
            }
            return true;
        }

        public void addInteractiveObject(InteractiveObject io)
        {
            if (this.list != null)
            {
                this.list.Add(io);
            }
            else
            {
                this.list = new List<InteractiveObject>();
                this.list.Add(io);
            }
        }
    }
}
