using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
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
        /* Sascha:
         * Die Repräsentation eines Levels im Spiel.
        */

        #region Definitions
            [XmlAttribute()]
            public string name;
            public static World Physics;
            private const float _PixelPerMeter = 100.0f;
            public static float PixelPerMeter { get { return _PixelPerMeter; } }
            Vector2 gravitation;

            public Vector2 levelStartPosition;

            DebugViewXNA debugView;
            bool DebugViewEnabled = false;
            bool GraphicsEnabled = false;

            SpriteBatch spriteBatch;

            private List<Layer> _layerList;
            private CollisionLayer _collisionLayer;
            private EventLayer _eventLayer;

            public List<Layer> layerList { get { return _layerList; } }
            public CollisionLayer collisionLayer { get { return _collisionLayer; } set { _collisionLayer = value; } }
            public EventLayer eventLayer { get { return _eventLayer; } set { _eventLayer = value; } }

            Matrix proj;

            KeyboardState keyboardState;
            KeyboardState oldKeyboardState;

            private const string LevelFilePath = "/Level";
        #endregion

        public Level()
        {
            _layerList = new List<Layer>();
        }

        public void Initialize()
        {
            this.spriteBatch = new SpriteBatch(GameLoop.gameInstance.GraphicsDevice);
            gravitation = new Vector2(0.0f, 9.8f);
            Physics = new World(gravitation);
            debugView = new DebugViewXNA(Level.Physics);
            Camera.initialize(0, 0);

            foreach (Layer l in layerList)
            {
                l.initializeLayer();
            }

            if(collisionLayer != null)
                collisionLayer.Initialize();
            if(eventLayer != null)
                eventLayer.Initialize();
        }

        public void LoadContent()
        {
            proj = Matrix.CreateOrthographicOffCenter(0, GameSettings.Default.resolutionWidth / PixelPerMeter, GameSettings.Default.resolutionHeight / PixelPerMeter, 0, 0, 1);

            foreach (Layer l in layerList)
            {
                l.loadLayer();
            }

            if (collisionLayer != null)
                collisionLayer.LoadContent();
            if (eventLayer != null)
            eventLayer.LoadContent();
        }

        public void Update(GameTime gameTime)
        {
            Physics.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f, (1f / 30f)));

            foreach (Layer l in layerList)
            {
                l.updateLayer(gameTime);
            }

            if (collisionLayer != null)
                collisionLayer.updateCollisionLayer(gameTime);
            if (eventLayer != null)
                eventLayer.updateEventLayer(gameTime);

            #region DebugView
                keyboardState = Keyboard.GetState();

                if (keyboardState.IsKeyDown(Keys.F1) && oldKeyboardState.IsKeyUp(Keys.F1))
                    DebugViewEnabled = !DebugViewEnabled;
                if (keyboardState.IsKeyDown(Keys.F2) && oldKeyboardState.IsKeyUp(Keys.F2))
                    EnableOrDisableFlag(DebugViewFlags.DebugPanel);
                if (keyboardState.IsKeyDown(Keys.F3) && oldKeyboardState.IsKeyUp(Keys.F3))
                    EnableOrDisableFlag(DebugViewFlags.Shape);
                if (keyboardState.IsKeyDown(Keys.F4) && oldKeyboardState.IsKeyUp(Keys.F4))
                    EnableOrDisableFlag(DebugViewFlags.Joint);
                if (keyboardState.IsKeyDown(Keys.F5) && oldKeyboardState.IsKeyUp(Keys.F5))
                    EnableOrDisableFlag(DebugViewFlags.AABB);
                if (keyboardState.IsKeyDown(Keys.F6) && oldKeyboardState.IsKeyUp(Keys.F6))
                    EnableOrDisableFlag(DebugViewFlags.CenterOfMass);
                if (keyboardState.IsKeyDown(Keys.F7) && oldKeyboardState.IsKeyUp(Keys.F7))
                    EnableOrDisableFlag(DebugViewFlags.Pair);
                if (keyboardState.IsKeyDown(Keys.F8) && oldKeyboardState.IsKeyUp(Keys.F8))
                {
                    EnableOrDisableFlag(DebugViewFlags.ContactPoints);
                    EnableOrDisableFlag(DebugViewFlags.ContactNormals);
                }
                if (keyboardState.IsKeyDown(Keys.F9) && oldKeyboardState.IsKeyDown(Keys.F9))
                    EnableOrDisableFlag(DebugViewFlags.PolygonPoints);
                if (keyboardState.IsKeyDown(Keys.F10) && oldKeyboardState.IsKeyDown(Keys.F10))
                    GraphicsEnabled = !GraphicsEnabled;

                oldKeyboardState = keyboardState;
            #endregion
        }

        public void Draw(GameTime gameTime)
        {
            if (!GraphicsEnabled)
            {
                foreach (Layer l in layerList)
                {
                    Vector2 oldCameraPosition = Camera.Position;
                    Camera.Position *= l.scrollSpeed;
                    spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Camera.matrix);
                    l.drawLayer(spriteBatch);
                    spriteBatch.End();
                    Camera.Position = oldCameraPosition;
                }
            }
            //if(!DebugViewEnabled)
                //debugView.RenderDebugData(ref proj, ref Camera.matrix);
        }

        public static void LoadLevelFile(int levelNumber)
        {

        }

        #region DebugViewMethods
            private void EnableOrDisableFlag(DebugViewFlags flag)
            {
                if ((debugView.Flags & flag) == flag)
                    debugView.RemoveFlags(flag);
                else
                    debugView.AppendFlags(flag);
            }
        #endregion
    }
}
