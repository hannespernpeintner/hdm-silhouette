using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Silhouette;
using Silhouette.Engine;
using Silhouette.Engine.Manager;
using Silhouette.GameMechs;
using SilhouetteEditor.Forms;

namespace SilhouetteEditor
{
    class Editor
    {
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

        public Level level;
        public Layer selectedLayer;

        public void Initialize()
        { 
            
        }

        public void Update()
        { 
            
        }

        public void Draw()
        { 
            
        }

        public void NewLevel(string name)
        {
            level = new Level();

            if (name.Length == 0)
                level.name = "Level";
            else
                level.name = name;

            MainForm.Default.UpdateTreeView();
        }

        public void LoadLevel()
        {
 
        }

        public void SaveLevel()
        { 
        
        }

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

        public void AddCollisionLayer(string name)
        {
            CollisionLayer cl = new CollisionLayer();
            cl.name = name;
            level.collisionLayer = cl;
            MainForm.Default.UpdateTreeView();
        }

        public void AddEventLayer(string name)
        {
            EventLayer el = new EventLayer();
            el.name = name;
            level.eventLayer = el;
            MainForm.Default.UpdateTreeView();
        }

        public void AutomaticLevelCreation(Layer l)
        { 
            
        }
    }
}
