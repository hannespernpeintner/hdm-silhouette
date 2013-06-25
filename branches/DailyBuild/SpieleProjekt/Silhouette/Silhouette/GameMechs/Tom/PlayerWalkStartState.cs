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

namespace Silhouette.GameMechs
{
    public class PlayerWalkStartState : PlayerState
    {
        public PlayerWalkStartState(Tom tom, Animation animationLeft, Animation animationRight)
            : base(tom, animationLeft, animationRight)
        {
            animationLeft.Handler = new Animation.OnFinish(SwitchToWalkAnimation);
            animationRight.Handler = new Animation.OnFinish(SwitchToWalkAnimation);
        }
        public override void handleInput(KeyboardState cbs, KeyboardState oks)
        {
            if (cbs.IsKeyDown(Keys.Space) && oks.IsKeyUp(Keys.Space))
            {
                tom.State = tom.JumpState;
            }
            else if (cbs.IsKeyDown(Keys.A) && cbs.IsKeyDown(Keys.D))
            {
                tom.State = tom.WalkStopState;
            }
            else if (cbs.IsKeyDown(Keys.D))
            {
                if (tom.Facing != Tom.FacingState.Right)
                {
                    tom.State = tom.WalkStopState;
                }
                else
                {
                    tom.MoveRight();
                }
            }
            else if (cbs.IsKeyDown(Keys.A))
            {
                if (tom.Facing != Tom.FacingState.Left)
                {
                    tom.State = tom.WalkStopState;
                }
                else
                {
                    tom.MoveLeft();
                }
            }
            else if (cbs.IsKeyUp(Keys.A) && cbs.IsKeyUp(Keys.D))
            {
                tom.State = tom.WalkStopState;
            }
        }

        private void SwitchToWalkAnimation()
        {
            tom.State = tom.WalkState;
        }
    }
}
