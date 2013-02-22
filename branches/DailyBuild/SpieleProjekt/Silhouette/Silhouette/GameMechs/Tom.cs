using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Controllers;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Silhouette.Engine;
using Silhouette.Engine.Manager;

namespace Silhouette.GameMechs
{
    public class Tom : DrawableLevelObject
    {
        #region Enums
        private enum PlayerState
        { 
            Idle,
            WalkStart,
            Walk,
            WalkStop,
            JumpStart,
            Jump,
            Fall,
            Landing,
            Script
        }

        private enum FacingState
        {
            Right,
            Left
        }

        private enum SuperpowerState
        {
            Remembers,
            Regrets,
            None
        }
        #endregion
        #region States
        private PlayerState _state;
        private PlayerState State
        {
            get { return _state; }
            set { _state = value; }
        }
        
        private FacingState _facing;
        private FacingState Facing
        {
            get { return _facing; }
            set { _facing = value; }
        }

        private SuperpowerState _superpower;
        private SuperpowerState Superpower
        {
            get { return _superpower; }
            set { _superpower = value; }
        }
        #endregion
        #region Properties
        private static Vector2 WALKSPEEDRIGHT = new Vector2(40, 0);
        private static Vector2 WALKSPEEDLEFT = new Vector2(-40, 0);
        private static Vector2 JUMPVECTOR = new Vector2(0, -850);
        private static Vector2 JUMPVECTORSUPER = new Vector2(0, -1100);
        private static int VELOCITYXTHRESH = 4;
        #endregion
        #region Fields
        private Vector2 _origin;
        public Vector2 Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        private bool _canClimb;
        public bool CanClimb
        {
            get { return _canClimb; }
            set { _canClimb = value; }
        }

        private Fixture _charFix;
        public Fixture CharFix
        {
            get { return _charFix; }
            set { _charFix = value; }
        }

        private Fixture _camFix;
        public Fixture CamFix
        {
            get { return _camFix; }
            set { _camFix = value; }
        }

        private Fixture sRect;
        private Fixture nRect;
        private Fixture eRect;
        private Fixture wRect;
        private Fixture landRect;

        private Animation _currentAnimation;
        public Animation CurrentAnimation
        {
            get { return _currentAnimation; }
            set { _currentAnimation = value; }
        }

        private KeyboardState _kState;
        public KeyboardState KState
        {
          get { return _kState; }
          set { _kState = value; }
        }


        #endregion
        #region Animations
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
        #endregion
        #region Altlasten
        public float sJTimer = 0;
        public float sJRecoveryTimer = 0;
        public float fadeBlue = 0;
        public float fadeOrange = 0;
        #endregion
        #region Delegates

        public delegate void InputDelegate();
        private InputDelegate InputHandler;
        private InputDelegate InputHandlerIdle;
        private InputDelegate InputHandlerWalk;
        private InputDelegate InputHandlerJump;

        public void HandleInputIdle()
        { 
            if (KState.IsKeyDown(Keys.Left))
            {
                if (Facing == FacingState.Right)
                {
                    Facing = FacingState.Left;
                }
                else
                {
                    SetState(PlayerState.WalkStart);
                    MoveLeft();
                }
            }
            else if (KState.IsKeyDown(Keys.Right))
            {
                if (Facing == FacingState.Left)
                {
                    Facing = FacingState.Right;
                }
                else
                {
                    SetState(PlayerState.WalkStart);
                    MoveRight();
                }
            }
            if (KState.IsKeyDown(Keys.Space))
            {
                SetState(PlayerState.JumpStart);
                Jump();
            }
            
        }

        public void HandleInputWalk()
        {
            Vector2 movement = CharFix.Body.LinearVelocity;
            int movX = (int)movement.X;

            if (KState.IsKeyDown(Keys.Space))
            {
                SetState(PlayerState.JumpStart);
                Jump(movX);
            }
            else if (KState.IsKeyDown(Keys.Left))
            {
                if (Facing == FacingState.Right)
                {
                    Stop();
                }
                else
                {
                    MoveLeft();
                }
            }
            else if (KState.IsKeyDown(Keys.Right))
            {
                if (Facing == FacingState.Left)
                {
                    Stop();
                }
                else
                {
                    MoveRight();
                }
            }
            else
            {
                Stop();
            }
        }

        public void HandleInputJump()
        {
        }

        #endregion

        private void MoveLeft()
        {
            Vector2 movement = CharFix.Body.LinearVelocity;
            int movX = (int)movement.X;
            int movY = (int)movement.Y;

            if (movX < VELOCITYXTHRESH && movX > -VELOCITYXTHRESH)
            {
                CharFix.Body.ApplyForce(ref WALKSPEEDLEFT);
            }
        }

        private void MoveRight()
        {
            Vector2 movement = CharFix.Body.LinearVelocity;
            int movX = (int)movement.X;
            int movY = (int)movement.Y;

            if (movX < VELOCITYXTHRESH && movX > -VELOCITYXTHRESH)
            {
                CharFix.Body.ApplyForce(ref WALKSPEEDRIGHT);
            }
        }

        private void Jump()
        {
            if (Superpower == SuperpowerState.Remembers)
            {
                Jump(0);
            }
            else if (Superpower == SuperpowerState.None)
            {
                Jump(0);
            }

        }
        private void Jump(int yForce)
        {
            if (Superpower == SuperpowerState.Remembers)
            {
                CharFix.Body.ApplyForce(ref JUMPVECTORSUPER);
                Vector2 vertMov = new Vector2(0, yForce);
                CharFix.Body.ApplyLinearImpulse(ref vertMov);
            }
            else if (Superpower == SuperpowerState.None)
            {
                CharFix.Body.ApplyForce(ref JUMPVECTOR);
                Vector2 vertMov = new Vector2(0, yForce);
                CharFix.Body.ApplyLinearImpulse(ref vertMov);
            }

        }

        private void Stop()
        {
            if (State != PlayerState.WalkStop)
            {
                SetState(PlayerState.WalkStop);
            }
            CharFix.Body.LinearVelocity *= 0.25f;

            if (CharFix.Body.LinearVelocity.X < 0.2f)
            {
                SetState(PlayerState.Idle);
            }
        }

        private void Reset()
        {
            SetState(PlayerState.Fall);
            CharFix.Body.SleepingAllowed = false;
            position = GameStateManager.Default.currentLevel.startPosition * Level.PixelPerMeter;
            CharFix.Body.Position = GameStateManager.Default.currentLevel.startPosition / Level.PixelPerMeter;
            CamFix.Body.Position = GameStateManager.Default.currentLevel.startPosition / Level.PixelPerMeter;
        }

        public override void Initialise()
        {
            InputHandlerIdle += HandleInputIdle;
            InputHandlerWalk += HandleInputWalk;
            InputHandlerJump += HandleInputJump;

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

            Facing = FacingState.Right;
            State = PlayerState.Idle;
            Superpower = SuperpowerState.None;
            KState = Keyboard.GetState();
            SetState(PlayerState.Idle);
        }

        public override void LoadContent()
        {
            // Hier müssen alle Sprites geladen werden.
            idle_left.Load(6, "Sprites/Player/idleA_left_", 10, false);
            idle_right.Load(6, "Sprites/Player/idleA_right_", 10, false);
            idleb_left.Load(6, "Sprites/Player/idleB_left_", 10, false);
            idleb_right.Load(6, "Sprites/Player/idleB_right_", 10, false);
            idlec_left.Load(6, "Sprites/Player/idleC_left_", 10, false);
            idlec_right.Load(6, "Sprites/Player/idleC_right_", 10, false);

            noJumpIdle_left.Load(2, "Sprites/Player/nojumpI_left_", 10, false);
            noJumpIdle_right.Load(2, "Sprites/Player/nojumpI_right_", 10, false);

            jumpStarting_left.Load(5, "Sprites/Player/jumpStart_left_", 10, false);
            jumpStarting_right.Load(5, "Sprites/Player/jumpStart_right_", 10, false);
            falling_left.Load(2, "Sprites/Player/falling_left_", 10, true);
            falling_right.Load(2, "Sprites/Player/falling_right_", 10, true);
            landing_left.Load(8, "Sprites/Player/landing_left_", 5, false);
            landing_right.Load(8, "Sprites/Player/landing_right_", 5, false);
            //1,5

            running_left.Load(8, "Sprites/Player/walk_left_", 18, true);
            running_right.Load(8, "Sprites/Player/walk_right_", 18, true);
            runStarting_left.Load(8, "Sprites/Player/walkStart_left_", 18, false);
            runStarting_right.Load(8, "Sprites/Player/walkStart_right_", 18, false);
            runStopping_left.Load(8, "Sprites/Player/walkStop_left_", 18, false);
            runStopping_right.Load(8, "Sprites/Player/walkStop_right_", 18, false);

            noJumpRunning_left.Load(2, "Sprites/Player/nojumpW_left_", 10, false);
            noJumpRunning_right.Load(2, "Sprites/Player/nojumpW_right_", 10, false);

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

            CurrentAnimation = idle_right;
            CharFix = FixtureManager.CreatePolygon(idle_left.pictures[0], new Vector2(0.85f, 0.95f), BodyType.Dynamic, position, 1);
            CharFix.Friction = 1;
            CharFix.Restitution = 0;
            CharFix.isPlayer = true;

            CamFix = FixtureManager.CreateRectangle(100, 100, position, BodyType.Dynamic, 0.1f);
            CamFix.Body.IgnoreGravity = true;
            CamFix.Body.FixedRotation = true;
            CamFix.IsSensor = true;
            CamFix.isPlayer = true;

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

            sRect.IgnoreCollisionWith(CharFix);
            sRect.IgnoreCollisionWith(CamFix);
            nRect.IgnoreCollisionWith(CharFix);
            nRect.IgnoreCollisionWith(CamFix);
            nRect.IgnoreCollisionWith(sRect);
            eRect.IgnoreCollisionWith(CharFix);
            eRect.IgnoreCollisionWith(CamFix);
            wRect.IgnoreCollisionWith(CharFix);
            wRect.IgnoreCollisionWith(CamFix);
            CharFix.IgnoreCollisionWith(sRect);
            CharFix.IgnoreCollisionWith(nRect);
            CharFix.IgnoreCollisionWith(eRect);
            CharFix.IgnoreCollisionWith(wRect);
            CharFix.IgnoreCollisionWith(CamFix);

            CharFix.OnSeparation += this.OnSeperation;
            CharFix.OnCollision += this.OnCollision;
            nRect.OnCollision += this.nOnCollision;
            sRect.OnCollision += this.sOnCollision;
            landRect.OnCollision += this.landOnCollision;
            wRect.OnCollision += this.ewOnCollision;
            wRect.OnSeparation += this.ewOnSeparation;
            eRect.OnCollision += this.ewOnCollision;
            eRect.OnSeparation += this.ewOnSeparation;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 movement = CharFix.Body.LinearVelocity;
            int movX = (int)movement.X;
            int movY = (int)movement.Y;
            String velocityString = "Velo: " + movX + " - " + movY;
            spriteBatch.Draw(CurrentAnimation.activeTexture, position, null, Color.White, CharFix.Body.Rotation, new Vector2(250, 250), 1, SpriteEffects.None, 1);
            spriteBatch.DrawString(FontManager.Arial, State.ToString(), position + new Vector2(0, -100), Color.Green);
            spriteBatch.DrawString(FontManager.Arial, velocityString, position + new Vector2(0, -50), Color.Green);
            //spriteBatch.Draw(CurrentAnimation.activeTexture, CharFix.Body.Position / Level.PixelPerMeter, null, Color.White, CharFix.Body.Rotation, new Vector2(250, 250), 1, SpriteEffects.None, 1);
        }

        public override void Update(GameTime gameTime)
        {
            int dt = gameTime.ElapsedGameTime.Milliseconds;
            HandleInput();

            UpdatePositions();
            UpdateAnimation(gameTime);
            ObserveAnimations();
            ObserveRotation();
            ObserveMovement();
        }

        private void UpdatePositions()
        {
            Vector2 centerPosition = new Vector2(CharFix.Body.Position.X * Level.PixelPerMeter, CharFix.Body.Position.Y * Level.PixelPerMeter);
            position = new Vector2(centerPosition.X, centerPosition.Y);
            sRect.Body.Position = CharFix.Body.Position + new Vector2(0, 120 / Level.PixelPerMeter);
            nRect.Body.Position = CharFix.Body.Position + new Vector2(0, -85 / Level.PixelPerMeter);
            wRect.Body.Position = CharFix.Body.Position + new Vector2(-110 / Level.PixelPerMeter, 0);
            eRect.Body.Position = CharFix.Body.Position + new Vector2(105 / Level.PixelPerMeter, 0);
            landRect.Body.Position = CharFix.Body.Position + new Vector2(0, 300 / Level.PixelPerMeter);
            
            UpdateCamera();


        }

        private void UpdateAnimation(GameTime gameTime)
        { 
            CurrentAnimation.Update(gameTime);
        }

        private void ObserveRotation()
        {
            if (CharFix.Body.Rotation > 0.5f)
            {
                CharFix.Body.Rotation = 0.5f;
            }
            else if (CharFix.Body.Rotation < -0.5f)
            {
                CharFix.Body.Rotation = -0.5f;
            }

            if (State == PlayerState.Landing || State == PlayerState.Fall || State == PlayerState.JumpStart)
            {
                CharFix.Body.Rotation *= 0.8f;
            }
        }

        private void ObserveAnimations()
        {
            // Wenn die Animation nicht gelooped ist und zu Ende ist, suchen wir eine neue aus
            if (!CurrentAnimation.Looped && CurrentAnimation.activeFrameNumber == CurrentAnimation.pictures.Count - 1)
            {
                CurrentAnimation.stop();
                if (State == PlayerState.Idle)
                {
                    CurrentAnimation = choseIdleAnimation();
                    CurrentAnimation.start();
                }
                else if (State == PlayerState.JumpStart)
                {
                    SetState(PlayerState.Fall);
                    if (Facing == FacingState.Left)
                    {
                        CurrentAnimation = falling_left;
                        CurrentAnimation.start();
                    }
                    else
                    {
                        CurrentAnimation = falling_right;
                        CurrentAnimation.start();
                    }
                }
                else if (State == PlayerState.WalkStart)
                {
                    SetState(PlayerState.Walk);
                    if (Facing == FacingState.Left)
                    {
                        CurrentAnimation = running_left;
                        CurrentAnimation.start();
                    }
                    else
                    {
                        CurrentAnimation = running_right;
                        CurrentAnimation.start();
                    }
                }
                else if (State == PlayerState.WalkStop)
                {
                    SetState(PlayerState.Idle);
                }
                else if (State == PlayerState.Fall)
                {
                    SetState(PlayerState.Idle);
                }
                else if (State == PlayerState.Landing)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.Right))
                    {
                        SetState(PlayerState.Walk);
                    }
                    else SetState(PlayerState.Idle);
                }
            }
        }

        private void ObserveMovement()
        {            
            Vector2 movement = CharFix.Body.LinearVelocity;
            int movX = (int)movement.X;
            int movY = (int)movement.Y;

            if (CharFix.Body.LinearVelocity.Y > 2 && (State == PlayerState.Walk || State == PlayerState.WalkStart || State == PlayerState.WalkStop))
            {
                SetState(PlayerState.Fall);
            }
            else if (movX == 0 && movY == 0 && (State == PlayerState.Fall))
            {
                SetState(PlayerState.Idle);
            }
        }

        public void UpdateCamera()
        {
            CamFix.Body.Position = CharFix.Body.Position;
            Camera.Position = CamFix.Body.Position * Level.PixelPerMeter;
        }

        private void SetState(PlayerState target)
        {
            if (target == PlayerState.Idle)
            {
                InputHandler = InputHandlerIdle;
                CurrentAnimation = choseIdleAnimation();
            }
            else if (target == PlayerState.WalkStart)
            {
                InputHandler = InputHandlerWalk;
                if (Facing == FacingState.Left)
                {
                    CurrentAnimation = runStarting_left;
                }
                else
                {
                    CurrentAnimation = runStarting_right;
                }
            }
            else if (target == PlayerState.Walk)
            {
                InputHandler = InputHandlerWalk;
                if (Facing == FacingState.Left)
                {
                    CurrentAnimation = running_left;
                }
                else
                {
                    CurrentAnimation = running_right;
                }
            }
            else if (target == PlayerState.WalkStop)
            {
                InputHandler = InputHandlerWalk;
                if (Facing == FacingState.Left)
                {
                    CurrentAnimation = runStopping_left;
                }
                else
                {
                    CurrentAnimation = runStopping_right;
                }
            }
            else if (target == PlayerState.JumpStart)
            {
                InputHandler = InputHandlerJump;
                if (Facing == FacingState.Left)
                {
                    CurrentAnimation = jumpStarting_left;
                }
                else
                {
                    CurrentAnimation = jumpStarting_right;
                }
            }
            else if (target == PlayerState.Fall)
            {
                InputHandler = InputHandlerJump;
                if (Facing == FacingState.Left)
                {
                    CurrentAnimation = falling_left;
                }
                else
                {
                    CurrentAnimation = falling_right;
                }
            }
            else if (target == PlayerState.Landing)
            {
                InputHandler = InputHandlerJump;
                if (Facing == FacingState.Left)
                {
                    CurrentAnimation = landing_left;
                }
                else
                {
                    CurrentAnimation = landing_right;
                }
            }

            CurrentAnimation.start();
            State = target;
        }

        public Animation choseIdleAnimation()
        {
            // Methode dient um zufällig zw. Idle-Animationen zu wechseln.
            double random = new Random().NextDouble();

            if (Facing == FacingState.Left)
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

        private void HandleInput()
        {
            KState = Keyboard.GetState();
            if (KState.IsKeyDown(Keys.R))
            {
                Reset();
            }
            InputHandler();
        }

        public bool OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {

            if ((State == PlayerState.Landing || State == PlayerState.Fall) && !fixtureB.IsSensor)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    SetState(PlayerState.Walk);
                }
                else
                {
                    SetState(PlayerState.Idle);
                }
            }

            return true;
        }

        public void OnSeperation(Fixture fixtureA, Fixture fixtureB)
        {
            if (fixtureB.isHalfTransparent && CharFix.IsSensor)
            {
                CharFix.IsSensor = false;
            }
        }

        private bool landOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (!fixtureB.IsSensor && (State == PlayerState.Fall || State == PlayerState.JumpStart))
            {
                SetState(PlayerState.Landing);
            }
            
            return true;
        }

        public bool nOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.isHalfTransparent)
            {
                CharFix.IsSensor = true;
            }
            return true;

        }

        private bool sOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {

            if (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                SetState(PlayerState.Walk);
            }
            else
            {
                SetState(PlayerState.Idle);
            }

            if (CharFix.IsSensor && !fixtureB.IsSensor && fixtureB.isHalfTransparent)
            {
                try
                {
                    CharFix.IsSensor = false;
                }
                catch (Exception e) { }
            }

            return true;
        }

        private bool ewOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.isClimbable)
            {
                this.CanClimb = true;
            }

            return true;
        }

        public void ewOnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            if (CanClimb)
            {
                CanClimb = false;
            }
        }
    }
}
