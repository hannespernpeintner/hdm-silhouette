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

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
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
                    MessageBox.Show("Width and Height must be declared!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                Editor.Default.createTextureWrapper(listView1.FocusedItem.Name, Convert.ToInt32(textBox2.Text), Convert.ToInt32(textBox3.Text));
            }
        }
    }
}
