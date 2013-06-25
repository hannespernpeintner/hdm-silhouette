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
    public abstract class PlayerState : DrawableLevelObject
    {
        protected Animation animationLeft;
        protected Animation animationRight;
        protected Tom tom;

        public PlayerState(Tom tom, Animation animationLeft, Animation animationRight)
        {
            this.tom = tom;
            this.animationLeft = animationLeft;
            this.animationRight = animationRight;
        }
        public virtual void handleInput(KeyboardState cbs, KeyboardState oks){}
        public virtual void onUnset() { }
        public virtual void onSet(Tom.FacingState facing)
        {
            if (facing == Tom.FacingState.Left)
            {
                animationLeft.stop();
                animationLeft.start();
            }
            else 
            {
                animationRight.stop();
                animationRight.start();
            }
        }
        public override void Update(GameTime gt)
        {
            if (tom.Facing == Tom.FacingState.Left)
            {
                animationLeft.Update(gt);
            }
            else
            {
                animationRight.Update(gt);
            }
        }
        public override void Draw(SpriteBatch batch)
        {
            if (tom.Facing == Tom.FacingState.Left)
            {
                batch.Draw(animationLeft.activeTexture, tom.position, null, Color.White, tom.CharFix.Body.Rotation, new Vector2(250, 250), 1, SpriteEffects.None, 1);
            }
            else
            {
                batch.Draw(animationRight.activeTexture, tom.position, null, Color.White, tom.CharFix.Body.Rotation, new Vector2(250, 250), 1, SpriteEffects.None, 1);
            }
        }
        public override void LoadContent()
        {
        }
        public override void Initialise() { }

        public virtual bool OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return true;
        }

        public virtual void OnSeperation(Fixture fixtureA, Fixture fixtureB)
        {

        }

        public virtual bool landOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return true;
        }

        public virtual bool nOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return true;

        }

        public virtual bool sOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {

            return true;
        }

        public virtual bool ewOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {

            return true;
        }

        public virtual void ewOnSeparation(Fixture fixtureA, Fixture fixtureB)
        {

        }

    }
}
