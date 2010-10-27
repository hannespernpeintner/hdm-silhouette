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
        public static World Physics;
        private const string LevelFilePath = "/Level";

        public Level(Game game)
            : base(game)
        {
        }

        protected override void LoadContent()
        {
            LevelSettings.Initialise();
            Physics = new World(LevelSettings.Default.gravitation);
            base.LoadContent();
        }
        public override void Initialize()
        {
            base.Initialize();
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

        public void LoadLevel(string relativePath)
        {
        }
    }
}
