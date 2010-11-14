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
            switch (comboBox1.SelectedIndex)
            { 
                case 0:
                    Editor.Default.AddLayer(textBox1.Text, Convert.ToInt32(textBox2.Text), Convert.ToInt32(textBox3.Text));
                    break;
                case 1:
                    Editor.Default.AddCollisionLayer(textBox1.Text);
                    break;
                case 2:
                    Editor.Default.AddEventLayer(textBox1.Text);
                    break;
            }
            this.Hide();
        }
    }
}
