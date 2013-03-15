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
    public partial class AddEffect : Form
    {
        public AddEffect()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string selectedEffect = (string) selectEffect.SelectedItem;

            switch (selectedEffect)
            {
                case "GodRays":
                {
                    Silhouette.Engine.Effects.EffectObject effectObject = new Silhouette.Engine.Effects.GodRays();
                    effectObject.Initialise();
                    effectObject.loadContentInEditor(EditorLoop.EditorLoopInstance.GraphicsDevice, EditorLoop.EditorLoopInstance.Content);
                    Editor.Default.selectedLayer.Effects.Add(effectObject);
                    break;
                }
                case "Bleach":
                {
                    Silhouette.Engine.Effects.EffectObject effectObject = new Silhouette.Engine.Effects.Bleach();
                    effectObject.Initialise();
                    effectObject.loadContentInEditor(EditorLoop.EditorLoopInstance.GraphicsDevice, EditorLoop.EditorLoopInstance.Content);
                    Editor.Default.selectedLayer.Effects.Add(effectObject);
                    break;
                }
                case "Bloom":
                {
                    Silhouette.Engine.Effects.EffectObject effectObject = new Silhouette.Engine.Effects.Bloom();
                    effectObject.Initialise();
                    effectObject.loadContentInEditor(EditorLoop.EditorLoopInstance.GraphicsDevice, EditorLoop.EditorLoopInstance.Content);
                    Editor.Default.selectedLayer.Effects.Add(effectObject);
                    break;
                }
                case "Blur":
                {
                    Silhouette.Engine.Effects.EffectObject effectObject = new Silhouette.Engine.Effects.Blur();
                    effectObject.Initialise();
                    effectObject.loadContentInEditor(EditorLoop.EditorLoopInstance.GraphicsDevice, EditorLoop.EditorLoopInstance.Content);
                    Editor.Default.selectedLayer.Effects.Add(effectObject);
                    break;
                }
                case "ColorFade":
                {
                    Silhouette.Engine.Effects.EffectObject effectObject = new Silhouette.Engine.Effects.ColorFade();
                    effectObject.Initialise();
                    effectObject.loadContentInEditor(EditorLoop.EditorLoopInstance.GraphicsDevice, EditorLoop.EditorLoopInstance.Content);
                    Editor.Default.selectedLayer.Effects.Add(effectObject);
                    break;
                }
                case "VignettenBlur":
                {
                    Silhouette.Engine.Effects.EffectObject effectObject = new Silhouette.Engine.Effects.VignettenBlur();
                    effectObject.Initialise();
                    effectObject.loadContentInEditor(EditorLoop.EditorLoopInstance.GraphicsDevice, EditorLoop.EditorLoopInstance.Content);
                    Editor.Default.selectedLayer.Effects.Add(effectObject);
                    break;
                }
                case "Water":
                {
                    Silhouette.Engine.Effects.EffectObject effectObject = new Silhouette.Engine.Effects.Water();
                    effectObject.Initialise();
                    effectObject.loadContentInEditor(EditorLoop.EditorLoopInstance.GraphicsDevice, EditorLoop.EditorLoopInstance.Content);
                    Editor.Default.selectedLayer.Effects.Add(effectObject);
                    break;
                }
            }
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
