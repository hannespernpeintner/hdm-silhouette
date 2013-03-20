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
using Silhouette.Engine.Effects;
using Silhouette.Engine;
using Silhouette.GameMechs;

//Physik-Engine Klassen
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;


namespace Silhouette.Engine
{
    public partial class Level
    {
        public void InitializeInEditor(GraphicsDevice graphics, SpriteBatch spriteBatch, float viewportWidth, float viewportHeight)
        {
            this.spriteBatch = spriteBatch;
            _flipFlop = new RenderTargetFlipFlop(ref spriteBatch);
            _flipFlop.InitialiseInEditor(graphics, viewportWidth, viewportHeight);
            _Gravitation = new Vector2(0.0f, 9.8f);
            Physics = new World(_Gravitation);
            debugView = new DebugViewXNA(Level.Physics);
            Camera.initialize(viewportWidth, viewportHeight);
            Camera.Position = new Vector2(viewportWidth / 2, viewportHeight / 2);

            this._contentPath = Path.Combine(Directory.GetCurrentDirectory(), "Content");

            foreach (Layer l in layerList)
            {
                l.initializeLayer();
            }
        }

        public void LoadContentInEditor(GraphicsDeviceManager graphicsM, GraphicsDevice graphics, ContentManager content)
        {
            ParticleManager.initializeInEditor(content);
            EffectManager.loadEffectsInEditor(graphics, content);
            proj = Matrix.CreateOrthographicOffCenter(0, GameSettings.Default.resolutionWidth / PixelPerMeter, GameSettings.Default.resolutionHeight / PixelPerMeter, 0, 0, 1);

            if (Effects == null)
            {
                Effects = new List<EffectObject>();
            }

            foreach (EffectObject eo in Effects)
            {
                //eo.Initialise();
                eo.loadContentInEditor(graphics, content);
            }

            foreach (Layer l in layerList)
            {
                l.loadContentInEditor(graphicsM, graphics, content);
                //l.loadLayerInEditor();
            }
        }

        public void UpdateInEditor(GameTime gameTime, Boolean physicsEnabled) //<---- This is so fuckin ugly, but our software design is MESSED UP BEYOND BELIEVE!!!!!!!!
        {

            if (physicsEnabled)
            {
                Physics.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f, (1f / 30f)));

            }


            foreach (Layer layer in _layerList)
            {
                layer.updateLayerInEditor(gameTime);
            }

            foreach (EffectObject eo in Effects)
            {
                eo.UpdateInEditor(gameTime);
            }

        }

        public void resetPhysics()
        {
            foreach (Layer layer in _layerList)
            {
                foreach (LevelObject go in layer.loList)
                {
                    if (go is InteractiveObject)
                    {
                        go.Initialise();
                        go.LoadContent();

                    }
                }
            }
        }

        public void DrawInEditor(GraphicsDevice graphics, int treeviewOffset)
        {
            if (!isVisible)
                return;

            if (!GraphicsEnabled)
            {

                _flipFlop.DrawInEditor(this, graphics);
                graphics.SetRenderTarget(null);
                graphics.Clear(Color.White);
                spriteBatch.Begin();

                spriteBatch.Draw(_flipFlop.Result, Vector2.Zero, Color.White);
                Primitives.Instance.drawBoxFilled(spriteBatch, new Rectangle(0, 0, GameSettings.Default.resolutionWidth, 96), Color.Black);
                Primitives.Instance.drawBoxFilled(spriteBatch, new Rectangle(0, GameSettings.Default.resolutionHeight - 96, GameSettings.Default.resolutionWidth, 96), Color.Black);

                spriteBatch.End();
            }
        }

        public void SaveLevel(string fullPath)
        {
            //transactional saving
            try
            {
                FileStream file = FileManager.SaveLevelFile(fullPath + ".tmp");

                if (file != null)
                {
                    BinaryFormatter serializer = new BinaryFormatter();
                    serializer.Serialize(file, this);
                    file.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception(@"Something went terribly wrong while saving your file! " + e.Message);
            }

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            System.IO.File.Move(fullPath + ".tmp", fullPath);
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
