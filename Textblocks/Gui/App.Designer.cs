namespace cwsoft.Textblocks.Gui;

partial class App
{
   /// <summary>
   /// Erforderliche Designervariable.
   /// </summary>
   private System.ComponentModel.IContainer components = null;

   /// <summary>
   /// Verwendete Ressourcen bereinigen.
   /// </summary>
   /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
   protected override void Dispose(bool disposing)
   {
      if (disposing && (components != null)) {
         components.Dispose();
         _catalog?.Dispose();
      }
      base.Dispose(disposing);
   }

   #region Vom Windows Form-Designer generierter Code

   /// <summary>
   /// Erforderliche Methode für die Designerunterstützung.
   /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
   /// </summary>
   private void InitializeComponent()
   {
         this.components = new System.ComponentModel.Container();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(App));
         this.Rtb_Preview = new System.Windows.Forms.RichTextBox();
         this.Lbl_Category = new System.Windows.Forms.Label();
         this.menuStrip1 = new System.Windows.Forms.MenuStrip();
         this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
         this.Men_OpenCatalog = new System.Windows.Forms.ToolStripMenuItem();
         this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
         this.Men_Quit = new System.Windows.Forms.ToolStripMenuItem();
         this.hilfeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.Men_Help = new System.Windows.Forms.ToolStripMenuItem();
         this.Men_About = new System.Windows.Forms.ToolStripMenuItem();
         this.Cbx_Categories = new System.Windows.Forms.ComboBox();
         this.label1 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.Cbx_Textblocks = new System.Windows.Forms.ComboBox();
         this.Grp_Textblocks = new System.Windows.Forms.GroupBox();
         this.Grp_SearchFilter = new System.Windows.Forms.GroupBox();
         this.label4 = new System.Windows.Forms.Label();
         this.Rbt_Or = new System.Windows.Forms.RadioButton();
         this.Rbt_And = new System.Windows.Forms.RadioButton();
         this.Btn_ResetFilter = new System.Windows.Forms.Button();
         this.Btn_ApplyFilter = new System.Windows.Forms.Button();
         this.Tbx_SearchFilter = new System.Windows.Forms.TextBox();
         this.Lbl_SearchFilter = new System.Windows.Forms.Label();
         this.statusStrip1 = new System.Windows.Forms.StatusStrip();
         this.Lbl_StatusBar = new System.Windows.Forms.ToolStripStatusLabel();
         this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
         this.menuStrip1.SuspendLayout();
         this.Grp_Textblocks.SuspendLayout();
         this.Grp_SearchFilter.SuspendLayout();
         this.statusStrip1.SuspendLayout();
         this.SuspendLayout();
         // 
         // Rtb_Preview
         // 
         this.Rtb_Preview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.Rtb_Preview.BackColor = System.Drawing.Color.White;
         this.Rtb_Preview.Font = new System.Drawing.Font("Calibri", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Rtb_Preview.ForeColor = System.Drawing.Color.Blue;
         this.Rtb_Preview.ImeMode = System.Windows.Forms.ImeMode.Off;
         this.Rtb_Preview.Location = new System.Drawing.Point(9, 151);
         this.Rtb_Preview.Margin = new System.Windows.Forms.Padding(4);
         this.Rtb_Preview.Name = "Rtb_Preview";
         this.Rtb_Preview.ReadOnly = true;
         this.Rtb_Preview.Size = new System.Drawing.Size(866, 285);
         this.Rtb_Preview.TabIndex = 6;
         this.Rtb_Preview.Text = "";
         // 
         // Lbl_Category
         // 
         this.Lbl_Category.AutoSize = true;
         this.Lbl_Category.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Lbl_Category.Location = new System.Drawing.Point(11, 130);
         this.Lbl_Category.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
         this.Lbl_Category.Name = "Lbl_Category";
         this.Lbl_Category.Size = new System.Drawing.Size(96, 18);
         this.Lbl_Category.TabIndex = 9;
         this.Lbl_Category.Text = "Kategorie: -";
         // 
         // menuStrip1
         // 
         this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
         this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.hilfeToolStripMenuItem});
         this.menuStrip1.Location = new System.Drawing.Point(0, 0);
         this.menuStrip1.Name = "menuStrip1";
         this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
         this.menuStrip1.Size = new System.Drawing.Size(884, 24);
         this.menuStrip1.TabIndex = 14;
         this.menuStrip1.Text = "menuStrip1";
         // 
         // toolStripMenuItem1
         // 
         this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Men_OpenCatalog,
            this.toolStripSeparator1,
            this.Men_Quit});
         this.toolStripMenuItem1.Name = "toolStripMenuItem1";
         this.toolStripMenuItem1.Size = new System.Drawing.Size(46, 20);
         this.toolStripMenuItem1.Text = "&Datei";
         // 
         // Men_OpenCatalog
         // 
         this.Men_OpenCatalog.Name = "Men_OpenCatalog";
         this.Men_OpenCatalog.Size = new System.Drawing.Size(152, 22);
         this.Men_OpenCatalog.Text = "Katalog &öffnen";
         this.Men_OpenCatalog.Click += new System.EventHandler(this.Men_OpenCatalog_Click);
         // 
         // toolStripSeparator1
         // 
         this.toolStripSeparator1.Name = "toolStripSeparator1";
         this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
         // 
         // Men_Quit
         // 
         this.Men_Quit.Name = "Men_Quit";
         this.Men_Quit.Size = new System.Drawing.Size(152, 22);
         this.Men_Quit.Text = "&Beenden";
         this.Men_Quit.Click += new System.EventHandler(this.Men_Quit_Click);
         // 
         // hilfeToolStripMenuItem
         // 
         this.hilfeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Men_Help,
            this.Men_About});
         this.hilfeToolStripMenuItem.Name = "hilfeToolStripMenuItem";
         this.hilfeToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
         this.hilfeToolStripMenuItem.Text = "&Hilfe";
         // 
         // Men_Help
         // 
         this.Men_Help.Name = "Men_Help";
         this.Men_Help.Size = new System.Drawing.Size(157, 22);
         this.Men_Help.Text = "Hilfe &anzeigen";
         this.Men_Help.Click += new System.EventHandler(this.Men_Help_Click);
         // 
         // Men_About
         // 
         this.Men_About.Name = "Men_About";
         this.Men_About.Size = new System.Drawing.Size(157, 22);
         this.Men_About.Text = "&Über Textblocks";
         this.Men_About.Click += new System.EventHandler(this.Men_About_Click);
         // 
         // Cbx_Categories
         // 
         this.Cbx_Categories.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.Cbx_Categories.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.Cbx_Categories.FormattingEnabled = true;
         this.Cbx_Categories.ItemHeight = 15;
         this.Cbx_Categories.Location = new System.Drawing.Point(78, 20);
         this.Cbx_Categories.Margin = new System.Windows.Forms.Padding(2);
         this.Cbx_Categories.MaxDropDownItems = 10;
         this.Cbx_Categories.Name = "Cbx_Categories";
         this.Cbx_Categories.Size = new System.Drawing.Size(437, 23);
         this.Cbx_Categories.TabIndex = 1;
         this.Cbx_Categories.SelectionChangeCommitted += new System.EventHandler(this.Cbx_Categories_SelectionChangeCommitted);
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(4, 22);
         this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(70, 15);
         this.label1.TabIndex = 18;
         this.label1.Text = "Kategorien:";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(5, 52);
         this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(69, 15);
         this.label2.TabIndex = 19;
         this.label2.Text = "Textblöcke:";
         // 
         // Cbx_Textblocks
         // 
         this.Cbx_Textblocks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.Cbx_Textblocks.FormattingEnabled = true;
         this.Cbx_Textblocks.ItemHeight = 15;
         this.Cbx_Textblocks.Location = new System.Drawing.Point(78, 50);
         this.Cbx_Textblocks.Margin = new System.Windows.Forms.Padding(2);
         this.Cbx_Textblocks.MaxDropDownItems = 10;
         this.Cbx_Textblocks.Name = "Cbx_Textblocks";
         this.Cbx_Textblocks.Size = new System.Drawing.Size(437, 23);
         this.Cbx_Textblocks.TabIndex = 2;
         this.Cbx_Textblocks.SelectionChangeCommitted += new System.EventHandler(this.Cbx_Textblocks_SelectionChangeCommitted);
         // 
         // Grp_Textblocks
         // 
         this.Grp_Textblocks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.Grp_Textblocks.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
         this.Grp_Textblocks.Controls.Add(this.label1);
         this.Grp_Textblocks.Controls.Add(this.Cbx_Textblocks);
         this.Grp_Textblocks.Controls.Add(this.Cbx_Categories);
         this.Grp_Textblocks.Controls.Add(this.label2);
         this.Grp_Textblocks.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Grp_Textblocks.Location = new System.Drawing.Point(9, 34);
         this.Grp_Textblocks.Margin = new System.Windows.Forms.Padding(2);
         this.Grp_Textblocks.Name = "Grp_Textblocks";
         this.Grp_Textblocks.Padding = new System.Windows.Forms.Padding(2);
         this.Grp_Textblocks.Size = new System.Drawing.Size(523, 91);
         this.Grp_Textblocks.TabIndex = 21;
         this.Grp_Textblocks.TabStop = false;
         this.Grp_Textblocks.Text = "Verfügbare Textblöcke (0/0):";
         // 
         // Grp_SearchFilter
         // 
         this.Grp_SearchFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.Grp_SearchFilter.Controls.Add(this.label4);
         this.Grp_SearchFilter.Controls.Add(this.Rbt_Or);
         this.Grp_SearchFilter.Controls.Add(this.Rbt_And);
         this.Grp_SearchFilter.Controls.Add(this.Btn_ResetFilter);
         this.Grp_SearchFilter.Controls.Add(this.Btn_ApplyFilter);
         this.Grp_SearchFilter.Controls.Add(this.Tbx_SearchFilter);
         this.Grp_SearchFilter.Controls.Add(this.Lbl_SearchFilter);
         this.Grp_SearchFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Grp_SearchFilter.Location = new System.Drawing.Point(544, 34);
         this.Grp_SearchFilter.Margin = new System.Windows.Forms.Padding(2);
         this.Grp_SearchFilter.Name = "Grp_SearchFilter";
         this.Grp_SearchFilter.Padding = new System.Windows.Forms.Padding(2);
         this.Grp_SearchFilter.Size = new System.Drawing.Size(330, 91);
         this.Grp_SearchFilter.TabIndex = 22;
         this.Grp_SearchFilter.TabStop = false;
         this.Grp_SearchFilter.Text = "Suchfilter inaktiv:";
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(4, 55);
         this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(46, 15);
         this.label4.TabIndex = 21;
         this.label4.Text = "Option:";
         // 
         // Rbt_Or
         // 
         this.Rbt_Or.AutoSize = true;
         this.Rbt_Or.Location = new System.Drawing.Point(104, 53);
         this.Rbt_Or.Name = "Rbt_Or";
         this.Rbt_Or.Size = new System.Drawing.Size(52, 19);
         this.Rbt_Or.TabIndex = 20;
         this.Rbt_Or.Text = "Oder";
         this.Rbt_Or.UseVisualStyleBackColor = true;
         // 
         // Rbt_And
         // 
         this.Rbt_And.AutoSize = true;
         this.Rbt_And.Checked = true;
         this.Rbt_And.Location = new System.Drawing.Point(53, 53);
         this.Rbt_And.Name = "Rbt_And";
         this.Rbt_And.Size = new System.Drawing.Size(48, 19);
         this.Rbt_And.TabIndex = 19;
         this.Rbt_And.TabStop = true;
         this.Rbt_And.Text = "Und";
         this.Rbt_And.UseVisualStyleBackColor = true;
         this.Rbt_And.CheckedChanged += new System.EventHandler(this.Rbt_And_CheckedChanged);
         // 
         // Btn_ResetFilter
         // 
         this.Btn_ResetFilter.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.Btn_ResetFilter.Location = new System.Drawing.Point(252, 49);
         this.Btn_ResetFilter.Margin = new System.Windows.Forms.Padding(2);
         this.Btn_ResetFilter.Name = "Btn_ResetFilter";
         this.Btn_ResetFilter.Size = new System.Drawing.Size(70, 24);
         this.Btn_ResetFilter.TabIndex = 5;
         this.Btn_ResetFilter.Text = "Löschen";
         this.Btn_ResetFilter.UseVisualStyleBackColor = true;
         this.Btn_ResetFilter.Click += new System.EventHandler(this.Btn_ResetFilter_Click);
         // 
         // Btn_ApplyFilter
         // 
         this.Btn_ApplyFilter.Location = new System.Drawing.Point(173, 49);
         this.Btn_ApplyFilter.Margin = new System.Windows.Forms.Padding(2);
         this.Btn_ApplyFilter.Name = "Btn_ApplyFilter";
         this.Btn_ApplyFilter.Size = new System.Drawing.Size(75, 24);
         this.Btn_ApplyFilter.TabIndex = 4;
         this.Btn_ApplyFilter.Text = "Anwenden";
         this.Btn_ApplyFilter.UseVisualStyleBackColor = true;
         this.Btn_ApplyFilter.Click += new System.EventHandler(this.Btn_ApplyFilter_Click);
         // 
         // Tbx_SearchFilter
         // 
         this.Tbx_SearchFilter.Location = new System.Drawing.Point(53, 20);
         this.Tbx_SearchFilter.Margin = new System.Windows.Forms.Padding(2);
         this.Tbx_SearchFilter.Name = "Tbx_SearchFilter";
         this.Tbx_SearchFilter.Size = new System.Drawing.Size(269, 21);
         this.Tbx_SearchFilter.TabIndex = 3;
         this.Tbx_SearchFilter.WordWrap = false;
         this.Tbx_SearchFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Tbx_SearchFilter_KeyDown);
         // 
         // Lbl_SearchFilter
         // 
         this.Lbl_SearchFilter.AutoSize = true;
         this.Lbl_SearchFilter.Location = new System.Drawing.Point(4, 22);
         this.Lbl_SearchFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
         this.Lbl_SearchFilter.Name = "Lbl_SearchFilter";
         this.Lbl_SearchFilter.Size = new System.Drawing.Size(48, 15);
         this.Lbl_SearchFilter.TabIndex = 18;
         this.Lbl_SearchFilter.Text = "Enthält:";
         // 
         // statusStrip1
         // 
         this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
         this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Lbl_StatusBar});
         this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
         this.statusStrip1.Location = new System.Drawing.Point(0, 439);
         this.statusStrip1.Name = "statusStrip1";
         this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
         this.statusStrip1.Size = new System.Drawing.Size(884, 22);
         this.statusStrip1.TabIndex = 23;
         this.statusStrip1.Text = "statusStrip1";
         // 
         // Lbl_StatusBar
         // 
         this.Lbl_StatusBar.Name = "Lbl_StatusBar";
         this.Lbl_StatusBar.Size = new System.Drawing.Size(50, 17);
         this.Lbl_StatusBar.Text = "Katalog:";
         // 
         // App
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
         this.CancelButton = this.Btn_ResetFilter;
         this.ClientSize = new System.Drawing.Size(884, 461);
         this.Controls.Add(this.statusStrip1);
         this.Controls.Add(this.Grp_SearchFilter);
         this.Controls.Add(this.Grp_Textblocks);
         this.Controls.Add(this.Lbl_Category);
         this.Controls.Add(this.Rtb_Preview);
         this.Controls.Add(this.menuStrip1);
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.MainMenuStrip = this.menuStrip1;
         this.Margin = new System.Windows.Forms.Padding(2);
         this.MaximizeBox = false;
         this.MaximumSize = new System.Drawing.Size(1050, 700);
         this.MinimumSize = new System.Drawing.Size(750, 400);
         this.Name = "App";
         this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
         this.Text = "TextBlocks - (c) Christian Sommer";
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.App_FormClosing);
         this.Shown += new System.EventHandler(this.App_Shown);
         this.menuStrip1.ResumeLayout(false);
         this.menuStrip1.PerformLayout();
         this.Grp_Textblocks.ResumeLayout(false);
         this.Grp_Textblocks.PerformLayout();
         this.Grp_SearchFilter.ResumeLayout(false);
         this.Grp_SearchFilter.PerformLayout();
         this.statusStrip1.ResumeLayout(false);
         this.statusStrip1.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

   }

   #endregion
   private System.Windows.Forms.RichTextBox Rtb_Preview;
   private System.Windows.Forms.Label Lbl_Category;
   private System.Windows.Forms.MenuStrip menuStrip1;
   private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
   private System.Windows.Forms.ToolStripMenuItem Men_OpenCatalog;
   private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
   private System.Windows.Forms.ToolStripMenuItem Men_Quit;
   private System.Windows.Forms.ToolStripMenuItem hilfeToolStripMenuItem;
   private System.Windows.Forms.ToolStripMenuItem Men_Help;
   private System.Windows.Forms.ToolStripMenuItem Men_About;
   private System.Windows.Forms.ComboBox Cbx_Categories;
   private System.Windows.Forms.Label label1;
   private System.Windows.Forms.Label label2;
   private System.Windows.Forms.ComboBox Cbx_Textblocks;
   private System.Windows.Forms.GroupBox Grp_Textblocks;
   private System.Windows.Forms.GroupBox Grp_SearchFilter;
   private System.Windows.Forms.Button Btn_ResetFilter;
   private System.Windows.Forms.Button Btn_ApplyFilter;
   private System.Windows.Forms.TextBox Tbx_SearchFilter;
   private System.Windows.Forms.Label Lbl_SearchFilter;
   private System.Windows.Forms.StatusStrip statusStrip1;
   private System.Windows.Forms.ToolStripStatusLabel Lbl_StatusBar;
   private System.Windows.Forms.ToolTip ToolTip;
   private System.Windows.Forms.Label label4;
   private System.Windows.Forms.RadioButton Rbt_Or;
   private System.Windows.Forms.RadioButton Rbt_And;
}

