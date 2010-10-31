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
        }

        protected override void Initialize()
        {
            gameInstance = this;
            level = new Level();        //Provisorisch
            level.Initialize();
            //Voreinstellungen gemäß der Spezifikationen
            GameSettings.Initialise();
            GameSettings.ApplyChanges(ref graphics);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //Lädt alle Fonts, die im FontManager deklariert wurden
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
            GraphicsDevice.Clear(Color.Black);
            level.Draw(gameTime);       //Provisorisch
            base.Draw(gameTime);
        }
    }
}
