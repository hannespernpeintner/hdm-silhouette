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
    class Player : DrawableLevelObject
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

        private int facing;                    // Wo der Chara hinschaut. 0 bedeutet links, 1 bedeutet rechts.

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

        public override void Initialise()
        {
            // Aus dem Level geht die initiale StartPos des Chars hervor. Aktuell Testposition eingetragen.
            // position = Level.LevelSetting.CharacterStartPosition;

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

            position = new Vector2(100, 100);
            movement = Vector2.Zero;

            isRunning = false;
            isJumping = false;
            isFalling = false;
            isIdle = true;
            facing = 1;
        }

        public override void LoadContent()
        {

            // Hier müssen alle Sprites geladen werden.
            idle_left.Load(6, "Sprites/Player/idle_left_0", 6.0f, true);
            idle_right.Load(6, "Sprites/Player/idle_right_0", 6.0f, true);
            idleb_left.Load(6, "Sprites/Player/idleb_left_0", 6.0f, true);
            idleb_right.Load(6, "Sprites/Player/idleb_right_0", 6.0f, true);
            idlec_left.Load(6, "Sprites/Player/idlec_left_0", 6.0f, true);
            idlec_right.Load(6, "Sprites/Player/idlec_right_0", 6.0f, true);
            jumping_left.Load(7, "Sprites/Player/jump_left_0", 8.0f, true);
            jumping_right.Load(7, "Sprites/Player/jump_right_0", 8.0f, true);
            running_left.Load(8, "Sprites/Player/walk_left_0", 6.0f, true);
            running_right.Load(8, "Sprites/Player/walk_right_0", 6.0f, true);

            activeAnimation = idlec_right;
            nextAnimation = idlec_right;
            charRect = FixtureManager.CreateRectangle(150, 120, position, BodyType.Dynamic, 1);
            nRect = FixtureManager.CreateRectangle(120, 20, position, BodyType.Dynamic, 0);
            eRect = FixtureManager.CreateRectangle(20, 90, position, BodyType.Dynamic, 0);
            sRect = FixtureManager.CreateRectangle(120, 20, position, BodyType.Dynamic, 0);
            wRect = FixtureManager.CreateRectangle(20, 90, position, BodyType.Dynamic, 0);
            Joint joint0 = JointFactory.CreateWeldJoint(Level.Physics, charRect.Body, nRect.Body, new Vector2(0, -60 / Level.PixelPerMeter), Vector2.Zero);
            Joint joint1 = JointFactory.CreateWeldJoint(Level.Physics, charRect.Body, eRect.Body, new Vector2(75 / Level.PixelPerMeter, 0), Vector2.Zero);
            Joint joint2 = JointFactory.CreateWeldJoint(Level.Physics, charRect.Body, sRect.Body, new Vector2(0, 60 / Level.PixelPerMeter), Vector2.Zero);
            Joint joint3 = JointFactory.CreateWeldJoint(Level.Physics, charRect.Body, wRect.Body, new Vector2(-75 / Level.PixelPerMeter, 0), Vector2.Zero);

            sRect.OnCollision += sOnCollision;
            //sRect.OnSeparation += sOnSeperation;
        }

        public override void Update(GameTime gameTime)
        {
            ObserveMovement();
            UpdateNextAnimation();
            UpdatePositions();
            UpdateControls(gameTime);
            UpdateTexture(gameTime);
        }

        public void UpdatePositions()
        {
            centerPosition = new Vector2(charRect.Body.Position.X * Level.PixelPerMeter, charRect.Body.Position.Y * Level.PixelPerMeter);
            position = new Vector2(centerPosition.X - 150, centerPosition.Y - 150);
        }

        private void UpdateControls(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.A) && !isFalling && !isJumping)
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
                    nextAnimation = choseIdleAnimation();
                    charRect.Body.ApplyForce(new Vector2(-20, 0));
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D) && !isFalling && !isJumping)
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
                    nextAnimation = choseIdleAnimation();
                    charRect.Body.ApplyForce(new Vector2(20, 0));
                }
            }

            if (Keyboard.GetState().IsKeyUp(Keys.D) && oldState.IsKeyDown(Keys.D))
            {
                isRunning = false;
                activeAnimation = choseIdleAnimation();
            }

            if (Keyboard.GetState().IsKeyUp(Keys.A) && oldState.IsKeyDown(Keys.A))
            {
                isRunning = false;
                activeAnimation = choseIdleAnimation();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space) && isIdle)
            {
                if (facing == 0)
                {
                    activeAnimation = jumping_left;
                    activeAnimation.activeFrameNumber = 0;
                    isJumping = true;
                    isIdle = false;
                    isFalling = false;
                    isRunning = false;
                    charRect.Body.ApplyForce(new Vector2(-1000, -1400));
                }
                else if (facing == 1)
                {
                    activeAnimation = jumping_right;
                    activeAnimation.activeFrameNumber = 0;
                    isJumping = true;
                    isIdle = false;
                    isFalling = false;
                    isRunning = false;
                    charRect.Body.ApplyForce(new Vector2(1000, -1400));
                }
            }

            oldState = Keyboard.GetState();
            oldPosition = charRect.Body.Position;
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
            }

            if (activeAnimation.activeFrameNumber == activeAnimation.amount - 1 && isJumping)
            {
                activeAnimation = nextAnimation;
                activeAnimation.activeFrameNumber = 0;
                nextAnimation = null;
            }

            if (activeAnimation.activeFrameNumber == activeAnimation.amount - 1 && isIdle)
            {
                activeAnimation = nextAnimation;
                activeAnimation.activeFrameNumber = 0;
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
            spriteBatch.DrawString(FontManager.Arial, charRect.Body.Position.ToString(), new Vector2(300, 120), Color.Black);*/
        }

        //---> Editor-Stuff, für Player unwichtig <---//

        public override LevelObject clone()
        {
            throw new NotImplementedException();
        }
        public override bool contains(Vector2 worldPosition)
        {
            throw new NotImplementedException();
        }
        public override string getPrefix()
        {
            throw new NotImplementedException();
        }
        public override void transformed()
        {
            throw new NotImplementedException();
        }
        public override void drawSelectionFrame(SpriteBatch spriteBatch, Matrix matrix)
        {
            throw new NotImplementedException();
        }
        public override bool canScale() { return false; }
        public override Vector2 getScale() { return Vector2.One; }
        public override void setScale(Vector2 scale) { }
        public override bool canRotate() { return false; }
        public override float getRotation() { return 0; }
        public override void setRotation(float rotate) { }

        public override void loadContentInEditor(GraphicsDevice graphics)
        {
            throw new NotImplementedException();
        }

        public override void drawInEditor(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
