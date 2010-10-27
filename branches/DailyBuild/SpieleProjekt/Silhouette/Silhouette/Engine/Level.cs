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
         * Die Repräsentation eines Levels im Spiel.
        */
        GameLoop currentGame;

        private static World _Physics;

        public static World Physics
        {
            get { return _Physics; }
        }

        private const string LevelFilePath = "/Level";

        ScreenManager screenManager;

        public Level(Game game)
            : base(game)
        {
            currentGame = (GameLoop)game;
        }

        public override void Initialize()
        {
            LevelSettings.Initialise();
            _Physics = new World(LevelSettings.Default.gravitation);

            ScreenManager.Initialise(currentGame);
            base.Initialize();
        }

        protected override void LoadContent()
        {
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
            FileStream file = FileManager.LoadLevelFile(LevelFilePath, levelNumber);

            if (file == null)
            {
                //Sascha: Abbruchmeldung im Menü, da Fehler
            }
            else
            { 
                //Sascha: Level laden
            }
        }
    }
}
