using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
    public class EndBoss : InteractiveObject
    {
        public float tempRotation;
        public float contactTimer;
        public Vector2 centerPosition;      // Hannes: Rectangles für Kollisionserkennung. Muss noch um Kollisionsgruppe erweitert werden.
        public Vector2 camPosition;
        public Fixture charRect;
        public Fixture sRect;
        public Fixture nRect;

        private Random random;
        

        public KeyboardState oldState;
       
        public float distance;  //Distanz Boss - Player

        private Vector2 jitterVector;
        private Boolean isJitterMoving;
        public Vector2 movement;
        private Boolean goingDown;
        private Boolean mayBackup;

        private float backupCooldown;
        private float backupCooldownRemaining;

        private float xPositionBoss;
        private float xPositionPlayer;

      

        private int facing;                    // Wo der Chara hinschaut. 0 bedeutet links, 1 bedeutet rechts.

        private float sJTimer;
        private float sJRecoveryTimer;

        private String actScriptedMove;

        private bool canClimb;

        private int actClimbHeight;

        private Animation activeAnimation;      // Die aktive Sprite, die in der UpdateMethode auch geupdatet wird.
        private Animation nextAnimation;        // Die nächste Animation, die gespielt wird, sobald die aktuelle abgelaufen is oder unterbrochen wird.



        public EndBoss()
            : base("Sprites/Boss/arm_vorlaeufig_01")
        {
            
           
        }
        public override void Initialise()
        {
           
            base.Initialise();
            this.position = new Vector2(0,0);
            activeAnimation = new Animation();
            random = new Random();

            backupCooldown = 0.2f;

            facing = 1;
            Camera.Scale = 0.5f;
         
        }

        public override void LoadContent()
        {
            //Julius: den base kram initialisieren
            base.LoadContent();

            //Julius: Diverse Settings für den Body:
            this.fixture.Body.IgnoreGravity = true;
            this.fixture.Body.FixedRotation = true;
            this.fixture.Body.Mass = 0;
            this.fixture.Body.Inertia = 0;

            
            
           

            // Hier müssen alle Sprites geladen werden.
            activeAnimation.Load(1, "Sprites/Boss/arm_vorlaeufig_", 1.0f, true);
                
                      
           
            activeAnimation.start();
            
            

           

            

     //       charRect.OnCollision += this.OnCollision;
     //
    //        nRect.OnCollision += this.nOnCollision;
     //       sRect.OnCollision += this.sOnCollision;
        }

        public override void Update(GameTime gameTime)
        {
           /*
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
            * */
            UpdateBackupTimer(gameTime);
            UpdateJitterMovement();
            UpdateAttackMovement(gameTime);
           
            UpdateTexture(gameTime);
            
            ObserveTimer(gameTime);
            base.Update(gameTime);
        }

        public void UpdateJitterMovement()
        {
            if (isJitterMoving)
            {
                jitterVector = new Vector2((1.0f / (0.5f * random.Next(1,1))), (1.0f / (0.1f * random.Next(1, 5))));
            }
            else
                jitterVector = Vector2.One;

        }

        public void UpdateBackupTimer(GameTime gameTime)
        {
            if (mayBackup == false)

            {
               backupCooldownRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;

               if (backupCooldownRemaining <= 0)
               {
                   mayBackup = true;
                   backupCooldownRemaining = backupCooldown;
               }
            
            }


        }

      

        private void UpdateAttackMovement(GameTime gameTime)
        {
            Vector2 playerPos = GameLoop.gameInstance.playerInstance.position / Level.PixelPerMeter;
            Vector2 bossPos =  this.fixture.Body.Position;
            
            movement =  playerPos - bossPos;

            distance = (float)Math.Sqrt(Math.Pow((double)(playerPos.X - bossPos.X), 2.0d) + Math.Pow((double)(playerPos.Y - bossPos.Y), 2.0d));
            movement.Normalize();

            

        //    movement *= 3.5f;
            

         //    movement *= 0.9f * jitterVector;

            xPositionPlayer = GameLoop.gameInstance.playerInstance.position.X / Level.PixelPerMeter;
            xPositionBoss = this.fixture.Body.Position.X;
            

          //  if ((distance < 9) && (distance > 6))  //Too close
            if ((xPositionBoss < xPositionPlayer + 8) && (xPositionBoss > xPositionPlayer + 6))
            {
                isJitterMoving = false;
                mayBackup = false;

                if (!goingDown)
                {
                    this.fixture.Body.ResetDynamics();
                    goingDown = true;
                }
                    
                


                this.fixture.Body.LinearDamping = 2.0f;
                this.fixture.Body.ApplyForce(new Vector2(4.0f, 10.0f));  //Going down
                goingDown = true;

      

               
            }
            else
            {

                this.fixture.Body.IgnoreGravity = true;
                movement *= 4.0f;

               if (isJitterMoving) 
                movement *= (0.5f * jitterVector);
                

                
                 isJitterMoving = true;


                this.fixture.Body.LinearDamping = 1.5f;
                if ((xPositionBoss < xPositionPlayer + 6))
                {
                    if (mayBackup)
                    {
                        if (distance < 4)
                            this.fixture.Body.ApplyForce(((-movement) - new Vector2(0, 1.5f)));  //Backup, Going Up
                        else
                            this.fixture.Body.ApplyForce(-movement);
                        goingDown = false;
                    }
                }
                else
                {
                    
                    this.fixture.Body.ApplyForce(movement); //Head towards
                    goingDown = false;
                         
                }
            }

            

            

            

        }



        private void die()
        {
           
    
           

        }

        private void UpdateTexture(GameTime gameTime)
        {
       //     activeAnimation.Update(gameTime, position);
        }

    

        public void UpdateNextAnimation()
        {


        }

        public void ObserveMovement()
        {
           /*
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
            */
        }

        private void ObserveTimer(GameTime gameTime)
        {
            
        }

        public void calcRotation(GameTime gameTime)
        {
            
        }

        public bool OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return false;
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
            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //activeAnimation.Draw(spriteBatch);
            spriteBatch.Draw(activeAnimation.activeTexture, position, null, Color.White, tempRotation, new Vector2(250, 250), 1, SpriteEffects.None, 1);
            //Das auskommentierte hier kann als Debugview dienen.
            spriteBatch.DrawString(FontManager.Arial, "movement Vector: " + movement, new Vector2(300, 20), Color.Black);
            
            spriteBatch.DrawString(FontManager.Arial, "Boss Position: " + this.fixture.Body.Position, new Vector2(300, 45), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "Player Position: " + GameLoop.gameInstance.playerInstance.position / Level.PixelPerMeter, new Vector2(300, 70), Color.Black);
            
            spriteBatch.DrawString(FontManager.Arial, "Distance Player-Boss: " + distance, new Vector2(300, 95), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "isJitterMoving: " + isJitterMoving, new Vector2(300, 120), Color.Black);

            spriteBatch.DrawString(FontManager.Arial, "GoingDown: " + goingDown, new Vector2(300, 155), Color.Black);
           
            spriteBatch.DrawString(FontManager.Arial, "X-Position Boss: " + xPositionBoss,  new Vector2(300, 180), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "X-Position Player: " + xPositionPlayer, new Vector2(300, 205), Color.Black);
           /*
            spriteBatch.DrawString(FontManager.Arial, "CamRectRotation: " + camRect.Body.Rotation.ToString(), new Vector2(300, 230), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "CamRotation: " + Camera.Rotation.ToString(), new Vector2(300, 255), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "tempRotation: " + tempRotation.ToString(), new Vector2(300, 280), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "sJTimer: " + sJTimer.ToString() + " sJRecoveryTimer: " + sJRecoveryTimer.ToString(), new Vector2(300, 305), Color.Black);*/
        }
    }
}
