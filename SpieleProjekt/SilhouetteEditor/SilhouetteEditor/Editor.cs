using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Silhouette;
using Silhouette.Engine;
using Silhouette.Engine.Manager;
using Silhouette.GameMechs;
using SilhouetteEditor.Forms;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
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

namespace SilhouetteEditor
{
    public enum FixtureType
    { 
        Rectangle,
        Circle
    }

    public enum EditorState
    { 
        IDLE,
        CAMERAMOVING,
        CREATE
    }

    class Editor
    {
        /* Sascha:
         * Die Hauptklasse des Editors. Stellt alle direkt im Editor gebrauchten Funktionen zur Verfügung.
        */
        static Editor Instance;
        public static Editor Default 
        {
            get
            {
                if (Instance == null) 
                    Instance = new Editor();
                return Instance;
            }
        }

        SpriteBatch spriteBatch;

        EditorState editorState;

        public Level level;
        public Layer selectedLayer;
        public List<LevelObject> selectedLevelObjects;

        public TextureWrapper currentTexture;

        Vector2 MouseWorldPosition, GrabbedPoint;

        KeyboardState kstate, oldkstate;
        MouseState mstate, oldmstate;

        //---> EditorLoop-Functions <---//

        public void Initialize()
        {
            spriteBatch = new SpriteBatch(EditorLoop.EditorLoopInstance.GraphicsDevice);
            selectedLevelObjects = new List<LevelObject>();
            editorState = EditorState.IDLE;
        }

        public void Update(GameTime gameTime)
        {
            kstate = Keyboard.GetState();
            mstate = Mouse.GetState();

            if (level == null)
                return;

            level.UpdateInEditor(gameTime);

            #region CameraControl
                if(kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
                {
                    Camera.PositionX -= Constants.CameraMovingSpeed;
                }
                if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
                {
                    Camera.PositionX += Constants.CameraMovingSpeed;
                }
                if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
                {
                    Camera.PositionY += Constants.CameraMovingSpeed;
                }
                if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
                {
                    Camera.PositionY -= Constants.CameraMovingSpeed;
                }
            #endregion

            #region getMouseWorldPosition
                Vector2 maincameraposition = Camera.Position;
                if (selectedLayer != null) Camera.Position *= selectedLayer.ScrollSpeed;
                MouseWorldPosition = Vector2.Transform(new Vector2(mstate.X, mstate.Y), Matrix.Invert(Camera.matrix));
                MouseWorldPosition = MouseWorldPosition.Round();
                MainForm.Default.MouseWorldPosition.Text = "Mouse: (" + MouseWorldPosition.X + ", " + MouseWorldPosition.Y + ")";
                Camera.Position = maincameraposition;
            #endregion

            #region Editorstate-Logic
                if (editorState == EditorState.IDLE)
                { 
                
                }
                if (editorState == EditorState.CREATE)
                { 
                
                }
            #endregion

            oldkstate = kstate;
            oldmstate = mstate;
        }

        public void Draw(GameTime gameTime)
        {
            if (level == null)
                return;

            level.Draw(gameTime);
        }

        //---> New/Load/Save <---//

        public void NewLevel(string name)
        {
            level = new Level();

            if (name.Length == 0)
                level.name = "Level";
            else
                level.name = name;

            level.InitializeInEditor(spriteBatch);
            level.LoadContentInEditor();
            MainForm.Default.UpdateTreeView();
        }

        public void LoadLevel()
        {

        }

        public void SaveLevel()
        {

        }

        //---> Add-Stuff <---//

        public void AddLayer(string name, int width, int height)
        {
            Layer l = new Layer();
            l.name = name;
            l.width = width;
            l.height = height;
            l.initializeLayer();
            level.layerList.Add(l);
            MainForm.Default.UpdateTreeView();
        }

        public void AddFixture(FixtureType fixtureType)
        {
            if (level.layerList.Count() == 0)
            {
                System.Windows.Forms.MessageBox.Show("There is no Layer to add Fixtures to it.", "Error", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }

            if (selectedLayer == null)
            {
                System.Windows.Forms.MessageBox.Show("You have to select a Layer to add Fixtures to it.", "Error", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        public void SetMousePosition(int ScreenX, int ScreenY)
        {
            Vector2 maincameraposition = Camera.Position;
            if (selectedLayer != null) Camera.Position *= selectedLayer.ScrollSpeed;
            MouseWorldPosition = Vector2.Transform(new Vector2(ScreenX, ScreenY), Matrix.Invert(Camera.matrix));
            Camera.Position = maincameraposition;
        }

        

        public Image getThumbNail(Bitmap bmp, int imgWidth, int imgHeight)
        {
            Bitmap retBmp = new Bitmap(imgWidth, imgHeight, System.Drawing.Imaging.PixelFormat.Format64bppPArgb);
            Graphics grp = Graphics.FromImage(retBmp);
            int tnWidth = imgWidth, tnHeight = imgHeight;
            if (bmp.Width > bmp.Height)
                tnHeight = (int)(((float)bmp.Height / (float)bmp.Width) * tnWidth);
            else if (bmp.Width < bmp.Height)
                tnWidth = (int)(((float)bmp.Width / (float)bmp.Height) * tnHeight);
            int iLeft = (imgWidth / 2) - (tnWidth / 2);
            int iTop = (imgHeight / 2) - (tnHeight / 2);
            grp.DrawImage(bmp, iLeft, iTop, tnWidth, tnHeight);
            retBmp.Tag = bmp;
            return retBmp;
        }

        public void createTextureWrapper(string path, int width, int height)
        {
            this.currentTexture = new TextureWrapper(path, width, height);
            paintTextureWrapper();
        }

        public void destroyTextureWrapper()
        {
            this.currentTexture = null;
        }

        public void paintTextureWrapper()
        {
            if (selectedLayer == null)
            {
                System.Windows.Forms.MessageBox.Show("You have to choose a Layer in order to be able to add textures to it!", "Warning", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                destroyTextureWrapper();
                return;
            }

            selectedLayer.layerTexture[currentTexture.width, currentTexture.height] = currentTexture.texture;
            selectedLayer.assetName[currentTexture.width, currentTexture.height] = Path.GetFileNameWithoutExtension(currentTexture.fullPath);

            MainForm.Default.UpdateTreeView();
            destroyTextureWrapper();
        }

        //---> Selection <---//

        //---> TreeViewSelection

        public void selectLayer(Layer l)
        {
            selectedLayer = l;
            MainForm.Default.propertyGrid1.SelectedObject = l;
        }

        public void selectLevel()
        {
            MainForm.Default.propertyGrid1.SelectedObject = this.level;
        }
    }
}
