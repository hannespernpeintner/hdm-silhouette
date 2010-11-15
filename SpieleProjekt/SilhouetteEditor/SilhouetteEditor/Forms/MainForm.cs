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

        private void FileExit(object sender, EventArgs e)
        {
            EditorLoop.EditorLoopInstance.Exit();
        }

        private void FileNew(object sender, EventArgs e)
        {
            new NewLevel().Show();
        }

        public void UpdateTreeView()
        {
            treeView1.Nodes.Clear();

            TreeNode levelTreeNode = treeView1.Nodes.Add(Editor.Default.level.name);
            levelTreeNode.ContextMenuStrip = LevelContextMenu;

            foreach (Layer l in Editor.Default.level.layerList)
            {
                TreeNode layerTreeNode = levelTreeNode.Nodes.Add(l.name);
                layerTreeNode.ContextMenuStrip = LayerContextMenu;

                foreach (LevelObject lo in l.loList)
                {
                    TreeNode loTreeNode = layerTreeNode.Nodes.Add(lo.name);
                }
                foreach (DrawableLevelObject dlo in l.dloList)
                {
                    TreeNode dloTreeNode = layerTreeNode.Nodes.Add(dlo.name);
                }
            }

            if(Editor.Default.level.collisionLayer != null)
            {
                TreeNode collisionTreeNode = levelTreeNode.Nodes.Add("Collision Layer: " + Editor.Default.level.collisionLayer.name);
            }

            if (Editor.Default.level.eventLayer != null)
            {
                TreeNode eventTreeNode = levelTreeNode.Nodes.Add("Event Layer: " + Editor.Default.level.eventLayer.name);
            }
        }

        private void LevelToolStrip_AddLayer(object sender, EventArgs e)
        {
            new AddLayer().Show();
        }

        private void HelpAbout(object sender, EventArgs e)
        {
            new About().Show();
        }

        private void HelpHelp(object sender, EventArgs e)
        {
            new Help().Show();
        }

        private void LayerToolStrip_AutomaticLevelCreation(object sender, EventArgs e)
        {

        }

        private void ViewToolBox(object sender, EventArgs e)
        {
            new ToolBox().Show();
        }
    }
}
