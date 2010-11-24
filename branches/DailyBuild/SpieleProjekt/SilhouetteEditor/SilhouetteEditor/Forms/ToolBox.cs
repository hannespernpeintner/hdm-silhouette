using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SilhouetteEditor.Forms
{
    public partial class ToolBox : Form
    {
        public ToolBox()
        {
            InitializeComponent();
        }

        private void ButtonChoose(object sender, EventArgs e)
        {
            FolderBrowserDialog d = new FolderBrowserDialog();
            d.SelectedPath = textBox1.Text;
            if (d.ShowDialog() == DialogResult.OK) loadFolder(d.SelectedPath);
        }

        public void loadFolder(string path)
        {
            imageList32.Images.Clear();
            listView1.Clear();

            DirectoryInfo di = new DirectoryInfo(path);
            textBox1.Text = di.FullName;
            DirectoryInfo[] folders = di.GetDirectories();
            foreach (DirectoryInfo folder in folders)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = folder.Name;
                lvi.ToolTipText = folder.Name;
                lvi.ImageIndex = 0;
                lvi.Tag = "folder";
                lvi.Name = folder.FullName;
                listView1.Items.Add(lvi);
            }

            string filters = "*.jpg;*.png;*.bmp;";
            List<FileInfo> fileList = new List<FileInfo>();
            string[] extensions = filters.Split(';');
            foreach (string filter in extensions) fileList.AddRange(di.GetFiles(filter));
            FileInfo[] files = fileList.ToArray();

            foreach (FileInfo file in files)
            {
                Bitmap bmp = new Bitmap(file.FullName);
                imageList32.Images.Add(file.FullName, Editor.Default.getThumbNail(bmp, 32, 32));


                ListViewItem lvi = new ListViewItem();
                lvi.Name = file.FullName;
                lvi.Text = file.Name;
                lvi.ImageKey = file.FullName;
                lvi.Tag = "file";
                lvi.ToolTipText = file.Name + " (" + bmp.Width.ToString() + " x " + bmp.Height.ToString() + ")";

                listView1.Items.Add(lvi);
            }
        }

        private void AddTexture()
        { 
            string itemtype = listView1.FocusedItem.Tag.ToString();
            if (itemtype == "folder")
            {
                loadFolder(listView1.FocusedItem.Name);
            }
            if (itemtype == "file")
            {
                if (textBox2.Text == "" || textBox3.Text == "")
                {
                    Editor.Default.createTextureObject(listView1.FocusedItem.Name);
                }
                else
                {
                    Editor.Default.createTextureWrapper(listView1.FocusedItem.Name, Convert.ToInt32(textBox2.Text), Convert.ToInt32(textBox3.Text));
                }
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            AddTexture();
        }

        private void listView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            ListViewItem lvi = (ListViewItem)e.Item;
            if (lvi.Tag.ToString() == "folder") return;
            Bitmap bmp = new Bitmap(listView1.LargeImageList.Images[lvi.ImageKey]);
            listView1.DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void listView1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            Point p = MainForm.Default.GameView.PointToClient(new Point(e.X, e.Y));
            Editor.Default.SetMousePosition(p.X, p.Y);
            EditorLoop.EditorLoopInstance.GraphicsDevice.Present();
        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            listView1.Cursor = Cursors.Default;
            MainForm.Default.GameView.Cursor = Cursors.Default;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            AddTexture();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
