using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Silhouette.Engine.Manager;
using Silhouette.Engine.Screens;
using Silhouette.Engine;

//Physik-Engine Klassen
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;

namespace Silhouette.Engine
{
    public partial class Level
    {
        public void InitializeInEditor(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
            _Gravitation = new Vector2(0.0f, 9.8f);
            Physics = new World(_Gravitation);
            debugView = new DebugViewXNA(Level.Physics);
            Camera.initialize(0, 0);

            foreach (Layer l in layerList)
            {
                l.initializeLayer();
            }
        }

        public void LoadContentInEditor(GraphicsDevice graphics)
        {
            proj = Matrix.CreateOrthographicOffCenter(0, GameSettings.Default.resolutionWidth / PixelPerMeter, GameSettings.Default.resolutionHeight / PixelPerMeter, 0, 0, 1);

            foreach (Layer l in layerList)
            {
                l.loadContentInEditor(graphics);
            }
        }

        public void UpdateInEditor(GameTime gameTime)
        {
            Physics.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f, (1f / 30f)));
        }

        public void DrawInEditor()
        {
            if (!isVisible)
                return;

            if (!GraphicsEnabled)
            {
                foreach (Layer l in layerList)
                {
                    Vector2 oldCameraPosition = Camera.Position;
                    Camera.Position *= l.ScrollSpeed;
                    spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Camera.matrix);
                    l.drawInEditor(spriteBatch);
                    spriteBatch.End();
                    Camera.Position = oldCameraPosition;
                }
            }
        }

        public void SaveLevel(string fullPath)
        {
            FileStream file = FileManager.SaveLevelFile(fullPath);

            if (file != null)
            {
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(file, this);
                file.Close();
            }
        }

        public Layer getLayerByName(string name)
        {
            foreach (Layer layer in layerList)
            {
                if (layer.name == name) return layer;
            }
            return null;
        }
    }
}
