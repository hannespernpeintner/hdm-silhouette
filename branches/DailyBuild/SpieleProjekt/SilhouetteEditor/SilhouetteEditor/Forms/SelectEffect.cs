using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Silhouette.Engine.Effects;
using Silhouette.Engine;
using Silhouette.GameMechs;
using Silhouette.GameMechs.Events;

namespace SilhouetteEditor.Forms
{
    public partial class SelectEffect : Form
    {
        public SelectEffect()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string selectedEffect = (string) selectEffect.SelectedItem;

            foreach (EffectObject eo in Editor.Default.level.Effects)
            {
                if (eo.name.Equals(selectedEffect))
                {
                    LevelObject levelObject = Editor.Default.selectedLevelObjects[0];
                    if (levelObject is ChangeEffectEvent)
                    {
                        ((ChangeEffectEvent)levelObject).EffectList.Add((EffectObject)eo);
                    }
                }
            }

            foreach (Layer layer in Editor.Default.level.layerList)
            {
                foreach(EffectObject eo in layer.Effects)
                {
                    if (eo.name.Equals(selectedEffect) && eo is EffectObject)
                    {
                        LevelObject levelObject = Editor.Default.selectedLevelObjects[0];
                        if (levelObject is ChangeEffectEvent)
                        {
                            ((ChangeEffectEvent)levelObject).EffectList.Add((EffectObject)eo);
                        }
                    }
                }

            }


            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void SelectEffect_Load(object sender, EventArgs e)
        {
            foreach (EffectObject eo in Editor.Default.level.Effects)
            {
                selectEffect.Items.Add(eo.name);
            }
            foreach (Layer layer in Editor.Default.level.layerList)
            {
                foreach (EffectObject eo in layer.Effects)
                {
                    selectEffect.Items.Add(eo.name);
                }
            }

        }
    }
}
