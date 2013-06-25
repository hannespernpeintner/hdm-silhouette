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
using Silhouette.GameMechs;

namespace Silhouette.GameMechs
{
    public class PlayerWalkState : PlayerState
    {
        public PlayerWalkState(Tom tom, Animation animationLeft, Animation animationRight)
            : base(tom, animationLeft, animationRight)
        {
        }
        public override void handleInput(KeyboardState cbs, KeyboardState oks)
        {
            if (cbs.IsKeyDown(Keys.Space) && oks.IsKeyUp(Keys.Space))
            {
                tom.State = tom.JumpState;
            }
            else if (cbs.IsKeyDown(Keys.D))
            {
                if (tom.Facing != Tom.FacingState.Right)
                {
                    tom.State = tom.WalkStartState;
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
                    tom.State = tom.WalkStartState;
                }
                else
                {
                    tom.MoveLeft();
                }
            }
            else if (cbs.IsKeyUp(Keys.A) && cbs.IsKeyUp(Keys.A))
            {
                tom.State = tom.WalkStopState;
            }
        }
    }
}
