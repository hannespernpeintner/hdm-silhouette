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
        public GraphicsDeviceManager graphics;
        public GameMechs.Player playerInstance;
        //public GameMechs.Tom playerInstance;

        public static GameLoop gameInstance;

        //DisplayFPS displayFPS;
        GameStateManager gameStateManager;

        public bool parameterNoVideo { get; set; }
        public string parameterLevelToLoad { get; set; }

        public GameLoop()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //displayFPS = new DisplayFPS(this);
            //Components.Add(displayFPS);

            gameInstance = this;

            GameSettings.Initialise();
            GameSettings.ApplyChanges(ref graphics);
        }

        protected override void Initialize()
        {
            Primitives.Instance.Initialize(this.GraphicsDevice);
            SoundManager.Initialize();
            VideoManager.Initialize();


            gameStateManager = new GameStateManager();
            gameStateManager.Initialize();
            TimerManager.initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            DebugViewXNA.LoadContent(GameLoop.gameInstance.GraphicsDevice, GameLoop.gameInstance.Content);

            VideoManager.LoadContent();
            FontManager.loadFonts();
            EffectManager.loadEffects();

            gameStateManager.LoadContent();
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            VideoManager.Update(gameTime);
            SoundManager.Update(gameTime);

            gameStateManager.Update(gameTime);
            TimerManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            gameStateManager.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
