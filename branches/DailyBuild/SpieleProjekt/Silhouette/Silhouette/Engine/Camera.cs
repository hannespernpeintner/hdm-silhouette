﻿using System;
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

namespace Silhouette.Engine
{
    public static class Camera
    {
        static Vector2 _Position;
        public static Vector2 Position
        {
            get
            {
                return _Position;
            }
            set
            {
                _Position = value;
                updateMatrix();
            }
        }

        static float _Rotation;
        public static float Rotation
        {
            get
            {
                return _Rotation;
            }
            set
            {
                _Rotation = value;
                updateMatrix();
            }
        }

        static float _Scale;
        public static float Scale
        {
            get
            {
                return _Scale;
            }
            set
            {
                _Scale = value;
                updateMatrix();
            }
        }

        public static Matrix matrix;
        static Vector2 viewport; 


        public static void initialise(float width, float height)
        {
            _Position = Vector2.Zero;
            _Rotation = 0;
            _Scale = 1.0f;
            viewport = new Vector2(width, height);
            updateMatrix();
        }

        public static void updateMatrix()
        {
            matrix = Matrix.CreateTranslation(-_Position.X, -_Position.Y, 0.0f) *
                     Matrix.CreateRotationZ(_Rotation) *
                     Matrix.CreateScale(_Scale) *
                     Matrix.CreateTranslation(viewport.X / 2, viewport.Y / 2, 0.0f);
        }
       
        public static void updateViewport(float width, float height)
        {
            viewport.X = width;
            viewport.Y = height;
            updateMatrix();
        }

    }
}
