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
    class Camera:LevelObject
    {
        public Vector2 position { get; set; }
        public Fixture fixture { get; set; }
        public Fixture anchorPoint { get; set; }
        public Matrix matrix { get; set; }
        public float zoom { get; set; }
        public bool isFix { get; set; }
        public Joint joint;

        public Camera(GameLoop game, Vector2 position)
        {
            this.position = position;
            zoom = 1.0f;
            isFix = true;
            fixture = FixtureFactory.CreateRectangle(Level.Physics, 1, 1, 1.0f);
            fixture.Body.IgnoreGravity = true;
            joint = null;

        }

        public void Link(Fixture fixture)
        {
            anchorPoint = fixture;
            joint = JointFactory.CreateDistanceJoint(Level.Physics, this.fixture.Body, anchorPoint.Body, Vector2.Zero, Vector2.Zero);
            
        }

        public void Delink()
        {
            anchorPoint = null;
            joint = null;
        }

        public void Update(GameTime gameTime, Viewport port, Vector2 position)              // Viewport muss leider übergeben
        {                                                                                   // werden. Position kann zB die
                float elapsed = gameTime.ElapsedGameTime.Milliseconds;

                if (!isFix) { this.position = position; }

                else
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.W))
                    {
                        MoveUp(1);
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.A))
                    {
                        MoveLeft(1);
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.S))
                    {
                        MoveDown(1);
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.D))
                    {
                        MoveRight(1);
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
