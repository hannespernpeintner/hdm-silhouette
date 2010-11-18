using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Silhouette.Engine;
using Silhouette.GameMechs;

//Physik-Engine Klassen
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;

namespace SilhouetteEditor.Forms
{
    public partial class MainForm : Form
    {
        public static MainForm Default;

        public MainForm()
        {
            Default = this;
            InitializeComponent();
        }
 
        public IntPtr getDrawSurface()
        {
            return GameView.Handle;
        }


        //---> MenuBar-Steuerung <---//

        private void FileExit(object sender, EventArgs e)
        {
            EditorLoop.EditorLoopInstance.Exit();
        }

        private void FileNew(object sender, EventArgs e)
        {
            new NewLevel().ShowDialog();
        }

        private void HelpAbout(object sender, EventArgs e)
        {
            new About().Show();
        }

        private void HelpHelp(object sender, EventArgs e)
        {
            new Help().Show();
        }

        private void ToolsToolBox(object sender, EventArgs e)
        {
            new ToolBox().Show();
        }

        //---> ToolBar-Steuerung <---//

        private void ToolStripButton_AddRectangleFixture(object sender, EventArgs e)
        {
            Editor.Default.AddFixture(FixtureType.Rectangle);
        }

        private void ToolStripButton_AddCircleFixture(object sender, EventArgs e)
        {
            Editor.Default.AddFixture(FixtureType.Circle);
        }


        //---> ContextMenu-Steuerungen <---//

        //--->Level

        private void LevelToolStrip_AddLayer(object sender, EventArgs e)
        {
            new AddLayer().ShowDialog();
        }

        private void LevelToolStrip_AddCollisionLayer(object sender, EventArgs e)
        {
            Editor.Default.AddCollisionLayer();
        }

        private void LevelToolStrip_AddEventLayer(object sender, EventArgs e)
        {
            Editor.Default.AddEventLayer();
        }


        //---> TreeView-Steuerung <---//

        public void UpdateTreeView()
        {
            treeView1.Nodes.Clear();

            TreeNode levelTreeNode = treeView1.Nodes.Add(Editor.Default.level.name);
            levelTreeNode.Tag = Editor.Default.level;
            levelTreeNode.ContextMenuStrip = LevelContextMenu;

            foreach (Layer l in Editor.Default.level.layerList)
            {
                TreeNode layerTreeNode = levelTreeNode.Nodes.Add(l.name);
                layerTreeNode.Tag = l;
                layerTreeNode.ContextMenuStrip = LayerContextMenu;

                foreach (LevelObject lo in l.loList)
                {
                    TreeNode loTreeNode = layerTreeNode.Nodes.Add(lo.name);
                    loTreeNode.Tag = lo;
                }
                foreach (DrawableLevelObject dlo in l.dloList)
                {
                    TreeNode dloTreeNode = layerTreeNode.Nodes.Add(dlo.name);
                    dloTreeNode.Tag = dlo;
                }
            }

            if(Editor.Default.level.collisionLayer != null)
            {
                TreeNode collisionTreeNode = levelTreeNode.Nodes.Add("Collision Layer");
                collisionTreeNode.Tag = Editor.Default.level.collisionLayer;

                int fixtureCounter = 1;

                foreach (Fixture f in Editor.Default.level.collisionLayer.fixtureList)
                {
                    TreeNode fixtureTreeNode = collisionTreeNode.Nodes.Add("Fixture " + fixtureCounter.ToString());
                    fixtureCounter++;
                }
            }

            if (Editor.Default.level.eventLayer != null)
            {
                TreeNode eventTreeNode = levelTreeNode.Nodes.Add("Event Layer");
                eventTreeNode.Tag = Editor.Default.level.eventLayer;
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is Level)
            {
                Editor.Default.selectLevel();
            }
            if (e.Node.Tag is Layer)
            {
                Layer l = (Layer)e.Node.Tag;
                Editor.Default.selectLayer(l);
            }
            if (e.Node.Tag is CollisionLayer)
            {
                Editor.Default.selectCollisionLayer();
            }
            if (e.Node.Tag is EventLayer)
            {
                Editor.Default.selectEventLayer();
            }
        }
    }
}
