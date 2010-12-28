using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

using Silhouette.GameMechs;

namespace SilhouetteEditor.Forms
{
    public partial class LevelObjectSelector : UserControl
    {
        public LevelObject levelObject;

        public LevelObjectSelector(LevelObject levelObject)
        {
            InitializeComponent();
            this.levelObject = levelObject;
        }

        private void LevelObjectSelector_Load(object sender, EventArgs e)
        {
            treeView1.Nodes.Add((TreeNode)MainForm.Default.treeView1.Nodes[0].Clone());
            treeView1.Nodes[0].Expand();
            if (levelObject != null)
            {
                TreeNode[] nodes = treeView1.Nodes.Find(levelObject.name, true);
                if (nodes.Length > 0)
                {
                    treeView1.SelectedNode = nodes[0];
                }
                else
                {
                    levelObject = null;
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is LevelObject) levelObject = (LevelObject)e.Node.Tag;
        }

    }

    public class LevelObjectUITypeEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService wfes =
                provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

            if (wfes != null)
            {
                LevelObjectSelector uc1 = new LevelObjectSelector((LevelObject)value);
                wfes.DropDownControl(uc1);
                value = uc1.levelObject;
            }
            return value;
        }

        public override bool IsDropDownResizable
        {
            get
            {
                return true;
            }
        }

    }
}
