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
using Silhouette.Engine.Manager;

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

            GameSettings.Initialise();
            GameSettings.ApplyChanges(ref graphics);    
        }

        protected override void Initialize()
        {
            Primitives.Instance.Initialize(this.GraphicsDevice);
            SoundManager.Initialize();
            VideoManager.Initialize();
            level = Level.LoadLevelFile("12345");
            level.Initialize();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            VideoManager.LoadContent();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            
            DebugViewXNA.LoadContent(GameLoop.gameInstance.GraphicsDevice, GameLoop.gameInstance.Content);
            FontManager.loadFonts();

           
            
            level.LoadContent();        //Provisorisch
        }

        protected override void UnloadContent(){}

        protected override void Update(GameTime gameTime)
        {
            VideoManager.Update(gameTime);
            SoundManager.Update(gameTime);
            level.Update(gameTime);     //Provisorisch
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
           
            GraphicsDevice.Clear(Color.White);

            if (!VideoManager.IsPlaying)
            {
                level.Draw();       //Provisorisch

            }
            else
            {
                spriteBatch.Begin();
                spriteBatch.Draw(VideoManager.VideoFrame,  new Rectangle(0, 0, (int) GameSettings.Default.resolutionWidth, (int) GameSettings.Default.resolutionHeight), Color.White);
                spriteBatch.End();
                
            }
            base.Draw(gameTime);
        }
    }
}
