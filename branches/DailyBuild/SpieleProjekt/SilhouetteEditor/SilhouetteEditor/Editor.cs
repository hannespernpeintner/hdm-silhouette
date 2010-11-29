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
        Circle,
        Path
    }

    public enum EditorState
    { 
        IDLE,
        CAMERAMOVING,
        CREATE_FIXTURES,
        CREATE_TEXTURES,
        CREATE_INTERACTIVE,
        CREATE_ANIMATION,
        CREATE_EVENTS,
        ROTATING,
        SCALING,
        POSITIONING,
        SELECTING
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

        public LevelObject currentObject;
        public LevelObject lastObject;
        public TextureWrapper currentTexture;
        public FixtureType currentFixture;
        public bool fixtureStarted;

        List<Vector2> clickedPoints, initialPosition;
        Vector2 MouseWorldPosition, GrabbedPoint;
        Microsoft.Xna.Framework.Rectangle selectionRectangle;

        KeyboardState kstate, oldkstate;
        MouseState mstate, oldmstate;

        //---> EditorLoop-Functions <---//

        public void Initialize()
        {
            spriteBatch = new SpriteBatch(EditorLoop.EditorLoopInstance.GraphicsDevice);
            selectedLevelObjects = new List<LevelObject>();
            clickedPoints = new List<Vector2>();
            initialPosition = new List<Vector2>();
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
                    Camera.PositionX -= Constants.CameraMovingSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
                {
                    Camera.PositionX += Constants.CameraMovingSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
                {
                    Camera.PositionY += Constants.CameraMovingSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
                {
                    Camera.PositionY -= Constants.CameraMovingSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                int mwheeldelta = mstate.ScrollWheelValue - oldmstate.ScrollWheelValue;
                if (mwheeldelta > 0)
                {
                    float zoom = (float)Math.Round(Camera.Scale * 10) * 10.0f + 10.0f;
                    Camera.Scale = zoom / 100.0f;
                }
                if (mwheeldelta < 0)
                {
                    float zoom = (float)Math.Round(Camera.Scale * 10) * 10.0f - 10.0f;
                    if (zoom <= 0.0f) return;
                    Camera.Scale = zoom / 100.0f;
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
                        levelObject.mouseOn = true;
                    }
                    else
                    {
                        MainForm.Default.SelectedItem.Text = "Object: -";
                    }
                    if (levelObject != lastObject && lastObject != null) lastObject.mouseOn = false;

                    lastObject = levelObject;

                    if (mstate.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && oldmstate.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                    {
                        if(selectedLevelObjects.Contains(levelObject))
                            startPositioning();
                        else if(!selectedLevelObjects.Contains(levelObject))
                        { 
                            selectLevelObject(levelObject);
                            if (levelObject != null)
                                startPositioning();
                            else
                            {
                                GrabbedPoint = MouseWorldPosition;
                                selectionRectangle = Microsoft.Xna.Framework.Rectangle.Empty;
                                editorState = EditorState.SELECTING;
                            }
                        }
                        else if(kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl) && levelObject != null)
                        {
                            if (selectedLevelObjects.Contains(levelObject))
                                selectedLevelObjects.Remove(levelObject);
                            else
                                selectedLevelObjects.Add(levelObject);
                        }                  
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
                            if (currentFixture != FixtureType.Path)
                            {
                                paintFixtureItem();
                                clickedPoints.Clear();
                                fixtureStarted = false;
                            }
                        }
                    }
                    if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Back) && oldkstate.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Back))
                    {
                        if (currentFixture == FixtureType.Path && clickedPoints.Count > 1)
                        {
                            clickedPoints.RemoveAt(clickedPoints.Count - 1);
                        }
                    }

                    if (mstate.MiddleButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && oldmstate.MiddleButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                    {
                        if (currentFixture == FixtureType.Path && fixtureStarted)
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

                if (editorState == EditorState.CREATE_TEXTURES || editorState == EditorState.CREATE_INTERACTIVE)
                {
                    if (mstate.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && oldmstate.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                    {
                        paintCurrentObject(true);
                    }
                    if (mstate.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && oldmstate.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                    {
                        editorState = EditorState.IDLE;
                    }
                }

                if (editorState == EditorState.POSITIONING)
                {
                    int i = 0;
                    foreach (LevelObject lo in selectedLevelObjects)
                    {
                        Vector2 newPosition = initialPosition[i] + MouseWorldPosition - GrabbedPoint;
                        lo.position = newPosition;
                        i++;
                    }
                    MainForm.Default.propertyGrid1.Refresh();
                    if (mstate.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && oldmstate.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        editorState = EditorState.IDLE;
                    }
                }

                if (editorState == EditorState.SELECTING)
                {
                    if (selectedLayer == null) return;
                    Vector2 distance = MouseWorldPosition - GrabbedPoint;
                    if (distance.Length() > 0)
                    {
                        selectedLevelObjects.Clear();
                        selectionRectangle = Extensions.RectangleFromVectors(GrabbedPoint, MouseWorldPosition);
                        foreach (LevelObject lo in selectedLayer.loList)
                        {
                            if (selectionRectangle.Contains((int)lo.position.X, (int)lo.position.Y)) selectedLevelObjects.Add(lo);
                        }
                    }
                    if (mstate.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                    {
                        if(selectedLevelObjects.Count > 0)
                            MainForm.Default.propertyGrid1.SelectedObject = selectedLevelObjects[0];
                        editorState = EditorState.IDLE;
                    }
                }
            #endregion

            oldkstate = kstate;
            oldmstate = mstate;
        }

        public void Draw()
        {
            if (level == null)
                return;

            level.Draw();
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
            level.LoadContentInEditor(EditorLoop.EditorLoopInstance.GraphicsDevice);
            MainForm.Default.UpdateTreeView();
        }

        public void LoadLevel(string filename)
        {
            Editor.Default.levelFileName = filename;
            Editor.Default.level = Level.LoadLevelFile(filename);
            level.InitializeInEditor(spriteBatch);
            level.LoadContentInEditor(EditorLoop.EditorLoopInstance.GraphicsDevice);
            MainForm.Default.UpdateTreeView();
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
            l.initializeInEditor();
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

        //---> Create Objects <---//

        public void createTextureWrapper(string path, int width, int height)
        {
            this.currentTexture = new TextureWrapper(path, width, height);
            paintTextureWrapper();
        }

        public void createTextureObject(string path)
        {
            editorState = EditorState.CREATE_TEXTURES;
            TextureObject to = new TextureObject(path);
            to.texture = Texture2DLoader.Instance.LoadFromFile(path);
            currentObject = to;
        }

        public void createInteractiveObject(string path)
        {
            editorState = EditorState.CREATE_INTERACTIVE;
            InteractiveObject io = new InteractiveObject(path);
            io.texture = Texture2DLoader.Instance.LoadFromFile(path);
            currentObject = io;
        }

        //---> Destroy Objects <---//

        public void destroyTextureWrapper()
        {
            editorState = EditorState.IDLE;
            this.currentTexture = null;
        }

        public void destroyCurrentObject()
        {
            editorState = EditorState.IDLE;
            this.currentObject = null;
        }

        //---> Paint Objects <---//

        public void paintCurrentObject(bool continueAfterPaint)
        {
            if (currentObject is TextureObject)
            {
                TextureObject temp = (TextureObject)currentObject;
                TextureObject to = new TextureObject(temp.fullPath);
                to.texture = temp.texture;
                to.position = MouseWorldPosition - new Vector2((to.texture.Width / 2), (to.texture.Height / 2));
                to.name = to.getPrefix() + selectedLayer.getNextObjectNumber();
                to.layer = selectedLayer;
                AddLevelObject(to);
            }
            if (currentObject is InteractiveObject)
            {
                InteractiveObject temp = (InteractiveObject)currentObject;
                InteractiveObject io = new InteractiveObject(temp.fullPath);
                io.texture = temp.texture;
                io.position = MouseWorldPosition - new Vector2((io.texture.Width / 2), (io.texture.Height / 2));
                io.name = io.getPrefix() + selectedLayer.getNextObjectNumber();
                io.layer = selectedLayer;
                AddLevelObject(io);
            }
            MainForm.Default.UpdateTreeView();

            if (!continueAfterPaint)
                destroyCurrentObject();
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
            selectedLayer.assetFullPath[currentTexture.width, currentTexture.height] = currentTexture.fullPath;
            selectedLayer.assetName[currentTexture.width, currentTexture.height] = Path.GetFileNameWithoutExtension(currentTexture.fullPath);

            MainForm.Default.UpdateTreeView();
            destroyTextureWrapper();
        }

        public void paintFixtureItem()
        {
            switch (currentFixture)
            {
                case FixtureType.Rectangle:
                    LevelObject l1 = new RectangleFixtureItem(Extensions.RectangleFromVectors(clickedPoints[0], clickedPoints[1]));
                    l1.name = l1.getPrefix() + selectedLayer.getNextObjectNumber();
                    l1.layer = selectedLayer;
                    selectedLayer.loList.Add(l1);
                    break;
                case FixtureType.Circle:
                    LevelObject l2 = new CircleFixtureItem(clickedPoints[0], (MouseWorldPosition - clickedPoints[0]).Length());
                    l2.name = l2.getPrefix() + selectedLayer.getNextObjectNumber();
                    l2.layer = selectedLayer;
                    selectedLayer.loList.Add(l2);
                    break;
                case FixtureType.Path:
                    LevelObject l3 = new PathFixtureItem(clickedPoints.ToArray());
                    l3.name = l3.getPrefix() + selectedLayer.getNextObjectNumber();
                    l3.layer = selectedLayer;
                    selectedLayer.loList.Add(l3);
                    break;
            }
            MainForm.Default.UpdateTreeView();
        }

        //---> Selection <---//

        //---> TreeViewSelection

        public void selectLevelObject(LevelObject lo)
        {
            selectedLevelObjects.Clear();

            if (lo != null)
            {
                selectedLevelObjects.Add(lo);
                selectedLayer = lo.layer;
                MainForm.Default.propertyGrid1.SelectedObject = lo;
                MainForm.Default.SelectedItem.Text = "Object: " + lo.name;
            }
            else
                selectLayer(selectedLayer);
        }

        public void selectLayer(Layer l)
        {
            if (l == null)
                return;

            selectedLayer = l;
            MainForm.Default.propertyGrid1.SelectedObject = l;
            MainForm.Default.Selection.Text = "Selected Layer: " + l.name;
        }

        public void selectLevel()
        {
            MainForm.Default.propertyGrid1.SelectedObject = this.level;
        }

        //---> TreeViewDelete

        public void deleteLayer(Layer l)
        {
            level.layerList.Remove(l);
            MainForm.Default.UpdateTreeView();
        }

        public void deleteLevelObject(LevelObject lo)
        {
            foreach (Layer l in level.layerList)
            {
                l.loList.Remove(lo);
            }
            MainForm.Default.UpdateTreeView();
        }

        //---> Zusätzliche Editorfunktionalität <---//

        public void startPositioning()
        {
            GrabbedPoint = MouseWorldPosition;
            initialPosition.Clear();

            foreach (LevelObject lo in selectedLevelObjects)
            {
                initialPosition.Add(lo.position);
            }

            editorState = EditorState.POSITIONING;
        }

        public void drawEditorRelated()
        {
            foreach (Layer l in level.layerList)
            {
                if (!l.isVisible)
                    return;
                Vector2 oldCameraPosition = Camera.Position;
                Camera.Position *= l.ScrollSpeed;
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Camera.matrix);
                foreach (LevelObject lo in l.loList)
                {
                    if (lo is RectangleFixtureItem)
                    {
                        RectangleFixtureItem r = (RectangleFixtureItem)lo;
                        Microsoft.Xna.Framework.Color color = Constants.ColorFixtures;
                        if (r.mouseOn) color = Constants.ColorMouseOn;
                        Primitives.Instance.drawBoxFilled(spriteBatch, r.rectangle, color);
                    }
                    if (lo is CircleFixtureItem)
                    {
                        CircleFixtureItem c = (CircleFixtureItem)lo;
                        Microsoft.Xna.Framework.Color color = Constants.ColorFixtures;
                        if (c.mouseOn) color = Constants.ColorMouseOn;
                        Primitives.Instance.drawCircleFilled(spriteBatch, c.position, c.radius, color);
                    }
                    if (lo is PathFixtureItem)
                    {
                        PathFixtureItem p = (PathFixtureItem)lo;
                        Microsoft.Xna.Framework.Color color = Constants.ColorFixtures;
                        if (p.mouseOn) color = Constants.ColorMouseOn;
                        if (p.isPolygon)
                            Primitives.Instance.drawPolygon(spriteBatch, p.WorldPoints, color, p.lineWidth);
                        else
                            Primitives.Instance.drawPath(spriteBatch, p.WorldPoints, color, p.lineWidth);
                    }
                }

                if (selectedLevelObjects.Count > 0)
                {
                    foreach (LevelObject lo2 in selectedLevelObjects)
                    {
                        lo2.drawSelectionFrame();
                    }
                }

                if (l == selectedLayer && editorState == EditorState.CREATE_FIXTURES && fixtureStarted)
                {
                    switch (currentFixture)
                    {
                        case FixtureType.Rectangle:
                            Microsoft.Xna.Framework.Rectangle r = Extensions.RectangleFromVectors(clickedPoints[0], MouseWorldPosition);
                            Primitives.Instance.drawBoxFilled(spriteBatch, r, Constants.ColorFixtures);
                            break;
                        case FixtureType.Circle:
                            Primitives.Instance.drawCircleFilled(spriteBatch, clickedPoints[0], (MouseWorldPosition - clickedPoints[0]).Length(), Constants.ColorFixtures);
                            break;
                        case FixtureType.Path:
                            Primitives.Instance.drawPath(spriteBatch, clickedPoints.ToArray(), Constants.ColorFixtures, Constants.DefaultPathItemLineWidth);
                            Primitives.Instance.drawLine(spriteBatch, clickedPoints.Last(), MouseWorldPosition, Constants.ColorFixtures, Constants.DefaultPathItemLineWidth);
                            break;
                    }
                }
                if (l == selectedLayer && editorState == EditorState.CREATE_TEXTURES)
                {
                    TextureObject to = (TextureObject)currentObject;
                    spriteBatch.Draw(to.texture, new Vector2(MouseWorldPosition.X, MouseWorldPosition.Y), null, new Microsoft.Xna.Framework.Color(1f, 1f, 1f, 7f), 0, new Vector2(to.texture.Width / 2, to.texture.Height / 2), 1, SpriteEffects.None, 0);
                }
                if (l == selectedLayer && editorState == EditorState.CREATE_INTERACTIVE)
                {
                    InteractiveObject io = (InteractiveObject)currentObject;
                    spriteBatch.Draw(io.texture, new Vector2(MouseWorldPosition.X, MouseWorldPosition.Y), null, new Microsoft.Xna.Framework.Color(1f, 1f, 1f, 7f), 0, new Vector2(io.texture.Width / 2, io.texture.Height / 2), 1, SpriteEffects.None, 0);
                }
                if (l == selectedLayer && editorState == EditorState.SELECTING)
                {
                    Primitives.Instance.drawBoxFilled(spriteBatch, selectionRectangle, Constants.ColorSelectionBox);
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
    }
}
