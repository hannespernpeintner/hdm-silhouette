using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Silhouette.Engine.Manager;
using Silhouette.Engine.Screens;

//Physik-Engine Klassen
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;

namespace Silhouette.Engine
{
    [Serializable]
    public class Level
    {
        /* Sascha:
         * Die Repräsentation eines Levels im Spiel.
        */

        #region Definitions

            public static World Physics;
            const float _PixelPerMeter = 100.0f;
            public static float PixelPerMeter { get { return _PixelPerMeter; } }
            Vector2 gravitation;
            SpriteBatch spriteBatch;
            List<Layer> layerList;

            private const string LevelFilePath = "/Level";
        #endregion

        public void Initialize()
        {
            gravitation = new Vector2(0.0f, 9.8f);
            Physics = new World(gravitation);
        }

        public void LoadContent()
        {

        }

        public void Update(GameTime gameTime)
        {
            Physics.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f, (1f / 30f)));
        }

        public void Draw(GameTime gameTime)
        {

        }

        public void LoadLevel(string relativePath, int levelNumber)
        {

        }
    }
}
