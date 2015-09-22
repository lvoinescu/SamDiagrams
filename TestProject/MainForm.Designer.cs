/*
 * Created by SharpDevelop.
 * User: L
 * Date: 2/16/2013
 * Time: 12:22 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace TestProject
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.diagramContainer1 = new SamDiagrams.DiagramContainer();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.zoomValueLabel = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.trackBar1 = new System.Windows.Forms.TrackBar();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.diagramContainer1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
			this.splitContainer1.Panel2.Controls.Add(this.zoomValueLabel);
			this.splitContainer1.Panel2.Controls.Add(this.label1);
			this.splitContainer1.Panel2.Controls.Add(this.trackBar1);
			this.splitContainer1.Size = new System.Drawing.Size(1245, 705);
			this.splitContainer1.SplitterDistance = 758;
			this.splitContainer1.SplitterWidth = 3;
			this.splitContainer1.TabIndex = 5;
			// 
			// diagramContainer1
			// 
			this.diagramContainer1.AutoSizeItem = true;
			this.diagramContainer1.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.diagramContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.diagramContainer1.DrawableHeight = 705;
			this.diagramContainer1.DrawableWidth = 758;
			this.diagramContainer1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.diagramContainer1.Location = new System.Drawing.Point(0, 0);
			this.diagramContainer1.Margin = new System.Windows.Forms.Padding(2);
			this.diagramContainer1.Name = "diagramContainer1";
			this.diagramContainer1.Size = new System.Drawing.Size(758, 705);
			this.diagramContainer1.SnapObjectsToGrid = false;
			this.diagramContainer1.TabIndex = 0;
			this.diagramContainer1.ZoomFactor = 100;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radioButton1);
			this.groupBox1.Controls.Add(this.radioButton2);
			this.groupBox1.Location = new System.Drawing.Point(22, 60);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(450, 59);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Line style";
			// 
			// radioButton1
			// 
			this.radioButton1.Checked = true;
			this.radioButton1.Location = new System.Drawing.Point(15, 21);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(104, 24);
			this.radioButton1.TabIndex = 2;
			this.radioButton1.TabStop = true;
			this.radioButton1.Text = "single lines";
			this.radioButton1.UseVisualStyleBackColor = true;
			this.radioButton1.CheckedChanged += new System.EventHandler(this.LineStyleChanged);
			// 
			// radioButton2
			// 
			this.radioButton2.Location = new System.Drawing.Point(125, 21);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new System.Drawing.Size(104, 24);
			this.radioButton2.TabIndex = 3;
			this.radioButton2.Text = "rect lines";
			this.radioButton2.UseVisualStyleBackColor = true;
			this.radioButton2.CheckedChanged += new System.EventHandler(this.LineStyleChanged);
			// 
			// zoomValueLabel
			// 
			this.zoomValueLabel.Location = new System.Drawing.Point(13, 23);
			this.zoomValueLabel.Name = "zoomValueLabel";
			this.zoomValueLabel.Size = new System.Drawing.Size(56, 23);
			this.zoomValueLabel.TabIndex = 1;
			this.zoomValueLabel.Text = "100%";
			this.zoomValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(13, 2);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 23);
			this.label1.TabIndex = 1;
			this.label1.Text = "Zoom";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// trackBar1
			// 
			this.trackBar1.Location = new System.Drawing.Point(75, 12);
			this.trackBar1.Maximum = 500;
			this.trackBar1.Name = "trackBar1";
			this.trackBar1.Size = new System.Drawing.Size(397, 42);
			this.trackBar1.TabIndex = 0;
			this.trackBar1.Value = 100;
			this.trackBar1.Scroll += new System.EventHandler(this.TrackBar1Scroll);
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
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
			this.ResumeLayout(false);

		}
		private SamDiagrams.DiagramContainer diagramContainer1;
 
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label zoomValueLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
	}
}
