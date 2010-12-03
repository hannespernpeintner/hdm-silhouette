using System;
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

//Klassen unserer eigenen Engine
using Silhouette;
using Silhouette.Engine;

//Partikel-Engine Klassen
using Silhouette.Engine.PartikelEngine;
using ProjectMercury;
using ProjectMercury.Emitters;
using ProjectMercury.Modifiers;
using ProjectMercury.Renderers;

//Physik-Engine Klassen
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;

namespace Silhouette
{
    public class GameLoop : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static GameLoop gameInstance;

        DisplayFPS displayFPS;
        Level level;

        public GameLoop()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            displayFPS = new DisplayFPS(this);
            Components.Add(displayFPS);

            gameInstance = this;
            //Voreinstellungen gemäß der Spezifikationen
            GameSettings.Initialise();
            GameSettings.ApplyChanges(ref graphics);    
        }

        protected override void Initialize()
        {
            Primitives.Instance.Initialize(this.GraphicsDevice);
            level = Level.LoadLevelFile("12345");        //Provisorisch
            level.Initialize();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            DebugViewXNA.LoadContent(GameLoop.gameInstance.GraphicsDevice, GameLoop.gameInstance.Content);
            FontManager.loadFonts();
            level.LoadContent();        //Provisorisch
        }

        protected override void UnloadContent(){}

        protected override void Update(GameTime gameTime)
        {
            level.Update(gameTime);     //Provisorisch
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            level.Draw();       //Provisorisch
            base.Draw(gameTime);
        }
    }
}
