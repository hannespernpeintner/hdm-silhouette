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
    public enum EventType
    { 
    
    }

    public enum PhysicsType
    { 
    
    }

    public enum PrimitiveType
    { 
        Rectangle,
        Circle,
        Path
    }

    public enum FixtureType
    { 
        Rectangle,
        Circle,
        Path
    }

    public enum EditorState
    { 
        IDLE,
        CREATE_FIXTURES,
        CREATE_PRIMITIVES,
        CREATE_TEXTURES,
        CREATE_INTERACTIVE,
        CREATE_ANIMATION,
        CREATE_PHYSICS,
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

        public FixtureType currentFixture;
        public PrimitiveType currentPrimitive;
        public PhysicsType currentPhysics;
        public EventType currentEvent;

        public bool fixtureStarted;
        public bool primitiveStarted;

        List<Vector2> clickedPoints, initialPosition, initialScale;
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
            initialScale = new List<Vector2>();
            editorState = EditorState.IDLE;

            NewLevel("");
            MainForm.Default.loadFolder(level.contentPath);
            MainForm.Default.loadFolderInteractive(level.contentPath);
            MainForm.Default.EditorStatus.Text = "Editorstatus: IDLE";
            MainForm.Default.ZoomStatus.Text = "Zoom: 100%";
        }

        public void Update(GameTime gameTime)
        {
            kstate = Keyboard.GetState();
            mstate = Mouse.GetState();

            if (level == null)
                return;

            level.UpdateInEditor(gameTime);

            #region CameraControl
                if(kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A) && !kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl))
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
                    if (zoom >= 110) zoom = 100;
                    MainForm.Default.ZoomStatus.Text = "Zoom: " + zoom.ToString() + "%";
                    Camera.Scale = zoom / 100.0f;
                }
                if (mwheeldelta < 0)
                {
                    float zoom = (float)Math.Round(Camera.Scale * 10) * 10.0f - 10.0f;
                    if (zoom <= 0.0f) zoom = 10;
                    MainForm.Default.ZoomStatus.Text = "Zoom: " + zoom.ToString() + "%";
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
                    MainForm.Default.EditorStatus.Text = "Editorstatus: Idle";

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
                    }

                    if (mstate.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && oldmstate.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                    {
                        if (selectedLevelObjects.Count > 0)
                        {
                            GrabbedPoint = MouseWorldPosition - selectedLevelObjects[0].position;

                            initialScale.Clear();
                            foreach (LevelObject selLO in selectedLevelObjects)
                            {
                                if(selLO.canScale())
                                    initialScale.Add(selLO.getScale());
                            }

                            editorState = EditorState.SCALING;
                        }
                    }

                    /* Sascha:
                     * Wenn der Editor im Status IDLE ist und der Benutzer die Entfernen - Taste drückt, werden alle selektierten LevelObjects gelöscht.
                    */

                    if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Delete) && oldkstate.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Delete))
                    {
                        deleteLevelObjects();
                    }

                    /* Sascha:
                     * Durch drücken von ShiftLeft kann man alle momentan selektierten Objekte kopieren.
                    */

                    if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift) && oldkstate.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.LeftShift) && selectedLevelObjects.Count > 0)
                    {
                        List<LevelObject> selectedLevelObjectsCopy = new List<LevelObject>();
                        foreach (LevelObject lo in selectedLevelObjects)
                        {
                            LevelObject lo2 = (LevelObject)lo.clone();
                            selectedLevelObjectsCopy.Add(lo2);
                        }
                        foreach (LevelObject lo in selectedLevelObjectsCopy)
                        {
                            lo.name = lo.getPrefix() + lo.layer.getNextObjectNumber();
                            AddLevelObject(lo);
                        }
                        selectLevelObject(selectedLevelObjectsCopy[0]);
                        MainForm.Default.UpdateTreeView();

                        foreach (LevelObject lo in selectedLevelObjectsCopy)
                        {
                            selectedLevelObjects.Add(lo);
                        }
                        startPositioning();
                    }

                    /* Sascha:
                     * Mehrfachauswahl durch drücken von LeftControl.
                    */

                    if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl) && oldkstate.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.LeftControl) && levelObject != null)
                    {
                        if (selectedLevelObjects.Contains(levelObject)) selectedLevelObjects.Remove(levelObject);
                        else selectedLevelObjects.Add(levelObject);
                    }

                    /* Sascha:
                     * Auswahl aller Objekte der aktuell selektierten Layer. 
                    */

                    if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl) && kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
                    {
                        selectedLevelObjects.Clear();
                        foreach (LevelObject lo in selectedLayer.loList)
                        {
                            selectedLevelObjects.Add(lo);
                        }
                    }
                }

                if (mstate.MiddleButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                {
                    if (selectedLevelObjects.Count > 0)
                    {
                        GrabbedPoint = MouseWorldPosition - selectedLevelObjects[0].position;
                        editorState = EditorState.ROTATING;
                    }
                }

                if (editorState == EditorState.CREATE_PRIMITIVES)
                {
                    MainForm.Default.EditorStatus.Text = "Editorstatus: Brush Primitives";

                    if (mstate.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && oldmstate.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                    {
                        clickedPoints.Add(MouseWorldPosition);

                        if (!primitiveStarted)
                            primitiveStarted = true;
                        else
                        {
                            if (currentPrimitive != PrimitiveType.Path)
                            {
                                paintPrimitiveObject();
                                clickedPoints.Clear();
                                primitiveStarted = false;
                            }
                        }
                    }
                    if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Back) && oldkstate.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Back))
                    {
                        if (currentPrimitive == PrimitiveType.Path && clickedPoints.Count > 1)
                        {
                            clickedPoints.RemoveAt(clickedPoints.Count - 1);
                        }
                    }

                    if (mstate.MiddleButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && oldmstate.MiddleButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                    {
                        if (currentPrimitive == PrimitiveType.Path && primitiveStarted)
                        {
                            paintPrimitiveObject();
                            clickedPoints.Clear();
                            primitiveStarted = false;
                        }
                    }
                    if (mstate.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && oldmstate.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                    {
                        if (primitiveStarted)
                        {
                            clickedPoints.Clear();
                            primitiveStarted = false;
                        }
                        else
                        {
                            clickedPoints.Clear();
                            primitiveStarted = false;
                            editorState = EditorState.IDLE;
                        }
                    }
                }

                if (editorState == EditorState.CREATE_FIXTURES)
                {
                    MainForm.Default.EditorStatus.Text = "Editorstatus: Brush Collision";

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
                    MainForm.Default.EditorStatus.Text = "Editorstatus: Brush Texture";

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
                    MainForm.Default.EditorStatus.Text = "Editorstatus: Positioning";

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

                if (editorState == EditorState.SCALING)
                {
                    MainForm.Default.EditorStatus.Text = "Editorstatus: Scaling";

                    Vector2 newdistance = MouseWorldPosition - selectedLevelObjects[0].position;
                    float factor = newdistance.Length() / GrabbedPoint.Length();
                    int i = 0;
                    foreach (LevelObject selLO in selectedLevelObjects)
                    {
                        if (selLO.canScale())
                        {
                            Vector2 newscale = initialScale[i];
                            if (!kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Y)) newscale.X = initialScale[i].X * (((factor - 1.0f) * 0.5f) + 1.0f);
                            if (!kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.X)) newscale.Y = initialScale[i].Y * (((factor - 1.0f) * 0.5f) + 1.0f);
                            selLO.setScale(newscale);

                            if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl))
                            {
                                Vector2 scale;
                                scale.X = (float)Math.Round(selLO.getScale().X * 10) / 10;
                                scale.Y = (float)Math.Round(selLO.getScale().Y * 10) / 10;
                                selLO.setScale(scale);
                            }
                            i++;
                        }
                    }
                    MainForm.Default.propertyGrid1.Refresh();
                    if (mstate.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released && oldmstate.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        editorState = EditorState.IDLE;
                    }
                }

                if (editorState == EditorState.ROTATING)
                {
                    MainForm.Default.EditorStatus.Text = "Editorstatus: Rotating";

                    float deltatheta = (float)Math.Atan2(GrabbedPoint.Y, GrabbedPoint.X) - (float)Math.Atan2(MouseWorldPosition.Y, MouseWorldPosition.X);
                    int i = 0;
                    foreach (LevelObject selLO in selectedLevelObjects)
                    {
                        if (selLO.canRotate())
                        {
                            selLO.setRotation(deltatheta);
                            if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl))
                            {
                                selLO.setRotation((float)Math.Round(selLO.getRotation() / MathHelper.PiOver4) * MathHelper.PiOver4);
                            }
                            i++;
                        }
                    }
                    MainForm.Default.propertyGrid1.Refresh();
                    if (mstate.MiddleButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                    {
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

            level.DrawInEditor();
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
            level.contentPath = Path.Combine(Directory.GetCurrentDirectory(), "Content");
            MainForm.Default.UpdateTreeView();
        }

        public void LoadLevel(string filename)
        {
            Editor.Default.levelFileName = filename;
            Editor.Default.level = Level.LoadLevelFile(filename);
            level.InitializeInEditor(spriteBatch);
            level.LoadContentInEditor(EditorLoop.EditorLoopInstance.GraphicsDevice);
            editorState = EditorState.IDLE;
            selectLayer(level.layerList.First());
            selectLevelObject(selectedLayer.loList.First());
            MainForm.Default.UpdateTreeView();
        }

        public void SaveLevel(string fullPath)
        {
            level.SaveLevel(fullPath);
        }

        //---> Add-Stuff <---//

        public void AddLayer(string name)
        {
            if (level == null)
            {
                MessageBox.Show("You have to create a Level in order to be able to add Layers!", "Error", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }

            Layer l = new Layer();
            l.name = name;
            l.level = level;
            l.initializeInEditor();
            level.layerList.Add(l);
            selectLayer(level.layerList.Last());
            MainForm.Default.UpdateTreeView();
        }

        public void AddLevelObject(LevelObject lo)
        {
            selectedLayer.loList.Add(lo);
            selectLevelObject(lo);
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

        public void AddPrimitive(PrimitiveType primitiveType)
        {
            if (level.layerList.Count() == 0)
            {
                System.Windows.Forms.MessageBox.Show("There is no Layer to add Primitives to it.", "Error", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            if (selectedLayer == null)
            {
                System.Windows.Forms.MessageBox.Show("You have to select a Layer to add Primitives to it.", "Error", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }

            currentPrimitive = primitiveType;
            primitiveStarted = false;
            editorState = EditorState.CREATE_PRIMITIVES;
        }

        //---> Create Objects <---//

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

        public void destroyCurrentObject()
        {
            editorState = EditorState.IDLE;
            this.currentObject = null;
        }

        //---> Paint Objects <---//

        public void copyLevelObjects(Layer layer)
        {
            if (layer == null)
                return;

            foreach (LevelObject lo in selectedLevelObjects)
            {
                LevelObject temp = lo.clone();
                temp.name = temp.getPrefix() + temp.layer.getNextObjectNumber();
                layer.loList.Add(temp);
            }
            selectedLevelObjects.Clear();
            MainForm.Default.UpdateTreeView();
        }

        /* Sascha:
         * Der Name ist eventuell etwas irreführend. Es wird in dieser Funktion nicht wirklich etwas gezeichnet, sondern es wir ein Objekt modeliert.
         * Je nachdem welches Objekt momentan durch den Editor gesetzt wird, erzeugt diese Funktion ein entsprechendes LevelObject mit den gewünschten
         * Attributen.
        */

        public void paintCurrentObject(bool continueAfterPaint)
        {
            if (currentObject is TextureObject)
            {
                TextureObject temp = (TextureObject)currentObject;
                TextureObject to = new TextureObject(temp.fullPath);
                to.texture = temp.texture;
                to.position = MouseWorldPosition;
                to.name = to.getPrefix() + selectedLayer.getNextObjectNumber();
                to.layer = selectedLayer;
                to.loadContentInEditor(EditorLoop.EditorLoopInstance.GraphicsDevice);
                AddLevelObject(to);
            }
            if (currentObject is InteractiveObject)
            {
                InteractiveObject temp = (InteractiveObject)currentObject;
                InteractiveObject io = new InteractiveObject(temp.fullPath);
                io.texture = temp.texture;
                io.position = MouseWorldPosition;
                io.name = io.getPrefix() + selectedLayer.getNextObjectNumber();
                io.layer = selectedLayer;
                io.loadContentInEditor(EditorLoop.EditorLoopInstance.GraphicsDevice);
                AddLevelObject(io);
            }
            MainForm.Default.UpdateTreeView();

            if (!continueAfterPaint)
                destroyCurrentObject();
        }

        /* Sascha:
         * Hier gilt das gleiche wie oben. Hier werden Wrapperobjekte für Fixtures (Kollisionsdomänen) erzeugt, die im Editor noch transformiert werden können. Erst in der 
         * Spielengine werden sie dann in Fixtures umgewandelt mit der Funktion ToFixture().
        */

        public void paintFixtureItem()
        {
            switch (currentFixture)
            {
                case FixtureType.Rectangle:
                    LevelObject l1 = new RectangleFixtureItem(Extensions.RectangleFromVectors(clickedPoints[0], clickedPoints[1]));
                    l1.name = l1.getPrefix() + selectedLayer.getNextObjectNumber();
                    l1.layer = selectedLayer;
                    selectedLayer.loList.Add(l1);
                    selectLevelObject(l1);
                    break;
                case FixtureType.Circle:
                    LevelObject l2 = new CircleFixtureItem(clickedPoints[0], (MouseWorldPosition - clickedPoints[0]).Length());
                    l2.name = l2.getPrefix() + selectedLayer.getNextObjectNumber();
                    l2.layer = selectedLayer;
                    selectedLayer.loList.Add(l2);
                    selectLevelObject(l2);
                    break;
                case FixtureType.Path:
                    LevelObject l3 = new PathFixtureItem(clickedPoints.ToArray());
                    l3.name = l3.getPrefix() + selectedLayer.getNextObjectNumber();
                    l3.layer = selectedLayer;
                    selectedLayer.loList.Add(l3);
                    selectLevelObject(l3);
                    break;
            }
            MainForm.Default.UpdateTreeView();
        }

        /* Sascha:
         * Hier gilt das gleiche wie oben. Einfache Primitive zum schnellen zeichnen einer farbigen Fläche.
        */

        public void paintPrimitiveObject()
        {
            switch (currentPrimitive)
            {
                case PrimitiveType.Rectangle:
                    LevelObject l1 = new RectanglePrimitiveObject(Extensions.RectangleFromVectors(clickedPoints[0], clickedPoints[1]));
                    l1.name = l1.getPrefix() + selectedLayer.getNextObjectNumber();
                    l1.layer = selectedLayer;
                    selectedLayer.loList.Add(l1);
                    selectLevelObject(l1);
                    break;
                case PrimitiveType.Circle:
                    LevelObject l2 = new CirclePrimitiveObject(clickedPoints[0], (MouseWorldPosition - clickedPoints[0]).Length());
                    l2.name = l2.getPrefix() + selectedLayer.getNextObjectNumber();
                    l2.layer = selectedLayer;
                    selectedLayer.loList.Add(l2);
                    selectLevelObject(l2);
                    break;
                case PrimitiveType.Path:
                    LevelObject l3 = new PathPrimitiveObject(clickedPoints.ToArray());
                    l3.name = l3.getPrefix() + selectedLayer.getNextObjectNumber();
                    l3.layer = selectedLayer;
                    selectedLayer.loList.Add(l3);
                    selectLevelObject(l3);
                    break;
            }
            MainForm.Default.UpdateTreeView();
        }

        //---> Selection <---//

        //---> TreeViewSelection

        /* Sascha:
         * Funktion um ein LevelObject auszuwählen. Für currentLevelObject UND die PropertyGrid.
        */

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

        /* Sascha:
         * Funktion um eine Layer auszuwählen. Für currentLevelObject UND die PropertyGrid.
        */

        public void selectLayer(Layer l)
        {
            if (l == null)
                return;

            selectedLayer = l;
            MainForm.Default.propertyGrid1.SelectedObject = l;
            MainForm.Default.Selection.Text = "Selected Layer: " + l.name;
        }

        /* Sascha:
         * Wählt das aktuelle Level in der PropertyGrid aus.
        */

        public void selectLevel()
        {
            MainForm.Default.propertyGrid1.SelectedObject = this.level;
        }

        //---> TreeViewDelete

        public void deleteLayer(Layer l)
        {
            if (selectedLayer == null)
                return;

            if (MessageBox.Show("Do you really want to delete the layer " + Editor.Default.selectedLayer.name + "?", "Question", MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == DialogResult.Yes)
                level.layerList.Remove(l);

            MainForm.Default.UpdateTreeView();
        }

        public void deleteLevelObjects()
        {
            if (MessageBox.Show("Do you really want to delete all selected LevelObjects?", "Question", MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == DialogResult.Yes)
            {
                foreach (LevelObject lo in selectedLevelObjects)
                {
                    lo.layer.loList.Remove(lo);
                }
                selectLevelObject(null);
                MainForm.Default.UpdateTreeView();
            }     
        }

        //---> Zusätzliche Editorfunktionalität <---//

        /* Sascha:
         * Durch diese Funktion wird der Editor in den Status POSITIONING versetzt, was die Neupositionierung von LevelObjects erlaubt.
        */

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

        /* Sascha:
         * Diese Funktion zeichnet alle Sachen, die rein editorspezifisch sind. Zum Beispiel die Vorschaubilder beim Platzieren von Textures oder dem Selection Frame.
         * Alle anderen Objekte (die später auch im Spiel gezeichnet werden) werden direkt in der Engine gezeichnet, auch mit speziellen EditorDraw-Funktionen.
        */

        public void drawEditorRelated()
        {
            foreach (Layer l in level.layerList)
            {
                if (l.isVisible)
                {
                    Vector2 oldCameraPosition = Camera.Position;
                    Camera.Position *= l.ScrollSpeed;
                    spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Camera.matrix);
                    foreach (LevelObject lo in l.loList)
                    {
                        if (lo is RectangleFixtureItem)
                        {
                            RectangleFixtureItem r = (RectangleFixtureItem)lo;
                            if (r.isVisible)
                            {
                                Microsoft.Xna.Framework.Color color = Constants.ColorFixtures;
                                if (r.mouseOn) color = Constants.ColorMouseOn;
                                Primitives.Instance.drawBoxFilled(spriteBatch, r.rectangle, color);
                            }
                        }
                        if (lo is CircleFixtureItem)
                        {
                            CircleFixtureItem c = (CircleFixtureItem)lo;
                            if (c.isVisible)
                            {
                                Microsoft.Xna.Framework.Color color = Constants.ColorFixtures;
                                if (c.mouseOn) color = Constants.ColorMouseOn;
                                Primitives.Instance.drawCircleFilled(spriteBatch, c.position, c.radius, color);
                            }
                        }
                        if (lo is PathFixtureItem)
                        {
                            PathFixtureItem p = (PathFixtureItem)lo;
                            if (p.isVisible)
                            {
                                Microsoft.Xna.Framework.Color color = Constants.ColorFixtures;
                                if (p.mouseOn) color = Constants.ColorMouseOn;
                                if (p.isPolygon)
                                    Primitives.Instance.drawPolygon(spriteBatch, p.WorldPoints, color, p.lineWidth);
                                else
                                    Primitives.Instance.drawPath(spriteBatch, p.WorldPoints, color, p.lineWidth);
                            }
                        }
                    }

                    if (selectedLevelObjects.Count > 0)
                    {
                        foreach (LevelObject lo2 in selectedLevelObjects)
                        {
                            if (l == selectedLayer)
                                lo2.drawSelectionFrame(spriteBatch, Camera.matrix);
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
                    if (l == selectedLayer && editorState == EditorState.CREATE_PRIMITIVES && primitiveStarted)
                    {
                        switch (currentPrimitive)
                        {
                            case PrimitiveType.Rectangle:
                                Microsoft.Xna.Framework.Rectangle r = Extensions.RectangleFromVectors(clickedPoints[0], MouseWorldPosition);
                                Primitives.Instance.drawBoxFilled(spriteBatch, r, Constants.ColorPrimitives);
                                break;
                            case PrimitiveType.Circle:
                                Primitives.Instance.drawCircleFilled(spriteBatch, clickedPoints[0], (MouseWorldPosition - clickedPoints[0]).Length(), Constants.ColorPrimitives);
                                break;
                            case PrimitiveType.Path:
                                Primitives.Instance.drawPath(spriteBatch, clickedPoints.ToArray(), Constants.ColorPrimitives, Constants.DefaultPathItemLineWidth);
                                Primitives.Instance.drawLine(spriteBatch, clickedPoints.Last(), MouseWorldPosition, Constants.ColorPrimitives, Constants.DefaultPathItemLineWidth);
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
        }

        /* Sascha:
         * Gibt das LevelObject an der momentanen Mouse-Position zurück. Verwendet wird dafür die Klasse Rectangle mit ihrer Funktion Contains().
        */

        public LevelObject getItemAtPosition(Vector2 worldPosition)
        {
            if (selectedLayer == null)
                return null;
            return selectedLayer.getItemAtPosition(worldPosition);
        }

        /* Sascha:
         * Mit SetMousePosition() setzen wir die Position der Mouse bei Drag and Drop wieder in die PictureView. Ohne diese Funktion würde die Transformation der Layer
         * durch die Camera nicht berücksichtigt!
        */

        public void SetMousePosition(int ScreenX, int ScreenY)
        {
            Vector2 maincameraposition = Camera.Position;
            if (selectedLayer != null) Camera.Position *= selectedLayer.ScrollSpeed;
            MouseWorldPosition = Vector2.Transform(new Vector2(ScreenX, ScreenY), Matrix.Invert(Camera.matrix));
            Camera.Position = maincameraposition;
        }

        /* Sascha:
         * Diese Funktion erstellt aus einem Bild ein Vorschaubild, dass dann in der ListView angezeigt wird.
        */

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
