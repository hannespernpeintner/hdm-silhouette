using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Silhouette.Engine;
using Silhouette.Engine.Manager;
using Silhouette.GameMechs;
using Silhouette.GameMechs.Events;

namespace SilhouetteEditor.Forms
{
    public partial class ManageEvents : Form
    {
        public ManageEvents()
        {
            InitializeComponent();
            UpdateEventView();
            UpdateObjectView();
        }

        //---> EventView Steuerung <---//

        public void UpdateEventView()
        {
            EventView.Nodes.Clear();

            foreach (Layer l in Editor.Default.level.layerList)
            {
                foreach (LevelObject lo in l.loList)
                {
                    if (lo is Event)
                    {
                        Event e = (Event)lo;
                        TreeNode eventTreeNode = EventView.Nodes.Add(e.name);
                        eventTreeNode.Tag = e;

                        foreach (LevelObject lo2 in e.list)
                        {
                            TreeNode levelObjectTreeNode = eventTreeNode.Nodes.Add(lo2.name);
                            levelObjectTreeNode.Tag = lo2;
                        }
                    }
                    EventView.ExpandAll();
                }
            }
        }

        //---> ObjectView Steuerung<---//

        public void UpdateObjectView()
        {
            ObjectView.Nodes.Clear();

            foreach (Layer l in Editor.Default.level.layerList)
            {
                TreeNode layerTreeNode = ObjectView.Nodes.Add(l.name);
                layerTreeNode.Tag = l;

                foreach (LevelObject lo in l.loList)
                {
                    if (lo is InteractiveObject || lo is FixtureItem)
                    {
                        TreeNode levelObjectTreeNode = layerTreeNode.Nodes.Add(lo.name);
                        levelObjectTreeNode.Tag = lo;
                    }
                }
            }

            ObjectView.ExpandAll();
        }
    }
}
