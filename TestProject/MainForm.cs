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
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using SamDiagrams;
using SamDiagrams.Drawers;
using SamDiagrams.Drawings;
using SamDiagrams.Linking;

namespace TestProject
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//

			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		private void test(object sender, BeforeNodeExpandOrCollapseArg e)
		{
			textBox1.Text = e.Nod.Text;
		}
		
		void MainFormLoad(object sender, EventArgs e)
		{
			Image intImg = Image.FromFile("diamond.png");
			Image tImg = Image.FromFile("table.png");
			var k = 5;
			Random r = new Random(255);
			try {
				for (int i = 0; i < k; i++) {
					Structure di = new Structure(diagramContainer1, "ITEM" + i.ToString());
					di.TitleImage = tImg;
					Node cols = new Node("Columns", di);
					cols.AddNode(new Node("id", true, intImg, di));
					cols.AddNode(new Node("type", true, intImg, di));
					cols.AddNode(new Node("nume", di));
					cols.AddNode(new Node("prenume", di));
					cols.AddNode(new Node("cheie", di));
					cols.AddNode(new Node("altacheia", di));
					di.Nodes.Add(cols);
					di.AddOnDiagram(diagramContainer1, Color.FromArgb(r.Next(255), r.Next(255), r.Next(255)));
					//di.SetLocation(r.Next(300), r.Next(300));
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


			//DiagramItem item1 = new DiagramItem(this.diagramContainer1, "Table1",   new Point(0,0), new Size(100,250));
			//item1.TitleImage = Image.FromFile("table.png");
			//item1.BeforeNodeExpandOrCollapse += new BeforeNodeExpandOrCollapseHandler(test);
			//DiagramItemNode n1 = new DiagramItemNode("Columns",false);
			////n1.Images.Add();
			//n1.AddNode(new DiagramItemNode("id", true, intImg));
			//DiagramItemNode n111 = new DiagramItemNode("type", true, Image.FromFile("diamond.png"));
			//n111.Images.Insert(0, Image.FromFile("key.png"));
			//n1.AddNode(n111);
			//n1.AddNode(new DiagramItemNode("nume", true, Image.FromFile("diamond.png")));
			//n1.AddNode(new DiagramItemNode("prenume", true, Image.FromFile("diamond.png")));
			//n1.AddNode(new DiagramItemNode("cheie"));
			//n1.AddNode(new DiagramItemNode("altacheia"));
			//n1["altacheia"].AddNode(new DiagramItemNode("copil"));
			//DiagramItemNode n2 = new DiagramItemNode("Indexes");
			//n2.AddNode(new DiagramItemNode("idx"));
			//n2.AddNode(new DiagramItemNode("i2"));
			//item1.Nodes.Add(n1);
			//item1.Nodes.Add(n2);

			
			//DiagramItem item2 = new DiagramItem(this.diagramContainer1, "Table2",   new Point(0,0), new Size(120,250));
			//item2.TitleImage = item1.TitleImage;
			//DiagramItemNode n21 = new DiagramItemNode("Columns");
			//n21.AddNode(new DiagramItemNode("id", true, intImg));
			//n21.AddNode(new DiagramItemNode("type", true, intImg));
			//n21.AddNode(new DiagramItemNode("nume"));
			//n21.AddNode(new DiagramItemNode("prenume"));
			//n21.AddNode(new DiagramItemNode("cheie"));
			//n21.AddNode(new DiagramItemNode("altacheia"));
			//item2.Nodes.Add(n21);

			//DiagramItem item3 = new DiagramItem(this.diagramContainer1, "Table3", new Point(0, 0), new Size(120, 250));
			//item3.TitleImage = item1.TitleImage;
			//DiagramItemNode n22 = new DiagramItemNode("Columns2");
			//n22.AddNode(new DiagramItemNode("id"));
			//n22.AddNode(new DiagramItemNode("type"));
			//n22.AddNode(new DiagramItemNode("nume"));
			//n22.AddNode(new DiagramItemNode("prenume"));
			//n22.AddNode(new DiagramItemNode("cheie"));
			//n22.AddNode(new DiagramItemNode("altacheia"));
			//item3.Nodes.Add(n22);


			//DiagramItem item4 = new DiagramItem(this.diagramContainer1, "Table4", new Point(0, 0), new Size(120, 250));
			//item4.TitleImage = item1.TitleImage;
			//DiagramItemNode n23 = new DiagramItemNode("Columns2");
			//n23.AddNode(new DiagramItemNode("id"));
			//n23.AddNode(new DiagramItemNode("type"));
			//n23.AddNode(new DiagramItemNode("nume"));
			//n23.AddNode(new DiagramItemNode("prenume"));
			//n23.AddNode(new DiagramItemNode("cheie"));
			//n23.AddNode(new DiagramItemNode("altacheia"));
			//item4.Nodes.Add(n23);
			//DiagramItem item5 = new DiagramItem(this.diagramContainer1, "Table5", new Point(0, 0), new Size(120, 250));
			//item5.TitleImage = item1.TitleImage;
			//DiagramItemNode n24 = new DiagramItemNode("Columns2");
			//n24.AddNode(new DiagramItemNode("id"));
			//n24.AddNode(new DiagramItemNode("type"));
			//n24.AddNode(new DiagramItemNode("nume"));
			//n24.AddNode(new DiagramItemNode("prenume"));
			//n24.AddNode(new DiagramItemNode("cheie"));
			//n24.AddNode(new DiagramItemNode("altacheia"));
			//item5.Nodes.Add(n24);

			//ItemLink l1 = new ItemLink(item1, item2);
			//ItemLink l2 = new ItemLink(item1, item3);
			//ItemLink l3 = new ItemLink(this.diagramContainer1, item2, item3);
			//diagramContainer1.AddItem(l1);
			//diagramContainer1.DrawableHeight = diagramContainer1.Height;
			//diagramContainer1.AddItem(l2);
			//diagramContainer1.AddItem(l3);


			//item1.AddOnDiagram(diagramContainer1, Color.DeepPink);
			//item2.AddOnDiagram(diagramContainer1, Color.BlueViolet);
			//item3.AddOnDiagram(diagramContainer1, Color.Beige);
			//item4.AddOnDiagram(diagramContainer1, Color.Aqua);
			//item5.AddOnDiagram(diagramContainer1, Color.MediumBlue);
			//diagramContainer1.LinkManager.AddLink(item1, item2);
			//diagramContainer1.LinkManager.AddLink(item1, item3);
			//diagramContainer1.LinkManager.AddLink(item1, item4);
			//diagramContainer1.LinkManager.AddLink(item1, item5);
			diagramContainer1.Invalidate();

			this.diagramContainer1.DrawableHeight = 705;
			this.diagramContainer1.DrawableWidth = 758;
		}

		void TrackBar1Scroll(object sender, EventArgs e)
		{
			diagramContainer1.ZoomFactor = trackBar1.Value;
			diagramContainer1.Refresh();
			textBox2.Text = trackBar1.Value.ToString();
		}
		
		void DiagramContainer1SelectedItemsChanged(object sender, SelectedItemsChangeArgs e)
		{
			listBox1.Items.Clear();
			foreach (StructureDrawing structureDrawing in e.SelectedItems)
				listBox1.Items.Add(structureDrawing.Structure.Title);
		}

		private void button1_Click(object sender, EventArgs e)
		{
			diagramContainer1.SnapObjectsToGrid = true;
		}

		private void trackBar2_Scroll(object sender, EventArgs e)
		{
			diagramContainer1.DrawableWidth = trackBar2.Value;
		}

		private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
		{

		}

		private void button2_Click(object sender, EventArgs e)
		{
			 
		}

 

		private void button3_Click(object sender, EventArgs e)
		{
			diagramContainer1.AutoSizeItem = !diagramContainer1.AutoSizeItem;
		}
		
		void DiagramContainer1Load(object sender, EventArgs e)
		{
			
		}
	}
}
