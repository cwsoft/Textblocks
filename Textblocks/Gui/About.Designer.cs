namespace cwsoft.Textblocks.Gui;

partial class About
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
      if (disposing && (components != null)) {
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
         this.label1 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.pictureBox1 = new System.Windows.Forms.PictureBox();
         this.label8 = new System.Windows.Forms.Label();
         this.label6 = new System.Windows.Forms.Label();
         this.label5 = new System.Windows.Forms.Label();
         this.label4 = new System.Windows.Forms.Label();
         this.LblVersion = new System.Windows.Forms.Label();
         this.LblReleaseDate = new System.Windows.Forms.Label();
         this.label7 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
         this.SuspendLayout();
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Font = new System.Drawing.Font("Arial Black", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label1.ForeColor = System.Drawing.Color.Navy;
         this.label1.Location = new System.Drawing.Point(100, 9);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(191, 41);
         this.label1.TabIndex = 5;
         this.label1.Text = "Textblocks";
         // 
         // label3
         // 
         this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label3.Location = new System.Drawing.Point(107, 59);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(458, 92);
         this.label3.TabIndex = 7;
         this.label3.Text = resources.GetString("label3.Text");
         // 
         // pictureBox1
         // 
         this.pictureBox1.Image = global::cwsoft.Textblocks.Properties.Resources.textblocks;
         this.pictureBox1.Location = new System.Drawing.Point(12, 18);
         this.pictureBox1.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
         this.pictureBox1.Name = "pictureBox1";
         this.pictureBox1.Size = new System.Drawing.Size(69, 64);
         this.pictureBox1.TabIndex = 8;
         this.pictureBox1.TabStop = false;
         // 
         // label8
         // 
         this.label8.AutoSize = true;
         this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label8.ForeColor = System.Drawing.Color.Black;
         this.label8.Location = new System.Drawing.Point(104, 178);
         this.label8.Name = "label8";
         this.label8.Size = new System.Drawing.Size(168, 15);
         this.label8.TabIndex = 101;
         this.label8.Text = "Christian Sommer (cwsoft.de)";
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label6.ForeColor = System.Drawing.Color.Black;
         this.label6.Location = new System.Drawing.Point(10, 197);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(46, 15);
         this.label6.TabIndex = 100;
         this.label6.Text = "Lizenz:";
         // 
         // label5
         // 
         this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label5.ForeColor = System.Drawing.Color.DarkBlue;
         this.label5.Location = new System.Drawing.Point(7, 223);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(558, 35);
         this.label5.TabIndex = 99;
         this.label5.Text = "Hinweis: Die Nutzung von Textblocks geschieht auf eigene Verantwortung und ohne d" +
    "ie implizite Garantie der Marktreife oder der Verwendbarkeit für einen bestimmte" +
    "n Zweck.";
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label4.ForeColor = System.Drawing.Color.Black;
         this.label4.Location = new System.Drawing.Point(10, 178);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(38, 15);
         this.label4.TabIndex = 98;
         this.label4.Text = "Autor:";
         // 
         // LblVersion
         // 
         this.LblVersion.AutoSize = true;
         this.LblVersion.Font = new System.Drawing.Font("Arial Black", 10F, System.Drawing.FontStyle.Bold);
         this.LblVersion.ForeColor = System.Drawing.Color.Black;
         this.LblVersion.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
         this.LblVersion.Location = new System.Drawing.Point(297, 24);
         this.LblVersion.Name = "LblVersion";
         this.LblVersion.Size = new System.Drawing.Size(76, 19);
         this.LblVersion.TabIndex = 103;
         this.LblVersion.Text = "(Version)";
         // 
         // LblReleaseDate
         // 
         this.LblReleaseDate.AutoSize = true;
         this.LblReleaseDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.LblReleaseDate.ForeColor = System.Drawing.Color.Black;
         this.LblReleaseDate.Location = new System.Drawing.Point(104, 159);
         this.LblReleaseDate.Name = "LblReleaseDate";
         this.LblReleaseDate.Size = new System.Drawing.Size(11, 15);
         this.LblReleaseDate.TabIndex = 105;
         this.LblReleaseDate.Text = "-";
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label7.ForeColor = System.Drawing.Color.Black;
         this.label7.Location = new System.Drawing.Point(10, 159);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(80, 15);
         this.label7.TabIndex = 104;
         this.label7.Text = "Freigabe am:";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label2.ForeColor = System.Drawing.Color.Black;
         this.label2.Location = new System.Drawing.Point(104, 197);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(226, 15);
         this.label2.TabIndex = 106;
         this.label2.Text = "GNU General Public License (Version 3)";
         // 
         // About
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(569, 260);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.LblReleaseDate);
         this.Controls.Add(this.label7);
         this.Controls.Add(this.LblVersion);
         this.Controls.Add(this.label8);
         this.Controls.Add(this.label6);
         this.Controls.Add(this.label5);
         this.Controls.Add(this.label4);
         this.Controls.Add(this.pictureBox1);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.label1);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "About";
         this.ShowInTaskbar = false;
         this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "Über Textblocks";
         this.TopMost = true;
         ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

   }

   #endregion

   private System.Windows.Forms.Label label1;
   private System.Windows.Forms.Label label3;
   private System.Windows.Forms.PictureBox pictureBox1;
   private System.Windows.Forms.Label label8;
   private System.Windows.Forms.Label label6;
   private System.Windows.Forms.Label label5;
   private System.Windows.Forms.Label label4;
   private System.Windows.Forms.Label LblVersion;
   private System.Windows.Forms.Label LblReleaseDate;
   private System.Windows.Forms.Label label7;
   private System.Windows.Forms.Label label2;
}
