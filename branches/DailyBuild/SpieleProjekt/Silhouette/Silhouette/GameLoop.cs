using System;
using System.Collections.Generic;
using System.Linq;
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

        ParticleManager particleManager;

        World physicSimulation;
        Vector2 gravitation;
        const float pixelsPerMeter = 100.0f;        //Umwandlungseinheit von Pixel in die physikalische Einheit der Physikengine

        public GameLoop()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            particleManager = new ParticleManager(this, graphics);
            Components.Add(particleManager);
        }

        protected override void Initialize()
        {
            //Voreinstellungen gemäß der Spezifikationen
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();

            //Initialisierung der Physikengine mit Übergabe der Gravitationsstärke
            gravitation = new Vector2(0.0f, 9.0f);
            physicSimulation = new World(gravitation);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            FontManager.loadFonts(this);            //Lädt alle Fonts, die im FontManager deklariert wurden
        }

        protected override void UnloadContent(){}

        protected override void Update(GameTime gameTime)
        {
            //Aktualisiert die Physiksimulation
            physicSimulation.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f, (1f / 30f)));
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);  //Hintergrundfarbe Schwarz
            base.Draw(gameTime);
        }
    }
}
