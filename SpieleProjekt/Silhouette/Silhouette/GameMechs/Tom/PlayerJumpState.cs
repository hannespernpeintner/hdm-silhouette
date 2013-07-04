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
    public class PlayerJumpState : PlayerState
    {
        public PlayerJumpState(Tom tom, Animation animationLeft, Animation animationRight)
            : base(tom, animationLeft, animationRight)
        {
            animationLeft.Handler = new Animation.OnFinish(SwitchToFallingAnimation);
            animationRight.Handler = new Animation.OnFinish(SwitchToFallingAnimation);
        }

        public override void handleInput(KeyboardState cbs, KeyboardState oks)
        {

        }
        public override void onSet(Tom.FacingState facing)
        {
            if (tom.Superpower == Tom.SuperpowerState.Regrets) 
            {
                tom.State = tom.JumpTryState;
            }
            base.onSet(facing);
            tom.Jump();
        }

        private void SwitchToFallingAnimation() 
        {
            tom.State = tom.FallingState;
        }
    }
}
