using System;
using System.Collections;
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
using FarseerPhysics.Common;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Controllers;


using FarseerPhysics.Dynamics;

using FarseerPhysics.Factories;

namespace Silhouette.GameMechs
{
    public class EndBoss : DrawableLevelObject
    {
        public float tempRotation;
        public float contactTimer;
        public Fixture fixture;
        private BodyType _bodyType;


        public BodyType bodyType { get { return _bodyType; } set { _bodyType = value; } }




        public KeyboardState oldState;







        //cooldown control to prevent oscillation on discrete borders 
        private float backupCooldown;
        private float backupCooldownRemaining;
        private Boolean mayBackup;

        //Cooldown for the attacks
        private float attackCooldown;
        private float attackCooldownRemaining;
        private Boolean mayAttack;

        //attacks til next Rope Event
        private int numberOfAttacks;
        private Vector2 RopeOffset;

        //state Management
        private enum State { Rope1, Rope2, Rope3, Rope4, Rope5, Rope6, Rope7, Rope8, AttackMoving, Attacking, CutsceneLaura, CutsceneFinal }
        private State currentBossState;

        private Boolean HandInPosition;
        private Boolean animationStarted;

        
        //scriptedMove Management
        private Vector2 targetPosition;
        private Boolean targetPositionReached;

       
        //X-Positions for the Attack-Movement Stuff
        private float xPositionBoss;
        private float xPositionPlayer;
       
        //Other Attack-Movement Stuff
        public float distance;  //Distanz Boss - Player
        public Vector2 movement; //Force, die auf fixture.Body applied wird
        private Boolean goingDown;
       




       // private Animation activeAnimation;      // Die aktive Sprite, die in der UpdateMethode auch geupdatet wird.
        private ScalingAnimation activeAnimation;
        private ScalingAnimation nextAnimation;        // Die nächste Animation, die gespielt wird, sobald die aktuelle abgelaufen is oder unterbrochen wird.
      

        private ScalingAnimation idleState;
        private ScalingAnimation throughTheRoof;

        private ScalingAnimation attackFist;
        private ScalingAnimation attackWave;
        private ScalingAnimation attackSnip;

        private ScalingAnimation HandFlat;
        
        private ScalingAnimation HandRopeDown;
        private ScalingAnimation HandRopeLetGo;


        //Ropes and Stuff
        Rope r1;
        Rope r2;
        Rope r3;
        Rope r4;
        Rope r5;
        Rope r6;
        Rope r7;
        Rope r8;
        Rope r9;




        public EndBoss()
        {


        }
        public override void Initialise()
        {

            
            
            //The main fixture onto which all the forces will be applied
            fixture = FixtureFactory.CreateRectangle(Level.Physics, 700 / Level.PixelPerMeter, 300 / Level.PixelPerMeter, 1.0f);
            fixture.Body.BodyType = BodyType.Dynamic;
            fixture.Body.FixedRotation = true;
            fixture.Friction = 0.0f;
            fixture.Body.Mass = 0.0f;
            fixture.Body.IgnoreGravity = true;
            fixture.CollisionCategories = CollisionCategory.Cat14;

            this.position = new Vector2(5, 3);
            fixture.Body.Position = position;
            fixture.IsSensor = false;

         
            

            //All the Animations
            idleState = new ScalingAnimation(this, new Animation()); 
            idleState.scale = new Vector2(2.85714286f, 2.85714286f);
            idleState.PolygonOffset = new Vector2(1.8f, 2.5f);
            idleState.TextureOffset = new Vector2(2.0f * Level.PixelPerMeter, -12 * Level.PixelPerMeter);



            

            attackFist = new ScalingAnimation(this, new Animation());
            attackFist.scale = new Vector2(2.85714286f, 2.85714286f);
            attackFist.PolygonOffset = new Vector2(7.2f, 3.7f);
            attackFist.TextureOffset = new Vector2(-8.0f * Level.PixelPerMeter, -13 * Level.PixelPerMeter);

            attackWave = new ScalingAnimation(this, new Animation());
            attackWave.scale = new Vector2(2.85714286f, 2.85714286f);
            attackWave.PolygonOffset = new Vector2(10f, 3.7f);
            attackWave.TextureOffset = new Vector2(-12 * Level.PixelPerMeter, -13 * Level.PixelPerMeter);

            attackSnip = new ScalingAnimation(this, new Animation());
            attackSnip.scale = new Vector2(2.85714286f, 2.85714286f);
            attackSnip.PolygonOffset = new Vector2(9.2f, 3.7f);
            attackSnip.TextureOffset = new Vector2(-8.0f * Level.PixelPerMeter, -13 * Level.PixelPerMeter);



            throughTheRoof = new ScalingAnimation(this, new Animation());

            HandFlat = new ScalingAnimation(this, new Animation());
            HandFlat.scale = new Vector2(2.85714286f, 2.85714286f);
            HandFlat.PolygonOffset = new Vector2(6, 3.7f);
            HandFlat.TextureOffset = new Vector2(1 * Level.PixelPerMeter, -13 * Level.PixelPerMeter);

           
            
            HandRopeDown = new ScalingAnimation(this, new Animation());
            HandRopeDown.scale = new Vector2(2.85714286f, 2.85714286f);
            HandRopeDown.PolygonOffset = new Vector2(4.3f, 3.7f);
            HandRopeDown.TextureOffset = new Vector2(2.0f * Level.PixelPerMeter, -13 * Level.PixelPerMeter);


            HandRopeLetGo = new ScalingAnimation(this, new Animation());
            HandRopeLetGo.scale = new Vector2(2.85714286f, 2.85714286f);
            HandRopeLetGo.PolygonOffset = new Vector2(7.2f, 3.7f);
            HandRopeLetGo.TextureOffset = new Vector2(-5.0f * Level.PixelPerMeter, -13 * Level.PixelPerMeter);
            
            

            //Place Ropes Correctly

            r1 = new Rope(new Vector2(2.9f, -9), new Vector2(2.9f, -2));
          /*  r2 = new Rope(new Vector2(1, -5), new Vector2(1, -2));
            r3 = new Rope(new Vector2(2, -5), new Vector2(2, -2));
            r4 = new Rope(new Vector2(3, -5), new Vector2(3, -2));
            r5 = new Rope(new Vector2(4, -5), new Vector2(4, -2));
            r6 = new Rope(new Vector2(5, -5), new Vector2(5, -2));
            r7 = new Rope(new Vector2(6, -5), new Vector2(6, -2));
            r8 = new Rope(new Vector2(7, -5), new Vector2(7, -2));
            r9 = new Rope(new Vector2(8, -5), new Vector2(8, -2));
            */

           

            //camera settings
            Camera.Scale = 0.35f;
            Camera.fixedOnPlayer = false;
            Camera.Position = new Vector2(-0.2f * Level.PixelPerMeter, 0);

           

     


            //Backup Cooldown for the Attack-Movement
            backupCooldown = 0.2f;
            attackCooldown = 5.0f;

            //Inital BossState
            currentBossState = State.AttackMoving;
            activeAnimation = idleState;
            activeAnimation.isActive = true;
            numberOfAttacks = 1;

           

        }

        public override void LoadContent()
        {



        
            // Hier müssen alle Sprites geladen werden.


            idleState.animation.Load(1, "Sprites/Boss/idle_", 10, false);
           // idleState.animation.position = FixtureManager.ToPixel(new Vector2(0, 0));
            idleState.animation.start();
            idleState.Initialise();

            throughTheRoof.animation.Load(7, "Sprites/Boss/BreakingIn_", 5, false);
            

            attackFist.animation.Load(16, "Sprites/Boss/HandAttackFist_", 10 , false);
            attackFist.Initialise();

            attackWave.animation.Load(24, "Sprites/Boss/HandAttackWave_", 10, false);
            attackWave.Initialise();

            attackSnip.animation.Load(15, "Sprites/Boss/HandAttackSnip_", 10, false);
            attackSnip.Initialise();

            HandFlat.animation.Load(14, "Sprites/Boss/HandFlat_", 10, false);
            HandFlat.Initialise();

            HandRopeDown.animation.Load(9, "Sprites/Boss/HandRopeDownPt1_", 10, false);
            HandRopeDown.Initialise();

            HandRopeLetGo.animation.Load(8, "Sprites/Boss/HandRopeDownPt1_", 10, false);
            HandRopeLetGo.Initialise();







          
           
     
        }

        public override void Update(GameTime gameTime)
        {
            UpdateAttackCooldown(gameTime);

            switch (currentBossState)
            {
                case State.AttackMoving:
                    {
                       
                        UpdateAttackMovement(gameTime);

                    //    if ((fixture.Body.GetLinearVelocityFromWorldPoint(new Vector2(0, 0)).X == 0) && (fixture.Body.GetLinearVelocityFromWorldPoint(new Vector2(0, 0)).Y == 0) && (fixture.Body.Position.Y > 6) && (mayAttack == true))
                        if ((fixture.Body.Position.Y > 6.2f) && (mayAttack == true))
                        {
                            fixture.Body.ResetDynamics();
                            nextAnimation = attackWave;
                            currentBossState = State.Attacking;
                            mayAttack = false;
                            Console.WriteLine("StateSwitch: " + currentBossState);
                        }


                        if (numberOfAttacks == 0)
                        {
                            currentBossState = State.Rope1;
                            
                        }


                        break;
                    }
                case State.Attacking:
                    {

                        if (activeAnimation.animation.activeFrameNumber == activeAnimation.animation.Amount - 1 && !activeAnimation.animation.Looped)
                            {
                                currentBossState = State.AttackMoving;
                                Console.WriteLine("StateSwitch: " + currentBossState);
                                nextAnimation = idleState;
                               
                                activeAnimation.animation.PlayedOnce = false;
                                numberOfAttacks--;
                            
                            }



                

                      
                       
                        break;
                    }

                case State.Rope1:
                    {
                        if (!targetPositionReached && !HandInPosition)
                        {
                            targetPosition = r1.lastBody.Position - new Vector2(1.0f, -6.0f);
                            this.UpdateScriptedEventPosition(gameTime);

                        }
                        else
                        {
                            HandInPosition = true;
                            if (!animationStarted)
                            {
                                nextAnimation = HandRopeDown;
                                animationStarted = true;
                                RopeOffset = r1.chainLinks[0].Position - activeAnimation.activeFixture.Body.Position;
                                targetPositionReached = false;
                            }

                            if (activeAnimation.animation.activeFrameNumber == activeAnimation.animation.Amount - 1)
                            {
                                if (!targetPositionReached)
                                {
                                    targetPosition = new Vector2(this.fixture.Body.Position.X, 6);
                                    this.UpdateScriptedEventPosition(gameTime);


                                    foreach (Body bod in r1.chainLinks)
                                    {
                                        bod.Position = new Vector2(bod.Position.X, activeAnimation.activeFixture.Body.Position.Y - (RopeOffset.Y - (r1.chainLinks[0].Position.Y - bod.Position.Y)));

                                    }

                                    // r1.chainLinks[0].Position = activeAnimation.activeFixture.Body.Position;


                                }
                                else
                                {
                                    //    r1.chainLinks[0].IsStatic = false;
                                    currentBossState = State.Attacking;
                                }
                            }
                            


                        }


                        break;
                    }

            }


            updateAnimations(gameTime);
            

           
           // UpdateScriptedEventPosition(gameTime);


           
           
           


        }

        public void UpdateAttackCooldown(GameTime gameTime)
        {
            if (mayAttack == false)
            {
                attackCooldownRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (attackCooldownRemaining <= 0)
                {
                    mayAttack = true;
                    attackCooldownRemaining= attackCooldown;
                }

            }
        
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
            UpdateBackupTimer(gameTime);
            Vector2 playerPos = GameLoop.gameInstance.playerInstance.position / Level.PixelPerMeter;
            Vector2 bossPos = this.fixture.Body.Position;

            movement = playerPos - bossPos;

            distance = (float)Math.Sqrt(Math.Pow((double)(playerPos.X - bossPos.X), 2.0d) + Math.Pow((double)(playerPos.Y - bossPos.Y), 2.0d));
            movement.Normalize();




            xPositionPlayer = GameLoop.gameInstance.playerInstance.position.X / Level.PixelPerMeter;
            xPositionBoss = this.fixture.Body.Position.X;


            
            if ((xPositionBoss < xPositionPlayer + 12) && (xPositionBoss > xPositionPlayer + 10)) //Too close, better you don't touch this!
            {
              
                mayBackup = false;

                if (!goingDown)
                {
                 //   this.fixture.Body.ResetDynamics();
                    goingDown = true;
                }




                this.fixture.Body.LinearDamping = 2.0f;
                this.fixture.Body.ApplyForce(new Vector2(2.0f, 6.0f));  //Going down
                goingDown = true;




            }
            else
            {

                this.fixture.Body.IgnoreGravity = true;
                movement *= 4.0f;





                


                this.fixture.Body.LinearDamping = 1.5f;
                if ((xPositionBoss < xPositionPlayer + 10))
                {
                    if (mayBackup)
                    {
                        if (distance < 11.5)
                        {
                            Vector2 goingUp = (-movement * 2) + new Vector2(0, -20.0f);
                            this.fixture.Body.ApplyForce(goingUp);
                        }//Backup, Going Up
                        else
                            this.fixture.Body.ApplyForce(new Vector2(-movement.X, 0));
                        goingDown = false;
                    }
                }
                else
                {

                    this.fixture.Body.ApplyForce(movement * 1.1f); //Head towards
                    goingDown = false;

                }
            }




        }

        private void UpdateScriptedEventPosition(GameTime gameTime)
        {


            Vector2 movement = targetPosition - this.fixture.Body.Position ;
            movement.Normalize();
            movement *= 2;

            this.bodyType = BodyType.Static;
            this.fixture.Body.Position += movement * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if ((fixture.Body.Position.X > targetPosition.X - 0.3f) && (fixture.Body.Position.X < targetPosition.X + 0.3f) && (fixture.Body.Position.Y > targetPosition.Y - 0.3f) && (fixture.Body.Position.X < targetPosition.X + 0.3f))
            {
                targetPositionReached = true;
            }
        }



   

        private void UpdateTexture(GameTime gameTime)
        {
            
        }



        public void updateAnimations(GameTime gameTime)
        {

            if (nextAnimation != null)
            {
                if (activeAnimation.animation.activeFrameNumber == activeAnimation.animation.Amount - 1 && !activeAnimation.animation.Looped)
                {
                    activeAnimation.isActive = false;
                    activeAnimation.animation.activeFrameNumber = 0;

                    activeAnimation.animation.PlayedOnce = false;
                    activeAnimation = nextAnimation;
                    activeAnimation.isActive = true;
                    activeAnimation.animation.activeFrameNumber = 0;
                    activeAnimation.animation.start();
                    nextAnimation = null;
                }
            }
            
            
            
               HandFlat.Update(gameTime);
               attackSnip.Update(gameTime);
               attackFist.Update(gameTime);
               attackWave.Update(gameTime);
               idleState.Update(gameTime);
               HandRopeDown.Update(gameTime);
               HandRopeLetGo.Update(gameTime);


              

        }


        public bool OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.isPlayer && fixtureA.Body.Active)
            { 
            
            
            }
            return false;
        }


        public bool nOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.isHalfTransparent)
            {

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

            //  spriteBatch.Draw(activeAnimation.activeTexture, position, null, Color.White, tempRotation, new Vector2(250, 250), 1, SpriteEffects.None, 1);
            spriteBatch.Draw(activeAnimation.animation.activeTexture, activeAnimation.animation.position, null, Color.White, tempRotation, new Vector2(250, 250), 2.85714286f, SpriteEffects.None, 1);

            r1.draw(spriteBatch);
       /*     r2.draw(spriteBatch);
            r3.draw(spriteBatch);
            r4.draw(spriteBatch);
            r5.draw(spriteBatch);
            r6.draw(spriteBatch);
            r7.draw(spriteBatch);
            r8.draw(spriteBatch);
            r9.draw(spriteBatch); 
            */

            //Das auskommentierte hier kann als Debugview dienen.
   
           // spriteBatch.DrawString(FontManager.Arial, "distance: " + distance, new Vector2(300, 20), Color.Black);
            /*
            spriteBatch.DrawString(FontManager.Arial, "Boss Position: " + this.fixture.Body.Position, new Vector2(300, 45), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "Player Position: " + GameLoop.gameInstance.playerInstance.position / Level.PixelPerMeter, new Vector2(300, 70), Color.Black);

            spriteBatch.DrawString(FontManager.Arial, "Distance Player-Boss: " + distance, new Vector2(300, 95), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "isJitterMoving: " + isJitterMoving, new Vector2(300, 120), Color.Black);

            spriteBatch.DrawString(FontManager.Arial, "GoingDown: " + goingDown, new Vector2(300, 155), Color.Black);

            spriteBatch.DrawString(FontManager.Arial, "X-Position Boss: " + xPositionBoss, new Vector2(300, 180), Color.Black);
            spriteBatch.DrawString(FontManager.Arial, "X-Position Player: " + xPositionPlayer, new Vector2(300, 205), Color.Black);
             * */

            /*
             spriteBatch.DrawString(FontManager.Arial, "CamRectRotation: " + camRect.Body.Rotation.ToString(), new Vector2(300, 230), Color.Black);
             spriteBatch.DrawString(FontManager.Arial, "CamRotation: " + Camera.Rotation.ToString(), new Vector2(300, 255), Color.Black);
             spriteBatch.DrawString(FontManager.Arial, "tempRotation: " + tempRotation.ToString(), new Vector2(300, 280), Color.Black);
             spriteBatch.DrawString(FontManager.Arial, "sJTimer: " + sJTimer.ToString() + " sJRecoveryTimer: " + sJRecoveryTimer.ToString(), new Vector2(300, 305), Color.Black);*/
        }
    }

    public class Rope
    {
        FarseerPhysics.Common.Path path = new FarseerPhysics.Common.Path();


        PolygonShape shape;
       public List<Body> chainLinks = new List<FarseerPhysics.Dynamics.Body>();
        Texture2D ropeText;
        public Body lastBody;
        public Body MovingBody;
        public Rope(Vector2 RopeStart, Vector2 RopeEnd)
        {
            path.Add(RopeStart);
            path.Add(RopeEnd);


            ropeText = GameLoop.gameInstance.Content.Load<Texture2D>("Sprites/rope");
            shape = new PolygonShape(PolygonTools.CreateRectangle(0.05f, 0.1f), 0.6f);

            chainLinks = PathManager.EvenlyDistributeShapesAlongPath(Level.Physics, path, shape, BodyType.Dynamic, 20);
           /*
            MovingFixture = FixtureFactory.CreateRectangle(Level.Physics, 2, 2, 1);

            MovingFixture.Body.BodyType = BodyType.Static;
            MovingFixture.Body.Position = RopeStart;
            
            */
            


          


            foreach (Body chainLink in chainLinks)
            {
                chainLink.LinearDamping = 0.5f;
                foreach (Fixture f in chainLink.FixtureList)
                {
                    f.Friction = 2.0f;
                    f.CollidesWith = CollisionCategory.None;

                }
            }
            lastBody = chainLinks[(chainLinks.Count - 1)];



           // weld = new WeldJoint(MovingFixture.Body, chainLinks[0], Vector2.Zero, Vector2.Zero);
           // Level.Physics.AddJoint(weld);
            RopeJoint rope = new RopeJoint(chainLinks[0], chainLinks[chainLinks.Count - 1], Vector2.Zero, Vector2.Zero);

            chainLinks[0].BodyType = BodyType.Static;

            MovingBody = chainLinks[0];


            PathManager.AttachBodiesWithRevoluteJoint(Level.Physics, chainLinks, new Vector2(0, -0.1f), new Vector2(0, 0.1f),
                                                      false, false);
            
          
           



        }

        public void draw(SpriteBatch spriteBatch)
        {

            if (chainLinks != null)
                foreach (Body chainLink in chainLinks)
                {
                    foreach (Fixture f in chainLink.FixtureList)
                    {

                        PolygonShape ps = (PolygonShape)f.Shape;

                        Vector2 Edge1;
                        Vector2 Edge2;
                        Vector2 Edge3;
                        Vector2 Edge4;

                        FarseerPhysics.Collision.AABB aabb = ps.Vertices.GetCollisionBox();

                        Edge1 = new Vector2(aabb.LowerBound.X, aabb.LowerBound.Y);
                        Edge2 = new Vector2(aabb.UpperBound.X, aabb.LowerBound.Y);
                        Edge3 = new Vector2(aabb.UpperBound.X, aabb.UpperBound.Y);
                        Edge4 = new Vector2(aabb.LowerBound.X, aabb.UpperBound.Y);

                        Rectangle destRect = new Rectangle((int)(chainLink.Position.X * Level.PixelPerMeter), (int)(chainLink.Position.Y * Level.PixelPerMeter), (int)(Edge3.X * Level.PixelPerMeter) - (int)(Edge1.X * Level.PixelPerMeter), (int)(Edge3.Y * Level.PixelPerMeter) - (int)(Edge1.Y * Level.PixelPerMeter));

                        spriteBatch.Draw(ropeText, destRect, null, Color.White, chainLink.Rotation, Vector2.Zero, SpriteEffects.None, 1.0f);

                    }


                }

        }

    }

    public class ScalingAnimation
    {
        public Animation animation;
        private List<Fixture> fixtures;
        public Fixture activeFixture;
        public Boolean isScaled;
        public Vector2 scale = new Vector2(1.0f, 1.0f);
        public Vector2 PolygonOffset = new Vector2(0, 0);
        public Vector2 TextureOffset = new Vector2(0, 0);
        public Texture2D CurrentFrame;
        public Boolean isActive;

       
        
        private EndBoss boss;



        public ScalingAnimation(EndBoss Boss, Animation animation)
        {
            this.animation = animation;
            this.boss = Boss;

            fixtures = new List<Fixture>();
        }
 
        
        public void Initialise()
        {
            
            int i = 0;
            foreach (Texture2D texture in animation.pictures)
            {
                try
                {


                    Console.Out.WriteLine("Calculating Polygon for Texture No: " + (i + 1.0f));
                    fixtures.Add(FixtureManager.CreatePolygon(texture, scale, BodyType.Dynamic, animation.position, 1.0f));



                    fixtures[i].Body.Awake = false;


                    fixtures[i].Body.IgnoreGravity = true;
                    fixtures[i].Body.FixedRotation = true;
                    fixtures[i].Body.Mass = 0;
                    fixtures[i].Body.Inertia = 0;
                    fixtures[i].Body.BodyType = BodyType.Static;
                    fixtures[i].Body.IgnoreGravity = true;
                    fixtures[i].CollisionCategories = CollisionCategory.All;
                    fixtures[i].IsSensor = true;
                    fixtures[i].Body.Active = false;

                    fixtures[i].OnCollision += boss.OnCollision;

                }
                catch (Exception e)
                {
                    
                    
                    fixtures.Add(fixtures[i-1]);

                      

                }
                i++;

            }



        }

        public void Update(GameTime gameTime)
        {
            if (fixtures != null)
            {
                animation.Update2(gameTime, FixtureManager.ToPixel(boss.fixture.Body.Position) + TextureOffset);
               
                foreach (Fixture fix in fixtures)
                {
                    fix.IsSensor = true;
                    fix.Body.Position = new Vector2(animation.position.X / Level.PixelPerMeter, animation.position.Y / Level.PixelPerMeter) + PolygonOffset;
                    fix.Body.Active = false;
                }

                
                activeFixture = fixtures[animation.activeFrameNumber];

                

                if (isActive)
                {
                    CurrentFrame = animation.activeTexture;

                    activeFixture.IsSensor = true;
                    activeFixture.Body.Active = true;
                }
               
            }
        }


    }
}
