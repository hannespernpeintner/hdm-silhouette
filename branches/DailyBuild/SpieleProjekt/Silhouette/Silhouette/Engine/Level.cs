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
    public class Level
    {
        /* Sascha:
         * Die Repräsentation eines Levels im Spiel.
        */

        #region Definitions
            public static World Physics;
            private const float _PixelPerMeter = 100.0f;
            public static float PixelPerMeter { get { return _PixelPerMeter; } }
            Vector2 gravitation;

            DebugViewXNA debugView;
            bool DebugViewEnabled = false;
            bool GraphicsEnabled = false;

            SpriteBatch spriteBatch;
            List<Layer> layerList;
            CollisionLayer collisionLayer;
            EventLayer eventLayer;

            public Camera camera;
            Matrix proj;

            KeyboardState keyboardState;
            KeyboardState oldKeyboardState;

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
                    Vector2 oldCameraPosition = camera.Position;
                    camera.Position *= l.scrollSpeed;
                    spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.matrix);
                    l.drawLayer(spriteBatch);
                    spriteBatch.End();
                    camera.Position = oldCameraPosition;
                }
            }
            if(!DebugViewEnabled)
                debugView.RenderDebugData(ref proj, ref camera.matrix);
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
