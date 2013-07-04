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
    public class PlayerIdleState : PlayerState
    {
        public PlayerIdleState(Tom tom, Animation animationLeft, Animation animationRight)
            : base(tom, animationLeft, animationRight)
        {
        }

        public override void handleInput(KeyboardState cbs, KeyboardState oks)
        {
            if (cbs.IsKeyDown(Keys.Space) && oks.IsKeyUp(Keys.Space) )
            {
                tom.State = tom.JumpState;
            }
            else if (cbs.IsKeyDown(Keys.D))
            {
                tom.Facing = Tom.FacingState.Right;
                tom.State = tom.WalkState;
            }
            else if (cbs.IsKeyDown(Keys.A))
            {
                tom.Facing = Tom.FacingState.Left;
                tom.State = tom.WalkState;
            }
            else if (cbs.IsKeyDown(Keys.W))
            {
                if (tom.CanClimb)
                {
                    tom.State = tom.ClimbingState;
                }
            }
        }
    }
}
