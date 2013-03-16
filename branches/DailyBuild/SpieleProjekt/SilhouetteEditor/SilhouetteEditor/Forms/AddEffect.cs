using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Silhouette.Engine.Effects;

namespace SilhouetteEditor.Forms
{
    public partial class AddEffect : Form
    {
        public bool forLevel = false;
        public AddEffect()
        {
            InitializeComponent();
        }
        public AddEffect(bool forLevel)
        {
            InitializeComponent();
            this.forLevel = forLevel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string selectedEffect = (string) selectEffect.SelectedItem;
            Silhouette.Engine.Effects.EffectObject effectObject = null;

            switch (selectedEffect)
            {
                case "GodRays":
                {
                    effectObject = new Silhouette.Engine.Effects.GodRays();
                    effectObject.Initialise();
                    effectObject.loadContentInEditor(EditorLoop.EditorLoopInstance.GraphicsDevice, EditorLoop.EditorLoopInstance.Content);
                    
                    break;
                }
                case "Bleach":
                {
                    effectObject = new Silhouette.Engine.Effects.Bleach();
                    effectObject.Initialise();
                    effectObject.loadContentInEditor(EditorLoop.EditorLoopInstance.GraphicsDevice, EditorLoop.EditorLoopInstance.Content);
                    break;
                }
                case "Bloom":
                {
                    effectObject = new Silhouette.Engine.Effects.Bloom();
                    effectObject.Initialise();
                    effectObject.loadContentInEditor(EditorLoop.EditorLoopInstance.GraphicsDevice, EditorLoop.EditorLoopInstance.Content);
                    break;
                }
                case "Blur":
                {
                    effectObject = new Silhouette.Engine.Effects.Blur();
                    effectObject.Initialise();
                    effectObject.loadContentInEditor(EditorLoop.EditorLoopInstance.GraphicsDevice, EditorLoop.EditorLoopInstance.Content);
                    break;
                }
                case "ColorFade":
                {
                    effectObject = new Silhouette.Engine.Effects.ColorFade();
                    effectObject.Initialise();
                    effectObject.loadContentInEditor(EditorLoop.EditorLoopInstance.GraphicsDevice, EditorLoop.EditorLoopInstance.Content);
                    break;
                }
                case "VignettenBlur":
                {
                    effectObject = new Silhouette.Engine.Effects.VignettenBlur();
                    effectObject.Initialise();
                    effectObject.loadContentInEditor(EditorLoop.EditorLoopInstance.GraphicsDevice, EditorLoop.EditorLoopInstance.Content);
                    break;
                }
                case "Water":
                {
                    effectObject = new Silhouette.Engine.Effects.Water();
                    effectObject.Initialise();
                    effectObject.loadContentInEditor(EditorLoop.EditorLoopInstance.GraphicsDevice, EditorLoop.EditorLoopInstance.Content);
                    break;
                }
            }

            if (forLevel && effectObject != null)
            {
                Editor.Default.level.Effects.Add(effectObject);
            }
            else if (effectObject != null)
            {
                if (Editor.Default.selectedLayer.Effects == null)
                {
                    Editor.Default.selectedLayer.Effects = new List<EffectObject>();
                }
                Editor.Default.selectedLayer.Effects.Add(effectObject);
            }

            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void AddEffect_Load(object sender, EventArgs e)
        {
            selectEffect.Items.Add("GodRays");
            selectEffect.Items.Add("Bleach");
            selectEffect.Items.Add("Bloom");
            selectEffect.Items.Add("Blur");
            selectEffect.Items.Add("ColorFade");
            selectEffect.Items.Add("VignettenBlur");
            selectEffect.Items.Add("Water");
        }
    }
}
