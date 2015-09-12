/*
 * Created by SharpDevelop.
 * User: L
 * Date: 2/16/2013
 * Time: 12:22 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using SamDiagrams;
using SamDiagrams.Drawers.Links;
using SamDiagrams.Drawings;
using SamDiagrams.Drawings.Geometry;
using SamDiagrams.Model;

namespace TestProject
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}
		
 
		private class CustomItem : Item, ILinkable
		{
			public List<ILink> Links {
				get;
				set;
			}

			public string Name {
				get;
				set ;
			}
			public Color Color {
				get;
				set ;
			}
			public IDrawing Drawing {
				get;
				set ;
			}
			
		}
		
		private class CircleDrawing : DiagramComponent
		{
			
			public CircleDrawing(int width, int height)
				: base(new Size(width, height), true)
			{
				
			}
			
			public override void Draw(Graphics graphics)
			{
				using (Pen pen = new Pen(Color.Aquamarine, 3)) {
					graphics.DrawRectangle(pen, new Rectangle(location, size));
					graphics.DrawEllipse(Pens.Blue, this.Bounds);
				}
			}
			
			public override Rectangle InvalidatedRegion {
				get {
					MergableRectangle rectangle = new MergableRectangle(new Rectangle(this.location, this.size));
					foreach (LinkDrawing link in this.DrawingLinks) {
						rectangle.Merge(link.Bounds);
					}					
					return rectangle.Bounds;
				}
			}
			
		}
		private class CustomDrawing : DiagramComponent
		{

			public CustomDrawing(Item item)
				: base(item)
			{
				
			}

			public override void Draw(Graphics graphics)
			{
				graphics.DrawRectangle(Pens.Black, new Rectangle(location, size));
				graphics.DrawEllipse(Pens.Red, this.Bounds);
			}

			public override Rectangle InvalidatedRegion {
				get {
					MergableRectangle rectangle = new MergableRectangle(new Rectangle(this.location, this.size));
					foreach (LinkDrawing link in this.DrawingLinks) {
						rectangle.Merge(link.Bounds);
					}					
					return rectangle.Bounds;
				}
			}
			
		}
		
		void MainFormLoad(object sender, EventArgs e)
		{
			Image intImg = Image.FromFile("diamond.png");
			Image tImg = Image.FromFile("table.png");
			const int k = 3;
			Random r = new Random(255);
			try {
				for (int i = 0; i < k; i++) {
					Structure di = new Structure(diagramContainer1, "ITEM" + i.ToString());
					di.TitleImage = tImg;
					Node cols = new Node("Columns", di);
					cols.AddNode(new Node("id", true, intImg, di));
					cols.AddNode(new Node("type", true, intImg, di));
					cols.AddNode(new Node("name", di));
					cols.AddNode(new Node("surname", di)).AddNode(new Node("child1", di)).AddNode(new Node("child2", di));
					cols.AddNode(new Node("key", di));
					cols.AddNode(new Node("anotherKey", di));
					cols.AddNode(new Node("valid", di)).AddNode(new Node("child3", di)).AddNode(new Node("child4", di));
					di.AddNode(cols);
					di.AddOnDiagram(diagramContainer1, Color.FromArgb(r.Next(255), r.Next(255), r.Next(255)));
					di.Drawing.Location = new Point(r.Next(300), r.Next(300));
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
			for (int i = 0; i < k; i++) {
               
				Color c = Color.FromArgb(r.Next(255), r.Next(255), r.Next(255));
				int x = r.Next(k);
				int y = r.Next(k);
				if (x != y) {
					diagramContainer1.AddLink(diagramContainer1.DiagramItems[x], diagramContainer1.DiagramItems[y]);
				}
			}
			diagramContainer1.Invalidate();
			
			CustomItem customItem = new CustomItem();
			customItem.Color = Color.Red;
			customItem.Name = "CustomItem";
			
			CustomDrawing customDrawing = new CustomDrawing(customItem);
			customDrawing.Movable = true;
			customDrawing.Size = new Size(100, 100);
			customItem.Drawing = customDrawing;
			
			Structure stru = (diagramContainer1.DiagramItems[0] as Structure);
			diagramContainer1.AddLink(stru.Nodes[0], customItem);
			
			diagramContainer1.AddItem(customItem, customDrawing, true, true);
			diagramContainer1.AddLink(customItem, diagramContainer1.DiagramItems[0]);
						
			CircleDrawing customDrawing2 = new CircleDrawing(130, 130);
			customDrawing2.Movable = true;
			customDrawing2.Size = new Size(100, 100);
			diagramContainer1.AddDrawing(customDrawing2, true);
			
			
			diagramContainer1.AddLinkDrawing(customDrawing2, customDrawing);
			diagramContainer1.Invalidate();

			diagramContainer1.DrawableHeight = 705;
			diagramContainer1.DrawableWidth = 758;
		}
		void TrackBar1Scroll(object sender, EventArgs e)
		{
			diagramContainer1.ZoomFactor = trackBar1.Value;
			zoomValueLabel.Text = trackBar1.Value.ToString() + "%";
		}

 
		
  
	}
}
