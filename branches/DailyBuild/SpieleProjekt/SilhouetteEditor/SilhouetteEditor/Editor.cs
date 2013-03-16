using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Silhouette;
using Silhouette.Engine;
using Silhouette.Engine.Manager;
using Silhouette.GameMechs.Events;
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
        //Object
        Move,
        ChangeVisibility,

        //Physic
        ChangeBodyType,
        ApplyForce,


        //Audio
        Fade,
        Equalizer,
        ModifyPlayback,
        Mute,
        SetVolume,
        Crossfader,
        Reverb,

        //Video
        Play,
        ChangeEffect,

        Death,
        SaveState,
        LoadLevel,
        Camera
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
        CREATE_PARTICLE,
        CREATE_JOINT,
        ROTATING,
        SCALING,
        POSITIONING,
        SELECTING,
        MOVING
    }

    public class Editor
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

        public bool EditorPhysicsEnabled { get; set; }

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
        public EventType currentEvent;

        public bool fixtureStarted;
        public bool primitiveStarted;
        public bool eventStarted;
        public bool physicsStarted;
        public bool originsVisible;

        List<Vector2> clickedPoints, initialPosition, initialScale;
        List<float> initialRotation;
        Vector2 MouseWorldPosition, GrabbedPoint;
        Microsoft.Xna.Framework.Rectangle selectionRectangle;

        KeyboardState kstate, oldkstate;
        MouseState mstate, oldmstate;
        float gameCamScale = 0.4f; //Das hier ist das Scaling, was im Spiel 100% ergibt...

        //---> EditorLoop-Functions <---//

        public void Initialize()
        {
            spriteBatch = new SpriteBatch(EditorLoop.EditorLoopInstance.GraphicsDevice);
            selectedLevelObjects = new List<LevelObject>();
            clickedPoints = new List<Vector2>();
            initialPosition = new List<Vector2>();
            initialScale = new List<Vector2>();
            initialRotation = new List<float>();
            editorState = EditorState.IDLE;
            EditorPhysicsEnabled = false;
            NewLevel("");
            MainForm.Default.EditorStatus.Text = "Editorstatus: IDLE";
            MainForm.Default.ZoomStatus.Text = "Zoom: 100%";
            //ParticleManager.initializeInEditor(EditorLoop.EditorLoopInstance.Content);
        }

        public void Update(GameTime gameTime)
        {
            kstate = Keyboard.GetState();
            mstate = Mouse.GetState();

            if (level == null)
                return;

            /* Sascha:
             * Startet die Updatefunktionen aller Elemente des Levels. Es gibt jeweils verschiedene für Spiel und Editor. 
            */

            level.UpdateInEditor(gameTime, Editor.Default.EditorPhysicsEnabled);

            /* Sascha:
             * Diese Funktion kontrolliert die Camera im Editor-Viewport. Benutzt wird dabei die statische Klasse Camera aus der Spielengine.
             * Gibt die aktuellen Camera-Daten in der StatusBar aus.
            */

            #region CameraControl
            if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A) && !kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl))
            {
                Camera.PositionX -= Constants.CameraMovingSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D) && !kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl))
            {
                Camera.PositionX += Constants.CameraMovingSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S) && !kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl))
            {
                Camera.PositionY += Constants.CameraMovingSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W) && !kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl))
            {
                Camera.PositionY -= Constants.CameraMovingSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            int mwheeldelta = mstate.ScrollWheelValue - oldmstate.ScrollWheelValue;
            if (mwheeldelta > 0)
            {
                float zoom = (float)Math.Round(Camera.Scale * 10) * 10.0f + 10.0f;
                if (zoom >= 110) zoom = 100;
                MainForm.Default.ZoomStatus.Text = "Zoom: " + (zoom).ToString() + "%";
                Camera.Scale = zoom / 100.0f;
            }
            if (mwheeldelta < 0)
            {
                float zoom = (float)Math.Round(Camera.Scale * 10) * 10.0f - 10.0f;
                if (zoom <= 0.0f) zoom = 10;
                MainForm.Default.ZoomStatus.Text = "Zoom: " + (zoom).ToString() + "%";
                Camera.Scale = zoom / 100.0f;
            }
            #endregion

            /* Sascha:
             * Editorspezifische Anzeigen werden hier aktiviert/deaktiviert.
            */

            #region EditorShortcuts
            if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F1) && oldkstate.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.F1))
            {
                originsVisible = !originsVisible;
            }
            if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl) && kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S) && oldkstate.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.S))
            {
                if (this.levelFileName != null)
                    this.SaveLevel(this.levelFileName);
                else
                {
                    SaveFileDialog dialog = new SaveFileDialog();
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        this.levelFileName = dialog.FileName;
                        this.SaveLevel(dialog.FileName);
                    }
                }
            }
            #endregion

            /* Sascha:
             * Gibt die aktuelle Mouseposition zurück, berücksichtigt dabei auch den Scrollspeed und die aktuelle Transformation der selektierten Layer.
            */

            #region getMouseWorldPosition
            Vector2 maincameraposition = Camera.Position;
            if (selectedLayer != null) Camera.Position *= selectedLayer.ScrollSpeed;
            MouseWorldPosition = Vector2.Transform(new Vector2(mstate.X, mstate.Y), Matrix.Invert(Camera.matrix));
            MouseWorldPosition = MouseWorldPosition.Round();
            MainForm.Default.MouseWorldPosition.Text = "Mouse: (" + MouseWorldPosition.X + ", " + MouseWorldPosition.Y + ")";
            Camera.Position = maincameraposition;
            #endregion

            /* Sascha:
             * Die Satusmethoden des Editors. Hier wird bei jedem durchlauf der aktuelle Status des Editors geprüft und bei bestimmten Aktionen geändert. Arbeitet stark mit
             * drawEditorRelated zusammen um statusabhängige Anzeigen zu zeichnen.
            */

            #region Editorstate-Logic

            #region IDLE
            if (editorState == EditorState.IDLE)
            {
                MainForm.Default.EditorStatus.Text = "Editorstatus: Idle";
                MainForm.Default.GameView.Cursor = Cursors.Default;

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
                    if (selectedLevelObjects.Contains(levelObject))
                        startPositioning();
                    else if (!selectedLevelObjects.Contains(levelObject))
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
                        foreach (LevelObject lo in selectedLevelObjects)
                        {
                            if (lo.canScale())
                                initialScale.Add(lo.getScale());
                        }

                        editorState = EditorState.SCALING;
                    }
                }

                if (mstate.MiddleButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                {
                    if (selectedLevelObjects.Count > 0)
                    {
                        GrabbedPoint = MouseWorldPosition - selectedLevelObjects[0].position;

                        initialRotation.Clear();
                        foreach (LevelObject lo in selectedLevelObjects)
                        {
                            if (lo.canRotate())
                            {
                                initialRotation.Add(lo.getRotation());
                            }
                        }

                        editorState = EditorState.ROTATING;
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
                 * Durch drücken von Control + Alt kann man alle momentan selektierten Objekte kopieren.
                */

                if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl) && kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftAlt) && selectedLevelObjects.Count > 0)
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
                 * Mehrfachauswahl durch drücken von LeftShift.
                */

                if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift) && oldkstate.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.LeftShift) && levelObject != null)
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

                /* Sascha:
                 * Photoshop-Navigation!
                */

                if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space))
                {
                    MainForm.Default.GameView.Cursor = Cursors.NoMove2D;

                    if (mstate.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        GrabbedPoint = MouseWorldPosition;
                        editorState = EditorState.MOVING;
                    }
                }

                /* Sascha:
                 * Automatische Erstellung eines Kollisionsobjekt aus einem gegebenen Primitiv.
                */

                if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.C) && oldkstate.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.C) && selectedLevelObjects.Count > 0)
                {
                    List<LevelObject> copy = new List<LevelObject>();

                    foreach (LevelObject lo in selectedLevelObjects)
                    {
                        if (lo is PrimitiveObject)
                            copy.Add(lo);
                    }

                    selectedLevelObjects.Clear();

                    foreach (LevelObject lo in copy)
                    {
                        if (lo is RectanglePrimitiveObject)
                        {
                            RectanglePrimitiveObject r = (RectanglePrimitiveObject)lo;

                            RectangleCollisionObject rf = new RectangleCollisionObject(r.rectangle);
                            rf.name = rf.getPrefix() + r.layer.getNextObjectNumber();
                            rf.layer = r.layer;
                            r.layer.loList.Add(rf);
                            selectedLevelObjects.Add(rf);
                        }

                        if (lo is CirclePrimitiveObject)
                        {
                            CirclePrimitiveObject c = (CirclePrimitiveObject)lo;

                            CircleCollisionObject cf = new CircleCollisionObject(c.position, c.radius);
                            cf.name = cf.getPrefix() + c.layer.getNextObjectNumber();
                            cf.layer = c.layer;
                            c.layer.loList.Add(cf);
                            selectedLevelObjects.Add(cf);
                        }

                        if (lo is PathPrimitiveObject)
                        {
                            PathPrimitiveObject p = (PathPrimitiveObject)lo;

                            PathCollisionObject pf = new PathCollisionObject((Vector2[])p.WorldPoints.Clone());
                            pf.name = pf.getPrefix() + p.layer.getNextObjectNumber();
                            pf.layer = p.layer;
                            p.layer.loList.Add(pf);
                            selectedLevelObjects.Add(pf);
                        }
                    }

                    MainForm.Default.UpdateTreeView();
                    startPositioning();
                }

                /* Sascha:
                 * Setzt die Startposition des Players an die Mausposition.
                */

                if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.P) && oldkstate.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.P) && !(kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl)))
                {
                    if (level.layerList.Contains(level.getLayerByName("Player")))
                    {
                        selectLayer(level.getLayerByName("Player"));
                        level.startPosition = MouseWorldPosition;
                    }
                    else
                    {
                        AddLayer("Player");
                        selectLayer(level.getLayerByName("Player"));
                        level.startPosition = MouseWorldPosition;
                    }
                }

                if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl) && kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.P) && oldkstate.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.P) && level.layerList.Contains(level.getLayerByName("Player")))
                {
                    Camera.Scale = 0.4f;
                    Camera.Position = new Vector2(level.startPosition.X, level.startPosition.Y - (MainForm.Default.GameView.Height / 2) - 50);
                    MainForm.Default.ZoomStatus.Text = "Zoom: 40%";
                }
            }
            #endregion

            #region CREATE_PRIMITIVES
            if (editorState == EditorState.CREATE_PRIMITIVES)
            {
                MainForm.Default.EditorStatus.Text = "Editorstatus: Brush Primitives";
                MainForm.Default.GameView.Cursor = Cursors.Cross;

                if (mstate.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && oldmstate.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                {
                    clickedPoints.Add(MouseWorldPosition);

                    if (currentPrimitive == PrimitiveType.Path)
                        selectedLevelObjects.Clear();

                    if (!primitiveStarted)
                        primitiveStarted = true;
                    else
                    {
                        if (currentPrimitive != PrimitiveType.Path)
                        {
                            createPrimitiveObject();
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
                    if (currentPrimitive == PrimitiveType.Path && primitiveStarted && clickedPoints.Count > 1)
                    {
                        createPrimitiveObject();
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
            #endregion

            #region CREATE_FIXTURES
            if (editorState == EditorState.CREATE_FIXTURES)
            {
                MainForm.Default.EditorStatus.Text = "Editorstatus: Brush Collision";
                MainForm.Default.GameView.Cursor = Cursors.Cross;

                if (mstate.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && oldmstate.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                {
                    clickedPoints.Add(MouseWorldPosition);

                    if (currentFixture == FixtureType.Path)
                        selectedLevelObjects.Clear();

                    if (!fixtureStarted)
                        fixtureStarted = true;
                    else
                    {
                        if (currentFixture != FixtureType.Path)
                        {
                            createCollisionObject();
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
                    if (currentFixture == FixtureType.Path && fixtureStarted && clickedPoints.Count > 1)
                    {
                        selectedLevelObjects.Clear();
                        createCollisionObject();
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

            #region CREATE_EVENTS
            if (editorState == EditorState.CREATE_EVENTS)
            {
                MainForm.Default.EditorStatus.Text = "Editorstatus: Brush Events";
                MainForm.Default.GameView.Cursor = Cursors.Cross;

                if (mstate.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && oldmstate.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                {
                    clickedPoints.Add(MouseWorldPosition);

                    if (!eventStarted)
                        eventStarted = true;
                    else
                    {
                        createEvent();
                        clickedPoints.Clear();
                        eventStarted = false;
                    }
                }

                if (mstate.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && oldmstate.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                {
                    if (eventStarted)
                    {
                        clickedPoints.Clear();
                        eventStarted = false;
                    }
                    else
                    {
                        clickedPoints.Clear();
                        eventStarted = false;
                        editorState = EditorState.IDLE;
                    }
                }
            }
            #endregion

            #region CREATE_TEXTURES/INTERACTIVE/ANIMATION
            if (editorState == EditorState.CREATE_TEXTURES || editorState == EditorState.CREATE_INTERACTIVE || editorState == EditorState.CREATE_ANIMATION)
            {
                MainForm.Default.EditorStatus.Text = "Editorstatus: Brush Texture";
                MainForm.Default.GameView.Cursor = null;

                if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.G))
                    MouseWorldPosition = SnapToGrid(MouseWorldPosition);

                if (mstate.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && oldmstate.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                {
                    createCurrentObject(true);
                }
                if (mstate.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && oldmstate.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                {
                    editorState = EditorState.IDLE;
                }

                /* Sascha:
                 * UltraBrush-Scaling!
                */
                if (currentObject != null)
                {
                    if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.E))
                    {
                        Vector2 tempScale = currentObject.getScale();
                        tempScale.X *= 1.05f;
                        tempScale.Y *= 1.05f;
                        currentObject.setScale(tempScale);
                    }
                    if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Q))
                    {
                        Vector2 tempScale = currentObject.getScale();
                        tempScale.X *= 0.95f;
                        tempScale.Y *= 0.95f;
                        currentObject.setScale(tempScale);
                    }

                    /* Sascha:
                     * UltraBrush-Rotation!
                    */

                    if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.X))
                    {
                        float tempRotation = currentObject.getRotation();
                        tempRotation += 0.05f;
                        currentObject.setRotation(tempRotation);
                    }
                    if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Y))
                    {
                        float tempRotation = currentObject.getRotation();
                        tempRotation += -0.05f;
                        currentObject.setRotation(tempRotation);
                    }
                }
            }
            #endregion

            #region CREATE_PARTICLE
            if (editorState == EditorState.CREATE_PARTICLE)
            {
                MainForm.Default.EditorStatus.Text = "Editorstatus: Create Particle";
                MainForm.Default.GameView.Cursor = null;

                if (mstate.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && oldmstate.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                {
                    createCurrentObject(false);
                }
                if (mstate.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && oldmstate.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                {
                    editorState = EditorState.IDLE;
                }
            }
            #endregion

            #region CREATE_JOINT
            if (editorState == EditorState.CREATE_JOINT)
            {
                MainForm.Default.EditorStatus.Text = "Editorstatus: Create Joint";
                MainForm.Default.GameView.Cursor = null;

                if (mstate.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && oldmstate.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                {
                    createJointObject();
                }
                if (mstate.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && oldmstate.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                {
                    editorState = EditorState.IDLE;
                }
            }
            #endregion

            #region POSITIONING
            if (editorState == EditorState.POSITIONING)
            {
                MainForm.Default.EditorStatus.Text = "Editorstatus: Positioning";
                MainForm.Default.GameView.Cursor = Cursors.SizeAll;

                int i = 0;
                foreach (LevelObject lo in selectedLevelObjects)
                {
                    Vector2 newPosition = initialPosition[i] + MouseWorldPosition - GrabbedPoint;

                    if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.G))
                    {
                        lo.position = SnapToGrid(newPosition);
                    }
                    else
                        lo.position = newPosition;
                    if (lo.GetType() == typeof(InteractiveObject))
                    {

                        //     System.Console.WriteLine(@"Position: " + lo.position);
                        //     System.Console.WriteLine(@"NewPosition: " + newPosition);
                        //    System.Console.WriteLine(@"fixture Position: " + ((InteractiveObject)lo).fixture.Body.Position);


                        ((InteractiveObject)lo).bodyType = BodyType.Kinematic;
                        ((InteractiveObject)lo).fixture.Body.Position = FixtureManager.ToMeter(newPosition);
                        ((InteractiveObject)lo).bodyType = BodyType.Dynamic;


                    }

                    i++;
                }
                MainForm.Default.propertyGrid1.Refresh();
                if (mstate.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && oldmstate.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                {
                    editorState = EditorState.IDLE;
                }
            }
            #endregion

            #region SELECTING
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
                    if (selectedLevelObjects.Count > 0)
                        MainForm.Default.propertyGrid1.SelectedObject = selectedLevelObjects[0];
                    editorState = EditorState.IDLE;
                }
            }
            #endregion

            #region SCALING
            if (editorState == EditorState.SCALING)
            {
                MainForm.Default.EditorStatus.Text = "Editorstatus: Scaling";
                MainForm.Default.GameView.Cursor = null;

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
            #endregion

            #region ROTATING
            if (editorState == EditorState.ROTATING)
            {
                MainForm.Default.EditorStatus.Text = "Editorstatus: Rotating";
                MainForm.Default.GameView.Cursor = null;

                Vector2 newpos = MouseWorldPosition - selectedLevelObjects[0].position;
                float deltatheta = (float)Math.Atan2(GrabbedPoint.Y, GrabbedPoint.X) - (float)Math.Atan2(newpos.Y, newpos.X);
                int i = 0;
                foreach (LevelObject selLO in selectedLevelObjects)
                {
                    if (selLO.canRotate())
                    {
                        selLO.setRotation(initialRotation[i] - deltatheta);
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

            #region MOVING
            if (editorState == EditorState.MOVING)
            {
                MainForm.Default.EditorStatus.Text = "Editorstatus: Moving";
                MainForm.Default.GameView.Cursor = Cursors.NoMove2D;

                Vector2 newPosition = GrabbedPoint - MouseWorldPosition;

                Camera.Position += newPosition;

                if (mstate.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                {
                    editorState = EditorState.IDLE;
                }
            }
            #endregion

            #endregion

            oldkstate = kstate;
            oldmstate = mstate;
        }

        public void Draw(int treeviewOffset)
        {
            if (level == null)
                return;

            level.DrawInEditor(EditorLoop.EditorLoopInstance.GraphicsDevice, treeviewOffset);
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

            level.InitializeInEditor(EditorLoop.EditorLoopInstance.GraphicsDevice, spriteBatch, MainForm.Default.GameView.Width, MainForm.Default.GameView.Height);
            level.LoadContentInEditor(EditorLoop.EditorLoopInstance.graphics, EditorLoop.EditorLoopInstance.GraphicsDevice, EditorLoop.EditorLoopInstance.Content);
            MainForm.Default.UpdateTreeView();
        }

        public void LoadLevel(string filename)
        {
            try
            {
                MainForm.Default.Cursor = Cursors.WaitCursor;
                Editor.Default.levelFileName = filename;
                Editor.Default.level = Level.LoadLevelFile(filename);

                if (level != null)
                {
                    level.InitializeInEditor(EditorLoop.EditorLoopInstance.GraphicsDevice, spriteBatch, MainForm.Default.GameView.Width, MainForm.Default.GameView.Height);
                    level.LoadContentInEditor(EditorLoop.EditorLoopInstance.graphics, EditorLoop.EditorLoopInstance.GraphicsDevice, EditorLoop.EditorLoopInstance.Content);
                    editorState = EditorState.IDLE;
                    MainForm.Default.UpdateTreeView();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("The level could not be loaded!\n" + e.Message, "Error", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            finally
            {
                MainForm.Default.Cursor = Cursors.Default;
            }
        }

        public void SaveLevel(string fullPath)
        {
            try
            {
                level.SaveLevel(fullPath);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

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
            //l.loadLayerInEditor();
            l.loadContentInEditor(EditorLoop.EditorLoopInstance.graphics, EditorLoop.EditorLoopInstance.GraphicsDevice, EditorLoop.EditorLoopInstance.Content);
            level.layerList.Add(l);
            selectLayer(level.layerList.Last());
            MainForm.Default.UpdateTreeView();
        }

        public void AddLevelObject(LevelObject lo)
        {
            selectedLayer.loList.Add(lo);
            selectLevelObject(lo);
        }

        public void AddEvents(EventType eventType)
        {
            if (level.layerList.Count() == 0)
            {
                System.Windows.Forms.MessageBox.Show("There is no Layer to add Events to it.", "Error", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            if (selectedLayer == null)
            {
                System.Windows.Forms.MessageBox.Show("You have to select a Layer to add Events to it.", "Error", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }

            currentEvent = eventType;
            eventStarted = false;
            editorState = EditorState.CREATE_EVENTS;
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

        public void AddSoundObject(string path)
        {
            SoundObject so = new SoundObject(path);
            Layer l = level.getLayerByName("SoundLayer");

            if (l == null)
            {
                AddLayer("SoundLayer");
                l = level.getLayerByName("SoundLayer");
            }

            so.name = so.getPrefix() + l.getNextObjectNumber();
            so.layer = l;
            l.loList.Add(so);

            MainForm.Default.UpdateTreeView();
        }

        //---> Create Objects <---//

        public void createTextureObject(string path)
        {
            editorState = EditorState.CREATE_TEXTURES;
            TextureObject to = new TextureObject(path);
            to.texture = TextureManager.Instance.LoadFromFile(path, EditorLoop.EditorLoopInstance.GraphicsDevice);
            currentObject = to;
        }
        public void createAnimatedObject(string path)
        {
            editorState = EditorState.CREATE_ANIMATION;
            AnimatedObject ao = new AnimatedObject(Vector2.Zero, 1, path, 25);
            ao.fullPath = path;
            //ao.LoadContent2();
            ao.texture = TextureManager.Instance.LoadFromFile(path, EditorLoop.EditorLoopInstance.GraphicsDevice);
            //ao.animation.activeTexture = ao.texture;

            currentObject = ao;
        }

        public void createInteractiveObject(string path)
        {
            editorState = EditorState.CREATE_INTERACTIVE;
            InteractiveObject io = new InteractiveObject(path);
            io.texture = TextureManager.Instance.LoadFromFile(path, EditorLoop.EditorLoopInstance.GraphicsDevice);
            currentObject = io;
        }

        public void createParticleObject()
        {
            editorState = EditorState.CREATE_PARTICLE;
            ParticleObject p = new ParticleObject();
            p.particleType = ParticleType.None;
            currentObject = p;
        }

        public void createRevoluteJointObject()
        {
            editorState = EditorState.CREATE_JOINT;
            RevoluteJointObject rjo = new RevoluteJointObject();
            currentObject = rjo;
            currentObject.Initialise();
        }

        public void createDistanceJointObject()
        {
            editorState = EditorState.CREATE_JOINT;
            DistanceJointObject djo = new DistanceJointObject();
            currentObject = djo;
            currentObject.Initialise();
        }

        public void createPrismaticJointObject()
        {
            editorState = EditorState.CREATE_JOINT;
            PrismaticJointObject pjo = new PrismaticJointObject();
            currentObject = pjo;
            currentObject.Initialise();
        }

        public void createGearJointObject()
        {
            editorState = EditorState.CREATE_JOINT;
            GearJointObject gjo = new GearJointObject();
            currentObject = gjo;
            currentObject.Initialise();
        }

        public void createPulleyJointObject()
        {
            editorState = EditorState.CREATE_JOINT;
            PulleyJointObject pjo = new PulleyJointObject();
            currentObject = pjo;
            currentObject.Initialise();
        }
        public void createRopeJointObject()
        {
            editorState = EditorState.CREATE_JOINT;
            RopeJointObject rjo = new RopeJointObject();
            currentObject = rjo;
            currentObject.Initialise();
        }

        //---> Destroy Objects <---//

        public void destroyCurrentObject()
        {
            editorState = EditorState.IDLE;
            this.currentObject = null;
        }

        //---> Create Objects <---//

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
         * Je nachdem welches Objekt momentan durch den Editor gesetzt wird, erzeugt diese Funktion ein entsprechendes LevelObject mit den gewünschten
         * Attributen.
        */

        public void createCurrentObject(bool continueAfterPaint)
        {
            if (currentObject is TextureObject)
            {
                TextureObject temp = (TextureObject)currentObject;
                TextureObject to = new TextureObject(temp.fullPath);
                to.texture = temp.texture;
                to.position = MouseWorldPosition;
                to.rotation = temp.rotation;
                to.scale = temp.scale;
                to.name = to.getPrefix() + selectedLayer.getNextObjectNumber();
                to.layer = selectedLayer;
                to.loadContentInEditor(EditorLoop.EditorLoopInstance.GraphicsDevice);
                AddLevelObject(to);
                selectedLevelObjects.Clear();
                selectedLevelObjects.Add(to);
            }
            if (currentObject is AnimatedObject)
            {
                AnimatedObject temp = (AnimatedObject)currentObject;
                AnimatedObject ao = new AnimatedObject(MouseWorldPosition, 1, temp.fullPath, temp.speed);
                ao.texture = temp.texture;
                ao.position = MouseWorldPosition;
                ao.rotation = temp.rotation;
                ao.scale = temp.scale;
                ao.speed = temp.speed;
                ao.name = ao.getPrefix() + selectedLayer.getNextObjectNumber();
                ao.StartInMiliseonds = temp.StartInMiliseonds;
                ao.layer = selectedLayer;
                ao.loadContentInEditor(EditorLoop.EditorLoopInstance.GraphicsDevice);
                AddLevelObject(ao);
                selectedLevelObjects.Clear();
                selectedLevelObjects.Add(ao);
            }
            if (currentObject is InteractiveObject)
            {
                InteractiveObject temp = (InteractiveObject)currentObject;
                InteractiveObject io = new InteractiveObject(temp.fullPath);
                io.texture = temp.texture;
                io.position = MouseWorldPosition;
                io.rotation = temp.rotation;
                io.scale = temp.scale;
                io.name = io.getPrefix() + selectedLayer.getNextObjectNumber();
                io.layer = selectedLayer;
                io.loadContentInEditor(EditorLoop.EditorLoopInstance.GraphicsDevice);
                AddLevelObject(io);
                selectedLevelObjects.Clear();
                selectedLevelObjects.Add(io);

                io.Initialise();
                io.LoadContent();

                System.Console.WriteLine(@"Position: " + io.position);
                System.Console.WriteLine(@"Fixture Position: " + io.fixture.Body.Position);
            }
            if (currentObject is ParticleObject)
            {
                ParticleObject temp = (ParticleObject)currentObject;
                ParticleObject po = new ParticleObject();
                po.particleType = temp.particleType;
                po.position = MouseWorldPosition;
                po.name = po.getPrefix() + selectedLayer.getNextObjectNumber();
                po.layer = selectedLayer;
                AddLevelObject(po);
                selectedLayer.particleRenderer.addParticleObjectsInEditor(po, EditorLoop.EditorLoopInstance.Content);
                po.Initialise();
                po.LoadContentInEditor(EditorLoop.EditorLoopInstance.Content);
                

                if (po.particleEffect != null)
                {
                    //po.particleEffect.Initialise();
                    //po.particleEffect.LoadContent(EditorLoop.EditorLoopInstance.Content);
                    selectedLayer.particleRenderer.initializeParticlesInEditor(EditorLoop.EditorLoopInstance.Content);
                    selectedLayer.particleRenderer.particleRenderer.LoadContent(EditorLoop.EditorLoopInstance.Content);
                }
                selectedLevelObjects.Clear();
                selectedLevelObjects.Add(po);
            }

            MainForm.Default.UpdateTreeView();

            if (!continueAfterPaint)
                destroyCurrentObject();
        }

        public void createJointObject()
        {
            if (currentObject is RevoluteJointObject)
            {
                if (Editor.Default.selectedLayer == null)
                {
                    DialogResult result = MessageBox.Show("There is no layer to add joints to it! Do you want to create one?", "Error", MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Error);

                    if (result == DialogResult.Yes)
                        new AddLayer().ShowDialog();
                    else
                        return;
                }

                RevoluteJointObject temp = (RevoluteJointObject)currentObject;
                RevoluteJointObject rjo = new RevoluteJointObject();
                rjo.position = MouseWorldPosition;
                rjo.name = rjo.getPrefix() + selectedLayer.getNextObjectNumber();
                rjo.layer = selectedLayer;
                rjo.Initialise();
                AddLevelObject(rjo);
                selectedLevelObjects.Clear();
                selectedLevelObjects.Add(rjo);
            }
            else if (currentObject is DistanceJointObject)
            {
                if (Editor.Default.selectedLayer == null)
                {
                    DialogResult result = MessageBox.Show("There is no layer to add joints to it! Do you want to create one?", "Error", MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Error);

                    if (result == DialogResult.Yes)
                        new AddLayer().ShowDialog();
                    else
                        return;
                }

                DistanceJointObject temp = (DistanceJointObject)currentObject;
                DistanceJointObject djo = new DistanceJointObject();
                djo.position = MouseWorldPosition;
                djo.name = djo.getPrefix() + selectedLayer.getNextObjectNumber();
                djo.layer = selectedLayer;
                djo.Initialise();
                AddLevelObject(djo);
                selectedLevelObjects.Clear();
                selectedLevelObjects.Add(djo);
            }
            else if (currentObject is PulleyJointObject)
            {
                if (Editor.Default.selectedLayer == null)
                {
                    DialogResult result = MessageBox.Show("There is no layer to add joints to it! Do you want to create one?", "Error", MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Error);

                    if (result == DialogResult.Yes)
                        new AddLayer().ShowDialog();
                    else
                        return;
                }

                PulleyJointObject temp = (PulleyJointObject)currentObject;
                PulleyJointObject pjo = new PulleyJointObject();
                pjo.position = MouseWorldPosition;
                pjo.name = pjo.getPrefix() + selectedLayer.getNextObjectNumber();
                pjo.layer = selectedLayer;
                pjo.Initialise();
                AddLevelObject(pjo);
                selectedLevelObjects.Clear();
                selectedLevelObjects.Add(pjo);
            }
            else if (currentObject is PrismaticJointObject)
            {
                if (Editor.Default.selectedLayer == null)
                {
                    DialogResult result = MessageBox.Show("There is no layer to add joints to it! Do you want to create one?", "Error", MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Error);

                    if (result == DialogResult.Yes)
                        new AddLayer().ShowDialog();
                    else
                        return;
                }

                PrismaticJointObject temp = (PrismaticJointObject)currentObject;
                PrismaticJointObject pjo = new PrismaticJointObject();
                pjo.position = MouseWorldPosition;
                pjo.name = pjo.getPrefix() + selectedLayer.getNextObjectNumber();
                pjo.layer = selectedLayer;
                pjo.Initialise();
                AddLevelObject(pjo);
                selectedLevelObjects.Clear();
                selectedLevelObjects.Add(pjo);
            }
            else if (currentObject is GearJointObject)
            {
                if (Editor.Default.selectedLayer == null)
                {
                    DialogResult result = MessageBox.Show("There is no layer to add joints to it! Do you want to create one?", "Error", MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Error);

                    if (result == DialogResult.Yes)
                        new AddLayer().ShowDialog();
                    else
                        return;
                }

                GearJointObject temp = (GearJointObject)currentObject;
                GearJointObject gjo = new GearJointObject();
                gjo.position = MouseWorldPosition;
                gjo.name = gjo.getPrefix() + selectedLayer.getNextObjectNumber();
                gjo.layer = selectedLayer;
                gjo.Initialise();
                AddLevelObject(gjo);
                selectedLevelObjects.Clear();
                selectedLevelObjects.Add(gjo);
            }
            else if (currentObject is RopeJointObject)
            {
                if (Editor.Default.selectedLayer == null)
                {
                    DialogResult result = MessageBox.Show("There is no layer to add joints to it! Do you want to create one?", "Error", MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Error);

                    if (result == DialogResult.Yes)
                        new AddLayer().ShowDialog();
                    else
                        return;
                }

                RopeJointObject temp = (RopeJointObject)currentObject;
                RopeJointObject rjo = new RopeJointObject();
                rjo.position = MouseWorldPosition;
                rjo.name = rjo.getPrefix() + selectedLayer.getNextObjectNumber();
                rjo.layer = selectedLayer;
                rjo.Initialise();
                AddLevelObject(rjo);
                selectedLevelObjects.Clear();
                selectedLevelObjects.Add(rjo);
            }

        }

        /* Sascha:
         * Fügt die Events in die LevelObject-Liste der aktuell selektierten Layer ein.
        */

        public void createEvent()
        {
            switch (currentEvent)
            {
                case EventType.ChangeBodyType:
                    PhysicChangeBodyTypeEvent e = new PhysicChangeBodyTypeEvent(Extensions.RectangleFromVectors(clickedPoints[0], clickedPoints[1]));
                    e.name = e.getPrefix() + selectedLayer.getNextObjectNumber();
                    e.layer = selectedLayer;
                    selectedLayer.loList.Add(e);
                    selectLevelObject(e);
                    break;
                case EventType.Fade:
                    AudioFadeEvent e1 = new AudioFadeEvent(Extensions.RectangleFromVectors(clickedPoints[0], clickedPoints[1]));
                    e1.name = e1.getPrefix() + selectedLayer.getNextObjectNumber();
                    e1.layer = selectedLayer;
                    selectedLayer.loList.Add(e1);
                    selectLevelObject(e1);
                    break;
                case EventType.Equalizer:
                    AudioEqualizerEvent e2 = new AudioEqualizerEvent(Extensions.RectangleFromVectors(clickedPoints[0], clickedPoints[1]));
                    e2.name = e2.getPrefix() + selectedLayer.getNextObjectNumber();
                    e2.layer = selectedLayer;
                    selectedLayer.loList.Add(e2);
                    selectLevelObject(e2);
                    break;
                case EventType.ModifyPlayback:
                    AudioModifyPlayback e3 = new AudioModifyPlayback(Extensions.RectangleFromVectors(clickedPoints[0], clickedPoints[1]));
                    e3.name = e3.getPrefix() + selectedLayer.getNextObjectNumber();
                    e3.layer = selectedLayer;
                    selectedLayer.loList.Add(e3);
                    selectLevelObject(e3);
                    break;
                case EventType.Mute:
                    AudioMuteEvent e4 = new AudioMuteEvent(Extensions.RectangleFromVectors(clickedPoints[0], clickedPoints[1]));
                    e4.name = e4.getPrefix() + selectedLayer.getNextObjectNumber();
                    e4.layer = selectedLayer;
                    selectedLayer.loList.Add(e4);
                    selectLevelObject(e4);
                    break;
                case EventType.SetVolume:
                    AudioSetVolumeEvent e5 = new AudioSetVolumeEvent(Extensions.RectangleFromVectors(clickedPoints[0], clickedPoints[1]));
                    e5.name = e5.getPrefix() + selectedLayer.getNextObjectNumber();
                    e5.layer = selectedLayer;
                    selectedLayer.loList.Add(e5);
                    selectLevelObject(e5);
                    break;
                case EventType.Play:
                    VideoPlayEvent e6 = new VideoPlayEvent(Extensions.RectangleFromVectors(clickedPoints[0], clickedPoints[1]));
                    e6.name = e6.getPrefix() + selectedLayer.getNextObjectNumber();
                    e6.layer = selectedLayer;
                    selectedLayer.loList.Add(e6);
                    selectLevelObject(e6);
                    break;
                case EventType.Reverb:
                    AudioReverbEvent e7 = new AudioReverbEvent(Extensions.RectangleFromVectors(clickedPoints[0], clickedPoints[1]));
                    e7.name = e7.getPrefix() + selectedLayer.getNextObjectNumber();
                    e7.layer = selectedLayer;
                    selectedLayer.loList.Add(e7);
                    selectLevelObject(e7);
                    break;
                case EventType.Crossfader:
                    AudioCrossfaderEvent e8 = new AudioCrossfaderEvent(Extensions.RectangleFromVectors(clickedPoints[0], clickedPoints[1]));
                    e8.name = e8.getPrefix() + selectedLayer.getNextObjectNumber();
                    e8.layer = selectedLayer;
                    selectedLayer.loList.Add(e8);
                    selectLevelObject(e8);
                    break;
                case EventType.Death:
                    DeathEvent e9 = new DeathEvent(Extensions.RectangleFromVectors(clickedPoints[0], clickedPoints[1]));
                    e9.name = e9.getPrefix() + selectedLayer.getNextObjectNumber();
                    e9.layer = selectedLayer;
                    selectedLayer.loList.Add(e9);
                    selectLevelObject(e9);
                    break;
                case EventType.Move:
                    MoveEvent e10 = new MoveEvent(Extensions.RectangleFromVectors(clickedPoints[0], clickedPoints[1]));
                    e10.name = e10.getPrefix() + selectedLayer.getNextObjectNumber();
                    e10.layer = selectedLayer;
                    selectedLayer.loList.Add(e10);
                    selectLevelObject(e10);
                    break;
                case EventType.Camera:
                    CameraEvent e11 = new CameraEvent(Extensions.RectangleFromVectors(clickedPoints[0], clickedPoints[1]));
                    e11.name = e11.getPrefix() + selectedLayer.getNextObjectNumber();
                    e11.layer = selectedLayer;
                    selectedLayer.loList.Add(e11);
                    selectLevelObject(e11);
                    break;
                case EventType.ApplyForce:
                    PhysicApplyForceEvent e12 = new PhysicApplyForceEvent(Extensions.RectangleFromVectors(clickedPoints[0], clickedPoints[1]));
                    e12.name = e12.getPrefix() + selectedLayer.getNextObjectNumber();
                    e12.layer = selectedLayer;
                    selectedLayer.loList.Add(e12);
                    selectLevelObject(e12);
                    break;
                case EventType.ChangeVisibility:
                    ChangeVisibilityEvent e13 = new ChangeVisibilityEvent(Extensions.RectangleFromVectors(clickedPoints[0], clickedPoints[1]));
                    e13.name = e13.getPrefix() + selectedLayer.getNextObjectNumber();
                    e13.layer = selectedLayer;
                    selectedLayer.loList.Add(e13);
                    selectLevelObject(e13);
                    break;
                case EventType.SaveState:
                    SaveStateEvent e14 = new SaveStateEvent(Extensions.RectangleFromVectors(clickedPoints[0], clickedPoints[1]));
                    e14.name = e14.getPrefix() + selectedLayer.getNextObjectNumber();
                    e14.layer = selectedLayer;
                    selectedLayer.loList.Add(e14);
                    selectLevelObject(e14);
                    break;
                case EventType.LoadLevel:
                    LoadLevelEvent e15 = new LoadLevelEvent(Extensions.RectangleFromVectors(clickedPoints[0], clickedPoints[1]));
                    e15.name = e15.getPrefix() + selectedLayer.getNextObjectNumber();
                    e15.layer = selectedLayer;
                    selectedLayer.loList.Add(e15);
                    selectLevelObject(e15);
                    break;
                case EventType.ChangeEffect:
                    ChangeEffectEvent e16 = new ChangeEffectEvent(Extensions.RectangleFromVectors(clickedPoints[0], clickedPoints[1]));
                    e16.name = e16.getPrefix() + selectedLayer.getNextObjectNumber();
                    e16.layer = selectedLayer;
                    selectedLayer.loList.Add(e16);
                    selectLevelObject(e16);
                    break;
            }
            MainForm.Default.UpdateTreeView();
        }

        /* Sascha:
         * Hier gilt das gleiche wie oben. Hier werden Wrapperobjekte für Fixtures (Kollisionsdomänen) erzeugt, die im Editor noch transformiert werden können. Erst in der 
         * Spielengine werden sie dann in Fixtures umgewandelt mit der Funktion ToFixture().
        */

        public void createCollisionObject()
        {
            switch (currentFixture)
            {
                case FixtureType.Rectangle:
                    LevelObject l1 = new RectangleCollisionObject(Extensions.RectangleFromVectors(clickedPoints[0], clickedPoints[1]));
                    l1.name = l1.getPrefix() + selectedLayer.getNextObjectNumber();
                    l1.layer = selectedLayer;
                    selectedLayer.loList.Add(l1);
                    selectLevelObject(l1);

                    l1.Initialise();
                    l1.LoadContent();
                    break;
                case FixtureType.Circle:
                    LevelObject l2 = new CircleCollisionObject(clickedPoints[0], (MouseWorldPosition - clickedPoints[0]).Length());
                    l2.name = l2.getPrefix() + selectedLayer.getNextObjectNumber();
                    l2.layer = selectedLayer;
                    selectedLayer.loList.Add(l2);
                    selectLevelObject(l2);
                    break;
                case FixtureType.Path:
                    LevelObject l3 = new PathCollisionObject(clickedPoints.ToArray());
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

        public void createPrimitiveObject()
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
            selectedLayer = l;

            if (l == null)
            {
                MainForm.Default.Selection.Text = "Selected Layer: -";
                return;
            }

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

            if (selectedLayer == l)
                selectLayer(null);

            if (MessageBox.Show("Do you really want to delete the layer " + l.name + "?", "Question", MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == DialogResult.Yes)
                level.layerList.Remove(l);

            MainForm.Default.UpdateTreeView();
        }

        public void deleteLevelObjects()
        {
            if (selectedLevelObjects.Count == 1)
            {
                selectedLevelObjects[0].layer.loList.Remove(selectedLevelObjects[0]);
                selectLevelObject(null);
                MainForm.Default.UpdateTreeView();
                return;
            }

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

        public void moveLayerUp(Layer l)
        {
            int index = level.layerList.IndexOf(l);

            if (index == 0)
                return;

            level.layerList[index] = level.layerList[index - 1];
            level.layerList[index - 1] = l;
            MainForm.Default.UpdateTreeView();
            selectLayer(l);
        }

        public void moveLayerDown(Layer l)
        {
            int index = level.layerList.IndexOf(l);

            if (index >= (level.layerList.Count - 1))
                return;

            level.layerList[index] = level.layerList[index + 1];
            level.layerList[index + 1] = l;
            MainForm.Default.UpdateTreeView();
            selectLayer(l);
        }

        public void moveObjectToLayer(LevelObject i1, Layer l2, LevelObject i2)
        {
            int index2 = i2 == null ? 0 : l2.loList.IndexOf(i2);
            i1.layer.loList.Remove(i1);
            l2.loList.Insert(index2, i1);
            i1.layer = l2;
        }

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
         * Funktion zur Anordnung über SnapToGrid.
        */

        public Vector2 SnapToGrid(Vector2 position)
        {
            Vector2 result = position;
            result.X = GameSettings.Default.resolutionWidth * (int)Math.Round(result.X / GameSettings.Default.resolutionWidth);
            result.Y = GameSettings.Default.resolutionHeight * (int)Math.Round(result.Y / GameSettings.Default.resolutionHeight);
            result = result - new Vector2(GameSettings.Default.resolutionWidth / 2, GameSettings.Default.resolutionHeight / 2);
            return result;
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
                        if (lo is RectangleCollisionObject)
                        {
                            RectangleCollisionObject r = (RectangleCollisionObject)lo;
                            if (r.isVisible)
                            {
                                Microsoft.Xna.Framework.Color color = Constants.ColorFixtures;
                                if (r.mouseOn) color = Constants.ColorMouseOn;
                                Primitives.Instance.drawBoxFilled(spriteBatch, r.rectangle, color);
                            }
                        }
                        if (lo is CircleCollisionObject)
                        {
                            CircleCollisionObject c = (CircleCollisionObject)lo;
                            if (c.isVisible)
                            {
                                Microsoft.Xna.Framework.Color color = Constants.ColorFixtures;
                                if (c.mouseOn) color = Constants.ColorMouseOn;
                                Primitives.Instance.drawCircleFilled(spriteBatch, c.position, c.radius, color);
                            }
                        }
                        if (lo is PathCollisionObject)
                        {
                            PathCollisionObject p = (PathCollisionObject)lo;
                            if (p.isVisible)
                            {
                                Microsoft.Xna.Framework.Color color = Constants.ColorFixtures;
                                if (p.mouseOn) color = Constants.ColorMouseOn;
                                Primitives.Instance.drawPolygon(spriteBatch, p.WorldPoints, color, p.lineWidth);
                            }
                        }
                        if (lo is Event)
                        {
                            Event e = (Event)lo;
                            if (e.isVisible)
                            {
                                Microsoft.Xna.Framework.Color color = Constants.ColorEvents;
                                if (e.mouseOn) color = Constants.ColorMouseOn;
                                Primitives.Instance.drawBoxFilled(spriteBatch, e.rectangle, color);
                            }
                        }
                        if (lo is ParticleObject)
                        {
                            ParticleObject p = (ParticleObject)lo;
                            if (p.isVisible)
                            {
                                Microsoft.Xna.Framework.Color color = Constants.ColorParticles;
                                if (lo.mouseOn) color = Constants.ColorMouseOn;
                                Primitives.Instance.drawCircleFilled(spriteBatch, p.position, p.radius, color);
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
                    if (l == selectedLayer && editorState == EditorState.CREATE_EVENTS && eventStarted)
                    {
                        Microsoft.Xna.Framework.Rectangle r = Extensions.RectangleFromVectors(clickedPoints[0], MouseWorldPosition);
                        Primitives.Instance.drawBoxFilled(spriteBatch, r, Constants.ColorEvents);
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
                        spriteBatch.Draw(to.texture, new Vector2(MouseWorldPosition.X, MouseWorldPosition.Y), null, new Microsoft.Xna.Framework.Color(1f, 1f, 1f, 7f), to.rotation, new Vector2(to.texture.Width / 2, to.texture.Height / 2), to.scale, SpriteEffects.None, 0);
                    }
                    if (l == selectedLayer && editorState == EditorState.CREATE_ANIMATION)
                    {
                        AnimatedObject ao = (AnimatedObject)currentObject;
                        spriteBatch.Draw(ao.texture, new Vector2(MouseWorldPosition.X, MouseWorldPosition.Y), null, new Microsoft.Xna.Framework.Color(1f, 1f, 1f, 7f), ao.rotation, new Vector2(ao.texture.Width / 2, ao.texture.Height / 2), ao.scale, SpriteEffects.None, 0);
                    }
                    if (l == selectedLayer && editorState == EditorState.CREATE_INTERACTIVE)
                    {
                        InteractiveObject io = (InteractiveObject)currentObject;
                        spriteBatch.Draw(io.texture, new Vector2(MouseWorldPosition.X, MouseWorldPosition.Y), null, new Microsoft.Xna.Framework.Color(1f, 1f, 1f, 7f), io.rotation, new Vector2(io.texture.Width / 2, io.texture.Height / 2), io.scale, SpriteEffects.None, 0);
                    }
                    if (l == selectedLayer && editorState == EditorState.CREATE_PARTICLE)
                    {
                        ParticleObject p = (ParticleObject)currentObject;
                        Primitives.Instance.drawCircleFilled(spriteBatch, MouseWorldPosition, p.radius, Constants.ColorParticles);
                    }
                    if (l == selectedLayer && editorState == EditorState.SELECTING)
                    {
                        Primitives.Instance.drawBoxFilled(spriteBatch, selectionRectangle, Constants.ColorSelectionBox);
                    }

                    if (level.layerList.Contains(level.getLayerByName("Player")))
                    {
                        if (l == level.getLayerByName("Player"))
                        {
                            Primitives.Instance.drawCircleFilled(spriteBatch, level.startPosition, 5, new Microsoft.Xna.Framework.Color(255, 0, 0));
                        }
                    }

                    if (originsVisible)
                    {
                        Primitives.Instance.drawLine(spriteBatch, new Vector2(0, 0), new Vector2(50, 0), new Microsoft.Xna.Framework.Color(100, 0, 100), 3);
                        Primitives.Instance.drawLine(spriteBatch, new Vector2(0, 0), new Vector2(0, 50), new Microsoft.Xna.Framework.Color(100, 0, 100), 3);
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