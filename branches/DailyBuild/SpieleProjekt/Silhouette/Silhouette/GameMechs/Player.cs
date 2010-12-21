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
using FarseerPhysics.Controllers;

namespace Silhouette.GameMechs
{
    public class Player : DrawableLevelObject
    {
        public float tempRotation;
        public Vector2 centerPosition;      // Hannes: Rectangles für Kollisionserkennung. Muss noch um Kollisionsgruppe erweitert werden.
        public Vector2 camPosition;
        public Fixture charRect;
        public Fixture sRect;
        public Fixture nRect;
        public Fixture camRect;             // Camrect ist n Rectangle, dessen Bewegung auf die Camera übertragen wird.
        public RopeJoint joint0;            // Joints nötig, um die Cam am Player zu fixieren
        public RopeJoint joint1;
        public RopeJoint joint2;
        public RopeJoint joint3;
        public AngleJoint joint4;           // Joint, der die Camera nicht zu weit rotieren lässt

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
            // position = Vector2.Zero;
            // position = new Vector2(200, 200);


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
            tempRotation = 0.0f;

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

            charRect = FixtureManager.CreateCircle(80, position, BodyType.Dynamic, 1);
            charRect.Body.FixedRotation = true;
            charRect.Friction = 5;

            nRect = FixtureManager.CreateRectangle(140, 10, new Vector2(position.X, position.Y - 85), BodyType.Dynamic, 1);
            nRect.Body.FixedRotation = true;
            nRect.IsSensor = true;

            sRect = FixtureManager.CreateRectangle(100, 10, new Vector2(position.X, position.Y + 120), BodyType.Dynamic, 1);
            sRect.Body.FixedRotation = true;
            sRect.IsSensor = true;

            camRect = FixtureManager.CreateRectangle(100, 100, charRect.Body.Position, BodyType.Dynamic, 0.1f);
            camRect.Body.IgnoreGravity = true;
            camRect.Body.FixedRotation = true;
            camRect.IsSensor = true;

            sRect.IgnoreCollisionWith(charRect);
            nRect.IgnoreCollisionWith(charRect);
            nRect.IgnoreCollisionWith(camRect);
            nRect.IgnoreCollisionWith(sRect);
            charRect.IgnoreCollisionWith(sRect);
            charRect.IgnoreCollisionWith(nRect);

            joint0 = new RopeJoint(charRect.Body, camRect.Body, new Vector2(100 / Level.PixelPerMeter, 80 / Level.PixelPerMeter) * 2, new Vector2(50 / Level.PixelPerMeter, 50 / Level.PixelPerMeter));
            joint1 = new RopeJoint(charRect.Body, camRect.Body, new Vector2(-100 / Level.PixelPerMeter, -80 / Level.PixelPerMeter) * 2, new Vector2(-50 / Level.PixelPerMeter, -50 / Level.PixelPerMeter));
            joint2 = new RopeJoint(charRect.Body, camRect.Body, new Vector2(100 / Level.PixelPerMeter, -80 / Level.PixelPerMeter) * 2, new Vector2(50 / Level.PixelPerMeter, -50 / Level.PixelPerMeter));
            joint3 = new RopeJoint(charRect.Body, camRect.Body, new Vector2(-100 / Level.PixelPerMeter, 80 / Level.PixelPerMeter) * 2, new Vector2(-50 / Level.PixelPerMeter, 50 / Level.PixelPerMeter));
            //joint4 = JointFactory.CreateAngleJoint(Level.Physics, charRect.Body, sRect.Body);
            //joint4.Softness = 0.999f;
            //joint4.Softness = 0.99f;

            joint0.MaxLength = 2.1f;
            joint1.MaxLength = 2.1f;
            joint2.MaxLength = 2.1f;
            joint3.MaxLength = 2.1f;


            Level.Physics.AddJoint(joint0);
            Level.Physics.AddJoint(joint1);
            Level.Physics.AddJoint(joint2);
            Level.Physics.AddJoint(joint3);
            //Level.Physics.AddJoint(joint4);

            charRect.OnCollision += this.OnCollision;
            charRect.OnSeparation += this.OnSeperation;
            nRect.OnCollision += this.nOnCollision;
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
            position = new Vector2(centerPosition.X, centerPosition.Y);
            sRect.Body.Position = charRect.Body.Position + new Vector2(0, 120 / Level.PixelPerMeter);
            nRect.Body.Position = charRect.Body.Position + new Vector2(0, -85 / Level.PixelPerMeter);
            camPosition = new Vector2(camRect.Body.Position.X * Level.PixelPerMeter, camRect.Body.Position.Y * Level.PixelPerMeter);
        }

        public void UpdateCamera()
        {
            Camera.Position = camPosition;
            // Die Camera wird nur rotiert, wenn die Rotation unter einem bestimmten Winkel bleibt. Damit das nicht ausartet.
            if (camRect.Body.Rotation >= -0.1f && camRect.Body.Rotation <= 0.1f) { Camera.Rotation = camRect.Body.Rotation; }
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
                    charRect.Body.ApplyForce(new Vector2(-80, 0));
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
                    charRect.Body.ApplyForce(new Vector2(80, 0));
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up) && !isScriptedMoving && canClimb)
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
                    charRect.Body.ApplyForce(new Vector2(-300, -800));
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
                    charRect.Body.ApplyForce(new Vector2(300, -800));
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
                    camRect.Body.Position += new Vector2(1.5f / Level.PixelPerMeter, -2 / Level.PixelPerMeter);
                }
                else
                {
                    charRect.Body.Position += new Vector2(-1.5f / Level.PixelPerMeter, -2 / Level.PixelPerMeter);
                    camRect.Body.Position += new Vector2(-1.5f / Level.PixelPerMeter, -2 / Level.PixelPerMeter);
                }
            }


            if (activeAnimation.activeFrameNumber == activeAnimation.amount - 1)
            {
                try
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
                    isIdle = false;
                    isRunning = false;
                    isFalling = true;
                    isJumping = false;
                    isDying = false;
                }
                catch (Exception e) { activeAnimation.activeFrameNumber = 0; }
            }

        }

        private void UpdateTexture(GameTime gameTime)
        {
            //if (lastNewContact != null) { calcRotation(fixtureBFromContact, lastNewContact); }
            //activeAnimation.rotation = tempRotation - tempRotation/2;
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

            if ((activeAnimation == falling_left || activeAnimation == falling_right) && isIdle)
            {
                activeAnimation = choseIdleAnimation();
                activeAnimation.activeFrameNumber = 0;
                activeAnimation.start();
                nextAnimation = null;
            }
        }

        public void ObserveMovement()
        {
            movement = charRect.Body.GetLinearVelocityFromWorldPoint(Vector2.Zero);

            if (Math.Max(oldPosition.X, charRect.Body.Position.X) - Math.Min(oldPosition.X, charRect.Body.Position.X) < 0.001 &&
                Math.Max(oldPosition.Y, charRect.Body.Position.Y) - Math.Min(oldPosition.Y, charRect.Body.Position.Y) < 0.001)
            {
                isIdle = true;
                isFalling = false;
                isRunning = false;
                isJumping = false;
            }

            if (movement.Y + Level.Physics.Gravity.Y < 0)
            {
                isIdle = false;
                isFalling = false;
                isRunning = false;
                isJumping = true;
            }

            else if (isJumping && oldPosition.Y < charRect.Body.Position.Y)
            {
                isIdle = false;
                isFalling = true;
                isRunning = false;
                isJumping = false;
            }

            else if (movement.Y > Level.Physics.Gravity.Y + 0.1f)
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

        public void calcRotation(Fixture fixB, Contact contact)
        {
            Vector2 temp;
            temp.X = (contact.Manifold.LocalPoint.X - charRect.Body.Position.X) * Level.PixelPerMeter;
            temp.Y = (contact.Manifold.LocalPoint.Y - charRect.Body.Position.Y) * Level.PixelPerMeter;
            temp.Normalize();
            Vector2 temp1 = new Vector2(-2, 1);
            temp1.Normalize();
            float temp2 = Vector2.Dot(temp, temp1);

            tempRotation = (temp2 / (temp.Length() * temp1.Length()));
        }

        public bool OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            /*this.lastNewContact = contact;
            this.fixtureBFromContact = fixtureB;

            Vector2 temp;
            temp.X = (contact.Manifold.LocalPoint.X - charRect.Body.Position.X) * Level.PixelPerMeter;
            temp.Y = (contact.Manifold.LocalPoint.Y - charRect.Body.Position.Y) * Level.PixelPerMeter;
            temp.Normalize();
            Vector2 temp1 = new Vector2(-2, 1);
            temp1.Normalize();
            float temp2 = Vector2.Dot(temp, temp1);
            tempRotation = temp2;
            activeAnimation.rotation = temp2;*/

            if (isFalling)
            {
                isFalling = false;
                isJumping = false;
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

            if (fixtureB.isClimbable)
            {
                this.canClimb = true;
            }

            return true;
        }

        public void OnSeperation(Fixture fixtureA, Fixture fixtureB)
        {
            if (canClimb)
            {
                canClimb = false;
            }
        }

        public bool nOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.isHalfTransparent)
            {
                charRect.IsSensor = true;
            }
            return true;

        }


        public bool sOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (isFalling)
            {
                isFalling = false;
                isIdle = true;
                isJumping = false;
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

            if (charRect.IsSensor == true)
            {
                try { charRect.IsSensor = false; }
                catch (Exception e) { }
            }

            return true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //activeAnimation.Draw(spriteBatch);
            spriteBatch.Draw(activeAnimation.activeTexture, position, null, Color.White, activeAnimation.rotation, new Vector2(250, 250), 1, SpriteEffects.None, 1);
            //Das auskommentierte hier kann als Debugview dienen.
            /*spriteBatch.DrawString(FontManager.Arial, "Standing: " + isIdle.ToString(), new Vector2(300, 20), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "Running: " + isRunning.ToString(), new Vector2(300, 45), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "Jumping: " + isJumping.ToString(), new Vector2(300, 70), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "Falling: " + isFalling.ToString(), new Vector2(300, 95), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, charRect.Body.Position.ToString(), new Vector2(300, 120), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "Scriptedmoving: " + isScriptedMoving, new Vector2(300, 155), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, actClimbHeight.ToString(), new Vector2(300, 180), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "X: " + movement.X.ToString() + " Y: " + movement.Y.ToString(), new Vector2(300, 205), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "CamRectRotation: " + camRect.Body.Rotation.ToString(), new Vector2(300, 230), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "CamRotation: " + Camera.Rotation.ToString(), new Vector2(300, 255), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "tempRotation: " + tempRotation.ToString(), new Vector2(300, 280), Color.Black);*/
        }
    }
}
