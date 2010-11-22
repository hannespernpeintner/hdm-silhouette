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
        /* Sascha:
         * Hauptfenster der Anwendung. Enthält eine TreeView zur Anzeige und hierarchischen Ordnung von Ebenen und Objekten.
         * Ein PropertyGrid zeigt Eigenschaften an und stellt Funktionen zur Anpassung bereit.
         * In die MainForm integriert ist eine PictureView, in der die grafische Darstellung über XNA erfolgt.
        */
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
            }
            levelTreeNode.ExpandAll();
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
            if (e.Node.Tag is LevelObject)
            {
                LevelObject lo = (LevelObject)e.Node.Tag;
                Editor.Default.selectLevelObject(lo);
            }
        }

        private void GameView_MouseEnter(object sender, EventArgs e)
        {
            GameView.Select();
        }

        private void GameView_MouseLeave(object sender, EventArgs e)
        {
            MenuBar.Select();
        }
    }
}
