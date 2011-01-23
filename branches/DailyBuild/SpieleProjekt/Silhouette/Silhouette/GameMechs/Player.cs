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
        public float contactTimer;
        public int resetTimer;
        public Vector2 centerPosition;      // Hannes: Rectangles für Kollisionserkennung. Muss noch um Kollisionsgruppe erweitert werden.
        public Vector2 camPosition;
        public Fixture charRect;
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
        public Contact contact;

        public KeyboardState oldState;
        public Vector2 oldPosition;
        public Vector2 movement;            // Bewegung des Characters. Wichtig zur Statusveränderungserkennung.

        private bool isRunning;             // Hannes: Statusveränderungen des Chars. Weitere folgen.
        private bool isJumping;
        private bool isFalling;
        private bool isIdle;
        private bool isScriptedMoving;
        private bool isDying;
        public bool isRemembering;         // So kommt Tom in den Erinnerungszustand
        public bool isRecovering;
        private bool sRectTouching;
        private bool rectTouching;

        private int facing;                    // Wo der Chara hinschaut. 0 bedeutet links, 1 bedeutet rechts.

        public float sJTimer;
        public float sJRecoveryTimer;
        public float fadeBlue;
        public float fadeOrange;

        private String actScriptedMove;

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
            isScriptedMoving = false;
            isRemembering = false;
            sJRecoveryTimer = 17000;
            sJTimer = 6000;
            fadeBlue = 0;
            fadeOrange = 0;
            canClimb = false;
            tempRotation = 0.0f;
            contactTimer = 0;
            resetTimer = 2000;

            facing = 1;
        }

        public override void LoadContent()
        {
            // Hier müssen alle Sprites geladen werden.
            idle_left.Load(6, "Sprites/Player/idleA_left_", 0.5f, true);
            idle_right.Load(6, "Sprites/Player/idleA_right_", 0.5f, true);
            idleb_left.Load(6, "Sprites/Player/idleB_left_", 0.5f, true);
            idleb_right.Load(6, "Sprites/Player/idleB_right_", 0.5f, true);
            idlec_left.Load(6, "Sprites/Player/idleC_left_", 0.5f, true);
            idlec_right.Load(6, "Sprites/Player/idleC_right_", 0.5f, true);

            noJumpIdle_left.Load(2, "Sprites/Player/nojumpI_left_", 0.5f, false);
            noJumpIdle_right.Load(2, "Sprites/Player/nojumpI_right_", 0.5f, false);

            jumpStarting_left.Load(5, "Sprites/Player/jumpStart_left_", 1.5f, false);
            jumpStarting_right.Load(5, "Sprites/Player/jumpStart_right_", 1.5f, false);
            falling_left.Load(2, "Sprites/Player/falling_left_", 0.5f, true);
            falling_right.Load(2, "Sprites/Player/falling_right_", 0.5f, true);
            landing_left.Load(8, "Sprites/Player/landing_left_", 1f, false);
            landing_right.Load(8, "Sprites/Player/landing_right_", 1f, false);
            //1,5

            running_left.Load(8, "Sprites/Player/walk_left_", 1.75f, true);
            running_right.Load(8, "Sprites/Player/walk_right_", 1.75f, true);
            runStarting_left.Load(8, "Sprites/Player/walkStart_left_", 2f, false);
            runStarting_right.Load(8, "Sprites/Player/walkStart_right_", 2f, false);
            runStopping_left.Load(8, "Sprites/Player/walkStop_left_", 1.0f, false);
            runStopping_right.Load(8, "Sprites/Player/walkStop_right_", 1.0f, false);

            noJumpRunning_left.Load(2, "Sprites/Player/nojumpW_left_", 0.5f, false);
            noJumpRunning_right.Load(2, "Sprites/Player/nojumpW_right_", 0.5f, false);

            climbing_left.Load(21, "Sprites/Player/climb_left_", 0.75f, false);
            climbing_right.Load(21, "Sprites/Player/climb_right_", 0.75f, false);

            hang_left.Load(5, "Sprites/Player/hang_left_", 0.5f, false);
            hang_right.Load(5, "Sprites/Player/hang_right_", 0.5f, false);
            hang2_left.Load(1, "Sprites/Player/hang2_left_", 1f, false);
            hang2_right.Load(1, "Sprites/Player/hang2_right_", 1f, false);
            pullup.Load(13, "Sprites/Player/pullup_left_", 1f, false);

            dying_left.Load(4, "Sprites/Player/die_left_", 0.5f, false);
            dying_right.Load(4, "Sprites/Player/die_right_", 0.5f, false);
            dying2_left.Load(1, "Sprites/Player/die2_left_", 0.1f, false);
            dying2_right.Load(1, "Sprites/Player/die2_right_", 0.1f, false);

            activeAnimation = choseIdleAnimation();
            activeAnimation.start();
            nextAnimation = choseIdleAnimation();

            charRect = FixtureManager.CreatePolygon(idle_left.pictures[0], new Vector2(0.85f, 0.95f), BodyType.Dynamic, position, 1);
            charRect.Friction = 1;
            charRect.isPlayer = true;

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

            eRect = FixtureManager.CreateRectangle(15, 100, new Vector2(position.X + 105, position.Y), BodyType.Dynamic, 0);
            eRect.Body.FixedRotation = true;
            eRect.IsSensor = true;
            eRect.isPlayer = true;
            eRect.isEvent = true;

            wRect = FixtureManager.CreateRectangle(15, 100, new Vector2(position.X - 110, position.Y), BodyType.Dynamic, 0);
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

            joint0 = new RopeJoint(charRect.Body, camRect.Body, new Vector2(100 / Level.PixelPerMeter, 80 / Level.PixelPerMeter) * 2, new Vector2(50 / Level.PixelPerMeter, 50 / Level.PixelPerMeter));
            joint1 = new RopeJoint(charRect.Body, camRect.Body, new Vector2(-100 / Level.PixelPerMeter, -80 / Level.PixelPerMeter) * 2, new Vector2(-50 / Level.PixelPerMeter, -50 / Level.PixelPerMeter));
            joint2 = new RopeJoint(charRect.Body, camRect.Body, new Vector2(100 / Level.PixelPerMeter, -80 / Level.PixelPerMeter) * 2, new Vector2(50 / Level.PixelPerMeter, -50 / Level.PixelPerMeter));
            joint3 = new RopeJoint(charRect.Body, camRect.Body, new Vector2(-100 / Level.PixelPerMeter, 80 / Level.PixelPerMeter) * 2, new Vector2(-50 / Level.PixelPerMeter, 50 / Level.PixelPerMeter));
            joint4 = JointFactory.CreateAngleJoint(Level.Physics, charRect.Body, sRect.Body);
            joint4.Softness = 0.999f;

            joint0.MaxLength = 2.15f;
            joint1.MaxLength = 2.15f;
            joint2.MaxLength = 2.15f;
            joint3.MaxLength = 2.15f;


            Level.Physics.AddJoint(joint0);
            Level.Physics.AddJoint(joint1);
            Level.Physics.AddJoint(joint2);
            Level.Physics.AddJoint(joint3);

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
            // Die Camera wird nur rotiert, wenn die Rotation unter einem bestimmten Winkel bleibt. Damit das nicht ausartet.
            //if (camRect.Body.Rotation >= -0.1f && camRect.Body.Rotation <= 0.1f) { Camera.Rotation = camRect.Body.Rotation; }
        }

        private void UpdateControls(GameTime gameTime)
        {
            // LEFT ARROW
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && !isFalling && !isJumping && movement.X >= -3.3f)
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

            if (Keyboard.GetState().IsKeyDown(Keys.Left) && (isFalling || isJumping) && movement.X >= -2.5f && !rectTouching)
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
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && !isFalling && !isJumping && movement.X <= 3.3f)
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

            if (Keyboard.GetState().IsKeyDown(Keys.Right) && (isFalling || isJumping) && movement.X <= 2.5f && !rectTouching)
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
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && !isScriptedMoving && canClimb)
            {
                climb();
            }

            // P BUTTON
            if (Keyboard.GetState().IsKeyDown(Keys.P) && !isScriptedMoving)
            {
                die();
            }

            // O BUTTON
            if (Keyboard.GetState().IsKeyDown(Keys.O) && !isRemembering && !isRecovering)
            {
                isRemembering = true;
            }

            /*if (Keyboard.GetState().IsKeyDown(Keys.H))
            {
                hang();
            }*/

            // SPACE BUTTON
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space)  && (isIdle || isRunning))
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
                        charRect.Body.ApplyForce(new Vector2(-50, -1650));
                    }
                    else
                    {
                        charRect.Body.ApplyForce(new Vector2(-50, -1400));
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
                        charRect.Body.ApplyForce(new Vector2(50, -1650));
                    }
                    else
                    {
                        charRect.Body.ApplyForce(new Vector2(50, -1400));
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
                            nextAnimation.activeFrameNumber = 0;
                        }
                        else if (facing == 1)
                        {
                            activeAnimation = noJumpIdle_right;
                            activeAnimation.activeFrameNumber = 0;
                            activeAnimation.start();
                            nextAnimation = choseIdleAnimation();
                            nextAnimation.activeFrameNumber = 0;
                        }
                    }
                }
            }

            // Für den Fall, dass man den Lauf abbricht, wird abgebremst
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

            // WENN KEIN BUTTON GEDRÜCKT IST
            if (Keyboard.GetState().GetPressedKeys().Length == 0 && oldState.GetPressedKeys().Length == 0)
            {
                charRect.Friction = 4;
            }

            else { charRect.Friction = 0.1f; }

            oldState = Keyboard.GetState();
            oldPosition = charRect.Body.Position;
        }

        private void doScriptedMove()
        {
            if (actScriptedMove.Equals("climb") && isScriptedMoving)
            {
                climb();
            }

            if (actScriptedMove.Equals("die") && isScriptedMoving)
            {
                die();
            }

            if (actScriptedMove.Equals("hang") && isScriptedMoving)
            {
                hang();
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
            if (activeAnimation.activeFrameNumber < activeAnimation.amount - 1)
            {
                int temp = activeAnimation.activeFrameNumber;
                actClimbHeight = temp;
                if (facing == 1)
                {
                    if (activeAnimation.activeFrameNumber <= 6 || activeAnimation.activeFrameNumber >= 14)
                    {
                        charRect.Body.Position += new Vector2(1.35f / Level.PixelPerMeter, 0);
                        if (Camera.fixedOnPlayer) { camRect.Body.Position += new Vector2(1.35f / Level.PixelPerMeter, 0); }
                    }
                    if (activeAnimation.activeFrameNumber > 6 && activeAnimation.activeFrameNumber < 14)
                    {
                        charRect.Body.Position += new Vector2(0, -2.2f / Level.PixelPerMeter);
                        if (Camera.fixedOnPlayer) { camRect.Body.Position += new Vector2(0, -2.2f / Level.PixelPerMeter); }
                    }
                }
                else
                {
                    if (activeAnimation.activeFrameNumber <= 6 || activeAnimation.activeFrameNumber >= 14)
                    {
                        charRect.Body.Position += new Vector2(-1.35f / Level.PixelPerMeter, 0);
                        if (Camera.fixedOnPlayer) { camRect.Body.Position += new Vector2(-1.35f / Level.PixelPerMeter, 0); }
                    }
                    if (activeAnimation.activeFrameNumber > 6 && activeAnimation.activeFrameNumber < 14)
                    {
                        charRect.Body.Position += new Vector2(0, -2.2f / Level.PixelPerMeter);
                        if (Camera.fixedOnPlayer) { camRect.Body.Position += new Vector2(0, -2.2f / Level.PixelPerMeter); }
                    }
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

            if ((activeAnimation == hang_right || activeAnimation == hang_left) && activeAnimation.activeFrameNumber < activeAnimation.amount - 1)
            {
                float maxHeight = 700/ Level.PixelPerMeter;
                if (charRect.Body.Position.Y < maxHeight)
                {
                    charRect.Body.Position += new Vector2(0, -10 / Level.PixelPerMeter);
                    if (Camera.fixedOnPlayer) { camRect.Body.Position += new Vector2(1.35f / Level.PixelPerMeter, 0); }
                }
            }

            if ((activeAnimation == hang_right || activeAnimation == hang_left) &&activeAnimation.activeFrameNumber == activeAnimation.amount - 1)
            {
                activeAnimation = hang2_left;
                activeAnimation.start();
            }

            if (activeAnimation == hang2_left)
            { 
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    activeAnimation = pullup;
                    activeAnimation.activeFrameNumber = 0;
                    activeAnimation.start();
                }
            }

            if (activeAnimation == pullup)
            {
                if (activeAnimation.activeFrameNumber >=8 && activeAnimation.activeFrameNumber < activeAnimation.amount - 1)
                {
                    charRect.Body.Position += new Vector2(-10f / Level.PixelPerMeter, -5f / Level.PixelPerMeter);
                    if (Camera.fixedOnPlayer) { camRect.Body.Position += new Vector2(-10f / Level.PixelPerMeter, -5f / Level.PixelPerMeter); }
                }

                else if (activeAnimation.activeFrameNumber == activeAnimation.amount - 1)
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
            if (!isScriptedMoving)
            {
                actScriptedMove = "die";
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
                    activeAnimation = dying_left;
                    activeAnimation.activeFrameNumber = 0;
                    activeAnimation.start();
                    nextAnimation = dying_left;
                }
                else
                {
                    activeAnimation = dying_right;
                    activeAnimation.activeFrameNumber = 0;
                    activeAnimation.start();
                    nextAnimation = dying_right;
                }
            }
            // Hier wird noch gebessert

            if (activeAnimation.activeFrameNumber == activeAnimation.amount - 1)
            {
                try
                {
                    if (facing == 0) { activeAnimation = dying2_left; }
                    else { activeAnimation = dying2_right; }
                    activeAnimation.activeFrameNumber = 0;
                    activeAnimation.start();

                    /*activeAnimation.activeFrameNumber = 0;
                    activeAnimation = choseIdleAnimation();
                    activeAnimation.activeFrameNumber = 0;
                    activeAnimation.start();
                    actScriptedMove = "";
                    charRect.Body.BodyType = BodyType.Dynamic;
                    charRect.Body.IgnoreGravity = false;
                    isScriptedMoving = false;
                    isIdle = true;
                    isRunning = false;
                    isFalling = false;
                    isJumping = false;
                    isDying = false;*/



                }
                catch (Exception e) { activeAnimation.activeFrameNumber = 0; }
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

            if (isRunning && !(activeAnimation == running_left ||
                                activeAnimation == running_right ||
                                activeAnimation == runStarting_left ||
                                activeAnimation == runStarting_right ||
                                activeAnimation == runStopping_left ||
                                activeAnimation == runStopping_right ||
                                activeAnimation == noJumpRunning_left ||
                                activeAnimation == noJumpRunning_right ||
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
                activeAnimation = choseIdleAnimation();
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
            //movement = charRect.Body.GetLinearVelocityFromWorldPoint(Vector2.Zero);
            movement = charRect.Body.GetLinearVelocityFromLocalPoint(Vector2.Zero);

            if (Math.Max(oldPosition.X, charRect.Body.Position.X) - Math.Min(oldPosition.X, charRect.Body.Position.X) < 0.0001 &&
                Math.Max(oldPosition.Y, charRect.Body.Position.Y) - Math.Min(oldPosition.Y, charRect.Body.Position.Y) < 0.0001)
            {
                isIdle = true;
                isFalling = false;
                isRunning = false;
                isJumping = false;

                charRect.Friction = 4;
            }

            else if (movement.Y + Level.Physics.Gravity.Y < 0.05f && !rectTouching)
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



            else if (movement.Y > 1.5f && !isFalling && !sRectTouching && !rectTouching && (activeAnimation != jumpStarting_left || activeAnimation != jumpStarting_right || activeAnimation != landing_left || activeAnimation != landing_right))
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
            isScriptedMoving = false;
            actScriptedMove = "";
            isIdle = true;
            isDying = false;
            isFalling = false;
            isRecovering = false;
            isRemembering = false;
            isJumping = false;
            position = GameStateManager.Default.currentLevel.startPosition;
            charRect.Body.BodyType = BodyType.Dynamic;
            camPosition = GameStateManager.Default.currentLevel.startPosition;
        
        }

        public bool OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            this.contact = contact;
            rectTouching = true;

            if (isFalling && (activeAnimation != landing_right && activeAnimation != landing_left))
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

            if (fixtureB.isDeadly && fixtureB.Body.Active)
            {
                die();
            }

            return true;
        }

        public void OnSeperation(Fixture fixtureA, Fixture fixtureB)
        {
            if (canClimb)
            {
                canClimb = false;
            }

            if (this.contact != null && this.contact.IsTouching())
            {
                rectTouching = true;
            }

            else if (this.contact != null && !this.contact.IsTouching())
            {
                rectTouching = false;
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
            sRectTouching = true;

            if (isFalling && (activeAnimation != landing_right && activeAnimation != landing_left))
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

        public bool landOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (isFalling)
            {

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

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(activeAnimation.activeTexture, position, null, Color.White, tempRotation, new Vector2(250, 250), 1, SpriteEffects.None, 1);
            //Das auskommentierte hier kann als Debugview dienen.
            /*
            spriteBatch.DrawString(FontManager.Arial, "Standing: " + isIdle.ToString(), new Vector2(300, 20), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "Running: " + isRunning.ToString(), new Vector2(300, 45), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "Jumping: " + isJumping.ToString(), new Vector2(300, 70), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "Falling: " + isFalling.ToString(), new Vector2(300, 95), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, charRect.Body.Position.ToString(), new Vector2(300, 120), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "Scriptedmoving: " + isScriptedMoving, new Vector2(300, 155), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, actClimbHeight.ToString(), new Vector2(300, 180), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "X: " + movement.X.ToString() + " Y: " + movement.Y.ToString(), new Vector2(300, 205), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "CamRectRotation: " + camRect.Body.Rotation.ToString(), new Vector2(300, 230), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "CamRotation: " + Camera.Rotation.ToString(), new Vector2(300, 255), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "tempRotation: " + tempRotation.ToString(), new Vector2(300, 280), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "sJTimer: " + sJTimer.ToString() + " sJRecoveryTimer: " + sJRecoveryTimer.ToString(), new Vector2(300, 305), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "canClimb: " + canClimb, new Vector2(300, 320), Color.Black);
            */
        }
    }
}
