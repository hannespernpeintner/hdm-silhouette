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

//Physik-Engine Klassen
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;

namespace Silhouette.Engine
{
    public class Level : Microsoft.Xna.Framework.DrawableGameComponent
    {
        /* Sascha:
         * Die Repr�sentation eines Levels im Spiel.
        */

        private static World _Physics;

        public static World Physics
        {
            get { return _Physics; }
        }

        private const string LevelFilePath = "/Level";

        public Level(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            LevelSettings.Initialise();
            _Physics = new World(LevelSettings.Default.gravitation);

            ScreenManager.Initialise();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            LoadLevel(LevelFilePath, 1);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Physics.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f, (1f / 30f)));
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(LevelSettings.Default.backgroundColor);
            base.Draw(gameTime);
        }

        public void LoadLevel(string relativePath, int levelNumber)
        {
            /* Sascha: Momentan noch ohne Funktion, da der Leveleditor fehlt
            FileStream file = FileManager.LoadLevelFile(LevelFilePath, levelNumber);

            if (file == null)
            {
                //Sascha: Abbruchmeldung im Men�, da Fehler
            }
            else
            { 
                //Sascha: Level laden
            }
            */

            ScreenManager.Default.LoadScreens();
        }
    }
}
