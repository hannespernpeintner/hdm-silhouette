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
    public partial class NewLevel : Form
    {
        public NewLevel()
        {
            InitializeComponent();
        }

        private void ButtonCancel(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void ButtonNew(object sender, EventArgs e)
        {
            Editor.Default.NewLevel(textBox1.Text);
            this.Hide();
        }

        private void Textbox_KeyEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Editor.Default.NewLevel(textBox1.Text);
                this.Hide();
            }
        }
    }
}
