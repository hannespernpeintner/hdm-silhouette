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
        CREATE_FIXTURES
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
        public string levelFileName;

        public Level level;
        public Layer selectedLayer;
        public List<LevelObject> selectedLevelObjects;

        public TextureWrapper currentTexture;
        public FixtureType currentFixture;
        public bool fixtureStarted;

        List<Vector2> clickedPoints;
        Vector2 MouseWorldPosition, GrabbedPoint;

        KeyboardState kstate, oldkstate;
        MouseState mstate, oldmstate;

        //---> EditorLoop-Functions <---//

        public void Initialize()
        {
            spriteBatch = new SpriteBatch(EditorLoop.EditorLoopInstance.GraphicsDevice);
            selectedLevelObjects = new List<LevelObject>();
            clickedPoints = new List<Vector2>();
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
                    LevelObject levelObject = getItemAtPosition(MouseWorldPosition);

                    if (levelObject != null)
                    {
                        MainForm.Default.SelectedItem.Text = "Object: " + levelObject.name;
                    }
                    else
                    {
                        MainForm.Default.SelectedItem.Text = "Object: -";
                    }
                }
                if (editorState == EditorState.CREATE_FIXTURES)
                {
                    if (mstate.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && oldmstate.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                    {
                        clickedPoints.Add(MouseWorldPosition);

                        if (!fixtureStarted)
                            fixtureStarted = true;
                        else
                        {
                            paintFixtureItem();
                            clickedPoints.Clear();
                            fixtureStarted = false;
                        }
                    }
                    if (mstate.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && oldmstate.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                    {
                        if (fixtureStarted)
                        {
                            clickedPoints.Clear();
                            fixtureStarted = false;
                        }
                        else
                        {
                            clickedPoints.Clear();
                            fixtureStarted = false;
                            editorState = EditorState.IDLE;
                        }
                    }
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
            drawEditorRelated();
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

        public void LoadLevel(string filename)
        {
            Editor.Default.levelFileName = filename;
            Editor.Default.level = Level.LoadLevelFile(filename);
            level.InitializeInEditor(spriteBatch);
            level.LoadContentInEditor();
            LoadStuff();
            MainForm.Default.UpdateTreeView();
        }

        public void LoadStuff()
        {
            foreach (Layer l in level.layerList)
            {
                for (int x = 0; x < l.width; x++)
                    for (int y = 0; y < l.height; y++)
                    {
                        if (l.assetName[x, y] != null)
                            l.layerTexture[x, y] = EditorLoop.EditorLoopInstance.Content.Load<Texture2D>("Sprites/Layer/" + l.assetName[x, y]);
                    }

                foreach (LevelObject lo in l.loList)
                {
                    lo.LoadContent();
                }
            }
        }

        public void SaveLevel(string fullPath)
        {
            level.SaveLevel(fullPath);
        }

        //---> Add-Stuff <---//

        public void AddLayer(string name, int width, int height)
        {
            Layer l = new Layer();
            l.name = name;
            l.width = width;
            l.height = height;
            l.initializeLayerInEditor();
            level.layerList.Add(l);
            MainForm.Default.UpdateTreeView();
        }

        public void AddLevelObject(LevelObject lo)
        {
            selectedLayer.loList.Add(lo);
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
                return;
            }

            currentFixture = fixtureType;
            fixtureStarted = false;
            editorState = EditorState.CREATE_FIXTURES;
        }


        public void drawEditorRelated()
        { 
            foreach (Layer l in level.layerList)
            {
                Vector2 oldCameraPosition = Camera.Position;
                Camera.Position *= l.ScrollSpeed;
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Camera.matrix);
                foreach (LevelObject lo in l.loList)
                {
                    if (lo is RectangleFixtureItem)
                    { 
                        RectangleFixtureItem r = (RectangleFixtureItem)lo;
                        Primitives.Instance.drawBoxFilled(spriteBatch, r.rectangle, Constants.ColorFixtures);
                    }
                    if (lo is CircleFixtureItem)
                    {
                        CircleFixtureItem c = (CircleFixtureItem)lo;
                        Primitives.Instance.drawCircleFilled(spriteBatch, c.position, c.radius, Constants.ColorFixtures);
                    }
                }

                if (l == selectedLayer && editorState == EditorState.CREATE_FIXTURES && fixtureStarted)
                {
                    switch (currentFixture)
                    { 
                        case FixtureType.Rectangle:
                            Microsoft.Xna.Framework.Rectangle r = Constants.RectangleFromVectors(clickedPoints[0], MouseWorldPosition);
                            Primitives.Instance.drawBoxFilled(spriteBatch, r, Constants.ColorFixtures);
                            break;
                    }
                }
                spriteBatch.End();
                Camera.Position = oldCameraPosition;
            }
        }

        public LevelObject getItemAtPosition(Vector2 worldPosition)
        {
            if (selectedLayer == null)
                return null;
            return selectedLayer.getItemAtPosition(worldPosition);
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
            selectedLayer.assetName[currentTexture.width, currentTexture.height] = currentTexture.fullPath;

            MainForm.Default.UpdateTreeView();
            destroyTextureWrapper();
        }

        public void paintFixtureItem()
        {
            switch (currentFixture)
            { 
                case FixtureType.Rectangle:
                    LevelObject l1 = new RectangleFixtureItem(Constants.RectangleFromVectors(clickedPoints[0], clickedPoints[1]));
                    l1.name = l1.getPrefix() + selectedLayer.getNextObjectNumber();
                    selectedLayer.loList.Add(l1);
                    break;
                case FixtureType.Circle:
                    LevelObject l2 = new CircleFixtureItem(clickedPoints[0], (MouseWorldPosition - clickedPoints[0]).Length());
                    l2.name = l2.getPrefix() + selectedLayer.getNextObjectNumber();
                    selectedLayer.loList.Add(l2);
                    break;
            }
            MainForm.Default.UpdateTreeView();
        }

        //---> Selection <---//

        //---> TreeViewSelection

        public void selectLevelObject(LevelObject lo)
        {
            selectedLevelObjects.Clear();
            selectedLevelObjects.Add(lo);
            MainForm.Default.propertyGrid1.SelectedObject = lo;
            MainForm.Default.SelectedItem.Text = "Object: " + lo.name;
        }

        public void selectLayer(Layer l)
        {
            selectedLayer = l;
            MainForm.Default.propertyGrid1.SelectedObject = l;
            MainForm.Default.Selection.Text = "Selected Layer: " + l.name;
        }

        public void selectLevel()
        {
            MainForm.Default.propertyGrid1.SelectedObject = this.level;
        }
    }
}
