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
    public class AnimationEvent
    {
        // Hannes: Die ganze Logik haben wir eigentlich schon in Deco, daher sieht man hier nicht viel.
        public Deco deco;

        public AnimationEvent() { }
        public AnimationEvent(Vector2 position, int amount, String path, float speed)
        {
            this.deco = new Deco(position, amount, path, speed);
        }
        public AnimationEvent(Deco deco)
        {
            this.deco = deco;
        }
    }
}
