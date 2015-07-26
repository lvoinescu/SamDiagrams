/*
 * Created by SharpDevelop.
 * User: L
 * Date: 2/16/2013
 * Time: 12:22 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace SamDiagrams
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.trackBar1 = new System.Windows.Forms.TrackBar();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.trackBar2 = new System.Windows.Forms.TrackBar();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.diagramContainer1 = new SamDiagrams.DiagramContainer();
			this.button3 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.textBox2 = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox1.Location = new System.Drawing.Point(144, 292);
			this.textBox1.Margin = new System.Windows.Forms.Padding(4);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(272, 22);
			this.textBox1.TabIndex = 1;
			// 
			// listBox1
			// 
			this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.listBox1.FormattingEnabled = true;
			this.listBox1.ItemHeight = 16;
			this.listBox1.Location = new System.Drawing.Point(180, 121);
			this.listBox1.Margin = new System.Windows.Forms.Padding(4);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(216, 148);
			this.listBox1.TabIndex = 0;
			// 
			// trackBar1
			// 
			this.trackBar1.Location = new System.Drawing.Point(33, 26);
			this.trackBar1.Margin = new System.Windows.Forms.Padding(4);
			this.trackBar1.Maximum = 500;
			this.trackBar1.Minimum = 10;
			this.trackBar1.Name = "trackBar1";
			this.trackBar1.Size = new System.Drawing.Size(363, 42);
			this.trackBar1.TabIndex = 1;
			this.trackBar1.TickFrequency = 10;
			this.trackBar1.Value = 100;
			this.trackBar1.Scroll += new System.EventHandler(this.TrackBar1Scroll);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(568, 372);
			this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(231, 128);
			this.pictureBox1.TabIndex = 3;
			this.pictureBox1.TabStop = false;
			// 
			// trackBar2
			// 
			this.trackBar2.Location = new System.Drawing.Point(83, 494);
			this.trackBar2.Margin = new System.Windows.Forms.Padding(4);
			this.trackBar2.Maximum = 3000;
			this.trackBar2.Name = "trackBar2";
			this.trackBar2.Size = new System.Drawing.Size(363, 42);
			this.trackBar2.TabIndex = 4;
			this.trackBar2.Value = 400;
			this.trackBar2.Scroll += new System.EventHandler(this.trackBar2_Scroll);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.diagramContainer1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.button3);
			this.splitContainer1.Panel2.Controls.Add(this.button2);
			this.splitContainer1.Panel2.Controls.Add(this.button1);
			this.splitContainer1.Panel2.Controls.Add(this.textBox2);
			this.splitContainer1.Panel2.Controls.Add(this.trackBar2);
			this.splitContainer1.Panel2.Controls.Add(this.trackBar1);
			this.splitContainer1.Panel2.Controls.Add(this.pictureBox1);
			this.splitContainer1.Panel2.Controls.Add(this.listBox1);
			this.splitContainer1.Panel2.Controls.Add(this.textBox1);
			this.splitContainer1.Size = new System.Drawing.Size(1245, 705);
			this.splitContainer1.SplitterDistance = 758;
			this.splitContainer1.SplitterWidth = 5;
			this.splitContainer1.TabIndex = 5;
			// 
			// diagramContainer1
			// 
			this.diagramContainer1.AutoSizeItem = true;
			this.diagramContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.diagramContainer1.DrawableHeight = 705;
			this.diagramContainer1.DrawableWidth = 758;
			this.diagramContainer1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.diagramContainer1.Location = new System.Drawing.Point(0, 0);
			this.diagramContainer1.Name = "diagramContainer1";
			this.diagramContainer1.Size = new System.Drawing.Size(758, 705);
			this.diagramContainer1.SnapObjectsToGrid = false;
			this.diagramContainer1.TabIndex = 0;
			this.diagramContainer1.ZoomFactor = 100;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(43, 303);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(83, 35);
			this.button3.TabIndex = 8;
			this.button3.Text = "button3";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(40, 227);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(69, 42);
			this.button2.TabIndex = 7;
			this.button2.Text = "button2";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(40, 126);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(87, 49);
			this.button1.TabIndex = 6;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// textBox2
			// 
			this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBox2.Location = new System.Drawing.Point(99, 353);
			this.textBox2.Margin = new System.Windows.Forms.Padding(0);
			this.textBox2.Multiline = true;
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(200, 18);
			this.textBox2.TabIndex = 5;
			this.textBox2.Text = "Mta";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1245, 705);
			this.Controls.Add(this.splitContainer1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "MainForm";
			this.Text = "SamDiagrams";
			this.Load += new System.EventHandler(this.MainFormLoad);
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		private SamDiagrams.DiagramContainer diagramContainer1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.TrackBar trackBar1;
 
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TrackBar trackBar2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
	}
}
