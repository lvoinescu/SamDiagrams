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
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.SuspendLayout();
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
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		private SamDiagrams.DiagramContainer diagramContainer1;
 
        private System.Windows.Forms.SplitContainer splitContainer1;
	}
}
