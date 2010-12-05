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
using System.IO;

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

        //---> Form-Steuerung <---//

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            EditorLoop.EditorLoopInstance.Exit();
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

        private void FileSaveAs(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Editor.Default.levelFileName = dialog.FileName;
                Editor.Default.SaveLevel(dialog.FileName);
            }
        }

        private void FileSave(object sender, EventArgs e)
        {
            if (Editor.Default.levelFileName != null)
            {
                Editor.Default.SaveLevel(Editor.Default.levelFileName);
            }
            else
            {
                FileSaveAs(sender, e);
            }
        }

        private void FileOpen(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Editor.Default.LoadLevel(dialog.FileName);
            }
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

        private void PathButton_Click(object sender, EventArgs e)
        {
            Editor.Default.AddFixture(FixtureType.Path);
        }

        private void ToolBoxButton_Click(object sender, EventArgs e)
        {
            new ToolBox().ShowDialog();
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
                    loTreeNode.ContextMenuStrip = ObjectContextMenu;
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

        //---> GameView-Steuerung <---//

        private void GameView_MouseEnter(object sender, EventArgs e)
        {
            GameView.Select();
        }

        private void GameView_MouseLeave(object sender, EventArgs e)
        {
            MenuBar.Select();
        }

        private void GameView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            ListViewItem lvi = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
            Editor.Default.createTextureObject(lvi.Name);
        }

        private void GameView_DragDrop(object sender, DragEventArgs e)
        {
            Editor.Default.paintCurrentObject(false);
        }

        private void GameView_Resize(object sender, EventArgs e)
        {
            if(EditorLoop.EditorLoopInstance != null) 
                EditorLoop.EditorLoopInstance.resizebackbuffer(GameView.Width, GameView.Height);
        }

        //---> TextureView-Steuerung <---//

        public void loadFolder(string path)
        {
            ImageList32.Images.Clear();
            TextureView.Clear();

            DirectoryInfo di = new DirectoryInfo(path);
            DirectoryInfo[] folders = di.GetDirectories();
            foreach (DirectoryInfo folder in folders)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = folder.Name;
                lvi.ToolTipText = folder.Name;
                lvi.ImageIndex = 0;
                lvi.Tag = "folder";
                lvi.Name = folder.FullName;
                TextureView.Items.Add(lvi);
            }

            string filters = "*.jpg;*.png;*.bmp;";
            List<FileInfo> fileList = new List<FileInfo>();
            string[] extensions = filters.Split(';');
            foreach (string filter in extensions) fileList.AddRange(di.GetFiles(filter));
            FileInfo[] files = fileList.ToArray();

            foreach (FileInfo file in files)
            {
                Bitmap bmp = new Bitmap(file.FullName);
                ImageList32.Images.Add(file.FullName, Editor.Default.getThumbNail(bmp, 32, 32));


                ListViewItem lvi = new ListViewItem();
                lvi.Name = file.FullName;
                lvi.Text = file.Name;
                lvi.ImageKey = file.FullName;
                lvi.Tag = "file";
                lvi.ToolTipText = file.Name + " (" + bmp.Width.ToString() + " x " + bmp.Height.ToString() + ")";

                TextureView.Items.Add(lvi);
            }
        }

        private void TextureView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            ListViewItem lvi = (ListViewItem)e.Item;
            if (lvi.Tag.ToString() == "folder") return;
            TextureView.DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void TextureView_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            Point p = GameView.PointToClient(new Point(e.X, e.Y));
            Editor.Default.SetMousePosition(p.X, p.Y);
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog d = new FolderBrowserDialog();
            if (d.ShowDialog() == DialogResult.OK) loadFolder(d.SelectedPath);
        }

        private void TextureView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string itemtype = TextureView.FocusedItem.Tag.ToString();
            if (itemtype == "folder")
            {
                loadFolder(TextureView.FocusedItem.Name);
            }
            if (itemtype == "file")
            {
                Editor.Default.createTextureObject(TextureView.FocusedItem.Name);
            }
        }

        //---> InteractiveView-Steuerung <---//

        public void loadFolderInteractive(string path)
        {
            ImageListInteractive32.Images.Clear();
            InteractiveView.Clear();

            DirectoryInfo di = new DirectoryInfo(path);
            DirectoryInfo[] folders = di.GetDirectories();
            foreach (DirectoryInfo folder in folders)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = folder.Name;
                lvi.ToolTipText = folder.Name;
                lvi.ImageIndex = 0;
                lvi.Tag = "folder";
                lvi.Name = folder.FullName;
                InteractiveView.Items.Add(lvi);
            }

            string filters = "*.jpg;*.png;*.bmp;";
            List<FileInfo> fileList = new List<FileInfo>();
            string[] extensions = filters.Split(';');
            foreach (string filter in extensions) fileList.AddRange(di.GetFiles(filter));
            FileInfo[] files = fileList.ToArray();

            foreach (FileInfo file in files)
            {
                Bitmap bmp = new Bitmap(file.FullName);
                ImageListInteractive32.Images.Add(file.FullName, Editor.Default.getThumbNail(bmp, 32, 32));


                ListViewItem lvi = new ListViewItem();
                lvi.Name = file.FullName;
                lvi.Text = file.Name;
                lvi.ImageKey = file.FullName;
                lvi.Tag = "file";
                lvi.ToolTipText = file.Name + " (" + bmp.Width.ToString() + " x " + bmp.Height.ToString() + ")";

                InteractiveView.Items.Add(lvi);
            }
        }

        private void InteractiveView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string itemtype = InteractiveView.FocusedItem.Tag.ToString();
            if (itemtype == "folder")
            {
                loadFolder(InteractiveView.FocusedItem.Name);
            }
            if (itemtype == "file")
            {
                Editor.Default.createInteractiveObject(InteractiveView.FocusedItem.Name);
            }
        }

        private void BrowseButton2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog d = new FolderBrowserDialog();
            if (d.ShowDialog() == DialogResult.OK) loadFolderInteractive(d.SelectedPath);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Editor.Default.deleteLayer(Editor.Default.selectedLayer);
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Editor.Default.deleteLevelObjects();
        }

        private void rectangleCollisionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Editor.Default.AddFixture(FixtureType.Rectangle);
        }

        private void circleCollisionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Editor.Default.AddFixture(FixtureType.Circle);
        }

        private void pathCollisionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Editor.Default.AddFixture(FixtureType.Path);
        }

        private void rectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Editor.Default.AddPrimitive(PrimitiveType.Rectangle);
        }

        private void circleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Editor.Default.AddPrimitive(PrimitiveType.Circle);
        }

        private void pathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Editor.Default.AddPrimitive(PrimitiveType.Path);
        }

        private void DeleteLayerButton_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;
            if (treeView1.SelectedNode.Tag is Layer)
            {
                Layer l = (Layer)treeView1.SelectedNode.Tag;
                Editor.Default.deleteLayer(l);
            }
        }

        private void renameToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            treeView1.SelectedNode.BeginEdit();
        }

        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Wird noch implementiert -> Copy -> LevelObject
        }

        private void renameToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            treeView1.SelectedNode.BeginEdit();
        }

        private void renameToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            treeView1.SelectedNode.BeginEdit();
        }
    }
}
