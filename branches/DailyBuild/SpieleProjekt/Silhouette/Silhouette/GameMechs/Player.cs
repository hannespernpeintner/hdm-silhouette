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
        public static int JUMPFORCENORMAL = 1350;
        public static int JUMPFORCESUPER = 1700;

        public float tempRotation;
        public int resetTimer;
        public Vector2 centerPosition;      // Hannes: Rectangles für Kollisionserkennung. Muss noch um Kollisionsgruppe erweitert werden.
        public Vector2 camPosition;
        public Fixture charRect;
        //public List<Fixture> charRect;
        public Fixture sRect;
        public Fixture nRect;
        public Fixture eRect;
        public Fixture wRect;
        public Fixture landRect;
        public Fixture camRect;             // Camrect ist n Rectangle, dessen Bewegung auf die Camera übertragen wird.
        public RopeJoint joint0;            // Joints nötig, um die Cam am Player zu fixieren
        public RopeJoint joint1;
        public RopeJoint joint2;
        public RopeJoint joint3;
        public AngleJoint joint4;           // Joint, der die Camera nicht zu weit rotieren lässt

        public KeyboardState oldState;
        public KeyboardState kState;
        public Vector2 oldPosition;
        public Vector2 movement;            // Bewegung des Characters. Wichtig zur Statusveränderungserkennung.

        private bool isRunning;             // Hannes: Statusveränderungen des Chars. Weitere folgen.
        private bool isJumping;
        private bool isFalling;
        private bool isIdle;
        private bool isScriptedMoving;
        private bool isUncontrollableMoving;
        private bool isDying;
        public bool isRemembering;         // So kommt Tom in den Erinnerungszustand
        public bool isRecovering;
        public bool isLanding;

        private bool sRectTouching;
        private bool rectTouching;

        private bool controlsEnabled;

        private int facing;                    // Wo der Chara hinschaut. 0 bedeutet links, 1 bedeutet rechts.

        public float sJTimer;
        public float sJRecoveryTimer;
        public float fadeBlue;
        public float fadeOrange;

        private String actScriptedMove;
        private String actUncontrollableMove;

        private bool canClimb;
        private int actClimbHeight;

        private Animation activeAnimation;      // Die aktive Sprite, die in der UpdateMethode auch geupdatet wird.
        private Animation nextAnimation;        // Die nächste Animation, die gespielt wird, sobald die aktuelle abgelaufen is oder unterbrochen wird.

        private Animation running_left;             // Hier kommen alle Animationen hin, die Tom hat.
        private Animation running_right;
        private Animation runStarting_left;
        private Animation runStarting_right;
        private Animation runStopping_left;
        private Animation runStopping_right;
        private Animation noJumpRunning_left;
        private Animation noJumpRunning_right;

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
        private Animation noJumpIdle_left;
        private Animation noJumpIdle_right;

        private Animation climbing_left;
        private Animation climbing_right;

        private Animation hang_left;
        private Animation hang_right;
        private Animation hang2_left;
        private Animation hang2_right;
        private Animation pullup;

        private Animation dying_left;
        private Animation dying_right;
        private Animation dying2_left;
        private Animation dying2_right;

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
            noJumpIdle_left = new Animation();
            noJumpIdle_right = new Animation();

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
            noJumpRunning_left = new Animation();
            noJumpRunning_right = new Animation();

            climbing_left = new Animation();
            climbing_right = new Animation();

            hang_left = new Animation();
            hang_right = new Animation();
            hang2_left = new Animation();
            hang2_right = new Animation();
            pullup = new Animation();

            dying_left = new Animation();
            dying_right = new Animation();

            dying2_left = new Animation();
            dying2_right = new Animation();

            movement = Vector2.Zero;

            isRunning = false;
            isJumping = false;
            isFalling = false;
            isIdle = true;
            isDying = false;
            isLanding = false;
            isScriptedMoving = false;
            isRemembering = false;
            isRecovering = false;
            sJRecoveryTimer = 17000;
            sJTimer = 6000;
            fadeBlue = 0;
            fadeOrange = 0;
            canClimb = false;
            tempRotation = 0.0f;
            resetTimer = 2000;

            facing = 1;
        }

        public override void LoadContent()
        {
            // Hier müssen alle Sprites geladen werden.
            idle_left.Load(6, "Sprites/Player/idleA_left_", 15, true);
            idle_right.Load(6, "Sprites/Player/idleA_right_", 15, true);
            idleb_left.Load(6, "Sprites/Player/idleB_left_", 15, true);
            idleb_right.Load(6, "Sprites/Player/idleB_right_", 15, true);
            idlec_left.Load(6, "Sprites/Player/idleC_left_", 15, true);
            idlec_right.Load(6, "Sprites/Player/idleC_right_", 15, true);

            noJumpIdle_left.Load(2, "Sprites/Player/nojumpI_left_", 15, false);
            noJumpIdle_right.Load(2, "Sprites/Player/nojumpI_right_", 15, false);

            jumpStarting_left.Load(5, "Sprites/Player/jumpStart_left_", 25, false);
            jumpStarting_right.Load(5, "Sprites/Player/jumpStart_right_", 25, false);
            falling_left.Load(2, "Sprites/Player/falling_left_", 15, true);
            falling_right.Load(2, "Sprites/Player/falling_right_", 15, true);
            landing_left.Load(8, "Sprites/Player/landing_left_", 10, false);
            landing_right.Load(8, "Sprites/Player/landing_right_", 10, false);
            //1,5

            running_left.Load(8, "Sprites/Player/walk_left_", 30, true);
            running_right.Load(8, "Sprites/Player/walk_right_", 30, true);
            runStarting_left.Load(8, "Sprites/Player/walkStart_left_", 30, false);
            runStarting_right.Load(8, "Sprites/Player/walkStart_right_", 30, false);
            runStopping_left.Load(8, "Sprites/Player/walkStop_left_", 15, false);
            runStopping_right.Load(8, "Sprites/Player/walkStop_right_", 15, false);

            noJumpRunning_left.Load(2, "Sprites/Player/nojumpW_left_", 15, false);
            noJumpRunning_right.Load(2, "Sprites/Player/nojumpW_right_", 15, false);

            climbing_left.Load(21, "Sprites/Player/climb_left_", 15, false);
            climbing_right.Load(21, "Sprites/Player/climb_right_", 15, false);

            hang_left.Load(5, "Sprites/Player/hang_left_", 15, false);
            hang_right.Load(5, "Sprites/Player/hang_right_", 15, false);
            hang2_left.Load(1, "Sprites/Player/hang2_left_", 10, false);
            hang2_right.Load(1, "Sprites/Player/hang2_right_", 10, false);
            pullup.Load(13, "Sprites/Player/pullup_left_", 10, false);

            dying_left.Load(4, "Sprites/Player/die_left_", 10, false);
            dying_right.Load(4, "Sprites/Player/die_right_", 10, false);
            dying2_left.Load(1, "Sprites/Player/die2_left_", 10, false);
            dying2_right.Load(1, "Sprites/Player/die2_right_", 10, false);

            activeAnimation = choseIdleAnimation();
            activeAnimation.start();
            nextAnimation = choseIdleAnimation();

            //Fördert schlechtes Charverhalten wegen statischer Physik
            charRect = FixtureManager.CreatePolygon(idle_left.pictures[0], new Vector2(0.85f, 0.95f), BodyType.Dynamic, position, 1);
            charRect.Friction = 1;
            charRect.isPlayer = true;

            //charRect = FixtureManager.CreateCapsule((idle_left.pictures[0].Height / 5) / Level.PixelPerMeter, 50, position, BodyType.Dynamic, 1);
            //charRect.Body.FixedRotation = true;
            //charRect.Friction = 5;
            //charRect.isPlayer = true;

            nRect = FixtureManager.CreateRectangle(140, 10, new Vector2(position.X, position.Y - 85), BodyType.Dynamic, 0);
            nRect.Body.FixedRotation = true;
            nRect.IsSensor = true;
            nRect.isPlayer = true;

            sRect = FixtureManager.CreateRectangle(100, 10, new Vector2(position.X, position.Y + 120), BodyType.Dynamic, 0);
            sRect.Body.FixedRotation = true;
            sRect.IsSensor = true;
            sRect.isPlayer = true;

            landRect = FixtureManager.CreateRectangle(100, 10, new Vector2(position.X, position.Y + 300), BodyType.Dynamic, 0);
            landRect.Body.FixedRotation = true;
            landRect.IsSensor = true;
            landRect.isPlayer = true;

            eRect = FixtureManager.CreateRectangle(25, 100, new Vector2(position.X + 105, position.Y), BodyType.Dynamic, 0);
            eRect.Body.FixedRotation = true;
            eRect.IsSensor = true;
            eRect.isPlayer = true;
            eRect.isEvent = true;

            wRect = FixtureManager.CreateRectangle(25, 100, new Vector2(position.X - 110, position.Y), BodyType.Dynamic, 0);
            wRect.Body.FixedRotation = true;
            wRect.IsSensor = true;
            wRect.isPlayer = true;

            camRect = FixtureManager.CreateRectangle(100, 100, position, BodyType.Dynamic, 0.1f);
            camRect.Body.IgnoreGravity = true;
            camRect.Body.FixedRotation = true;
            camRect.IsSensor = true;
            camRect.isPlayer = true;

            sRect.IgnoreCollisionWith(charRect);
            sRect.IgnoreCollisionWith(camRect);
            nRect.IgnoreCollisionWith(charRect);
            nRect.IgnoreCollisionWith(camRect);
            nRect.IgnoreCollisionWith(sRect);
            eRect.IgnoreCollisionWith(charRect);
            eRect.IgnoreCollisionWith(camRect);
            wRect.IgnoreCollisionWith(charRect);
            wRect.IgnoreCollisionWith(camRect);
            charRect.IgnoreCollisionWith(sRect);
            charRect.IgnoreCollisionWith(nRect);
            charRect.IgnoreCollisionWith(eRect);
            charRect.IgnoreCollisionWith(wRect);
            charRect.IgnoreCollisionWith(camRect);

            //joint0 = new RopeJoint(charRect[0].Body, camRect.Body, new Vector2(100 / Level.PixelPerMeter, 80 / Level.PixelPerMeter) * 2, new Vector2(50 / Level.PixelPerMeter, 50 / Level.PixelPerMeter));
            //joint1 = new RopeJoint(charRect[0].Body, camRect.Body, new Vector2(-100 / Level.PixelPerMeter, -80 / Level.PixelPerMeter) * 2, new Vector2(-50 / Level.PixelPerMeter, -50 / Level.PixelPerMeter));
            //joint2 = new RopeJoint(charRect[0].Body, camRect.Body, new Vector2(100 / Level.PixelPerMeter, -80 / Level.PixelPerMeter) * 2, new Vector2(50 / Level.PixelPerMeter, -50 / Level.PixelPerMeter));
            //joint3 = new RopeJoint(charRect[0].Body, camRect.Body, new Vector2(-100 / Level.PixelPerMeter, 80 / Level.PixelPerMeter) * 2, new Vector2(-50 / Level.PixelPerMeter, 50 / Level.PixelPerMeter));
            //joint4 = JointFactory.CreateAngleJoint(Level.Physics, charRect[0].Body, sRect.Body);
            //joint4.Softness = 0.999f;

            //joint0.MaxLength = 2.15f;
            //joint1.MaxLength = 2.15f;
            //joint2.MaxLength = 2.15f;
            //joint3.MaxLength = 2.15f;


            //Level.Physics.AddJoint(joint0);
            //Level.Physics.AddJoint(joint1);
            //Level.Physics.AddJoint(joint2);
            //Level.Physics.AddJoint(joint3);

            charRect.OnCollision += this.OnCollision;
            charRect.OnSeparation += this.OnSeperation;
            nRect.OnCollision += this.nOnCollision;
            sRect.OnCollision += this.sOnCollision;
            sRect.OnSeparation += this.sOnSeperation;
            landRect.OnCollision += this.landOnCollision;
            wRect.OnCollision += this.wOnCollision;
            wRect.OnSeparation += this.wOnSeparation;
            eRect.OnCollision += this.eOnCollision;
            eRect.OnSeparation += this.eOnSeparation;
        }

        public override void Update(GameTime gameTime)
        {
            calcRotation(gameTime);
            if (!isDying && !isScriptedMoving && !isUncontrollableMoving)
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
            if (isUncontrollableMoving)
            {
                doUncontrollableMove();
            }


            UpdatePositions();
            UpdateTexture(gameTime);
            UpdateCamera();
            ObserveTimer(gameTime);
        }

        public void UpdatePositions()
        {
            centerPosition = new Vector2(charRect.Body.Position.X * Level.PixelPerMeter, charRect.Body.Position.Y * Level.PixelPerMeter);
            position = new Vector2(centerPosition.X, centerPosition.Y);
            sRect.Body.Position = charRect.Body.Position + new Vector2(0, 120 / Level.PixelPerMeter);
            nRect.Body.Position = charRect.Body.Position + new Vector2(0, -85 / Level.PixelPerMeter);
            wRect.Body.Position = charRect.Body.Position + new Vector2(-110 / Level.PixelPerMeter, 0);
            eRect.Body.Position = charRect.Body.Position + new Vector2(105 / Level.PixelPerMeter, 0);
            landRect.Body.Position = charRect.Body.Position + new Vector2(0, 300 / Level.PixelPerMeter);
            camPosition = new Vector2(camRect.Body.Position.X * Level.PixelPerMeter, camRect.Body.Position.Y * Level.PixelPerMeter);
        }

        public void UpdateCamera()
        {
            if (Camera.fixedOnPlayer)
            {
                camPosition.Y -= 400;
                Camera.Position = camPosition;
            }

            Camera.Position = position;
        }

        private void UpdateControls(GameTime gameTime)
        {
            kState = Keyboard.GetState();

            // LEFT ARROW
            if (kState.IsKeyDown(Keys.Left) && (isRunning || isIdle) && movement.X >= -3.3f)
            {
                if (facing == 1)
                {
                    facing = 0;
                }

                else
                {
                    if (isIdle || activeAnimation == running_right || activeAnimation == runStarting_right || activeAnimation == runStopping_right)
                    {
                        activeAnimation = runStarting_left;
                        activeAnimation.activeFrameNumber = 0;
                        activeAnimation.start();
                    }

                    isIdle = false;
                    isRunning = true;
                    charRect.Body.ApplyForce(new Vector2(-40, 0));
                }
            }

            if (kState.IsKeyDown(Keys.Left) && (isFalling || isJumping || isLanding) && movement.X >= -2.5f && !rectTouching)
            {
                if (facing == 1)
                {
                    facing = 0;
                }
                else
                {
                    charRect.Body.ApplyForce(new Vector2(-15, 0));
                }
            }

            // RIGHT ARROW
            if (kState.IsKeyDown(Keys.Right) && (isRunning || isIdle) && movement.X <= 3.3f)
            {
                if (facing == 0)
                {
                    facing = 1;
                }

                else
                {
                    if (isIdle || activeAnimation == running_left || activeAnimation == runStarting_left || activeAnimation == runStopping_left)
                    {
                        activeAnimation = runStarting_right;
                        activeAnimation.activeFrameNumber = 0;
                        activeAnimation.start();
                    }

                    isIdle = false;
                    isRunning = true;
                    charRect.Body.ApplyForce(new Vector2(40, 0));
                }
            }

            if (kState.IsKeyDown(Keys.Right) && (isFalling || isJumping || isLanding) && movement.X <= 2.5f && !rectTouching)
            {
                if (facing == 0)
                {
                    facing = 1;
                }
                else
                {
                    charRect.Body.ApplyForce(new Vector2(15, 0));
                }
            }

            // UP ARROW
            if (kState.IsKeyDown(Keys.Up) && !isScriptedMoving && canClimb)
            {
                climb();
            }

            // P BUTTON
            if (kState.IsKeyDown(Keys.P) && !isScriptedMoving)
            {
                die();
            }

            // L BUTTON
            if (kState.IsKeyDown(Keys.L) && !isRemembering && !isRecovering)
            {
                isRemembering = true;
            }

            /*if (kState.IsKeyDown(Keys.H))
            {
                hang();
            }*/

            // SPACE BUTTON
            if (kState.IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space)  && (isIdle || isRunning))
            {
                if (facing == 0 && !isRecovering)
                {
                    activeAnimation = jumpStarting_left;
                    activeAnimation.activeFrameNumber = 0;
                    activeAnimation.start();
                    isJumping = true;
                    isIdle = false;
                    isFalling = false;
                    isRunning = false;
                    // Unterscheiden von Superjump und NormalJump
                    if (isRemembering)
                    {
                        charRect.Body.ApplyForce(new Vector2(-50, -JUMPFORCESUPER));
                    }
                    else
                    {
                        charRect.Body.ApplyForce(new Vector2(-50, -JUMPFORCENORMAL));
                    }
                }
                else if (facing == 1 && !isRecovering)
                {
                    activeAnimation = jumpStarting_right;
                    activeAnimation.activeFrameNumber = 0;
                    activeAnimation.start();
                    isJumping = true;
                    isIdle = false;
                    isFalling = false;
                    isRunning = false;
                    if (isRemembering)
                    {
                        charRect.Body.ApplyForce(new Vector2(50, -JUMPFORCESUPER));
                    }
                    else
                    {
                        charRect.Body.ApplyForce(new Vector2(50, -JUMPFORCENORMAL));
                    }
                }
                else if (isRecovering)
                {
                    if (isRunning)
                    {
                        if (facing == 0)
                        {
                            activeAnimation = noJumpRunning_left;
                            activeAnimation.activeFrameNumber = 0;
                            activeAnimation.start();
                            nextAnimation = running_left;
                        }
                        else if (facing == 1)
                        {
                            activeAnimation = noJumpRunning_right;
                            activeAnimation.activeFrameNumber = 0;
                            activeAnimation.start();
                            nextAnimation = running_right;
                        }
                    }
                    else if (isIdle)
                    {
                        if (facing == 0)
                        {
                            activeAnimation = noJumpIdle_left;
                            activeAnimation.activeFrameNumber = 0;
                            activeAnimation.start();
                            nextAnimation = choseIdleAnimation();
                        }
                        else if (facing == 1)
                        {
                            activeAnimation = noJumpIdle_right;
                            activeAnimation.activeFrameNumber = 0;
                            activeAnimation.start();
                            nextAnimation = choseIdleAnimation();
                        }
                    }
                }
            }

            // Für den Fall, dass man den Lauf abbricht, wird abgebremst
            if (kState.IsKeyUp(Keys.Left) && isRunning && facing == 0)
            {
                isRunning = false;
                isIdle = true;

                activeAnimation = runStopping_left;
                activeAnimation.activeFrameNumber = 0;
                activeAnimation.start();
            }

            if (kState.IsKeyUp(Keys.Right) && isRunning && facing == 1)
            {
                isRunning = false;
                isIdle = true;

                activeAnimation = runStopping_right;
                activeAnimation.activeFrameNumber = 0;
                activeAnimation.start();
            }

            // WENN KEIN BUTTON GEDRÜCKT IST
            if (kState.GetPressedKeys().Length == 0 && oldState.GetPressedKeys().Length == 0)
            {
                charRect.Friction = 4;
            }

            else { charRect.Friction = 0.1f; }

            oldState = kState;
            oldPosition = charRect.Body.Position;
        }

        private void doScriptedMove()
        {
            if ("climb".Equals(actScriptedMove) && isScriptedMoving)
            {
                climb();
            }

            /*if ("die".Equals(actScriptedMove) && (!controlsEnabled))
            {
                die();
            }*/

            if ("hang".Equals(actScriptedMove) && isScriptedMoving)
            {
                hang();
            }
        }

        private void doUncontrollableMove()
        { 
            if ("die".Equals(actUncontrollableMove) && (!controlsEnabled))
            {
                die();
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
            if (activeAnimation.activeFrameNumber < activeAnimation.Amount - 1)
            {
                int temp = activeAnimation.activeFrameNumber;
                actClimbHeight = temp;
                if (facing == 1)
                {
                    if (activeAnimation.activeFrameNumber <= 6 || activeAnimation.activeFrameNumber >= 14)
                    {
                        charRect.Body.Position += new Vector2(3.8f / Level.PixelPerMeter, 0);
                        if (Camera.fixedOnPlayer) { camRect.Body.Position += new Vector2(3.8f / Level.PixelPerMeter, 0); }
                    }
                    if (activeAnimation.activeFrameNumber > 6 && activeAnimation.activeFrameNumber < 14)
                    {
                        charRect.Body.Position += new Vector2(0, -4.7f / Level.PixelPerMeter);
                        if (Camera.fixedOnPlayer) { camRect.Body.Position += new Vector2(0, -4.7f / Level.PixelPerMeter); }
                    }
                }
                else
                {
                    if (activeAnimation.activeFrameNumber <= 6 || activeAnimation.activeFrameNumber >= 14)
                    {
                        charRect.Body.Position += new Vector2(-3.8f / Level.PixelPerMeter, 0);
                        if (Camera.fixedOnPlayer) { camRect.Body.Position += new Vector2(-3.8f / Level.PixelPerMeter, 0); }
                    }
                    if (activeAnimation.activeFrameNumber > 6 && activeAnimation.activeFrameNumber < 14)
                    {
                        charRect.Body.Position += new Vector2(0, -4.7f / Level.PixelPerMeter);
                        if (Camera.fixedOnPlayer) { camRect.Body.Position += new Vector2(0, -4.7f / Level.PixelPerMeter); }
                    }
                }
            }


            if (activeAnimation.activeFrameNumber == activeAnimation.Amount - 1)
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
                    isIdle = true;
                    isRunning = false;
                    isFalling = false;
                    isJumping = false;
                    isDying = false;
                }
                catch (Exception e) { activeAnimation.activeFrameNumber = 0; }
            }

        }

        private void hang()
        {
            if (!isScriptedMoving)
            {
                actScriptedMove = "hang";
                isScriptedMoving = true;
                isIdle = false;
                isRunning = false;
                isJumping = false;
                isDying = false;
                isFalling = false;

                charRect.Body.BodyType = BodyType.Static;
                charRect.Body.IgnoreGravity = true;

                if (facing == 0)
                {
                    activeAnimation = hang_left;
                    activeAnimation.activeFrameNumber = 0;
                    activeAnimation.start();
                    nextAnimation = hang2_left;
                }
                else
                {
                    activeAnimation = hang_right;
                    activeAnimation.activeFrameNumber = 0;
                    activeAnimation.start();
                    nextAnimation = hang2_right;
                }
            }

            if ((activeAnimation == hang_right || activeAnimation == hang_left) && activeAnimation.activeFrameNumber < activeAnimation.Amount - 1)
            {
                float maxHeight = 700/ Level.PixelPerMeter;
                if (charRect.Body.Position.Y < maxHeight)
                {
                    charRect.Body.Position += new Vector2(0, -10 / Level.PixelPerMeter);
                    if (Camera.fixedOnPlayer) { camRect.Body.Position += new Vector2(1.35f / Level.PixelPerMeter, 0); }
                }
            }

            if ((activeAnimation == hang_right || activeAnimation == hang_left) &&activeAnimation.activeFrameNumber == activeAnimation.Amount - 1)
            {
                activeAnimation = hang2_left;
                activeAnimation.start();
            }

            if (activeAnimation == hang2_left)
            { 
                if (kState.IsKeyDown(Keys.Space))
                {
                    activeAnimation = pullup;
                    activeAnimation.activeFrameNumber = 0;
                    activeAnimation.start();
                }
            }

            if (activeAnimation == pullup)
            {
                if (activeAnimation.activeFrameNumber >=8 && activeAnimation.activeFrameNumber < activeAnimation.Amount - 1)
                {
                    charRect.Body.Position += new Vector2(-10f / Level.PixelPerMeter, -5f / Level.PixelPerMeter);
                    if (Camera.fixedOnPlayer) { camRect.Body.Position += new Vector2(-10f / Level.PixelPerMeter, -5f / Level.PixelPerMeter); }
                }

                else if (activeAnimation.activeFrameNumber == activeAnimation.Amount - 1)
                {
                    try
                    {
                        activeAnimation = falling_left;
                        activeAnimation.activeFrameNumber = 0;
                        activeAnimation.start();
                        actScriptedMove = "";
                        charRect.Body.BodyType = BodyType.Dynamic;
                        charRect.Body.IgnoreGravity = false;
                        isScriptedMoving = false;
                        isFalling = true;
                    }
                    catch (Exception e)
                    {
                        charRect.Body.Position = Vector2.Zero;
                        charRect.Body.BodyType = BodyType.Dynamic;
                        charRect.Body.IgnoreGravity = false;
                        isScriptedMoving = false;
                        isIdle = true;
                    }
                }
            }
        }

        private void die()
        {
            //if (!isScriptedMoving)
            if (!isUncontrollableMoving)
            {
                actUncontrollableMove = "die";
                isUncontrollableMoving = true;
                controlsEnabled = false;
                isIdle = false;
                isRunning = false;
                isJumping = false;
                isDying = true;
                isFalling = false;

                //charRect.Body.BodyType = BodyType.Static;
                //charRect.Body.IgnoreGravity = true;

                if (facing == 0)
                {
                    activeAnimation = dying_left;
                    activeAnimation.activeFrameNumber = 0;
                    activeAnimation.start();
                    nextAnimation = dying2_left;
                }
                else
                {
                    activeAnimation = dying_right;
                    activeAnimation.activeFrameNumber = 0;
                    activeAnimation.start();
                    nextAnimation = dying2_right;
                }
            }
            // Hier wird noch gebessert

            if (activeAnimation.activeFrameNumber == activeAnimation.Amount - 1)
            {
                try
                {
                    if (facing == 0) { activeAnimation = dying2_left; }
                    else { activeAnimation = dying2_right; }
                    activeAnimation.activeFrameNumber = 0;
                    activeAnimation.start();
                    nextAnimation = choseIdleAnimation();
                }
                catch (Exception e) { activeAnimation.activeFrameNumber = 0; }
            }

        }

        private void UpdateTexture(GameTime gameTime)
        {
            activeAnimation.Update(gameTime);
            activeAnimation.UpdatePosition(position);
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

            if (activeAnimation == running_left && kState.IsKeyUp(Keys.Left))
            {
                activeAnimation = runStopping_left;
                activeAnimation.activeFrameNumber = 0;
                activeAnimation.start();
                nextAnimation = choseIdleAnimation();
            }

            if (activeAnimation == running_right && kState.IsKeyUp(Keys.Right))
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

            if (isRunning && !(activeAnimation == running_left ||
                                activeAnimation == running_right ||
                                activeAnimation == runStarting_left ||
                                activeAnimation == runStarting_right ||
                                activeAnimation == runStopping_left ||
                                activeAnimation == runStopping_right ||
                                activeAnimation == noJumpRunning_left ||
                                activeAnimation == noJumpRunning_right ||
                                activeAnimation != landing_right ||
                                activeAnimation != landing_left ||
                                activeAnimation != idle_left ||
                                activeAnimation != idle_right ||
                                activeAnimation != idleb_left ||
                                activeAnimation != idleb_right||
                                activeAnimation != idlec_left ||
                                activeAnimation != idlec_right))
            {
                if (facing == 0)
                {
                    activeAnimation = running_left;
                }
                else
                {
                    activeAnimation = running_right;
                }
            }

            // STERBEN

            if (activeAnimation == dying_left)
            {
                nextAnimation = dying2_left;
            }

            if (activeAnimation == dying_right)
            {
                nextAnimation = dying2_right;
            }


            if (activeAnimation.activeFrameNumber == activeAnimation.Amount - 1 &&
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
                activeAnimation = choseIdleAnimation();
                activeAnimation.activeFrameNumber = 0;
                activeAnimation.start();
            }

            if (activeAnimation.activeFrameNumber == activeAnimation.Amount - 1 && !activeAnimation.Looped)
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
                nextAnimation = choseIdleAnimation();
            }
        }

        public void ObserveMovement()
        {
            movement = charRect.Body.GetLinearVelocityFromLocalPoint(Vector2.Zero);

            if (isLanding)
            {
                charRect.Friction = 3;
            }

            if (Math.Max(oldPosition.X, charRect.Body.Position.X) - Math.Min(oldPosition.X, charRect.Body.Position.X) < 0.0001 &&
                Math.Max(oldPosition.Y, charRect.Body.Position.Y) - Math.Min(oldPosition.Y, charRect.Body.Position.Y) < 0.0001)
            {
                isIdle = true;
                isFalling = false;
                isRunning = false;
                isJumping = false;

                charRect.Friction = 4;
            }

            if (movement.Y + Level.Physics.Gravity.Y < 0.05f && !rectTouching)
            {
                isIdle = false;
                isFalling = false;
                isRunning = false;
                isJumping = true;
            }

            if (isJumping && oldPosition.Y < charRect.Body.Position.Y && !rectTouching)
            {
                isIdle = false;
                isFalling = true;
                isRunning = false;
                isJumping = false;
            }



            if (movement.Y > 2.50f && !isFalling && !isLanding)
            {
                if (!rectTouching && !sRectTouching &&
                    activeAnimation != landing_left && activeAnimation != landing_right &&
                    activeAnimation != falling_right && activeAnimation != falling_left)
                {
                    isIdle = false;
                    isFalling = true;
                    isRunning = false;
                    isJumping = false;

                    if (facing == 0)
                    {
                        activeAnimation = falling_left;
                        activeAnimation.activeFrameNumber = 0;
                        activeAnimation.start();
                        nextAnimation = landing_left;
                    }
                    else
                    {
                        activeAnimation = falling_right;
                        activeAnimation.activeFrameNumber = 0;
                        activeAnimation.start();
                        nextAnimation = landing_right;
                    }
                }
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

                if (activeAnimation == jumpStarting_right)
                {
                    activeAnimation = jumpStarting_left;
                }

                if (activeAnimation == falling_right)
                {
                    activeAnimation = falling_left;
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

                if (activeAnimation == jumpStarting_left)
                {
                    activeAnimation = jumpStarting_right;
                }

                if (activeAnimation == falling_left)
                {
                    activeAnimation = falling_right;
                }
            }
        }

        private void ObserveTimer(GameTime gameTime)
        {
            if (isDying)
            {
                resetTimer -= gameTime.ElapsedGameTime.Milliseconds;

                if (resetTimer < 0)
                {
                    resetTimer = 2000;
                    reset();
                }
            }

            // Schaut, ob der Player sich gerade erinnert. Wenn ja, wird Timer erniedrigt, Modus beendet nach Ablauf.
            if (isRemembering)
            {
                if (sJTimer <= 0)
                {
                    // Wenn Tom erinnert, aber der sJTimer abgelaufen ist, fängt er an zu recovern
                    isRemembering = false;
                    isRecovering = true;
                }
                else
                {
                    // Ansonsten wird der sJTimer runtergezählt
                    sJTimer -= gameTime.ElapsedGameTime.Milliseconds;
                }

                if (fadeOrange <= 1000 && sJTimer >= 500)
                {
                    fadeOrange += gameTime.ElapsedGameTime.Milliseconds;
                }
                if (sJTimer <= 500)
                {
                    fadeOrange -= gameTime.ElapsedGameTime.Milliseconds;
                }
            }
            else if (isRecovering)
            {
                if (sJRecoveryTimer <= 0)
                {
                    // Wenn Tom recovered, aber der Timer abgelaufen ist, isser wieder normal. Werte zurücksetzen net vergessen
                    isRecovering = false;
                    sJRecoveryTimer = 17000;
                    sJTimer = 6000;
                    fadeOrange = 0;
                    fadeBlue = 0;
                }
                else
                {
                    // Ansonsten wird der sJRecoveryTimer runtergezählt
                    sJRecoveryTimer -= gameTime.ElapsedGameTime.Milliseconds;
                }

                if (fadeBlue <= 1000 && sJRecoveryTimer >= 1000)
                {
                    fadeBlue += gameTime.ElapsedGameTime.Milliseconds;

                    if (fadeOrange >= 0)
                    {
                        fadeOrange -= gameTime.ElapsedGameTime.Milliseconds;
                    }
                }
                if (fadeBlue >= 0 && sJRecoveryTimer <= 1000)
                {
                    fadeBlue -= gameTime.ElapsedGameTime.Milliseconds;
                }
            }
        }

        public void calcRotation(GameTime gameTime)
        {
            if (charRect.Body.Rotation < 0.5f && charRect.Body.Rotation > -0.5f)
            { tempRotation = charRect.Body.Rotation; }
        }

        public void reset()
        {
            try
            {
                isScriptedMoving = false;
                isUncontrollableMoving = false;
                actScriptedMove = "";
                actUncontrollableMove = "";
                isIdle = true;
                isDying = false;
                isFalling = false;
                isRecovering = false;
                isRemembering = false;
                isJumping = false;
                charRect.Body.Position = GameStateManager.Default.currentLevel.startPosition / Level.PixelPerMeter;
                charRect.Body.BodyType = BodyType.Dynamic;
                charRect.Body.IgnoreGravity = false;
                camRect.Body.Position = GameStateManager.Default.currentLevel.startPosition / Level.PixelPerMeter;

                    isRecovering = false;
                    isRemembering = false;
                    sJRecoveryTimer = 17000;
                    sJTimer = 6000;
                    fadeOrange = 0;
                    fadeBlue = 0;
            }
            catch (Exception e)
            {
                isScriptedMoving = false;
                isUncontrollableMoving = false;
                actScriptedMove = "";
                actUncontrollableMove = "";
                isIdle = true;
                isDying = false;
                isFalling = false;
                isRecovering = false;
                isRemembering = false;
                isJumping = false;
                charRect.Body.Position = Vector2.Zero;
                charRect.Body.BodyType = BodyType.Dynamic;
                charRect.Body.IgnoreGravity = false;
                camRect.Body.Position = Vector2.Zero;

                    isRecovering = false;
                    isRemembering = false;
                    sJRecoveryTimer = 17000;
                    sJTimer = 6000;
                    fadeOrange = 0;
                    fadeBlue = 0;
            }
        
        }

        public bool OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            rectTouching = true;

            if (isFalling && (activeAnimation != landing_right && activeAnimation != landing_left) && !fixtureB.IsSensor && kState.GetPressedKeys().Length == 0)
            {
                isFalling = false;
                isJumping = false;
                isIdle = true;
                if (facing == 0)
                {
                    activeAnimation = landing_left;
                    activeAnimation.activeFrameNumber = 4;
                    activeAnimation.start();
                }
                else
                {
                    activeAnimation = landing_right;
                    activeAnimation.activeFrameNumber = 4;
                    activeAnimation.start();
                }
            }

            else if ((isFalling || isLanding) && kState.IsKeyDown(Keys.Left) && !fixtureB.IsSensor)
            {
                isFalling = false;
                isJumping = false;
                isIdle = false;
                isRunning = true;
                    activeAnimation = running_left;
                    activeAnimation.activeFrameNumber = 0;
                    activeAnimation.start();
            }

            else if ((isFalling || isLanding) && kState.IsKeyDown(Keys.Right) && !fixtureB.IsSensor)
            {
                isFalling = false;
                isJumping = false;
                isIdle = false;
                isRunning = true;
                    activeAnimation = running_right;
                    activeAnimation.activeFrameNumber = 0;
                    activeAnimation.start();
            }

            if (fixtureB.isDeadly && fixtureB.Body.Active)
            {
                die();
            }

            return true;
        }

        public void OnSeperation(Fixture fixtureA, Fixture fixtureB)
        {

            if (fixtureB.isHalfTransparent && charRect.IsSensor)
            {
                charRect.IsSensor = false;
            }

                rectTouching = false;
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
            if (!isScriptedMoving)
            {
                sRectTouching = true;

                if (isFalling && !fixtureB.IsSensor)
                {
                    isFalling = false;
                    isIdle = false;
                    isLanding = true;
                    isJumping = false;
                    if (facing == 0)
                    {
                        activeAnimation = landing_left;
                        activeAnimation.activeFrameNumber = 6;
                        activeAnimation.start();
                    }
                    else
                    {
                        activeAnimation = landing_right;
                        activeAnimation.activeFrameNumber = 6;
                        activeAnimation.start();
                    }
                }

                if (isLanding && !fixtureB.IsSensor && activeAnimation.activeFrameNumber >= 6)
                {
                    isFalling = false;
                    isLanding = false;

                    if (kState.IsKeyDown(Keys.Left))
                    {
                        isRunning = true;
                        activeAnimation = running_left;
                        activeAnimation.activeFrameNumber = 0;
                        activeAnimation.start();
                    }
                    if (kState.IsKeyDown(Keys.Right))
                    {
                        isRunning = true;
                        activeAnimation = running_right;
                        activeAnimation.activeFrameNumber = 0;
                        activeAnimation.start();
                    }

                    else
                    {
                        isIdle = true;
                        activeAnimation = choseIdleAnimation();
                        activeAnimation.activeFrameNumber = 0;
                        activeAnimation.start();
                    }
                }

                if (charRect.IsSensor == true && !fixtureB.IsSensor && fixtureB.isHalfTransparent)
                {
                    try
                    {
                        charRect.IsSensor = false;
                    }
                    catch (Exception e) { }
                }
            }
                return true;
        }

        public bool landOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (isFalling && !fixtureB.IsSensor && !isScriptedMoving)
            {
                isFalling = false;
                isLanding = true;
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

        public void sOnSeperation(Fixture fixtureA, Fixture fixtureB)
        {
            sRectTouching = false;
        }

        public bool eOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.isClimbable)
            {
                this.canClimb = true;
            }

            return true;
        }

        public void eOnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            if (canClimb)
            {
                canClimb = false;
            }
        }

        public bool wOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.isClimbable)
            {
                this.canClimb = true;
            }

            return true;
        }

        public void wOnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            if (canClimb)
            {
                canClimb = false;
            }
        }

        public void makeMeHang()
        {
            hang();
        }

        public void enableControls(bool b)
        {
            controlsEnabled = b;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(activeAnimation.activeTexture, position, null, Color.White, tempRotation, new Vector2(250, 250), 1, SpriteEffects.None, 1);
            //Das auskommentierte hier kann als Debugview dienen.
            /*
            spriteBatch.DrawString(FontManager.Arial, "Standing: " + isIdle.ToString(), position+new Vector2(300, 20), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "Running: " + isRunning.ToString(), position + new Vector2(300, 45), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "Jumping: " + isJumping.ToString(), position + new Vector2(300, 70), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "Falling: " + isFalling.ToString(), position + new Vector2(300, 95), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, charRect.Body.Position.ToString(), position + new Vector2(300, 120), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "Scriptedmoving: " + isScriptedMoving, position + new Vector2(300, 155), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, actClimbHeight.ToString(), position + new Vector2(300, 180), Color.Black);
            
            spriteBatch.DrawString(FontManager.Arial, "X: " + movement.X.ToString() + " Y: " + movement.Y.ToString(), position + new Vector2(300, 205), Color.Black);
            
            spriteBatch.DrawString(FontManager.Arial, "CamRectRotation: " + camRect.Body.Rotation.ToString(), position + new Vector2(300, 230), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "CamRotation: " + Camera.Rotation.ToString(), position + new Vector2(300, 255), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "tempRotation: " + tempRotation.ToString(), position + new Vector2(300, 280), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "sJTimer: " + sJTimer.ToString() + " sJRecoveryTimer: " + sJRecoveryTimer.ToString(), position + new Vector2(300, 305), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "canClimb: " + canClimb, position + new Vector2(300, 320), Color.Black);
            */
        }
    }
}
