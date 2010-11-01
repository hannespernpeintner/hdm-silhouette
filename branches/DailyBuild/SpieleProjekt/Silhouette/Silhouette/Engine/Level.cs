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

//Physik-Engine Klassen
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;

namespace Silhouette.Engine
{
    [Serializable]
    public class Level
    {
        /* Sascha:
         * Die Repräsentation eines Levels im Spiel.
        */

        #region Definitions
            [XmlAttribute()]
            string name;

            public static World Physics;
            private const float _PixelPerMeter = 100.0f;
            public static float PixelPerMeter { get { return _PixelPerMeter; } }
            Vector2 gravitation;

            DebugViewXNA debugView;
            bool DebugViewEnabled = false;

            SpriteBatch spriteBatch;
            List<Layer> layerList;

            public Camera camera;
            Matrix proj;

            private const string LevelFilePath = "/Level";
        #endregion

        public Level()
        {
            spriteBatch = new SpriteBatch(GameLoop.gameInstance.GraphicsDevice);
            layerList = new List<Layer>();
        }

        public void Initialize()
        {
            gravitation = new Vector2(0.0f, 9.8f);
            Physics = new World(gravitation);
            debugView = new DebugViewXNA(Level.Physics);
            camera = new Camera(0, 0);
        }

        public void LoadContent()
        {
            proj = Matrix.CreateOrthographicOffCenter(0, GameSettings.Default.resolutionWidth / PixelPerMeter, GameSettings.Default.resolutionHeight / PixelPerMeter, 0, 0, 1);

            foreach (Layer l in layerList)
            {
                l.loadLayer();
            }
        }

        public void Update(GameTime gameTime)
        {
            Physics.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f, (1f / 30f)));
            foreach (Layer l in layerList)
            {
                l.updateLayer(gameTime);
            }
            #region DebugView
                KeyboardState kb = Keyboard.GetState();
         
                if (kb.IsKeyDown(Keys.F1))
                    DebugViewEnabled = !DebugViewEnabled;
                if (kb.IsKeyDown(Keys.F2))
                    EnableOrDisableFlag(DebugViewFlags.DebugPanel);
                if (kb.IsKeyDown(Keys.F3))
                    EnableOrDisableFlag(DebugViewFlags.Shape);
                if (kb.IsKeyDown(Keys.F4))
                    EnableOrDisableFlag(DebugViewFlags.Joint);
                if (kb.IsKeyDown(Keys.F5))
                    EnableOrDisableFlag(DebugViewFlags.AABB);
                if (kb.IsKeyDown(Keys.F6))
                    EnableOrDisableFlag(DebugViewFlags.CenterOfMass);
                if (kb.IsKeyDown(Keys.F7))
                    EnableOrDisableFlag(DebugViewFlags.Pair);
                if (kb.IsKeyDown(Keys.F8))
                {
                    EnableOrDisableFlag(DebugViewFlags.ContactPoints);
                    EnableOrDisableFlag(DebugViewFlags.ContactNormals);
                }
                if (kb.IsKeyDown(Keys.F9))
                    EnableOrDisableFlag(DebugViewFlags.PolygonPoints);
            #endregion
        }

        public void Draw(GameTime gameTime)
        {
            foreach (Layer l in layerList)
            {
                Vector2 oldCameraPosition = camera.Position;
                camera.Position *= l.scrollSpeed;
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.matrix);
                l.drawLayer(spriteBatch);
                spriteBatch.End();
                camera.Position = oldCameraPosition;
            }

            if(!DebugViewEnabled)
                debugView.RenderDebugData(ref proj, ref camera.matrix);
        }

        public void LoadLevelFile(int levelNumber)
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
