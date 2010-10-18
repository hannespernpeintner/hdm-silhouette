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
using Silhouette.PartikelEngine;
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

        Texture2D plattformTexture, boxTexture;     //Physiktest
        Fixture plattformFixture, boxFixture;       //Physiktest
        Vector2 plattformPosition, boxPosition;     //Physiktest

        public GameLoop()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            particleManager = new ParticleManager();
        }

        protected override void Initialize()
        {
            //Voreinstellungen gemäß der Spezifikationen
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();

            //Initialisierung der Partikelenginehelferklasse ParticleManager
            particleManager.initialize(graphics, this);

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
            particleManager.loadParticles(this);    //Lädt und initialisiert alle Partikel


            //Physiktest
            plattformTexture = Content.Load<Texture2D>("Sprites/Plattform");
            plattformFixture = FixtureFactory.CreateRectangle(physicSimulation, plattformTexture.Width / pixelsPerMeter, plattformTexture.Height / pixelsPerMeter, 1);
            plattformPosition.X = graphics.PreferredBackBufferWidth / 2;
            plattformPosition.Y = 600;
            plattformFixture.Body.Position = new Vector2((plattformPosition.X + (plattformTexture.Width / 2)) / pixelsPerMeter, (plattformPosition.Y + (plattformTexture.Height / 2)) / pixelsPerMeter);
            plattformFixture.Body.BodyType = BodyType.Static;

            boxTexture = Content.Load<Texture2D>("Sprites/Box");
            boxFixture = FixtureFactory.CreateRectangle(physicSimulation, boxTexture.Width / pixelsPerMeter, boxTexture.Height / pixelsPerMeter, 1);
            boxPosition.X = graphics.PreferredBackBufferWidth / 2;
            boxPosition.Y = 100;
            boxFixture.Body.Position = new Vector2(boxPosition.X / pixelsPerMeter, boxPosition.Y / pixelsPerMeter);
            boxFixture.Body.BodyType = BodyType.Dynamic;
        }

        protected override void UnloadContent(){}

        protected override void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.A))
            { 
                boxFixture.Body.ApplyForce(new Vector2(0.0f, -10f));
            }
            if (kb.IsKeyDown(Keys.D))
            {
                boxFixture.Body.ApplyForce(new Vector2(1.0f, 0.0f));
            }
            if (kb.IsKeyDown(Keys.S))
            {
                boxFixture.Body.ApplyForce(new Vector2(-1.0f, 0.0f));
            }
            boxPosition.X = boxFixture.Body.Position.X * pixelsPerMeter;
            boxPosition.Y = boxFixture.Body.Position.Y * pixelsPerMeter;
            //Aktualisiert alle Partikel, die im Partikelmanager angemeldet sind
            particleManager.updateParticles(gameTime);
            //Aktualisiert die Physiksimulation
            physicSimulation.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f, (1f / 30f)));
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);  //Hintergrundfarbe Schwarz

            spriteBatch.Begin();
            spriteBatch.Draw(boxTexture, new Vector2(boxPosition.X - (boxTexture.Width/2), boxPosition.Y - (boxTexture.Height/2)), Color.White);
            spriteBatch.Draw(plattformTexture, plattformPosition, Color.White);
            spriteBatch.End();
            particleManager.drawParticles();    //Zeichnet alle Partikeleffekte
            base.Draw(gameTime);
        }
    }
}
