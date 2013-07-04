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
using Silhouette.Engine.Effects;

namespace Silhouette.GameMechs
{
    public class Tom : DrawableLevelObject
    {

        #region Enums

        public enum FacingState
        {
            Right,
            Left
        }

        public enum SuperpowerState
        {
            Remembers,
            Regrets,
            None
        }
        #endregion
        #region States
        public PlayerJumpState JumpState;
        public PlayerJumpTryState JumpTryState;
        public PlayerFallingState FallingState;
        public PlayerLandingState LandingFastState;
        public PlayerLandingState LandingState;
        public PlayerIdleState IdleState;
        public PlayerWalkState WalkState;
        public PlayerWalkStartState WalkStartState;
        public PlayerWalkStopState WalkStopState;
        public PlayerHangingState HangingState;
        public PlayerHangingState2 HangingState2;
        public PlayerDyingState DyingState;
        public PlayerClimbingState ClimbingState;

        private PlayerState _state;
        public PlayerState State
        {
            get { return _state; }
            set 
            {
                _state.onUnset();
                _state = value;
                value.onSet(Facing);
            }
        }

        #region Superpowerstuff
        public static int REMEMBERDURATION = 4000;
        public static int REMEMBERCLIMAX = 1500;
        public static int RECOVERDURATION = 8000;
        public static int RECOVERCLIMAX = 2500;
        public static int BACKTONORMALTIME = 600;
        public Interpolator RedInterpolator;
        public Interpolator GreenInterpolator;
        public Interpolator BlueInterpolator;
        public Timer RememberingTimer = new Timer(0, null);
        public Timer RecoveringTimer = new Timer(0, null);
        #endregion

        public FacingState _facing;
        public FacingState Facing
        {
            get { return _facing; }
            set { _facing = value; }
        }

        private SuperpowerState _superpower;
        public SuperpowerState Superpower
        {
            get { return _superpower; }
            set { _superpower = value; }
        }
        #endregion
        #region Properties
        private static Vector2 WALKSPEEDRIGHT = new Vector2(40, 0);
        private static Vector2 WALKSPEEDLEFT = new Vector2(-40, 0);
        private static Vector2 JUMPVECTOR = new Vector2(0, -1250);
        private static Vector2 JUMPVECTORSUPER = new Vector2(0, -2000);
        private static int VELOCITYXTHRESH = 4;
        // Maximale Character-Rotation in radians.
        // 0 bis 6,3 ( 2*PI) entsprechen 0 bis 360 grad
        public static float MAXROTATION = 0.3f;
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

        public Fixture sRect;
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
        private KeyboardState _oldKState;
        public KeyboardState OldKState
        {
            get { return _oldKState; }
            set { _oldKState = value; }
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
        private Animation landing_left_fast;
        private Animation landing_right_fast;

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

        public void MoveLeft()
        {
            Vector2 movement = CharFix.Body.LinearVelocity;
            int movX = (int)movement.X;
            int movY = (int)movement.Y;

            if (movX < VELOCITYXTHRESH && movX > -VELOCITYXTHRESH)
            {
                CharFix.Body.ApplyForce(ref WALKSPEEDLEFT);
            }
        }

        public void MoveRight()
        {
            Vector2 movement = CharFix.Body.LinearVelocity;
            int movX = (int)movement.X;
            int movY = (int)movement.Y;

            if (movX < VELOCITYXTHRESH && movX > -VELOCITYXTHRESH)
            {
                CharFix.Body.ApplyForce(ref WALKSPEEDRIGHT);
            }
        }

        public void Jump()
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

        }
        public void Kill()
        {
            State = DyingState;
        }

        public void Reset()
        {
            this.position = GameStateManager.Default.currentLevel.startPosition;
            this.CharFix.Body.Position = GameStateManager.Default.currentLevel.startPosition / Level.PixelPerMeter;
            this.CharFix.Body.LinearVelocity = Vector2.Zero;
            this.State = IdleState;
        }

        public override void Initialise()
        {

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
            landing_left_fast = new Animation();
            landing_right_fast = new Animation();

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
            Superpower = SuperpowerState.None;
            KState = Keyboard.GetState();
            OldKState = Keyboard.GetState();
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
            landing_left.Load(8, "Sprites/Player/landing_left_", 7, false);
            landing_right.Load(8, "Sprites/Player/landing_right_", 7, false);
            landing_left_fast.Load(8, "Sprites/Player/landing_left_", 25, false);
            landing_right_fast.Load(8, "Sprites/Player/landing_right_", 25, false);

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

            hang_left.Load(5, "Sprites/Player/hang_left_", 8, true);
            hang_right.Load(5, "Sprites/Player/hang_right_", 8, true);
            hang2_left.Load(1, "Sprites/Player/hang2_left_", 10, true);
            hang2_right.Load(1, "Sprites/Player/hang2_right_", 10, true);
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

            CamFix = FixtureManager.CreateRectangle(100, 100, position, BodyType.Dynamic, 0.00001f);
            CamFix.Body.IgnoreGravity = true;
            CamFix.IsSensor = true;
            CamFix.isPlayer = true;

            CamFix.Body.Position = CharFix.Body.Position;
            Camera.Position = CamFix.Body.Position * Level.PixelPerMeter;

            nRect = FixtureManager.CreateRectangle(140, 10, new Vector2(position.X, position.Y - 85), BodyType.Dynamic, 0);
            nRect.Body.FixedRotation = true;
            nRect.IsSensor = true;
            nRect.isPlayer = true;

            sRect = FixtureManager.CreateRectangle(135, 35, new Vector2(position.X, position.Y + 120), BodyType.Dynamic, 0);
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
            sRect.OnSeparation += this.sOnSeparation;
            landRect.OnCollision += this.landOnCollision;
            wRect.OnCollision += this.ewOnCollision;
            wRect.OnSeparation += this.ewOnSeparation;
            eRect.OnCollision += this.ewOnCollision;
            eRect.OnSeparation += this.ewOnSeparation;

            IdleState = new PlayerIdleState(this, idle_left, idle_right);
            JumpState = new PlayerJumpState(this, jumpStarting_left, jumpStarting_right);
            JumpTryState = new PlayerJumpTryState(this, noJumpIdle_left, noJumpIdle_right);
            FallingState = new PlayerFallingState(this, falling_left, falling_right);
            LandingFastState = new PlayerLandingState(this, landing_left_fast, landing_right_fast);
            LandingState = new PlayerLandingState(this, landing_left, landing_right);
            WalkState = new PlayerWalkState(this, running_left, running_right);
            WalkStartState = new PlayerWalkStartState(this, runStarting_left, runStarting_right);
            WalkStopState = new PlayerWalkStopState(this, runStopping_left, runStopping_right);
            HangingState = new PlayerHangingState(this, hang_left, hang_right);
            HangingState2 = new PlayerHangingState2(this, hang2_left, hang2_right);
            DyingState = new PlayerDyingState(this, dying_left, dying_right);
            ClimbingState = new PlayerClimbingState(this, climbing_left, climbing_right);
            _state = IdleState;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 movement = CharFix.Body.LinearVelocity;
            int movX = (int)movement.X;
            int movY = (int)movement.Y;
            String velocityString = "Velo: " + movX + " - " + movY;
            State.Draw(spriteBatch);
            if (GameStateManager.Default.currentLevel.DebugViewEnabled)
            {
                //spriteBatch.Draw(CurrentAnimation.activeTexture, position, null, Color.White, CharFix.Body.Rotation, new Vector2(250, 250), 1, SpriteEffects.None, 1);
                spriteBatch.DrawString(FontManager.Arial, State.ToString(), position + new Vector2(0, -100), Color.Green);
                spriteBatch.DrawString(FontManager.Arial, velocityString, position + new Vector2(0, -50), Color.Green);
                //spriteBatch.Draw(CurrentAnimation.activeTexture, CharFix.Body.Position / Level.PixelPerMeter, null, Color.White, CharFix.Body.Rotation, new Vector2(250, 250), 1, SpriteEffects.None, 1);
            }
        }

        public override void Update(GameTime gameTime)
        {
            int dt = gameTime.ElapsedGameTime.Milliseconds;
            OldKState = KState;
            KState = Keyboard.GetState();
            HandleInput();

            UpdatePositions();
            //UpdateAnimation(gameTime);
            State.Update(gameTime);
            //ObserveAnimations();
            ObserveRotation();
            ObserveMovement();
            UpdateInterpolators(gameTime);
        }

        private void UpdatePositions()
        {
            Vector2 centerPosition = new Vector2(CharFix.Body.Position.X * Level.PixelPerMeter, CharFix.Body.Position.Y * Level.PixelPerMeter);
            position = new Vector2(centerPosition.X, centerPosition.Y);
            sRect.Body.Position = CharFix.Body.Position + new Vector2(0, 105 / Level.PixelPerMeter);
            nRect.Body.Position = CharFix.Body.Position + new Vector2(0, -85 / Level.PixelPerMeter);
            wRect.Body.Position = CharFix.Body.Position + new Vector2(-110 / Level.PixelPerMeter, 0);
            eRect.Body.Position = CharFix.Body.Position + new Vector2(105 / Level.PixelPerMeter, 0);
            landRect.Body.Position = CharFix.Body.Position + new Vector2(0, 300 / Level.PixelPerMeter);
            
            UpdateCamera();


        }

        private void UpdateInterpolators(GameTime gameTime)
        {
            if (RedInterpolator != null)
            {
                RedInterpolator.Update(gameTime.ElapsedGameTime.Milliseconds);
            }
            if (GreenInterpolator != null)
            {
                GreenInterpolator.Update(gameTime.ElapsedGameTime.Milliseconds);
            }
            if (BlueInterpolator != null)
            {
                BlueInterpolator.Update(gameTime.ElapsedGameTime.Milliseconds);
            }
        }

        private void UpdateAnimation(GameTime gameTime)
        { 
            CurrentAnimation.Update(gameTime);
        }

        private void ObserveRotation()
        {
            if (CharFix.Body.Rotation > MAXROTATION)
            {
                CharFix.Body.Rotation = MAXROTATION;
            }
            else if (CharFix.Body.Rotation < -MAXROTATION)
            {
                CharFix.Body.Rotation = -MAXROTATION;
            }
        }

        private void ObserveAnimations()
        {
           
        }

        private void ObserveMovement()
        {            

        }

        public void UpdateCamera()
        {
            CamFix.Body.Position = CharFix.Body.Position;
            CamFix.Body.ApplyTorque(CharFix.Body.AngularVelocity / 5000);
            Camera.Position = CamFix.Body.Position * Level.PixelPerMeter;
            //Camera.Rotation = CamFix.Body.Rotation;
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
            this.HandleStatelessInput(KState, OldKState);
            State.handleInput(KState, OldKState);
        }
        private void HandleStatelessInput(KeyboardState cbs, KeyboardState oks)
        {
            if (cbs.IsKeyDown(Keys.R))
            {
                Reset();
            }
            else if (cbs.IsKeyDown(Keys.L) && Superpower == SuperpowerState.None)
            {
                SetRemembering();
            }
        }

        private void SetRemembering()
        {
            Superpower = SuperpowerState.Remembers;
            RememberingTimer = new Timer(REMEMBERDURATION, SetRecoveringDelegate);
            RedInterpolator = new Interpolator(0, ColorFade.OrangeTargetRed, REMEMBERCLIMAX);
            GreenInterpolator = new Interpolator(0, ColorFade.OrangeTargetGreen, REMEMBERCLIMAX);
            BlueInterpolator = new Interpolator(0, ColorFade.OrangeTargetBlue, REMEMBERCLIMAX);
            new Timer(REMEMBERCLIMAX, FadeToBlue);
        }

        public void SetRecoveringDelegate()
        {
            Superpower = SuperpowerState.Regrets;
            RecoveringTimer = new Timer(RECOVERDURATION, SetNoneDelegate);
            Console.WriteLine("Remembers...");
        }

        public void FadeToBlue()
        {
            RedInterpolator = new Interpolator(RedInterpolator.CurrentValue, ColorFade.BlueTargetRed, REMEMBERDURATION - REMEMBERCLIMAX);
            GreenInterpolator = new Interpolator(GreenInterpolator.CurrentValue, ColorFade.BlueTargetGreen, REMEMBERDURATION - REMEMBERCLIMAX);
            BlueInterpolator = new Interpolator(BlueInterpolator.CurrentValue, ColorFade.BlueTargetBlue, RECOVERDURATION + REMEMBERDURATION - REMEMBERCLIMAX);
            new Timer(RECOVERDURATION + REMEMBERDURATION - REMEMBERCLIMAX, FadeToNone);
        }

        public void FadeToNone()
        {
            RedInterpolator = new Interpolator(RedInterpolator.CurrentValue, 0, BACKTONORMALTIME);
            GreenInterpolator = new Interpolator(GreenInterpolator.CurrentValue, 0, BACKTONORMALTIME);
            BlueInterpolator = new Interpolator(BlueInterpolator.CurrentValue, 0, BACKTONORMALTIME);
        }

        public void SetNoneDelegate()
        {
            Superpower = SuperpowerState.None;
        }

        public bool OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {

            return State.OnCollision(fixtureA, fixtureB, contact);
        }

        public void OnSeperation(Fixture fixtureA, Fixture fixtureB)
        {
            if (fixtureB.isHalfTransparent && CharFix.IsSensor)
            {
                CharFix.IsSensor = false;
            }

            State.OnSeperation(fixtureA, fixtureB);
        }

        private bool landOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return State.landOnCollision(fixtureA, fixtureB, contact);
        }

        public bool nOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {

            return State.nOnCollision(fixtureA, fixtureB, contact);
        }

        private bool sOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {

            return State.sOnCollision(fixtureA, fixtureB, contact);
        }

        public virtual void sOnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            State.sOnSeperation(fixtureA, fixtureB);
        }

        private bool ewOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {

            return State.ewOnCollision(fixtureA, fixtureB, contact);
        }

        public void ewOnSeparation(Fixture fixtureA, Fixture fixtureB)
        {

            State.ewOnSeparation(fixtureA, fixtureB);
        }
    }
}
