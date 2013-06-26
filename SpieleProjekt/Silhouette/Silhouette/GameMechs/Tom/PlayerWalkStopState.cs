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

namespace Silhouette.GameMechs
{
    public class PlayerWalkStopState : PlayerState
    {
        public PlayerWalkStopState(Tom tom, Animation animationLeft, Animation animationRight)
            : base(tom, animationLeft, animationRight)
        {
            animationLeft.Handler = new Animation.OnFinish(SwitchToIdleAnimation);
            animationRight.Handler = new Animation.OnFinish(SwitchToIdleAnimation);
        }

        public override void sOnSeperation(Fixture fixtureA, Fixture fixtureB)
        {
            if (tom.CharFix.Body.LinearVelocity.Y >= PlayerFallingState.FALLINGTHRESHOLD)
            {
                tom.State = tom.FallingState;
            }
        }

        public override void handleInput(KeyboardState cbs, KeyboardState oks)
        {
            if (cbs.IsKeyDown(Keys.Space) && oks.IsKeyUp(Keys.Space))
            {
                tom.State = tom.JumpState;
            }
            else if (cbs.IsKeyDown(Keys.A) && cbs.IsKeyDown(Keys.W))
            {
            }
            else if (cbs.IsKeyDown(Keys.D))
            {
                if (tom.Facing != Tom.FacingState.Left)
                {
                    tom.State = tom.WalkState;
                }
            }
            else if (cbs.IsKeyDown(Keys.A))
            {
                if (tom.Facing != Tom.FacingState.Right)
                {
                    tom.State = tom.WalkState;
                }
            }
        }

        private void SwitchToIdleAnimation()
        {
            tom.State = tom.IdleState;
        }

        public override void onSet(Tom.FacingState facing)
        {
            base.onSet(facing);
            tom.CharFix.Friction = 3;
        }
        public override void onUnset()
        {
            tom.CharFix.Friction = 1;
        }
    }
}
