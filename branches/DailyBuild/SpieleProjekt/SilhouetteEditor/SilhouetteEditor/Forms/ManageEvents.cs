using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Silhouette.Engine;
using Silhouette.Engine.Manager;
using Silhouette.GameMechs;
using Silhouette.GameMechs.Events;

namespace SilhouetteEditor.Forms
{
    public partial class ManageEvents : Form
    {
        Event selectedEvent;
        LevelObject selectedLevelObject;
        LevelObject selectedLevelObject2;

        public ManageEvents()
        {
            InitializeComponent();
            UpdateEventView();
            UpdateObjectView();
        }

        //---> EventView Steuerung <---//

        public void UpdateEventView()
        {
            EventView.Nodes.Clear();

            foreach (Layer l in Editor.Default.level.layerList)
            {
                TreeNode layerTreeNode = EventView.Nodes.Add(l.name);
                layerTreeNode.Tag = l;

                foreach (LevelObject lo in l.loList)
                {
                    if (lo is Event)
                    {
                        Event e = (Event)lo;
                        TreeNode eventTreeNode = layerTreeNode.Nodes.Add(e.name);
                        eventTreeNode.Tag = e;

                        foreach (LevelObject lo2 in e.list)
                        {
                            TreeNode levelObjectTreeNode = eventTreeNode.Nodes.Add(lo2.name);
                            levelObjectTreeNode.Tag = lo2;
                        }
                    }
                    EventView.ExpandAll();
                }
            }
        }

        private void EventView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                EventView.SelectedNode = EventView.GetNodeAt(e.X, e.Y);

                if (EventView.SelectedNode == null)
                    return;

                if (EventView.SelectedNode.Tag is Layer)
                {
                    Layer l = (Layer)EventView.SelectedNode.Tag;
                    Editor.Default.selectLayer(l);
                    propertyGrid1.SelectedObject = l;
                }

                if (EventView.SelectedNode.Tag is Event)
                {
                    Event ev = (Event)EventView.SelectedNode.Tag;
                    Editor.Default.selectLevelObject(ev);
                    selectedEvent = ev;
                    propertyGrid1.SelectedObject = ev;
                    Camera.Position = ev.position;
                }

                if (EventView.SelectedNode.Tag is InteractiveObject)
                {
                    InteractiveObject lo = (InteractiveObject)EventView.SelectedNode.Tag;
                    Editor.Default.selectLevelObject(lo);
                    selectedLevelObject2 = lo;
                    Event ev = (Event)EventView.SelectedNode.Parent.Tag;
                    selectedEvent = ev;
                    propertyGrid1.SelectedObject = lo;
                    Camera.Position = lo.position;
                }

                if (EventView.SelectedNode.Tag is CollisionObject)
                {
                    CollisionObject lo = (CollisionObject)EventView.SelectedNode.Tag;
                    Editor.Default.selectLevelObject(lo);
                    selectedLevelObject2 = lo;
                    Event ev = (Event)EventView.SelectedNode.Parent.Tag;
                    selectedEvent = ev;
                    propertyGrid1.SelectedObject = lo;
                    Camera.Position = lo.position;
                }

                if (EventView.SelectedNode.Tag is SoundObject)
                {
                    SoundObject so = (SoundObject)EventView.SelectedNode.Tag;
                    Editor.Default.selectLevelObject(so);
                    selectedLevelObject2 = so;
                    Event ev = (Event)EventView.SelectedNode.Parent.Tag;
                    selectedEvent = ev;
                    propertyGrid1.SelectedObject = so;
                }

                if (EventView.SelectedNode.Tag is TextureObject)
                {
                    TextureObject to = (TextureObject)EventView.SelectedNode.Tag;
                    Editor.Default.selectLevelObject(to);
                    selectedLevelObject2 = to;
                    Event ev = (Event)EventView.SelectedNode.Parent.Tag;
                    selectedEvent = ev;
                    propertyGrid1.SelectedObject = to;
                }
            }
        }

        //---> ObjectView Steuerung<---//

        public void UpdateObjectView()
        {
            ObjectView.Nodes.Clear();

            foreach (Layer l in Editor.Default.level.layerList)
            {
                TreeNode layerTreeNode = ObjectView.Nodes.Add(l.name);
                layerTreeNode.Tag = l;

                foreach (LevelObject lo in l.loList)
                {
                    if (lo is InteractiveObject || lo is CollisionObject || lo is SoundObject || lo is TextureObject)
                    {
                        TreeNode levelObjectTreeNode = layerTreeNode.Nodes.Add(lo.name);
                        levelObjectTreeNode.Tag = lo;
                    }
                }
            }

            ObjectView.ExpandAll();
        }

        private void ObjectView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                ObjectView.SelectedNode = ObjectView.GetNodeAt(e.X, e.Y);

                if (ObjectView.SelectedNode == null)
                    return;

                if (ObjectView.SelectedNode.Tag is Layer)
                {
                    Layer l = (Layer)ObjectView.SelectedNode.Tag;
                    Editor.Default.selectLayer(l);
                    propertyGrid1.SelectedObject = l;
                }

                if (ObjectView.SelectedNode.Tag is LevelObject)
                {
                    LevelObject lo = (LevelObject)ObjectView.SelectedNode.Tag;
                    Editor.Default.selectLevelObject(lo);
                    selectedLevelObject = lo;
                    propertyGrid1.SelectedObject = lo;
                    Camera.Position = lo.position;
                }
            }
        }

        //---> Button Steuerung <---//

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (selectedEvent == null || selectedLevelObject2 == null)
                return;

            selectedEvent.list.Remove(selectedLevelObject2);
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (selectedEvent == null || selectedLevelObject == null)
                return;

            if (!selectedEvent.list.Contains(selectedLevelObject))
                selectedEvent.AddLevelObject(selectedLevelObject);
            else
                MessageBox.Show("Object is already in the list of this event!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            UpdateEventView();
            UpdateObjectView();
        }
    }
}
