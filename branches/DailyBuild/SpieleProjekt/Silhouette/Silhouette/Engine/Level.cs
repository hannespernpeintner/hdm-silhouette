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
            private const float _PixelPerMeter = 100.0f;
            public static float PixelPerMeter { get { return _PixelPerMeter; } }
            Vector2 gravitation;
            SpriteBatch spriteBatch;
            List<Layer> layerList;

            Camera camera;

            private const string LevelFilePath = "/Level";
        #endregion

        public Level()
        {
            spriteBatch = new SpriteBatch(GameLoop.gameInstance.GraphicsDevice);
            layerList = new List<Layer>();
        }

        public void Initialize()
        {
            gravitation = new Vector2(0.0f, 9.8f);
            Physics = new World(gravitation);
            camera = new Camera(GameSettings.Default.resolutionWidth, GameSettings.Default.resolutionHeight);
        }

        public void LoadContent()
        {
            foreach (Layer l in layerList)
            {
                l.loadLayer();
            }
        }

        public void Update(GameTime gameTime)
        {
            Physics.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f, (1f / 30f)));
            foreach (Layer l in layerList)
            {
                l.updateLayer(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (Layer l in layerList)
            {
                spriteBatch.Begin(/*SpriteSortMode.Deferred, null, null, null, null, null, camera.matrix*/);
                l.drawLayer(spriteBatch);
                spriteBatch.End();
            }
        }

        public void LoadLevelFile(int levelNumber)
        {

        }
    }
}
