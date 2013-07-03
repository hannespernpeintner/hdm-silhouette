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

using Silhouette.Engine;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Controllers;
using FarseerPhysics.Collision;

namespace Silhouette.GameMechs
{
    public class PlayerFallingState : PlayerState
    {
        public static float FALLINGTHRESHOLD = 2.0f;

        public PlayerFallingState(Tom tom, Animation animationLeft, Animation animationRight) 
            : base(tom, animationLeft, animationRight)
        {
        }

        public override bool sOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (!fixtureB.IsSensor)
            {
                tom.State = tom.LandingFastState;
            }
            return true;
        }

        public override bool landOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (!fixtureB.IsSensor)
            {
                tom.State = tom.LandingState;
            }
            return true;
        }

        public override void handleInput(KeyboardState cbs, KeyboardState oks)
        {
    
            if (cbs.IsKeyDown(Keys.H))
            {
                tom.State = tom.HangingState;
            }
        }
    }
}
