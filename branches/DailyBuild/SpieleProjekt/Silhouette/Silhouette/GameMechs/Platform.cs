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

        public String path;                                                         // Speichert den Ort des Ordners der Animation
        public Texture2D texture;

        // Da wir keine einfachen Rectangles verwenden, brauchen wir eine Fixtureliste.
        private List<Fixture> _polygon;
        public List<Fixture> polygon { get { return _polygon; } set { _polygon = value; } }

        public Platform(Vector2 position, String path)
        {
            this.position = position;
            this.path = path;
        }

        // Aufzurufen in der Initialise des Levels. Oder des Layers, entscheiden wir noch.
        public override void Initialise()
        {
            
        }

        // Aufzurufen in der Load des Levels. Oder des Layers, entscheiden wir noch.
        public override void LoadContent()
        {
            polygon = FixtureManager.TextureToPolygon(texture, BodyType.Static, position, 1.0f);
        }

        // Aufzurufen in der Update des Levels. Oder des Layers, entscheiden wir noch.
        public override void Update(GameTime gameTime)
        {
        
        }
        // Aufzurufen in der Draw des Levels. Oder des Layers, entscheiden wir noch.
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}