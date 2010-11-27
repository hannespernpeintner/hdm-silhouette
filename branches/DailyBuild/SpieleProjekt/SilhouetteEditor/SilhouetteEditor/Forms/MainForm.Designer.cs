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
            this.MenuBar = new System.Windows.Forms.MenuStrip();
            this.dateiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bearbeitenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ansichtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.leveleinstellungenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.ToolBoxButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.RectangleButton = new System.Windows.Forms.ToolStripButton();
            this.CircleButton = new System.Windows.Forms.ToolStripButton();
            this.PathButton = new System.Windows.Forms.ToolStripButton();
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
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.BrowseButton3 = new System.Windows.Forms.Button();
            this.AnimationView = new System.Windows.Forms.ListView();
            this.GameView = new System.Windows.Forms.PictureBox();
            this.LevelContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addLayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.renameToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.LayerContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.renameToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ObjectContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.renameToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
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
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GameView)).BeginInit();
            this.LevelContextMenu.SuspendLayout();
            this.LayerContextMenu.SuspendLayout();
            this.ObjectContextMenu.SuspendLayout();
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
            this.FPS});
            this.StatusBar.Location = new System.Drawing.Point(0, 708);
            this.StatusBar.Name = "StatusBar";
            this.StatusBar.Size = new System.Drawing.Size(1264, 22);
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
            this.EditorStatus.Size = new System.Drawing.Size(200, 17);
            this.EditorStatus.ToolTipText = "The status of the editor.";
            // 
            // FPS
            // 
            this.FPS.Name = "FPS";
            this.FPS.Size = new System.Drawing.Size(0, 17);
            // 
            // MenuBar
            // 
            this.MenuBar.BackColor = System.Drawing.SystemColors.Window;
            this.MenuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dateiToolStripMenuItem,
            this.bearbeitenToolStripMenuItem,
            this.ansichtToolStripMenuItem,
            this.leveleinstellungenToolStripMenuItem,
            this.runToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.MenuBar.Location = new System.Drawing.Point(0, 0);
            this.MenuBar.Name = "MenuBar";
            this.MenuBar.Size = new System.Drawing.Size(1264, 24);
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
            this.dateiToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
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
            // bearbeitenToolStripMenuItem
            // 
            this.bearbeitenToolStripMenuItem.Name = "bearbeitenToolStripMenuItem";
            this.bearbeitenToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.bearbeitenToolStripMenuItem.Text = "Edit";
            // 
            // ansichtToolStripMenuItem
            // 
            this.ansichtToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBoxToolStripMenuItem});
            this.ansichtToolStripMenuItem.Name = "ansichtToolStripMenuItem";
            this.ansichtToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.ansichtToolStripMenuItem.Text = "Tools";
            // 
            // toolBoxToolStripMenuItem
            // 
            this.toolBoxToolStripMenuItem.Name = "toolBoxToolStripMenuItem";
            this.toolBoxToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.toolBoxToolStripMenuItem.Text = "ToolBox";
            this.toolBoxToolStripMenuItem.Click += new System.EventHandler(this.ToolsToolBox);
            // 
            // leveleinstellungenToolStripMenuItem
            // 
            this.leveleinstellungenToolStripMenuItem.Name = "leveleinstellungenToolStripMenuItem";
            this.leveleinstellungenToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.leveleinstellungenToolStripMenuItem.Text = "Settings";
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.runToolStripMenuItem.Text = "Run";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.helpToolStripMenuItem1});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
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
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.GameView);
            this.splitContainer1.Size = new System.Drawing.Size(1264, 684);
            this.splitContainer1.SplitterDistance = 251;
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
            this.splitContainer2.Size = new System.Drawing.Size(251, 684);
            this.splitContainer2.SplitterDistance = 284;
            this.splitContainer2.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 25);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(247, 255);
            this.treeView1.TabIndex = 1;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // ToolBar
            // 
            this.ToolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewLayerButton,
            this.DeleteLayerButton,
            this.toolStripSeparator2,
            this.ToolBoxButton,
            this.toolStripSeparator3,
            this.RectangleButton,
            this.CircleButton,
            this.PathButton});
            this.ToolBar.Location = new System.Drawing.Point(0, 0);
            this.ToolBar.Name = "ToolBar";
            this.ToolBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ToolBar.Size = new System.Drawing.Size(247, 25);
            this.ToolBar.TabIndex = 0;
            this.ToolBar.Text = "ToolBar";
            // 
            // NewLayerButton
            // 
            this.NewLayerButton.Image = ((System.Drawing.Image)(resources.GetObject("NewLayerButton.Image")));
            this.NewLayerButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.NewLayerButton.Name = "NewLayerButton";
            this.NewLayerButton.Size = new System.Drawing.Size(23, 22);
            this.NewLayerButton.ToolTipText = "New Layer";
            this.NewLayerButton.Click += new System.EventHandler(this.LevelToolStrip_AddLayer);
            // 
            // DeleteLayerButton
            // 
            this.DeleteLayerButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DeleteLayerButton.Image = ((System.Drawing.Image)(resources.GetObject("DeleteLayerButton.Image")));
            this.DeleteLayerButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DeleteLayerButton.Name = "DeleteLayerButton";
            this.DeleteLayerButton.Size = new System.Drawing.Size(23, 22);
            this.DeleteLayerButton.Text = "Delete Layer";
            this.DeleteLayerButton.ToolTipText = "Delete Layer";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // ToolBoxButton
            // 
            this.ToolBoxButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolBoxButton.Image = ((System.Drawing.Image)(resources.GetObject("ToolBoxButton.Image")));
            this.ToolBoxButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolBoxButton.Name = "ToolBoxButton";
            this.ToolBoxButton.Size = new System.Drawing.Size(23, 22);
            this.ToolBoxButton.Text = "ToolBox";
            this.ToolBoxButton.ToolTipText = "ToolBox";
            this.ToolBoxButton.Click += new System.EventHandler(this.ToolBoxButton_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // RectangleButton
            // 
            this.RectangleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RectangleButton.Image = ((System.Drawing.Image)(resources.GetObject("RectangleButton.Image")));
            this.RectangleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RectangleButton.Name = "RectangleButton";
            this.RectangleButton.Size = new System.Drawing.Size(23, 22);
            this.RectangleButton.Text = "Create Rectangle Fixture";
            this.RectangleButton.ToolTipText = "Create Rectangle Fixture";
            this.RectangleButton.Click += new System.EventHandler(this.ToolStripButton_AddRectangleFixture);
            // 
            // CircleButton
            // 
            this.CircleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CircleButton.Image = ((System.Drawing.Image)(resources.GetObject("CircleButton.Image")));
            this.CircleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CircleButton.Name = "CircleButton";
            this.CircleButton.Size = new System.Drawing.Size(23, 22);
            this.CircleButton.Text = "Create Circle Fixture";
            this.CircleButton.ToolTipText = "Create Circle Fixture";
            this.CircleButton.Click += new System.EventHandler(this.ToolStripButton_AddCircleFixture);
            // 
            // PathButton
            // 
            this.PathButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.PathButton.Image = ((System.Drawing.Image)(resources.GetObject("PathButton.Image")));
            this.PathButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PathButton.Name = "PathButton";
            this.PathButton.Size = new System.Drawing.Size(23, 22);
            this.PathButton.Text = "Create Path Button";
            this.PathButton.ToolTipText = "Create Fixture Path";
            this.PathButton.Click += new System.EventHandler(this.PathButton_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.AllowDrop = true;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(247, 392);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.propertyGrid1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(239, 366);
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
            this.propertyGrid1.Size = new System.Drawing.Size(233, 360);
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
            this.tabPage2.Size = new System.Drawing.Size(239, 366);
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
            this.TextureView.Size = new System.Drawing.Size(233, 337);
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
            this.BrowseButton.Size = new System.Drawing.Size(233, 23);
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
            this.tabPage3.Size = new System.Drawing.Size(239, 366);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Interactive";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // BrowseButton2
            // 
            this.BrowseButton2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BrowseButton2.Location = new System.Drawing.Point(3, 340);
            this.BrowseButton2.Name = "BrowseButton2";
            this.BrowseButton2.Size = new System.Drawing.Size(233, 23);
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
            this.InteractiveView.Size = new System.Drawing.Size(233, 360);
            this.InteractiveView.TabIndex = 0;
            this.InteractiveView.UseCompatibleStateImageBehavior = false;
            this.InteractiveView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.InteractiveView_MouseDoubleClick);
            // 
            // ImageListInteractive32
            // 
            this.ImageListInteractive32.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.ImageListInteractive32.ImageSize = new System.Drawing.Size(32, 32);
            this.ImageListInteractive32.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.BrowseButton3);
            this.tabPage4.Controls.Add(this.AnimationView);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(239, 366);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Animation";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // BrowseButton3
            // 
            this.BrowseButton3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BrowseButton3.Location = new System.Drawing.Point(3, 340);
            this.BrowseButton3.Name = "BrowseButton3";
            this.BrowseButton3.Size = new System.Drawing.Size(233, 23);
            this.BrowseButton3.TabIndex = 1;
            this.BrowseButton3.Text = "Browse...";
            this.BrowseButton3.UseVisualStyleBackColor = true;
            // 
            // AnimationView
            // 
            this.AnimationView.AllowDrop = true;
            this.AnimationView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AnimationView.Location = new System.Drawing.Point(3, 3);
            this.AnimationView.Name = "AnimationView";
            this.AnimationView.Size = new System.Drawing.Size(233, 360);
            this.AnimationView.TabIndex = 0;
            this.AnimationView.UseCompatibleStateImageBehavior = false;
            // 
            // GameView
            // 
            this.GameView.AllowDrop = true;
            this.GameView.BackColor = System.Drawing.SystemColors.Window;
            this.GameView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GameView.Location = new System.Drawing.Point(0, 0);
            this.GameView.Name = "GameView";
            this.GameView.Size = new System.Drawing.Size(1005, 680);
            this.GameView.TabIndex = 0;
            this.GameView.TabStop = false;
            this.GameView.DragDrop += new System.Windows.Forms.DragEventHandler(this.GameView_DragDrop);
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
            this.renameToolStripMenuItem3});
            this.LevelContextMenu.Name = "contextMenuStrip1";
            this.LevelContextMenu.Size = new System.Drawing.Size(128, 54);
            // 
            // addLayerToolStripMenuItem
            // 
            this.addLayerToolStripMenuItem.Name = "addLayerToolStripMenuItem";
            this.addLayerToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.addLayerToolStripMenuItem.Text = "Add Layer";
            this.addLayerToolStripMenuItem.Click += new System.EventHandler(this.LevelToolStrip_AddLayer);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(124, 6);
            // 
            // renameToolStripMenuItem3
            // 
            this.renameToolStripMenuItem3.Name = "renameToolStripMenuItem3";
            this.renameToolStripMenuItem3.Size = new System.Drawing.Size(127, 22);
            this.renameToolStripMenuItem3.Text = "Rename";
            // 
            // LayerContextMenu
            // 
            this.LayerContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameToolStripMenuItem2,
            this.copyToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.LayerContextMenu.Name = "LayerContextMenu";
            this.LayerContextMenu.Size = new System.Drawing.Size(118, 70);
            // 
            // renameToolStripMenuItem2
            // 
            this.renameToolStripMenuItem2.Name = "renameToolStripMenuItem2";
            this.renameToolStripMenuItem2.Size = new System.Drawing.Size(117, 22);
            this.renameToolStripMenuItem2.Text = "Rename";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
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
            // 
            // copyToolStripMenuItem1
            // 
            this.copyToolStripMenuItem1.Name = "copyToolStripMenuItem1";
            this.copyToolStripMenuItem1.Size = new System.Drawing.Size(117, 22);
            this.copyToolStripMenuItem1.Text = "Copy";
            // 
            // deleteToolStripMenuItem1
            // 
            this.deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            this.deleteToolStripMenuItem1.Size = new System.Drawing.Size(117, 22);
            this.deleteToolStripMenuItem1.Text = "Delete";
            this.deleteToolStripMenuItem1.Click += new System.EventHandler(this.deleteToolStripMenuItem1_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GrayText;
            this.ClientSize = new System.Drawing.Size(1264, 730);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.StatusBar);
            this.Controls.Add(this.MenuBar);
            this.MainMenuStrip = this.MenuBar;
            this.Name = "MainForm";
            this.Text = "SilhouetteEditor";
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
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GameView)).EndInit();
            this.LevelContextMenu.ResumeLayout(false);
            this.LayerContextMenu.ResumeLayout(false);
            this.ObjectContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip StatusBar;
        private System.Windows.Forms.MenuStrip MenuBar;
        private System.Windows.Forms.ToolStripMenuItem dateiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bearbeitenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ansichtToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem leveleinstellungenToolStripMenuItem;
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
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
        private System.Windows.Forms.ToolStripButton NewLayerButton;
        private System.Windows.Forms.ToolStripButton DeleteLayerButton;
        private System.Windows.Forms.ToolStripButton ToolBoxButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ContextMenuStrip LevelContextMenu;
        private System.Windows.Forms.ContextMenuStrip LayerContextMenu;
        private System.Windows.Forms.ToolStripMenuItem addLayerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolBoxToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton RectangleButton;
        private System.Windows.Forms.ToolStripButton CircleButton;
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
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.ListView TextureView;
        private System.Windows.Forms.ImageList ImageList32;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel EditorStatus;
        public System.Windows.Forms.ToolStripStatusLabel FPS;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button BrowseButton2;
        private System.Windows.Forms.ListView InteractiveView;
        private System.Windows.Forms.Button BrowseButton3;
        private System.Windows.Forms.ListView AnimationView;
        private System.Windows.Forms.ImageList ImageListInteractive32;
        private System.Windows.Forms.ToolStripButton PathButton;
    }
}