using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Silhouette.GameMechs;

namespace Silhouette.Engine
{
    //Camera ist eine sehr frühe Fassung. Vorerst statisch, zu debug-Zwecken. Einbindung mit Physik ist erdacht, aber auskommentiert.
    class Camera:LevelObject
    {
        public static Vector2 position { get; set; }
        public static Fixture fixture { get; set; }
        public static Fixture anchorPoint { get; set; }
        public static Matrix matrix { get; set; }
        public static float zoom { get; set; }
        //public static bool isFix { get; set; }
        //public static Joint joint;

        public Camera(GameLoop game, Vector2 position)
        {
            Camera.position = position;
            zoom = 1.0f;
            //isFix = true;
            //fixture = FixtureFactory.CreateRectangle(Level.Physics, 1, 1, 1.0f);
            //fixture.Body.IgnoreGravity = true;
            //joint = null;

        }

        /*public void Link(Fixture fixture)
        {
            anchorPoint = fixture;
            joint = JointFactory.CreateDistanceJoint(Level.Physics, this.fixture.Body, anchorPoint.Body, Vector2.Zero, Vector2.Zero);
            
        }

        public void Delink()
        {
            anchorPoint = null;
            joint = null;
        }*/

        public void Update(GameTime gameTime, Viewport port, Vector2 position)              // Viewport muss leider übergeben
        {                                                                                   // werden. Position kann zB die
                                                                                            // des Players sein. Wird geändert,
                float elapsed = gameTime.ElapsedGameTime.Milliseconds;                      // sodass nur game übergeben werden muss.

                //if (!isFix) { this.position = position; }
                //else
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    {
                        MoveUp(2);
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    {
                        MoveLeft(2);
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    {
                        MoveDown(2);
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    {
                        MoveRight(2);
                    }
                }

                matrix = Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
                                                    Matrix.CreateRotationZ(0) *
                                                    Matrix.CreateScale(new Vector3(zoom, zoom, 0)) *
                                                    Matrix.CreateTranslation(new Vector3( port.Width* 0.5f, port.Height * 0.5f, 0)); //!!!
        }

        public void MoveLeft(float speed)
        {
            position = position - new Vector2(speed,0);
        }

        public void MoveRight(float speed)
        {
            position = position + new Vector2(speed, 0);
        }

        public void MoveUp(float speed)
        {
            position = position - new Vector2(0, speed);
        }

        public void MoveDown(float speed)
        {
            position = position + new Vector2(0, speed);
        }

        public void ZoomIn(float max, float speed)
        {
            if (zoom < max) { zoom += speed; }
            
        }

        public void ZoomOut(float max, float speed)
        {
            if (zoom > max) { zoom -= speed; }
            
        }

        public void Reset(float speed)                              // Wird weder rein- noch rausgezoomt, wird das normale
        {                                                           // Verhältnis wiedereingestellt.
            if (zoom > 1.0f) { zoom -= speed; }
            if (zoom < 1.0f) { zoom += speed; }
        }
    }
}