using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

//Physik-Engine Klassen
using FarseerPhysics.Dynamics;

using Silhouette.Engine.Manager;
using Silhouette.Engine;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Dynamics.Contacts;

namespace Silhouette.GameMechs
{
    public class Player : DrawableLevelObject
    {

        public Vector2 centerPosition;      // Hannes: Rectangles für Kollisionserkennung. Muss noch um Kollisionsgruppe erweitert werden.
        public Fixture charRect;
        public Fixture nRect;
        public Fixture eRect;
        public Fixture sRect;
        public Fixture wRect;

        public KeyboardState oldState;
        public Vector2 oldPosition;
        public Vector2 movement;    // Bewegung des Characters. Wichtig zur Statusveränderungserkennung.

        private bool isRunning;    // Hannes: Statusveränderungen des Chars. Weitere folgen.
        private bool isJumping;
        private bool isFalling;
        private bool isIdle;
        private bool isScriptedMoving;
        private bool isSuperMoving;
        private bool isDying;

        private int facing;                    // Wo der Chara hinschaut. 0 bedeutet links, 1 bedeutet rechts.

        private String actScriptedMove;

        private bool canClimb;      // Das hier wird vom Event auf true gesetzt, wenn Tom klettern können soll
        private int maxClimbHeight;
        private int actClimbHeight;

        private Animation activeAnimation;     // Die aktive Sprite, die in der UpdateMethode auch geupdatet wird.
        private Animation nextAnimation;        // Die nächste Animation, die gespielt wird, sobald die aktuelle abgelaufen is oder unterbrochen wird.

        private Animation running_left;             // Hier kommen alle Animationen hin, die Tom hat.
        private Animation running_right;
        private Animation jumping_left;
        private Animation jumping_right;
        private Animation falling_left;
        private Animation falling_right;
        private Animation idle_left;
        private Animation idle_right;
        private Animation idleb_left;
        private Animation idleb_right;
        private Animation idlec_left;
        private Animation idlec_right;
        private Animation climbing_left;
        private Animation climbing_right;
        private Animation dying_left;
        private Animation dying_right;

        public override void Initialise()
        {
            // Aus dem Level geht die initiale StartPos des Chars hervor. Aktuell Testposition eingetragen.
            // position = Level.LevelSetting.CharacterStartPosition;
            // position = new Vector2(100, 100);

            idle_left = new Animation();
            idle_right = new Animation();
            idleb_left = new Animation();
            idleb_right = new Animation();
            idlec_left = new Animation();
            idlec_right = new Animation();
            jumping_left = new Animation();
            jumping_right = new Animation();
            running_left = new Animation();
            running_right = new Animation();
            falling_left = new Animation();
            falling_right = new Animation();
            climbing_left = new Animation();
            climbing_right = new Animation();
            dying_left = new Animation();
            dying_right = new Animation();

            movement = Vector2.Zero;

            isRunning = false;
            isJumping = false;
            isFalling = false;
            isIdle = true;
            isDying = false;
            isScriptedMoving = false;
            isSuperMoving = false;
            canClimb = false;
            maxClimbHeight = 825;

            facing = 1;
        }

        public override void LoadContent()
        {

            // Hier müssen alle Sprites geladen werden.
            idle_left.Load(6, "Sprites/Player/idleA_left_", 1.0f, true);
            idle_right.Load(6, "Sprites/Player/idleA_right_", 1.0f, true);
            idleb_left.Load(6, "Sprites/Player/idleB_left_", 1.0f, true);
            idleb_right.Load(6, "Sprites/Player/idleB_right_", 1.0f, true);
            idlec_left.Load(6, "Sprites/Player/idleC_left_", 1.0f, true);
            idlec_right.Load(6, "Sprites/Player/idleC_right_", 1.0f, true);
            jumping_left.Load(10, "Sprites/Player/jump_left_", 1.0f, true);
            jumping_right.Load(10, "Sprites/Player/jump_right_", 1.0f, true);
            running_left.Load(9, "Sprites/Player/walk_left_", 1.5f, true);
            running_right.Load(9, "Sprites/Player/walk_right_", 1.5f, true);
            climbing_left.Load(11, "Sprites/Player/climb_left_", 0.75f, true);
            climbing_right.Load(11, "Sprites/Player/climb_right_", 0.75f, true);

            activeAnimation = choseIdleAnimation();
            activeAnimation.start();
            nextAnimation = choseIdleAnimation();
            charRect = FixtureManager.CreateRectangle(180, 140, position, BodyType.Dynamic, 1);
            nRect = FixtureManager.CreateRectangle(140, 20, position, BodyType.Dynamic, 0);
            eRect = FixtureManager.CreateRectangle(20, 90, position, BodyType.Dynamic, 0);
            sRect = FixtureManager.CreateRectangle(140, 20, position, BodyType.Dynamic, 0);
            wRect = FixtureManager.CreateRectangle(20, 90, position, BodyType.Dynamic, 0);

            nRect.IgnoreCollisionWith(charRect);
            nRect.IgnoreCollisionWith(eRect);
            nRect.IgnoreCollisionWith(sRect);
            nRect.IgnoreCollisionWith(sRect);
            eRect.IgnoreCollisionWith(charRect);
            eRect.IgnoreCollisionWith(nRect);
            eRect.IgnoreCollisionWith(sRect);
            eRect.IgnoreCollisionWith(wRect);
            sRect.IgnoreCollisionWith(charRect);
            sRect.IgnoreCollisionWith(nRect);
            sRect.IgnoreCollisionWith(eRect);
            sRect.IgnoreCollisionWith(wRect);
            wRect.IgnoreCollisionWith(charRect);
            wRect.IgnoreCollisionWith(nRect);
            wRect.IgnoreCollisionWith(eRect);
            wRect.IgnoreCollisionWith(sRect);

            Joint joint0 = JointFactory.CreateWeldJoint(Level.Physics, charRect.Body, nRect.Body, new Vector2(0, -70 / Level.PixelPerMeter), Vector2.Zero);
            Joint joint1 = JointFactory.CreateWeldJoint(Level.Physics, charRect.Body, eRect.Body, new Vector2(90 / Level.PixelPerMeter, 0), Vector2.Zero);
            Joint joint2 = JointFactory.CreateWeldJoint(Level.Physics, charRect.Body, sRect.Body, new Vector2(0, 70 / Level.PixelPerMeter), Vector2.Zero);
            Joint joint3 = JointFactory.CreateWeldJoint(Level.Physics, charRect.Body, wRect.Body, new Vector2(-90 / Level.PixelPerMeter, 0), Vector2.Zero);

            sRect.OnCollision += sOnCollision;
            //sRect.OnSeparation += sOnSeperation;
        }

        public override void Update(GameTime gameTime)
        {
            if (!isDying && !isScriptedMoving)
            {
                ObserveMovement();
            }
            UpdateNextAnimation();
            UpdatePositions();
            if (!isDying && !isScriptedMoving)
            {
                UpdateControls(gameTime);
            }
            if (isScriptedMoving)
            {
                doScriptedMove();
            }
            UpdateTexture(gameTime);
        }

        public void UpdatePositions()
        {
            centerPosition = new Vector2(charRect.Body.Position.X * Level.PixelPerMeter, charRect.Body.Position.Y * Level.PixelPerMeter);
            position = new Vector2(centerPosition.X - 250, centerPosition.Y - 250);
        }

        private void UpdateControls(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && !isFalling && !isJumping)
            {
                if (facing == 1)
                {
                    facing = 0;
                    if (isIdle)
                    {
                        choseIdleAnimation();
                    }
                }

                {
                    isRunning = true;
                    activeAnimation = running_left;
                    activeAnimation.start();
                    nextAnimation = choseIdleAnimation();
                    charRect.Body.ApplyForce(new Vector2(-20, 0));
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right) && !isFalling && !isJumping)
            {
                if (facing == 0)
                {
                    facing = 1;
                    if (isIdle)
                    {
                        choseIdleAnimation();
                    }
                }

                {
                    isRunning = true;
                    activeAnimation = running_right;
                    activeAnimation.start();
                    nextAnimation = choseIdleAnimation();
                    charRect.Body.ApplyForce(new Vector2(20, 0));
                }
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Right) && oldState.IsKeyDown(Keys.D))
            {
                isRunning = false;
                activeAnimation = choseIdleAnimation();
                activeAnimation.start();
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Left) && oldState.IsKeyDown(Keys.A))
            {
                isRunning = false;
                activeAnimation = choseIdleAnimation();
                activeAnimation.start();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up) && !isScriptedMoving)
            {
                climb();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space) && (isIdle || isRunning))
            {
                if (facing == 0)
                {
                    activeAnimation = jumping_left;
                    activeAnimation.activeFrameNumber = 0;
                    activeAnimation.start();
                    isJumping = true;
                    isIdle = false;
                    isFalling = false;
                    isRunning = false;
                    charRect.Body.ApplyForce(new Vector2(-1000, -1500));
                }
                else if (facing == 1)
                {
                    activeAnimation = jumping_right;
                    activeAnimation.activeFrameNumber = 0;
                    activeAnimation.start();
                    isJumping = true;
                    isIdle = false;
                    isFalling = false;
                    isRunning = false;
                    charRect.Body.ApplyForce(new Vector2(1000, -1500));
                }
            }

            oldState = Keyboard.GetState();
            oldPosition = charRect.Body.Position;
        }

        private void doScriptedMove()
        {
            if (actScriptedMove.Equals("climb") && isScriptedMoving)
            {
                climb();
            }
        }

        private void climb()
        {
            if (!isScriptedMoving)
            {
                actScriptedMove = "climb";
                isScriptedMoving = true;
                isIdle = false;
                isRunning = false;
                isJumping = false;
                isDying = false;
                isFalling = false;
                isSuperMoving = false;

                charRect.Body.BodyType = BodyType.Static;
                nRect.Body.BodyType = BodyType.Static;
                eRect.Body.BodyType = BodyType.Static;
                sRect.Body.BodyType = BodyType.Static;
                wRect.Body.BodyType = BodyType.Static;
                charRect.Body.IgnoreGravity = true;
                nRect.Body.IgnoreGravity = true;
                eRect.Body.IgnoreGravity = true;
                sRect.Body.IgnoreGravity = true;
                wRect.Body.IgnoreGravity = true;

                if (facing == 0)
                {
                    activeAnimation = climbing_left;
                    activeAnimation.activeFrameNumber = 0;
                    activeAnimation.start();
                    UpdateNextAnimation();
                }
                else
                {
                    activeAnimation = climbing_right;
                    activeAnimation.activeFrameNumber = 0;
                    activeAnimation.start();
                    UpdateNextAnimation();
                }
            }
            // Hier wird noch gebessert
            if (actClimbHeight < maxClimbHeight)
            {
                int temp = activeAnimation.activeFrameNumber;
                actClimbHeight = temp;
                charRect.Body.Position += new Vector2(1.5f / Level.PixelPerMeter, -2 / Level.PixelPerMeter);
                nRect.Body.Position += new Vector2(1.5f / Level.PixelPerMeter, -2 / Level.PixelPerMeter);
                eRect.Body.Position += new Vector2(1.5f / Level.PixelPerMeter, -2 / Level.PixelPerMeter);
                sRect.Body.Position += new Vector2(1.5f / Level.PixelPerMeter, -2 / Level.PixelPerMeter);
                wRect.Body.Position += new Vector2(1.5f / Level.PixelPerMeter, -2 / Level.PixelPerMeter);
            }


            if (activeAnimation.activeFrameNumber == activeAnimation.amount - 1)
            {
                activeAnimation.activeFrameNumber = 0;
                activeAnimation = nextAnimation;
                actClimbHeight = 0;
                actScriptedMove = "";
                charRect.Body.BodyType = BodyType.Dynamic;
                nRect.Body.BodyType = BodyType.Dynamic;
                eRect.Body.BodyType = BodyType.Dynamic;
                sRect.Body.BodyType = BodyType.Dynamic;
                wRect.Body.BodyType = BodyType.Dynamic;
                charRect.Body.IgnoreGravity = false;
                isScriptedMoving = false;
                isIdle = true;
                isRunning = false;
                isFalling = false;
                isJumping = false;
                isDying = false;
            }

        }

        private void UpdateTexture(GameTime gameTime)
        {
            activeAnimation.Update(gameTime, position);
        }

        public Animation choseIdleAnimation()
        {
            // Methode dient um zufällig zw. Idle-Animationen zu wechseln.
            double random = new Random().NextDouble();

            if (facing == 0)
            {
                if (random < 0.1)
                {
                    return idleb_left;
                }
                if (random > 0.1 && random < 0.2)
                {
                    return idlec_left;
                }
                else return idle_left;
            }
            else
            {
                if (random < 0.1)
                {
                    return idleb_right;
                }
                if (random > 0.1 && random < 0.2)
                {
                    return idlec_right;
                }
                else return idle_right;
            }
        }

        public void UpdateNextAnimation()
        {
            if (nextAnimation == null)
            {
                nextAnimation = choseIdleAnimation();
            }

            if (activeAnimation == null)
            {
                activeAnimation = choseIdleAnimation();
                activeAnimation.start();
            }

            if (activeAnimation.activeFrameNumber == activeAnimation.amount - 1 && isJumping)
            {
                activeAnimation = nextAnimation;
                activeAnimation.activeFrameNumber = 0;
                activeAnimation.start();
                nextAnimation = null;
            }

            if (activeAnimation.activeFrameNumber == activeAnimation.amount - 1 && isIdle)
            {
                activeAnimation = nextAnimation;
                activeAnimation.activeFrameNumber = 0;
                activeAnimation.start();
                nextAnimation = choseIdleAnimation();
            }
        }

        public void ObserveMovement()
        {
            movement = charRect.Body.GetLinearVelocityFromWorldPoint(Vector2.Zero);

            if (movement.Y + Level.Physics.Gravity.Y < 0)
            {
                isIdle = false;
                isFalling = false;
                isRunning = false;
                isJumping = true;
            }

            if (isIdle)
            {
                isRunning = false;
            }

            if (isJumping && oldPosition.Y < charRect.Body.Position.Y)
            {
                isIdle = false;
                isFalling = true;
                isRunning = false;
                isJumping = false;
            }

            if (movement.Y > Level.Physics.Gravity.Y)
            {
                isIdle = false;
                isFalling = true;
                isRunning = false;
                isJumping = false;
            }

            if (facing == 0)
            {
                if (activeAnimation == idle_right)
                {
                    activeAnimation = idle_left;
                }

                if (activeAnimation == idleb_right)
                {
                    activeAnimation = idleb_left;
                }

                if (activeAnimation == idlec_right)
                {
                    activeAnimation = idlec_left;
                }
            }

            if (facing == 1)
            {
                if (activeAnimation == idle_left)
                {
                    activeAnimation = idle_right;
                }

                if (activeAnimation == idleb_left)
                {
                    activeAnimation = idleb_right;
                }

                if (activeAnimation == idlec_left)
                {
                    activeAnimation = idlec_right;
                }
            }
        }

        public bool sOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {

            isJumping = false;
            isFalling = false;
            isIdle = true;

            activeAnimation = choseIdleAnimation();
            activeAnimation.activeFrameNumber = 0;
            nextAnimation = null;

            return true;
        }

        public void sOnSeperation(Fixture fixtureA, Fixture fixtureB)
        {
            if (facing == 0) activeAnimation = jumping_left;
            activeAnimation.activeFrameNumber = 0;
            if (facing == 1) activeAnimation = jumping_right;
            activeAnimation.activeFrameNumber = 0;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            activeAnimation.Draw(spriteBatch);
            //Das auskommentierte hier kann als Debugview dienen.
            /*spriteBatch.DrawString(FontManager.Arial, "Standing: " + isIdle.ToString(), new Vector2(300, 20), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "Running: " + isRunning.ToString(), new Vector2(300, 45), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "Jumping: " + isJumping.ToString(), new Vector2(300, 70), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "Falling: " + isFalling.ToString(), new Vector2(300, 95), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, charRect.Body.Position.ToString(), new Vector2(300, 120), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "Scriptedmoving: " + isScriptedMoving, new Vector2(300, 155), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, actClimbHeight.ToString(), new Vector2(300, 180), Color.Black);*/
        }
    }
}
