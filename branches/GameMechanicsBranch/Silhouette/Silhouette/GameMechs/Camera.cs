using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Silhouette.GameMechs;
using FarseerPhysics.Dynamics;

namespace Silhouette.Engine
{
    class Camera:LevelObject
    {
        Vector2 _position;            // Hannes: Position, Zoomfaktor (1 ist normal, 2 ist reingezoomt, 0,5 rausgezoomt),
        float _zoom;           // Transformationsmatrix ist an der Camera gespeichert.
        Matrix _matrix;
        bool _isFix;
        private Fixture _fixture;

        public Vector2 position { get { return _position; } set { _position = value; } }
        public Fixture fixture { get { return _fixture; } set { _fixture = value; } }
        public Matrix matrix { get}

        public Camera(Vector2 position)
        {
            _position = position;
            _zoom = 1.0f;
            _isFix = false;
        }

        public void Update(GameTime gameTime, Viewport port, Vector2 position)              // Viewport muss leider übergeben
        {                                                                                   // werden. Position kann zB die
                float elapsed = gameTime.ElapsedGameTime.Milliseconds;                      // Position der Spielfigur sein.
                //this.position = position;                                                 // Wird beim Testen nicht benutzt.

                if (Keyboard.GetState().IsKeyDown(Keys.Left))                               // Pfeiltasten bewegen, X und Y zoomen
                {
                    MoveLeft(3.0f);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    MoveRight(3.0f);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    MoveUp(3.0f);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    MoveDown(3.0f);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Y))
                {
                    ZoomOut(0.4f, 0.1f);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.X))
                {
                    ZoomIn(2.0f, 0.1f);
                }

                if (Keyboard.GetState().IsKeyUp(Keys.X) && Keyboard.GetState().IsKeyUp(Keys.Y)) { Reset(0.2f); }

                matrix = Matrix.CreateTranslation(new Vector3(-this.position.X, -this.position.Y, 0)) *
                                         Matrix.CreateRotationZ(0) *
                                         Matrix.CreateScale(new Vector3(zoom, zoom, 0)) *
                                         Matrix.CreateTranslation(new Vector3( port.Width* 0.5f, port.Height * 0.5f, 0));
            
        }



        public void MoveLeft(float speed)
        {
            position.X -= speed;
        }

        public void MoveRight(float speed)
        {
            position.X += speed;
        }

        public void MoveUp(float speed)
        {
            position.Y += speed;
        }

        public void MoveDown(float speed)
        {
            position.Y -= speed;
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

        public Matrix GetMatrix()                                   // Diese Matrix wird dem spriteBatch übergeben, der
        {                                                           // das ganze Bild zeichnet.
            return matrix;
        }

    }
}
