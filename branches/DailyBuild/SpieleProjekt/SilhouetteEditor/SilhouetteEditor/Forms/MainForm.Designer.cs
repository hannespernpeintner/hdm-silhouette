﻿using System.Drawing;
using System.IO;
using SilhouetteEditor.Properties;

namespace SilhouetteEditor.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.StatusBar = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.Selection = new System.Windows.Forms.ToolStripStatusLabel();
            this.MouseWorldPosition = new System.Windows.Forms.ToolStripStatusLabel();
            this.SelectedItem = new System.Windows.Forms.ToolStripStatusLabel();
            this.EditorStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.FPS = new System.Windows.Forms.ToolStripStatusLabel();
            this.ZoomStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.MenuBar = new System.Windows.Forms.MenuStrip();
            this.dateiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ansichtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eventManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripButton();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.ToolBar = new System.Windows.Forms.ToolStrip();
            this.NewLayerButton = new System.Windows.Forms.ToolStripButton();
            this.DeleteLayerButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.PrimitiveButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.rectangleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.circleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FixtureButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.rectangleCollisionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.circleCollisionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pathCollisionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PhysicsButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.saveStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.particleObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.soundObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EventButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.objectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveRotateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeVisibilityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.physicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeBodyTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applyForceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeGlobalGravityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.audioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fadeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.crossfaderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.equalizerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modifyPlaybackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.muteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setVolumeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reverbToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.videoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadLevelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeEffectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cameraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.JointButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.RevoluteJointButton = new System.Windows.Forms.ToolStripMenuItem();
            this.DistanceJointButton = new System.Windows.Forms.ToolStripMenuItem();
            this.PrismaticJointButton = new System.Windows.Forms.ToolStripMenuItem();
            this.GearJointButton = new System.Windows.Forms.ToolStripMenuItem();
            this.PulleyJointButton = new System.Windows.Forms.ToolStripMenuItem();
            this.RopeJointButton = new System.Windows.Forms.ToolStripMenuItem();
            this.GameScaleButton = new System.Windows.Forms.ToolStripButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.TextureView = new System.Windows.Forms.ListView();
            this.ImageList32 = new System.Windows.Forms.ImageList(this.components);
            this.BrowseButton = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.BrowseButton2 = new System.Windows.Forms.Button();
            this.InteractiveView = new System.Windows.Forms.ListView();
            this.ImageListInteractive32 = new System.Windows.Forms.ImageList(this.components);
            this.tabPageAnimations = new System.Windows.Forms.TabPage();
            this.TextureViewAnimations = new System.Windows.Forms.ListView();
            this.ImageListAnimations32 = new System.Windows.Forms.ImageList(this.components);
            this.BrowseButtonAnimations = new System.Windows.Forms.Button();
            this.GameView = new System.Windows.Forms.PictureBox();
            this.LevelContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addLayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.renameToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.addEffectToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.LayerContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.renameToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.addEffectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ObjectContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.renameToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.EventContextMenü = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.renameToolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.addObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addEffectToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnTogglePhysics = new System.Windows.Forms.Button();
            this.btnResetPhysics = new System.Windows.Forms.Button();
            this.StatusBar.SuspendLayout();
            this.MenuBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.ToolBar.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPageAnimations.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GameView)).BeginInit();
            this.LevelContextMenu.SuspendLayout();
            this.LayerContextMenu.SuspendLayout();
            this.ObjectContextMenu.SuspendLayout();
            this.EventContextMenü.SuspendLayout();
            this.SuspendLayout();
            // 
            // StatusBar
            // 
            this.StatusBar.BackColor = System.Drawing.SystemColors.Window;
            this.StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.Selection,
            this.MouseWorldPosition,
            this.SelectedItem,
            this.EditorStatus,
            this.FPS,
            this.ZoomStatus});
            this.StatusBar.Location = new System.Drawing.Point(0, 708);
            this.StatusBar.Name = "StatusBar";
            this.StatusBar.Size = new System.Drawing.Size(1256, 22);
            this.StatusBar.TabIndex = 0;
            this.StatusBar.Text = "StatusBar";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoSize = false;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(253, 17);
            // 
            // Selection
            // 
            this.Selection.AutoSize = false;
            this.Selection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Selection.Name = "Selection";
            this.Selection.Size = new System.Drawing.Size(200, 17);
            this.Selection.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.Selection.ToolTipText = "Displays the current selection.";
            // 
            // MouseWorldPosition
            // 
            this.MouseWorldPosition.AutoSize = false;
            this.MouseWorldPosition.Name = "MouseWorldPosition";
            this.MouseWorldPosition.Size = new System.Drawing.Size(200, 17);
            this.MouseWorldPosition.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.MouseWorldPosition.ToolTipText = "The current mouse position.";
            // 
            // SelectedItem
            // 
            this.SelectedItem.AutoSize = false;
            this.SelectedItem.Name = "SelectedItem";
            this.SelectedItem.Size = new System.Drawing.Size(250, 17);
            this.SelectedItem.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.SelectedItem.ToolTipText = "Displays the name of the selected object.";
            // 
            // EditorStatus
            // 
            this.EditorStatus.AutoSize = false;
            this.EditorStatus.Name = "EditorStatus";
            this.EditorStatus.Size = new System.Drawing.Size(180, 17);
            this.EditorStatus.ToolTipText = "The status of the editor.";
            // 
            // FPS
            // 
            this.FPS.AutoSize = false;
            this.FPS.Name = "FPS";
            this.FPS.Size = new System.Drawing.Size(80, 17);
            // 
            // ZoomStatus
            // 
            this.ZoomStatus.AutoSize = false;
            this.ZoomStatus.Name = "ZoomStatus";
            this.ZoomStatus.Size = new System.Drawing.Size(80, 17);
            this.ZoomStatus.ToolTipText = "The current zoom factor of the viewport.";
            // 
            // MenuBar
            // 
            this.MenuBar.BackColor = System.Drawing.SystemColors.Window;
            this.MenuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dateiToolStripMenuItem,
            this.ansichtToolStripMenuItem,
            this.runToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.MenuBar.Location = new System.Drawing.Point(0, 0);
            this.MenuBar.Name = "MenuBar";
            this.MenuBar.Size = new System.Drawing.Size(1256, 26);
            this.MenuBar.TabIndex = 1;
            this.MenuBar.Text = "MenuBar";
            // 
            // dateiToolStripMenuItem
            // 
            this.dateiToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.dateiToolStripMenuItem.Name = "dateiToolStripMenuItem";
            this.dateiToolStripMenuItem.Size = new System.Drawing.Size(37, 22);
            this.dateiToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.FileNew);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.FileOpen);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.FileSave);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.saveAsToolStripMenuItem.Text = "Save as...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.FileSaveAs);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(118, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.FileExit);
            // 
            // ansichtToolStripMenuItem
            // 
            this.ansichtToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.eventManagerToolStripMenuItem});
            this.ansichtToolStripMenuItem.Name = "ansichtToolStripMenuItem";
            this.ansichtToolStripMenuItem.Size = new System.Drawing.Size(48, 22);
            this.ansichtToolStripMenuItem.Text = "Tools";
            // 
            // eventManagerToolStripMenuItem
            // 
            this.eventManagerToolStripMenuItem.Name = "eventManagerToolStripMenuItem";
            this.eventManagerToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.eventManagerToolStripMenuItem.Text = "Event Manager";
            this.eventManagerToolStripMenuItem.Click += new System.EventHandler(this.ToolsEventManager);
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            this.runToolStripMenuItem.Text = "Run";
            this.runToolStripMenuItem.Click += new System.EventHandler(this.RunGame);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.helpToolStripMenuItem1});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 22);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.HelpAbout);
            // 
            // helpToolStripMenuItem1
            // 
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            this.helpToolStripMenuItem1.Size = new System.Drawing.Size(107, 22);
            this.helpToolStripMenuItem1.Text = "Help";
            this.helpToolStripMenuItem1.Click += new System.EventHandler(this.HelpHelp);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.Window;
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 26);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.GameView);
            this.splitContainer1.Size = new System.Drawing.Size(1256, 682);
            this.splitContainer1.SplitterDistance = 249;
            this.splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BackColor = System.Drawing.SystemColors.Window;
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.treeView1);
            this.splitContainer2.Panel1.Controls.Add(this.ToolBar);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer2.Size = new System.Drawing.Size(249, 682);
            this.splitContainer2.SplitterDistance = 282;
            this.splitContainer2.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.AllowDrop = true;
            this.treeView1.CheckBoxes = true;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.LabelEdit = true;
            this.treeView1.Location = new System.Drawing.Point(0, 25);
            this.treeView1.Name = "treeView1";
            this.treeView1.ShowLines = false;
            this.treeView1.Size = new System.Drawing.Size(245, 253);
            this.treeView1.TabIndex = 1;
            this.treeView1.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeView1_AfterLabelEdit);
            this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            this.treeView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView1_ItemDrag);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.DragOver += new System.Windows.Forms.DragEventHandler(this.treeView1_DragOver);
            this.treeView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeView1_KeyDown);
            this.treeView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseDown);
            // 
            // ToolBar
            // 
            this.ToolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewLayerButton,
            this.DeleteLayerButton,
            this.toolStripSeparator2,
            this.PrimitiveButton,
            this.FixtureButton,
            this.PhysicsButton,
            this.EventButton,
            this.JointButton,
            this.GameScaleButton});
            this.ToolBar.Location = new System.Drawing.Point(0, 0);
            this.ToolBar.Name = "ToolBar";
            this.ToolBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ToolBar.Size = new System.Drawing.Size(245, 25);
            this.ToolBar.TabIndex = 0;
            this.ToolBar.Text = "ToolBar";
            // 
            // NewLayerButton
            // 
            this.NewLayerButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.NewLayerButton.Image = global::SilhouetteEditor.Properties.Resource.NewLayer;
            this.NewLayerButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.NewLayerButton.Name = "NewLayerButton";
            this.NewLayerButton.Size = new System.Drawing.Size(23, 22);
            this.NewLayerButton.ToolTipText = "New Layer";
            this.NewLayerButton.Click += new System.EventHandler(this.LevelToolStrip_AddLayer);
            // 
            // DeleteLayerButton
            // 
            this.DeleteLayerButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DeleteLayerButton.Image = global::SilhouetteEditor.Properties.Resource.DeleteLayer;
            this.DeleteLayerButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DeleteLayerButton.Name = "DeleteLayerButton";
            this.DeleteLayerButton.Size = new System.Drawing.Size(23, 22);
            this.DeleteLayerButton.Text = "Delete Layer";
            this.DeleteLayerButton.ToolTipText = "Delete Layer";
            this.DeleteLayerButton.Click += new System.EventHandler(this.DeleteLayerButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // PrimitiveButton
            // 
            this.PrimitiveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.PrimitiveButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rectangleToolStripMenuItem,
            this.circleToolStripMenuItem,
            this.pathToolStripMenuItem});
            this.PrimitiveButton.Image = global::SilhouetteEditor.Properties.Resource.AddPrimitive;
            this.PrimitiveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PrimitiveButton.Name = "PrimitiveButton";
            this.PrimitiveButton.Size = new System.Drawing.Size(29, 22);
            this.PrimitiveButton.Text = "toolStripDropDownButton1";
            this.PrimitiveButton.ToolTipText = "Paint Primitives";
            // 
            // rectangleToolStripMenuItem
            // 
            this.rectangleToolStripMenuItem.Name = "rectangleToolStripMenuItem";
            this.rectangleToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.rectangleToolStripMenuItem.Text = "Rectangle";
            this.rectangleToolStripMenuItem.Click += new System.EventHandler(this.rectangleToolStripMenuItem_Click);
            // 
            // circleToolStripMenuItem
            // 
            this.circleToolStripMenuItem.Name = "circleToolStripMenuItem";
            this.circleToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.circleToolStripMenuItem.Text = "Circle";
            this.circleToolStripMenuItem.Click += new System.EventHandler(this.circleToolStripMenuItem_Click);
            // 
            // pathToolStripMenuItem
            // 
            this.pathToolStripMenuItem.Name = "pathToolStripMenuItem";
            this.pathToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.pathToolStripMenuItem.Text = "Path";
            this.pathToolStripMenuItem.Click += new System.EventHandler(this.pathToolStripMenuItem_Click);
            // 
            // FixtureButton
            // 
            this.FixtureButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.FixtureButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rectangleCollisionToolStripMenuItem,
            this.circleCollisionToolStripMenuItem,
            this.pathCollisionToolStripMenuItem});
            this.FixtureButton.Image = global::SilhouetteEditor.Properties.Resource.AddFixture;
            this.FixtureButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.FixtureButton.Name = "FixtureButton";
            this.FixtureButton.Size = new System.Drawing.Size(29, 22);
            this.FixtureButton.Text = "toolStripDropDownButton2";
            this.FixtureButton.ToolTipText = "Add Collision Domain";
            // 
            // rectangleCollisionToolStripMenuItem
            // 
            this.rectangleCollisionToolStripMenuItem.Name = "rectangleCollisionToolStripMenuItem";
            this.rectangleCollisionToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.rectangleCollisionToolStripMenuItem.Text = "Rectangle Collision";
            this.rectangleCollisionToolStripMenuItem.Click += new System.EventHandler(this.rectangleCollisionToolStripMenuItem_Click);
            // 
            // circleCollisionToolStripMenuItem
            // 
            this.circleCollisionToolStripMenuItem.Name = "circleCollisionToolStripMenuItem";
            this.circleCollisionToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.circleCollisionToolStripMenuItem.Text = "Circle Collision";
            this.circleCollisionToolStripMenuItem.Click += new System.EventHandler(this.circleCollisionToolStripMenuItem_Click);
            // 
            // pathCollisionToolStripMenuItem
            // 
            this.pathCollisionToolStripMenuItem.Name = "pathCollisionToolStripMenuItem";
            this.pathCollisionToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.pathCollisionToolStripMenuItem.Text = "Path Collision";
            this.pathCollisionToolStripMenuItem.Click += new System.EventHandler(this.pathCollisionToolStripMenuItem_Click);
            // 
            // PhysicsButton
            // 
            this.PhysicsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.PhysicsButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveStateToolStripMenuItem,
            this.particleObjectToolStripMenuItem,
            this.soundObjectToolStripMenuItem});
            this.PhysicsButton.Image = global::SilhouetteEditor.Properties.Resource.Physics;
            this.PhysicsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PhysicsButton.Name = "PhysicsButton";
            this.PhysicsButton.Size = new System.Drawing.Size(29, 22);
            this.PhysicsButton.Text = "toolStripDropDownButton3";
            this.PhysicsButton.ToolTipText = "Add Objects";
            // 
            // saveStateToolStripMenuItem
            // 
            this.saveStateToolStripMenuItem.Name = "saveStateToolStripMenuItem";
            this.saveStateToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.saveStateToolStripMenuItem.Text = "SaveState";
            this.saveStateToolStripMenuItem.Click += new System.EventHandler(this.saveStateToolStripMenuItem_Click);
            // 
            // particleObjectToolStripMenuItem
            // 
            this.particleObjectToolStripMenuItem.Name = "particleObjectToolStripMenuItem";
            this.particleObjectToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.particleObjectToolStripMenuItem.Text = "ParticleObject";
            this.particleObjectToolStripMenuItem.Click += new System.EventHandler(this.particleObjectToolStripMenuItem_Click);
            // 
            // soundObjectToolStripMenuItem
            // 
            this.soundObjectToolStripMenuItem.Name = "soundObjectToolStripMenuItem";
            this.soundObjectToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.soundObjectToolStripMenuItem.Text = "SoundObject";
            this.soundObjectToolStripMenuItem.Click += new System.EventHandler(this.soundObjectToolStripMenuItem_Click);
            // 
            // EventButton
            // 
            this.EventButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.EventButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.objectToolStripMenuItem,
            this.physicToolStripMenuItem,
            this.audioToolStripMenuItem,
            this.videoToolStripMenuItem,
            this.deathToolStripMenuItem,
            this.loadLevelToolStripMenuItem,
            this.changeEffectToolStripMenuItem,
            this.cameraToolStripMenuItem});
            this.EventButton.Image = global::SilhouetteEditor.Properties.Resource.AddEvent;
            this.EventButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.EventButton.Name = "EventButton";
            this.EventButton.Size = new System.Drawing.Size(29, 22);
            this.EventButton.Text = "toolStripDropDownButton1";
            this.EventButton.ToolTipText = "Add Event";
            // 
            // objectToolStripMenuItem
            // 
            this.objectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moveRotateToolStripMenuItem,
            this.changeVisibilityToolStripMenuItem});
            this.objectToolStripMenuItem.Name = "objectToolStripMenuItem";
            this.objectToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.objectToolStripMenuItem.Text = "Object";
            // 
            // moveRotateToolStripMenuItem
            // 
            this.moveRotateToolStripMenuItem.Name = "moveRotateToolStripMenuItem";
            this.moveRotateToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.moveRotateToolStripMenuItem.Text = "Move/Rotate/Scale";
            this.moveRotateToolStripMenuItem.Click += new System.EventHandler(this.moveRotateToolStripMenuItem_Click);
            // 
            // changeVisibilityToolStripMenuItem
            // 
            this.changeVisibilityToolStripMenuItem.Name = "changeVisibilityToolStripMenuItem";
            this.changeVisibilityToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.changeVisibilityToolStripMenuItem.Text = "Change Visibility";
            this.changeVisibilityToolStripMenuItem.Click += new System.EventHandler(this.changeVisibilityToolStripMenuItem_Click);
            // 
            // physicToolStripMenuItem
            // 
            this.physicToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeBodyTypeToolStripMenuItem,
            this.applyForceToolStripMenuItem,
            this.changeGlobalGravityToolStripMenuItem});
            this.physicToolStripMenuItem.Name = "physicToolStripMenuItem";
            this.physicToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.physicToolStripMenuItem.Text = "Physic";
            // 
            // changeBodyTypeToolStripMenuItem
            // 
            this.changeBodyTypeToolStripMenuItem.Name = "changeBodyTypeToolStripMenuItem";
            this.changeBodyTypeToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.changeBodyTypeToolStripMenuItem.Text = "Change BodyType";
            this.changeBodyTypeToolStripMenuItem.Click += new System.EventHandler(this.changeBodyTypeToolStripMenuItem_Click);
            // 
            // applyForceToolStripMenuItem
            // 
            this.applyForceToolStripMenuItem.Name = "applyForceToolStripMenuItem";
            this.applyForceToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.applyForceToolStripMenuItem.Text = "Apply Force";
            this.applyForceToolStripMenuItem.Click += new System.EventHandler(this.applyForceToolStripMenuItem_Click);
            // 
            // changeGlobalGravityToolStripMenuItem
            // 
            this.changeGlobalGravityToolStripMenuItem.Name = "changeGlobalGravityToolStripMenuItem";
            this.changeGlobalGravityToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.changeGlobalGravityToolStripMenuItem.Text = "Change global gravity";
            this.changeGlobalGravityToolStripMenuItem.Click += new System.EventHandler(this.changeGlobalGravityToolStripMenuItem_Click);
            // 
            // audioToolStripMenuItem
            // 
            this.audioToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fadeToolStripMenuItem,
            this.crossfaderToolStripMenuItem,
            this.equalizerToolStripMenuItem,
            this.modifyPlaybackToolStripMenuItem,
            this.muteToolStripMenuItem,
            this.setVolumeToolStripMenuItem,
            this.reverbToolStripMenuItem});
            this.audioToolStripMenuItem.Name = "audioToolStripMenuItem";
            this.audioToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.audioToolStripMenuItem.Text = "Audio";
            // 
            // fadeToolStripMenuItem
            // 
            this.fadeToolStripMenuItem.Name = "fadeToolStripMenuItem";
            this.fadeToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.fadeToolStripMenuItem.Text = "Fade";
            this.fadeToolStripMenuItem.Click += new System.EventHandler(this.fadeToolStripMenuItem_Click);
            // 
            // crossfaderToolStripMenuItem
            // 
            this.crossfaderToolStripMenuItem.Name = "crossfaderToolStripMenuItem";
            this.crossfaderToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.crossfaderToolStripMenuItem.Text = "Crossfader";
            this.crossfaderToolStripMenuItem.Click += new System.EventHandler(this.crossfaderToolStripMenuItem_Click);
            // 
            // equalizerToolStripMenuItem
            // 
            this.equalizerToolStripMenuItem.Name = "equalizerToolStripMenuItem";
            this.equalizerToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.equalizerToolStripMenuItem.Text = "Equalizer";
            this.equalizerToolStripMenuItem.Click += new System.EventHandler(this.equalizerToolStripMenuItem_Click);
            // 
            // modifyPlaybackToolStripMenuItem
            // 
            this.modifyPlaybackToolStripMenuItem.Name = "modifyPlaybackToolStripMenuItem";
            this.modifyPlaybackToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.modifyPlaybackToolStripMenuItem.Text = "Modify Playback";
            this.modifyPlaybackToolStripMenuItem.Click += new System.EventHandler(this.modifyPlaybackToolStripMenuItem_Click);
            // 
            // muteToolStripMenuItem
            // 
            this.muteToolStripMenuItem.Name = "muteToolStripMenuItem";
            this.muteToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.muteToolStripMenuItem.Text = "Mute";
            this.muteToolStripMenuItem.Click += new System.EventHandler(this.muteToolStripMenuItem_Click);
            // 
            // setVolumeToolStripMenuItem
            // 
            this.setVolumeToolStripMenuItem.Name = "setVolumeToolStripMenuItem";
            this.setVolumeToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.setVolumeToolStripMenuItem.Text = "Set Volume";
            this.setVolumeToolStripMenuItem.Click += new System.EventHandler(this.setVolumeToolStripMenuItem_Click);
            // 
            // reverbToolStripMenuItem
            // 
            this.reverbToolStripMenuItem.Name = "reverbToolStripMenuItem";
            this.reverbToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.reverbToolStripMenuItem.Text = "Reverb";
            this.reverbToolStripMenuItem.Click += new System.EventHandler(this.reverbToolStripMenuItem_Click);
            // 
            // videoToolStripMenuItem
            // 
            this.videoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playToolStripMenuItem});
            this.videoToolStripMenuItem.Name = "videoToolStripMenuItem";
            this.videoToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.videoToolStripMenuItem.Text = "Video";
            // 
            // playToolStripMenuItem
            // 
            this.playToolStripMenuItem.Name = "playToolStripMenuItem";
            this.playToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.playToolStripMenuItem.Text = "Play";
            this.playToolStripMenuItem.Click += new System.EventHandler(this.playToolStripMenuItem_Click);
            // 
            // deathToolStripMenuItem
            // 
            this.deathToolStripMenuItem.Name = "deathToolStripMenuItem";
            this.deathToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.deathToolStripMenuItem.Text = "Death";
            this.deathToolStripMenuItem.Click += new System.EventHandler(this.deathToolStripMenuItem_Click);
            // 
            // loadLevelToolStripMenuItem
            // 
            this.loadLevelToolStripMenuItem.Name = "loadLevelToolStripMenuItem";
            this.loadLevelToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.loadLevelToolStripMenuItem.Text = "LoadLevel";
            this.loadLevelToolStripMenuItem.Click += new System.EventHandler(this.loadLevelToolStripMenuItem_Click);
            // 
            // changeEffectToolStripMenuItem
            // 
            this.changeEffectToolStripMenuItem.Name = "changeEffectToolStripMenuItem";
            this.changeEffectToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.changeEffectToolStripMenuItem.Text = "Change Effect";
            this.changeEffectToolStripMenuItem.Click += new System.EventHandler(this.changeEffectToolStripMenuItem_Click);
            // 
            // cameraToolStripMenuItem
            // 
            this.cameraToolStripMenuItem.Name = "cameraToolStripMenuItem";
            this.cameraToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.cameraToolStripMenuItem.Text = "Camera";
            this.cameraToolStripMenuItem.Click += new System.EventHandler(this.cameraToolStripMenuItem_Click);
            // 
            // JointButton
            // 
            this.JointButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.JointButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RevoluteJointButton,
            this.DistanceJointButton,
            this.PrismaticJointButton,
            this.GearJointButton,
            this.PulleyJointButton,
            this.RopeJointButton});
            this.JointButton.Image = global::SilhouetteEditor.Properties.Resource.AddEvent;
            this.JointButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.JointButton.Name = "JointButton";
            this.JointButton.Size = new System.Drawing.Size(29, 22);
            this.JointButton.Text = "toolStripDropDownButton2";
            this.JointButton.ToolTipText = "Add Joint";
            // 
            // RevoluteJointButton
            // 
            this.RevoluteJointButton.Name = "RevoluteJointButton";
            this.RevoluteJointButton.Size = new System.Drawing.Size(123, 22);
            this.RevoluteJointButton.Text = "Revolute";
            this.RevoluteJointButton.Click += new System.EventHandler(this.revoluteJointObjectToolStripMenuItem_Click);
            // 
            // DistanceJointButton
            // 
            this.DistanceJointButton.Name = "DistanceJointButton";
            this.DistanceJointButton.Size = new System.Drawing.Size(123, 22);
            this.DistanceJointButton.Text = "Distance";
            this.DistanceJointButton.Click += new System.EventHandler(this.distanceJointObjectToolStripMenuItem_Click);
            // 
            // PrismaticJointButton
            // 
            this.PrismaticJointButton.Name = "PrismaticJointButton";
            this.PrismaticJointButton.Size = new System.Drawing.Size(123, 22);
            this.PrismaticJointButton.Text = "Prismatic";
            this.PrismaticJointButton.Click += new System.EventHandler(this.prismaticJointObjectToolStripMenuItem_Click);
            // 
            // GearJointButton
            // 
            this.GearJointButton.Name = "GearJointButton";
            this.GearJointButton.Size = new System.Drawing.Size(123, 22);
            this.GearJointButton.Text = "Gear";
            this.GearJointButton.Click += new System.EventHandler(this.gearJointObjectToolStripMenuItem_Click);
            // 
            // PulleyJointButton
            // 
            this.PulleyJointButton.Name = "PulleyJointButton";
            this.PulleyJointButton.Size = new System.Drawing.Size(123, 22);
            this.PulleyJointButton.Text = "Pulley";
            this.PulleyJointButton.Click += new System.EventHandler(this.pulleyJointObjectToolStripMenuItem_Click);
            // 
            // RopeJointButton
            // 
            this.RopeJointButton.Name = "RopeJointButton";
            this.RopeJointButton.Size = new System.Drawing.Size(123, 22);
            this.RopeJointButton.Text = "Rope";
            this.RopeJointButton.Click += new System.EventHandler(this.ropeJointObjectToolStripMenuItem_Click);
            // 
            // GameScaleButton
            // 
            this.GameScaleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.GameScaleButton.Image = global::SilhouetteEditor.Properties.Resource.ApplyGameScale;
            this.GameScaleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.GameScaleButton.Name = "GameScaleButton";
            this.GameScaleButton.Size = new System.Drawing.Size(23, 22);
            this.GameScaleButton.Text = "Set Zoom to ingame zoom";
            this.GameScaleButton.ToolTipText = "Set Zoom to ingame zoom";
            this.GameScaleButton.Click += new System.EventHandler(this.GameScaleButton_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.AllowDrop = true;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPageAnimations);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(245, 392);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.propertyGrid1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(237, 366);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Attributes";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.AllowDrop = true;
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(3, 3);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.propertyGrid1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.propertyGrid1.Size = new System.Drawing.Size(231, 360);
            this.propertyGrid1.TabIndex = 0;
            this.propertyGrid1.TabStop = false;
            this.propertyGrid1.ToolbarVisible = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.TextureView);
            this.tabPage2.Controls.Add(this.BrowseButton);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(237, 366);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Textures";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // TextureView
            // 
            this.TextureView.AllowDrop = true;
            this.TextureView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextureView.LargeImageList = this.ImageList32;
            this.TextureView.Location = new System.Drawing.Point(3, 3);
            this.TextureView.Name = "TextureView";
            this.TextureView.Size = new System.Drawing.Size(231, 337);
            this.TextureView.TabIndex = 1;
            this.TextureView.UseCompatibleStateImageBehavior = false;
            this.TextureView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.TextureView_ItemDrag);
            this.TextureView.DragOver += new System.Windows.Forms.DragEventHandler(this.TextureView_DragOver);
            this.TextureView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TextureView_MouseDoubleClick);
            // 
            // ImageList32
            // 
            this.ImageList32.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.ImageList32.ImageSize = new System.Drawing.Size(32, 32);
            this.ImageList32.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // BrowseButton
            // 
            this.BrowseButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BrowseButton.Location = new System.Drawing.Point(3, 340);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(231, 23);
            this.BrowseButton.TabIndex = 0;
            this.BrowseButton.Text = "Browse...";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.BrowseButton2);
            this.tabPage3.Controls.Add(this.InteractiveView);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(237, 366);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Interactive";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // BrowseButton2
            // 
            this.BrowseButton2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BrowseButton2.Location = new System.Drawing.Point(3, 340);
            this.BrowseButton2.Name = "BrowseButton2";
            this.BrowseButton2.Size = new System.Drawing.Size(231, 23);
            this.BrowseButton2.TabIndex = 1;
            this.BrowseButton2.Text = "Browse...";
            this.BrowseButton2.UseVisualStyleBackColor = true;
            this.BrowseButton2.Click += new System.EventHandler(this.BrowseButton2_Click);
            // 
            // InteractiveView
            // 
            this.InteractiveView.AllowDrop = true;
            this.InteractiveView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InteractiveView.LargeImageList = this.ImageListInteractive32;
            this.InteractiveView.Location = new System.Drawing.Point(3, 3);
            this.InteractiveView.Name = "InteractiveView";
            this.InteractiveView.Size = new System.Drawing.Size(231, 360);
            this.InteractiveView.TabIndex = 0;
            this.InteractiveView.UseCompatibleStateImageBehavior = false;
            this.InteractiveView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.InteractiveView_ItemDrag);
            this.InteractiveView.DragOver += new System.Windows.Forms.DragEventHandler(this.InteractiveView_DragOver);
            this.InteractiveView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.InteractiveView_MouseDoubleClick);
            // 
            // ImageListInteractive32
            // 
            this.ImageListInteractive32.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.ImageListInteractive32.ImageSize = new System.Drawing.Size(32, 32);
            this.ImageListInteractive32.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tabPageAnimations
            // 
            this.tabPageAnimations.Controls.Add(this.TextureViewAnimations);
            this.tabPageAnimations.Controls.Add(this.BrowseButtonAnimations);
            this.tabPageAnimations.Location = new System.Drawing.Point(4, 22);
            this.tabPageAnimations.Name = "tabPageAnimations";
            this.tabPageAnimations.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAnimations.Size = new System.Drawing.Size(237, 366);
            this.tabPageAnimations.TabIndex = 3;
            this.tabPageAnimations.Text = "Animations";
            this.tabPageAnimations.UseVisualStyleBackColor = true;
            // 
            // TextureViewAnimations
            // 
            this.TextureViewAnimations.AllowDrop = true;
            this.TextureViewAnimations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextureViewAnimations.LargeImageList = this.ImageListAnimations32;
            this.TextureViewAnimations.Location = new System.Drawing.Point(3, 3);
            this.TextureViewAnimations.Name = "TextureViewAnimations";
            this.TextureViewAnimations.Size = new System.Drawing.Size(231, 337);
            this.TextureViewAnimations.TabIndex = 3;
            this.TextureViewAnimations.UseCompatibleStateImageBehavior = false;
            this.TextureViewAnimations.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.TextureViewAnimations_ItemDrag);
            this.TextureViewAnimations.DragOver += new System.Windows.Forms.DragEventHandler(this.TextureViewAnimations_DragOver);
            this.TextureViewAnimations.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TextureViewAnimations_MouseDoubleClick);
            // 
            // ImageListAnimations32
            // 
            this.ImageListAnimations32.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.ImageListAnimations32.ImageSize = new System.Drawing.Size(32, 32);
            this.ImageListAnimations32.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // BrowseButtonAnimations
            // 
            this.BrowseButtonAnimations.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BrowseButtonAnimations.Location = new System.Drawing.Point(3, 340);
            this.BrowseButtonAnimations.Name = "BrowseButtonAnimations";
            this.BrowseButtonAnimations.Size = new System.Drawing.Size(231, 23);
            this.BrowseButtonAnimations.TabIndex = 0;
            this.BrowseButtonAnimations.Text = "Browse...";
            this.BrowseButtonAnimations.UseVisualStyleBackColor = true;
            this.BrowseButtonAnimations.Click += new System.EventHandler(this.BrowseButtonAnimations_Click);
            // 
            // GameView
            // 
            this.GameView.AllowDrop = true;
            this.GameView.BackColor = System.Drawing.SystemColors.Window;
            this.GameView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GameView.Location = new System.Drawing.Point(0, 0);
            this.GameView.Name = "GameView";
            this.GameView.Size = new System.Drawing.Size(999, 678);
            this.GameView.TabIndex = 0;
            this.GameView.TabStop = false;
            this.GameView.DragEnter += new System.Windows.Forms.DragEventHandler(this.GameView_DragEnter);
            this.GameView.MouseEnter += new System.EventHandler(this.GameView_MouseEnter);
            this.GameView.MouseLeave += new System.EventHandler(this.GameView_MouseLeave);
            this.GameView.Resize += new System.EventHandler(this.GameView_Resize);
            // 
            // LevelContextMenu
            // 
            this.LevelContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addLayerToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.renameToolStripMenuItem3,
            this.addEffectToolStripMenuItem1});
            this.LevelContextMenu.Name = "contextMenuStrip1";
            this.LevelContextMenu.Size = new System.Drawing.Size(130, 76);
            this.LevelContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.LevelContextMenu_Opening);
            // 
            // addLayerToolStripMenuItem
            // 
            this.addLayerToolStripMenuItem.Name = "addLayerToolStripMenuItem";
            this.addLayerToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.addLayerToolStripMenuItem.Text = "Add Layer";
            this.addLayerToolStripMenuItem.Click += new System.EventHandler(this.LevelToolStrip_AddLayer);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(126, 6);
            // 
            // renameToolStripMenuItem3
            // 
            this.renameToolStripMenuItem3.Name = "renameToolStripMenuItem3";
            this.renameToolStripMenuItem3.Size = new System.Drawing.Size(129, 22);
            this.renameToolStripMenuItem3.Text = "Rename";
            this.renameToolStripMenuItem3.Click += new System.EventHandler(this.renameToolStripMenuItem3_Click);
            // 
            // addEffectToolStripMenuItem1
            // 
            this.addEffectToolStripMenuItem1.Name = "addEffectToolStripMenuItem1";
            this.addEffectToolStripMenuItem1.Size = new System.Drawing.Size(129, 22);
            this.addEffectToolStripMenuItem1.Text = "Add Effect";
            this.addEffectToolStripMenuItem1.Click += new System.EventHandler(this.addEffectToolStripMenuItem1_Click);
            // 
            // LayerContextMenu
            // 
            this.LayerContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameToolStripMenuItem2,
            this.deleteToolStripMenuItem,
            this.toolStripSeparator5,
            this.moveUpToolStripMenuItem,
            this.moveDownToolStripMenuItem,
            this.toolStripSeparator4,
            this.addEffectToolStripMenuItem});
            this.LayerContextMenu.Name = "LayerContextMenu";
            this.LayerContextMenu.Size = new System.Drawing.Size(139, 126);
            this.LayerContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.LayerContextMenu_Opening);
            // 
            // renameToolStripMenuItem2
            // 
            this.renameToolStripMenuItem2.Name = "renameToolStripMenuItem2";
            this.renameToolStripMenuItem2.Size = new System.Drawing.Size(138, 22);
            this.renameToolStripMenuItem2.Text = "Rename";
            this.renameToolStripMenuItem2.Click += new System.EventHandler(this.renameToolStripMenuItem2_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(135, 6);
            // 
            // moveUpToolStripMenuItem
            // 
            this.moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
            this.moveUpToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.moveUpToolStripMenuItem.Text = "Move Up";
            this.moveUpToolStripMenuItem.Click += new System.EventHandler(this.moveUpToolStripMenuItem_Click);
            // 
            // moveDownToolStripMenuItem
            // 
            this.moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
            this.moveDownToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.moveDownToolStripMenuItem.Text = "Move Down";
            this.moveDownToolStripMenuItem.Click += new System.EventHandler(this.moveDownToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(135, 6);
            // 
            // addEffectToolStripMenuItem
            // 
            this.addEffectToolStripMenuItem.Name = "addEffectToolStripMenuItem";
            this.addEffectToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.addEffectToolStripMenuItem.Text = "Add Effect";
            this.addEffectToolStripMenuItem.Click += new System.EventHandler(this.addEffectToolStripMenuItem_Click);
            // 
            // ObjectContextMenu
            // 
            this.ObjectContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameToolStripMenuItem1,
            this.copyToolStripMenuItem1,
            this.deleteToolStripMenuItem1});
            this.ObjectContextMenu.Name = "ObjectContextMenu";
            this.ObjectContextMenu.Size = new System.Drawing.Size(118, 70);
            // 
            // renameToolStripMenuItem1
            // 
            this.renameToolStripMenuItem1.Name = "renameToolStripMenuItem1";
            this.renameToolStripMenuItem1.Size = new System.Drawing.Size(117, 22);
            this.renameToolStripMenuItem1.Text = "Rename";
            this.renameToolStripMenuItem1.Click += new System.EventHandler(this.renameToolStripMenuItem1_Click);
            // 
            // copyToolStripMenuItem1
            // 
            this.copyToolStripMenuItem1.Name = "copyToolStripMenuItem1";
            this.copyToolStripMenuItem1.Size = new System.Drawing.Size(117, 22);
            this.copyToolStripMenuItem1.Text = "Copy";
            this.copyToolStripMenuItem1.Click += new System.EventHandler(this.copyToolStripMenuItem1_Click);
            // 
            // deleteToolStripMenuItem1
            // 
            this.deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            this.deleteToolStripMenuItem1.Size = new System.Drawing.Size(117, 22);
            this.deleteToolStripMenuItem1.Text = "Delete";
            this.deleteToolStripMenuItem1.Click += new System.EventHandler(this.deleteToolStripMenuItem1_Click);
            // 
            // EventContextMenü
            // 
            this.EventContextMenü.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameToolStripMenuItem4,
            this.copyToolStripMenuItem,
            this.deleteToolStripMenuItem2,
            this.toolStripSeparator3,
            this.addObjectToolStripMenuItem,
            this.addEffectToolStripMenuItem2});
            this.EventContextMenü.Name = "EventContextMenü";
            this.EventContextMenü.Size = new System.Drawing.Size(155, 120);
            // 
            // renameToolStripMenuItem4
            // 
            this.renameToolStripMenuItem4.Name = "renameToolStripMenuItem4";
            this.renameToolStripMenuItem4.Size = new System.Drawing.Size(154, 22);
            this.renameToolStripMenuItem4.Text = "Rename";
            this.renameToolStripMenuItem4.Click += new System.EventHandler(this.renameToolStripMenuItem4_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem2
            // 
            this.deleteToolStripMenuItem2.Name = "deleteToolStripMenuItem2";
            this.deleteToolStripMenuItem2.Size = new System.Drawing.Size(154, 22);
            this.deleteToolStripMenuItem2.Text = "Delete";
            this.deleteToolStripMenuItem2.Click += new System.EventHandler(this.deleteToolStripMenuItem2_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(151, 6);
            // 
            // addObjectToolStripMenuItem
            // 
            this.addObjectToolStripMenuItem.Name = "addObjectToolStripMenuItem";
            this.addObjectToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.addObjectToolStripMenuItem.Text = "Manage Events";
            this.addObjectToolStripMenuItem.Click += new System.EventHandler(this.addObjectToolStripMenuItem_Click);
            // 
            // addEffectToolStripMenuItem2
            // 
            this.addEffectToolStripMenuItem2.Name = "addEffectToolStripMenuItem2";
            this.addEffectToolStripMenuItem2.Size = new System.Drawing.Size(154, 22);
            this.addEffectToolStripMenuItem2.Text = "Select Effects";
            this.addEffectToolStripMenuItem2.Click += new System.EventHandler(this.addEffectToolStripMenuItem2_Click_1);
            // 
            // btnTogglePhysics
            // 
            this.btnTogglePhysics.BackColor = System.Drawing.SystemColors.Window;
            this.btnTogglePhysics.Location = new System.Drawing.Point(255, 4);
            this.btnTogglePhysics.Name = "btnTogglePhysics";
            this.btnTogglePhysics.Size = new System.Drawing.Size(98, 22);
            this.btnTogglePhysics.TabIndex = 4;
            this.btnTogglePhysics.Text = "Enable Physics";
            this.btnTogglePhysics.UseVisualStyleBackColor = false;
            this.btnTogglePhysics.Click += new System.EventHandler(this.btnTogglePhysics_Click);
            // 
            // btnResetPhysics
            // 
            this.btnResetPhysics.BackColor = System.Drawing.SystemColors.Window;
            this.btnResetPhysics.Location = new System.Drawing.Point(359, 5);
            this.btnResetPhysics.Name = "btnResetPhysics";
            this.btnResetPhysics.Size = new System.Drawing.Size(103, 21);
            this.btnResetPhysics.TabIndex = 5;
            this.btnResetPhysics.Text = "Reset Physics";
            this.btnResetPhysics.UseVisualStyleBackColor = false;
            this.btnResetPhysics.Click += new System.EventHandler(this.btnResetPhysics_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GrayText;
            this.ClientSize = new System.Drawing.Size(1256, 730);
            this.Controls.Add(this.btnResetPhysics);
            this.Controls.Add(this.btnTogglePhysics);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.StatusBar);
            this.Controls.Add(this.MenuBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MenuBar;
            this.Name = "MainForm";
            this.Text = "Bound Editor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.StatusBar.ResumeLayout(false);
            this.StatusBar.PerformLayout();
            this.MenuBar.ResumeLayout(false);
            this.MenuBar.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ToolBar.ResumeLayout(false);
            this.ToolBar.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPageAnimations.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GameView)).EndInit();
            this.LevelContextMenu.ResumeLayout(false);
            this.LayerContextMenu.ResumeLayout(false);
            this.ObjectContextMenu.ResumeLayout(false);
            this.EventContextMenü.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip StatusBar;
        private System.Windows.Forms.MenuStrip MenuBar;
        private System.Windows.Forms.ToolStripMenuItem dateiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ansichtToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ToolStrip ToolBar;
        public System.Windows.Forms.TreeView treeView1;
        public System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton runToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
        private System.Windows.Forms.ToolStripButton NewLayerButton;
        private System.Windows.Forms.ToolStripButton DeleteLayerButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ContextMenuStrip LevelContextMenu;
        private System.Windows.Forms.ContextMenuStrip LayerContextMenu;
        private System.Windows.Forms.ToolStripMenuItem addLayerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        public System.Windows.Forms.ToolStripStatusLabel MouseWorldPosition;
        public System.Windows.Forms.ToolStripStatusLabel Selection;
        public System.Windows.Forms.PictureBox GameView;
        public System.Windows.Forms.ToolStripStatusLabel SelectedItem;
        private System.Windows.Forms.ContextMenuStrip ObjectContextMenu;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPageAnimations;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.Button BrowseButtonAnimations;
        private System.Windows.Forms.Button BrowseButton2;
        private System.Windows.Forms.ListView TextureView;
        private System.Windows.Forms.ListView TextureViewAnimations;
        private System.Windows.Forms.ImageList ImageList32;
        private System.Windows.Forms.ImageList ImageListAnimations32;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        public System.Windows.Forms.ToolStripStatusLabel EditorStatus;
        public System.Windows.Forms.ToolStripStatusLabel FPS;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ListView InteractiveView;
        private System.Windows.Forms.ImageList ImageListInteractive32;
        private System.Windows.Forms.ToolStripDropDownButton PrimitiveButton;
        private System.Windows.Forms.ToolStripMenuItem rectangleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem circleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pathToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton FixtureButton;
        private System.Windows.Forms.ToolStripMenuItem rectangleCollisionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem circleCollisionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pathCollisionToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton PhysicsButton;
        private System.Windows.Forms.ToolStripDropDownButton EventButton;
        private System.Windows.Forms.ToolStripDropDownButton JointButton;
        private System.Windows.Forms.ToolStripMenuItem RevoluteJointButton;
        private System.Windows.Forms.ToolStripMenuItem DistanceJointButton;
        private System.Windows.Forms.ToolStripMenuItem PrismaticJointButton;
        private System.Windows.Forms.ToolStripMenuItem GearJointButton;
        private System.Windows.Forms.ToolStripMenuItem PulleyJointButton;
        private System.Windows.Forms.ToolStripMenuItem RopeJointButton;
        private System.Windows.Forms.ToolStripButton GameScaleButton;
        public System.Windows.Forms.ToolStripStatusLabel ZoomStatus;
        private System.Windows.Forms.ToolStripMenuItem physicToolStripMenuItem;
        public System.Windows.Forms.ContextMenuStrip EventContextMenü;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem addObjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem moveUpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveDownToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eventManagerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem audioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fadeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem equalizerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modifyPlaybackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem muteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setVolumeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem videoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeBodyTypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem soundObjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem particleObjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reverbToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem crossfaderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cameraToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem objectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveRotateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeVisibilityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeGlobalGravityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem applyForceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeEffectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveStateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadLevelToolStripMenuItem;
        private System.Windows.Forms.Button btnTogglePhysics;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem addEffectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addEffectToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem addEffectToolStripMenuItem2;
        private System.Windows.Forms.Button btnResetPhysics;

    }
}