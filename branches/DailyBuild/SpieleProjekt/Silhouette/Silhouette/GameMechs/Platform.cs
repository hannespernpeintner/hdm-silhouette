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

//Physik-Engine Klassen
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;
using Silhouette.Engine.Manager;
using Silhouette.Engine;

namespace Silhouette.GameMechs
{
    public class Platform : DrawableLevelObject
    {
        // Hannes: Die Platform-Klasse wird für alle statischen Objekte des Levels verwendet. Also alles, was das Grundgerüst des
        // Levels bildet.

        public Animation animation;                                                 // Speichert die zugehörige Animation / Einzelbild
        public String path;                                                         // Speichert den Ort des Ordners der Animation
        public int pictureCount;                                                    // Speichert, aus wievielen Einzelbildern sie besteht
        public float animationSpeed;                                                // Und noch die BILDER PRO SEKUNDE

        // Da wir keine einfachen Rectangles verwenden, brauchen wir eine Fixtureliste.
        private List<Fixture> _polygon;
        public List<Fixture> polygon { get { return _polygon; } set { _polygon = value; } }

        public Platform(Vector2 position, String path, int pictureCount, float animationSpeed)
        {
            this.position = position;
            this.path = path;
            this.pictureCount = pictureCount;
            this.animationSpeed = animationSpeed;
        }

        // Aufzurufen in der Initialise des Levels. Oder des Layers, entscheiden wir noch.
        public override void Initialise()
        {
            animation = new Animation();
            
        }

        // Aufzurufen in der Load des Levels. Oder des Layers, entscheiden wir noch.
        public override void LoadContent()
        {
            animation.Load(pictureCount, path, animationSpeed);
            polygon = FixtureManager.TextureToPolygon(animation.activePicture, 1.0f);
            polygon[0].Body.Position = position;
            polygon[0].Body.BodyType = BodyType.Static;
        }

        // Aufzurufen in der Update des Levels. Oder des Layers, entscheiden wir noch.
        public override void Update(GameTime gameTime)
        {
            animation.Update(gameTime);
        
        }
        // Aufzurufen in der Draw des Levels. Oder des Layers, entscheiden wir noch.
        public override void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch, position, Color.White);
        }
    }
}