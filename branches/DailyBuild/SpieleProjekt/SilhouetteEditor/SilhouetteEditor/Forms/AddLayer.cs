using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SilhouetteEditor.Forms
{
    public partial class AddLayer : Form
    {
        public AddLayer()
        {
            InitializeComponent();
        }

        private void ButtonCancel(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void ButtonNew(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("You have to enter a layer name!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox2.Text == "" || textBox3.Text == "")
            {
                Editor.Default.AddLayer(textBox1.Text, 1, 1);
            }
            else
                Editor.Default.AddLayer(textBox1.Text, Convert.ToInt32(textBox2.Text), Convert.ToInt32(textBox3.Text));
            this.Hide();
        }
    }
}
