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
        public Fixture sRect;

        public KeyboardState oldState;
        public Vector2 oldPosition;
        public Vector2 movement;            // Bewegung des Characters. Wichtig zur Statusveränderungserkennung.

        private bool isRunning;             // Hannes: Statusveränderungen des Chars. Weitere folgen.
        private bool isJumping;
        private bool isFalling;
        private bool isIdle;
        private bool isScriptedMoving;
        private bool isSuperMoving;
        private bool isDying;

        private int facing;                    // Wo der Chara hinschaut. 0 bedeutet links, 1 bedeutet rechts.

        private String actScriptedMove;

        private bool canClimb;
        private bool canJummp;
        private bool canSuperJump;

        private int maxClimbHeight;
        private int actClimbHeight;

        private Animation activeAnimation;      // Die aktive Sprite, die in der UpdateMethode auch geupdatet wird.
        private Animation nextAnimation;        // Die nächste Animation, die gespielt wird, sobald die aktuelle abgelaufen is oder unterbrochen wird.

        private Animation running_left;             // Hier kommen alle Animationen hin, die Tom hat.
        private Animation running_right;
        private Animation runStarting_left;
        private Animation runStarting_right;
        private Animation runStopping_left;
        private Animation runStopping_right;

        private Animation jumpStarting_left;
        private Animation jumpStarting_right;

        private Animation falling_left;
        private Animation falling_right;

        private Animation landing_left;
        private Animation landing_right;

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
            //position = Vector2.Zero;

            idle_left = new Animation();
            idle_right = new Animation();
            idleb_left = new Animation();
            idleb_right = new Animation();
            idlec_left = new Animation();
            idlec_right = new Animation();

            jumpStarting_left = new Animation();
            jumpStarting_right = new Animation();

            falling_left = new Animation();
            falling_right = new Animation();

            landing_left = new Animation();
            landing_right = new Animation();

            runStarting_left = new Animation();
            runStarting_right = new Animation();
            running_left = new Animation();
            running_right = new Animation();
            runStopping_left = new Animation();
            runStopping_right = new Animation();

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

            jumpStarting_left.Load(3, "Sprites/Player/jumpStart_left_", 1.5f, false);
            jumpStarting_right.Load(3, "Sprites/Player/jumpStart_right_", 1.5f, false);
            falling_left.Load(2, "Sprites/Player/falling_left_", 0.5f, true);
            falling_right.Load(2, "Sprites/Player/falling_right_", 0.5f, true);
            landing_left.Load(4, "Sprites/Player/landing_left_", 1.0f, false);
            landing_right.Load(4, "Sprites/Player/landing_right_", 1.0f, false);

            running_left.Load(5, "Sprites/Player/walk_left_", 2f, true);
            running_right.Load(5, "Sprites/Player/walk_right_", 2f, true);
            runStarting_left.Load(2, "Sprites/Player/walkStart_left_", 1.0f, false);
            runStarting_right.Load(2, "Sprites/Player/walkStart_right_", 1.0f, false);
            runStopping_left.Load(2, "Sprites/Player/walkStop_left_", 1.0f, false);
            runStopping_right.Load(2, "Sprites/Player/walkStop_right_", 1.0f, false);

            climbing_left.Load(11, "Sprites/Player/climb_left_", 0.75f, false);
            climbing_right.Load(11, "Sprites/Player/climb_right_", 0.75f, false);

            activeAnimation = choseIdleAnimation();
            activeAnimation.start();
            nextAnimation = choseIdleAnimation();

            charRect = FixtureManager.CreateRectangle(200, 160, position, BodyType.Dynamic, 1);
            charRect.Body.FixedRotation = true;
            charRect.Friction = 5;

            sRect = FixtureManager.CreateRectangle(100, 80, new Vector2(position.X, position.Y + 65), BodyType.Dynamic, 1);
            sRect.IsSensor = true;

            charRect.OnCollision += this.OnCollision;
            sRect.OnCollision += this.sOnCollision;
        }

        public override void Update(GameTime gameTime)
        {
            if (!isDying && !isScriptedMoving)
            {
                ObserveMovement();
            }
            if (!isDying && !isScriptedMoving)
            {
                UpdateControls(gameTime);
            }
            if (!isDying && !isScriptedMoving)
            {
                UpdateNextAnimation();
            }
            if (isScriptedMoving)
            {
                doScriptedMove();
            }
            UpdatePositions();
            UpdateTexture(gameTime);
            UpdateCamera();
        }

        public void UpdatePositions()
        {
            centerPosition = new Vector2(charRect.Body.Position.X * Level.PixelPerMeter, charRect.Body.Position.Y * Level.PixelPerMeter);
            position = new Vector2(centerPosition.X - 250, centerPosition.Y - 250);
            sRect.Body.Position = charRect.Body.Position + new Vector2(0, 120 / Level.PixelPerMeter);
        }

        public void UpdateCamera()
        {
            //Vorerst nicht verwendet, wegen Editor
            //Camera.Position = position;
        }

        private void UpdateControls(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && !isFalling && !isJumping && movement.X >= -3.5)
            {
                if (facing == 1)
                {
                    facing = 0;
                }

                else
                {
                    if (isIdle)
                    {
                        activeAnimation = runStarting_left;
                        activeAnimation.activeFrameNumber = 0;
                        activeAnimation.start();
                    }

                    isIdle = false;
                    isRunning = true;
                    charRect.Body.ApplyForce(new Vector2(-50, 0));
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right) && !isFalling && !isJumping && movement.X <= 3.5)
            {
                if (facing == 0)
                {
                    facing = 1;
                }

                else
                {
                    if (isIdle)
                    {
                        activeAnimation = runStarting_right;
                        activeAnimation.activeFrameNumber = 0;
                        activeAnimation.start();
                    }

                    isIdle = false;
                    isRunning = true;
                    charRect.Body.ApplyForce(new Vector2(50, 0));
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up) && !isScriptedMoving)
            {
                climb();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space) && (isIdle || isRunning))
            {
                if (facing == 0)
                {
                    activeAnimation = jumpStarting_left;
                    activeAnimation.activeFrameNumber = 0;
                    activeAnimation.start();
                    isJumping = true;
                    isIdle = false;
                    isFalling = false;
                    isRunning = false;
                    charRect.Body.ApplyForce(new Vector2(-750, -850));
                }
                else if (facing == 1)
                {
                    activeAnimation = jumpStarting_right;
                    activeAnimation.activeFrameNumber = 0;
                    activeAnimation.start();
                    isJumping = true;
                    isIdle = false;
                    isFalling = false;
                    isRunning = false;
                    charRect.Body.ApplyForce(new Vector2(750, -850));
                }
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Left) && isRunning && facing == 0)
            {
                isRunning = false;
                isIdle = true;

                activeAnimation = runStopping_left;
                activeAnimation.activeFrameNumber = 0;
                activeAnimation.start();
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Right) && isRunning && facing == 1)
            {
                isRunning = false;
                isIdle = true;

                activeAnimation = runStopping_right;
                activeAnimation.activeFrameNumber = 0;
                activeAnimation.start();
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
                charRect.Body.IgnoreGravity = true;

                if (facing == 0)
                {
                    activeAnimation = climbing_left;
                    activeAnimation.activeFrameNumber = 0;
                    activeAnimation.start();
                    nextAnimation = choseIdleAnimation();
                }
                else
                {
                    activeAnimation = climbing_right;
                    activeAnimation.activeFrameNumber = 0;
                    activeAnimation.start();
                    nextAnimation = choseIdleAnimation();
                }
            }
            // Hier wird noch gebessert
            if (activeAnimation.activeFrameNumber < activeAnimation.amount - 1)
            {
                int temp = activeAnimation.activeFrameNumber;
                actClimbHeight = temp;
                if (facing == 1)
                {
                    charRect.Body.Position += new Vector2(1.5f / Level.PixelPerMeter, -2 / Level.PixelPerMeter);
                }
                else
                {
                    charRect.Body.Position += new Vector2(-1.5f / Level.PixelPerMeter, -2 / Level.PixelPerMeter);
                }
            }


            if (activeAnimation.activeFrameNumber == activeAnimation.amount - 1)
            {
                activeAnimation.activeFrameNumber = 0;
                activeAnimation = choseIdleAnimation();
                activeAnimation.activeFrameNumber = 0;
                activeAnimation.start();
                actClimbHeight = 0;
                actScriptedMove = "";
                charRect.Body.BodyType = BodyType.Dynamic;
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

            if (activeAnimation == null)
            {
                activeAnimation = choseIdleAnimation();
                activeAnimation.activeFrameNumber = 0;
                activeAnimation.start();
                nextAnimation = choseIdleAnimation();
            }

            // SPRINGEN
            if (activeAnimation == jumpStarting_left)
            {
                nextAnimation = falling_left;
            }

            if (activeAnimation == jumpStarting_right)
            {
                nextAnimation = falling_right;
            }

            if (activeAnimation == landing_left)
            {
                nextAnimation = choseIdleAnimation();
            }

            if (activeAnimation == landing_right)
            {
                nextAnimation = choseIdleAnimation();
            }


            // LAUFEN
            if (activeAnimation == runStarting_left)
            {
                nextAnimation = running_left;
            }

            if (activeAnimation == runStarting_right)
            {
                nextAnimation = running_right;
            }

            if (activeAnimation == running_left && Keyboard.GetState().IsKeyUp(Keys.Left))
            {
                activeAnimation = runStopping_left;
                activeAnimation.activeFrameNumber = 0;
                activeAnimation.start();
                nextAnimation = choseIdleAnimation();
            }

            if (activeAnimation == running_right && Keyboard.GetState().IsKeyUp(Keys.Right))
            {
                activeAnimation = runStopping_right;
                activeAnimation.activeFrameNumber = 0;
                activeAnimation.start();
                nextAnimation = choseIdleAnimation();
            }

            if (activeAnimation == runStopping_left)
            {
                nextAnimation = choseIdleAnimation();
            }

            if (activeAnimation == runStopping_right)
            {
                nextAnimation = choseIdleAnimation();
            }

            if (activeAnimation.activeFrameNumber == activeAnimation.amount - 1 &&
                (
                (activeAnimation == idle_left) ||
                (activeAnimation == idleb_left) ||
                (activeAnimation == idlec_left) ||
                (activeAnimation == idle_right) ||
                (activeAnimation == idleb_right) ||
                (activeAnimation == idlec_right))
                )
            {
                nextAnimation = choseIdleAnimation();
                activeAnimation = nextAnimation;
                activeAnimation.activeFrameNumber = 0;
                activeAnimation.start();
                nextAnimation = null;
            }

            if (activeAnimation.activeFrameNumber == activeAnimation.amount - 1 && !activeAnimation.looped)
            {
                activeAnimation = nextAnimation;
                activeAnimation.activeFrameNumber = 0;
                activeAnimation.start();
                nextAnimation = null;
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

        public bool OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (isFalling)
            {
                isFalling = false;
                isIdle = true;
                if (facing == 0)
                {
                    activeAnimation = landing_left;
                    activeAnimation.activeFrameNumber = 0;
                    activeAnimation.start();
                }
                else
                {
                    activeAnimation = landing_right;
                    activeAnimation.activeFrameNumber = 0;
                    activeAnimation.start();
                }
            }
            return true;
        }

        public bool sOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (isFalling)
            {
                isFalling = false;
                isIdle = true;
                if (facing == 0)
                {
                    activeAnimation = landing_left;
                    activeAnimation.activeFrameNumber = 0;
                    activeAnimation.start();
                }
                else
                {
                    activeAnimation = landing_right;
                    activeAnimation.activeFrameNumber = 0;
                    activeAnimation.start();
                }
            }

            return true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            activeAnimation.Draw(spriteBatch);
            //Das auskommentierte hier kann als Debugview dienen.
            spriteBatch.DrawString(FontManager.Arial, "Standing: " + isIdle.ToString(), new Vector2(300, 20), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "Running: " + isRunning.ToString(), new Vector2(300, 45), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "Jumping: " + isJumping.ToString(), new Vector2(300, 70), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "Falling: " + isFalling.ToString(), new Vector2(300, 95), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, charRect.Body.Position.ToString(), new Vector2(300, 120), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "Scriptedmoving: " + isScriptedMoving, new Vector2(300, 155), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, actClimbHeight.ToString(), new Vector2(300, 180), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "X: " + movement.X.ToString() + " Y: " + movement.Y.ToString(), new Vector2(300, 205), Color.Black);
        }
    }
}
