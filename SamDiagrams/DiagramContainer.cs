/*
 *   SamDiagrams - diagram component for .NET
 *   Copyright (C) 2011  Lucian Voinescu
 *
 *   This file is part of SamDiagrams
 *
 *   SamDiagrams is free software: you can redistribute it and/or modify
 *   it under the terms of the GNU Lesser General Public License as published by
 *   the Free Software Foundation, either version 3 of the License, or
 *   (at your option) any later version.
 *
 *   SamDiagrams is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU Lesser General Public License for more details.
 *
 *   You should have received a copy of the GNU Lesser General Public License
 *   along with SamDiagrams. If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using SamDiagrams.DiagramItem.NodeEditor;
using SamDiagrams.Drawers;
using SamDiagrams.Drawings;
using SamDiagrams.Drawings.Geometry;
using SamDiagrams.Drawings.Selection;

namespace SamDiagrams
{
	/// <summary>
	/// Description of DiagramContainer.
	/// </summary>
	public class DiagramContainer : UserControl
	{

		public event DiagramItemClickHandler DiagramItemClick;
		public event SelectedItemsChangeHandler SelectedItemsChanged;
		public event ZoomFactorChangedHandler ZoomFactorChanged;

		
		
		private List<IDrawing> selectedItems;
		private Structure primarySelectedItem;
		private bool isMouseHoldingForMovement = false;
		private bool isMouseHoldingForResize = false;
		private Point mouseHoldingInitial = new Point(0, 0);
		private Point itemMouseClickLocation = new Point(0, 0);
		List<Structure> structures;
		private int zoomFactor;
		private float scaleFactor = 1;
		private int gridSize = 16;
		private bool snapObjectsToGrid = false;
		private int drawableHeight, drawableWidth;

		private SelectionBorder currentResizeingBorder;
		private bool autoSizeItem = true;
		private Rectangle invalidatedRegion = new Rectangle();
		private Region oldIvalidatedRegion = new Region();
		private NodeTextEditor nodeEditor;
		private ContainerDrawer containerDrawer;
		
		public NodeTextEditor NodeEditor {
			get { return nodeEditor; }
		}


		public bool AutoSizeItem {
			get { return autoSizeItem; }
			set {
				autoSizeItem = value;
				foreach (IDrawing drawer in containerDrawer.Drawings)
					if (drawer is StructureDrawing)
						((StructureDrawing)drawer).AutoSizeContent();
				//Invalidate();
			}
		}

		public ContainerDrawer ContainerDrawer {
			get {
				return containerDrawer;
			}
		}
 
		public int DrawableWidth {
			get { return drawableWidth; }
			set {
				drawableWidth = value;
				SetScrollBarValues();
			}
		}

		public int DrawableHeight {
			get { return drawableHeight; }
			set {
				drawableHeight = value;
				SetScrollBarValues();
			}
		}

		HScrollBar hScrollBar = new HScrollBar();

		public HScrollBar HScrollBar {
			get {
				return hScrollBar;
			}
			set {
				hScrollBar = value;
			}
		}

		VScrollBar vScrollBar = new VScrollBar();

		public VScrollBar VScrollBar {
			get {
				return vScrollBar;
			}
			set {
				vScrollBar = value;
			}
		}
		public bool SnapObjectsToGrid {
			get { return snapObjectsToGrid; }
			set { snapObjectsToGrid = value; }
		}
		public int ZoomFactor {
			get { return zoomFactor; }
			set {
				zoomFactor = value;
				scaleFactor = (float)zoomFactor / 100;
				OnZoomChanged();
			}
		}



		private string tests;
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public List<Structure> DiagramItems {
			get { return structures; }
			set { structures = value; }
		}


		public DiagramContainer()
		{
			zoomFactor = 100;
			structures = new List<Structure>();
			selectedItems = new List<IDrawing>();
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw |
			ControlStyles.DoubleBuffer | ControlStyles.UserPaint, true);
			hScrollBar = new HScrollBar();
			hScrollBar.Dock = DockStyle.Bottom;
			vScrollBar = new VScrollBar();
			vScrollBar.Dock = DockStyle.Right;
			vScrollBar.Scroll += new ScrollEventHandler(vScrollBar1_Scroll);
			hScrollBar.Scroll += new ScrollEventHandler(hScrollBar1_Scroll);
			this.Resize += new EventHandler(DiagramContainer_Resize);
			this.Controls.Add(hScrollBar);
			this.Controls.Add(vScrollBar);
			nodeEditor = new NodeTextEditor(this);
			containerDrawer = new ContainerDrawer(this);
			this.Invalidate();
		}

 

		protected override void InitLayout()
		{
			base.InitLayout();
			this.drawableHeight = this.Height;
			this.drawableWidth = this.Width;
			SetScrollBarValues();
		}

		void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
		{
			this.Invalidate();
		}

		void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
		{
			this.Invalidate();
		}

		public void AddStructure(Structure structure)
		{
			StructureDrawing structureDrawing = new StructureDrawing(structure);
			structureDrawing.Invalidated = true;
			containerDrawer.ModelToDrawer[structure] = structureDrawing;
			containerDrawer.DrawerToModel[structureDrawing] = structure;
			
			BorderDrawingDecorator selectableDrawing = new BorderDrawingDecorator(structureDrawing);
			containerDrawer.Drawings.Add(selectableDrawing);
			containerDrawer.SelectedDrawings.Add(selectableDrawing);
			structures.Add(structure);
			structureDrawing.AutoSizeContent();
		}
		
		public void AddLink(Structure source, Structure destination)
		{
			this.ContainerDrawer.LinkOrchestrator.AddLink(source, destination);
		}

		void linkManager_LinkDirectionChangedEvent(object sender, LinkDirectionChangedArg e)
		{
			
		}


		Random rd = new Random();
		protected override void OnPaint(PaintEventArgs e)
		{
			try {
				RectangleF rectangle = e.Graphics.ClipBounds;
				#if DEBUG
			e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(rd.Next(255), rd.Next(255), rd.Next(255))), new Rectangle((int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height));
				#endif
				containerDrawer.Draw(scaleFactor, e.Graphics);
			} catch (Exception ex) {
				
			}
		}

		protected void OnZoomChanged()
		{
			SetScrollBarValues();
			if (ZoomFactorChanged != null)
				this.ZoomFactorChanged(this, new ZoomFactorChangedArg(zoomFactor));
			//this.textEditor.Hide();
			invalidatedRegion = new Rectangle(this.Location, this.Size);
		}

		protected void DrawGrid(Graphics g)
		{
			Pen p = new Pen(Color.FromArgb(100, 200, 200, 200));
			float step = gridSize;
			if (step < 1)
				step = 1;
			for (float i = 0; i < this.drawableWidth; i += step) {
				g.DrawLine(p, i, 0, i, this.drawableHeight);
			}
			for (float j = 0; j < this.drawableHeight; j += step) {
				g.DrawLine(p, 0, j, this.drawableWidth, j);
			}
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			for (int i = containerDrawer.Drawings.Count - 1; i >= 0; i--) {
				IBoundedShape drawer = containerDrawer.Drawings[i];
				if (drawer is Structure) {
					StructureDrawing structureDrawing = (StructureDrawing)drawer;
					Rectangle r = new Rectangle((int)(structureDrawing.Location.X * scaleFactor), 
						              (int)(structureDrawing.Location.Y * scaleFactor), 
						              (int)(structureDrawing.Size.Width * scaleFactor), 
						              (int)(structureDrawing.Size.Height * scaleFactor));
					Point p = e.Location;
					p.Offset(hScrollBar.Value, vScrollBar.Value);
					if (r.Contains(p)) {
						Point pInside = new Point(e.Location.X, e.Location.Y);
						pInside.Offset(hScrollBar.Value, vScrollBar.Value);
						int x = (int)(pInside.X - structureDrawing.Location.X * scaleFactor);
						int y = (int)(pInside.Y - structureDrawing.Location.Y * scaleFactor);
						structureDrawing.OnClick(new MouseEventArgs(e.Button, e.Clicks, x, y, e.Delta), scaleFactor);
						if (this.DiagramItemClick != null)
							DiagramItemClick(structureDrawing, e);
						return;
					}
				}

			}
		}

		//		protected override void OnMouseDoubleClick(MouseEventArgs e)
		//		{
		//			for (int i = DrawableItems.Count - 1; i >= 0; i--) {
		//				Drawing drawer = containerDrawer.Drawings[i];
		//				if (drawer is Drawing) {
		//					StructureDrawing structureDrawing = (StructureDrawing)drawer;
		//					Rectangle r = new Rectangle(structureDrawing.Location.X, structureDrawing.Location.Y, structureDrawing.Size.Width, structureDrawing.Size.Height);
		//					Point p = new Point((int)(e.Location.X / scaleFactor), (int)(e.Location.Y / scaleFactor));
		//					p.Offset(hScrollBar.Value, vScrollBar.Value);
		//					if (r.Contains(p)) {
		//						structureDrawing.OnDblClick(e, scaleFactor);
		//						return;
		//
		//
		//						//nodeEditor.ShowOnNode(diin.Nod);
		//					}
		//				}
		//			}
		//		}


		
		private bool checkEditorInsideMouseDown(MouseEventArgs e, double scaleFactor)
		{
			if (nodeEditor.Visible) {
				Point p = new Point((int)(e.Location.X / scaleFactor), (int)(e.Location.Y / scaleFactor));
				p.Offset(hScrollBar.Value, vScrollBar.Value);
				if (!nodeEditor.Bounds.Contains(p)) {
					nodeEditor.Visible = false;
					nodeEditor.currentNode.StructureDrawing.Invalidated = true;
					return true;
				}
				nodeEditor.onMouseDown(nodeEditor, e, scaleFactor);
			}
			return false;
		}
		

 

		private void textEditor_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter) {
				ValidateNodeEditor();
				//textEditor.Hide();
			}
		}

		private void textEditor_LostFocus(object sender, EventArgs e)
		{


		}

		private void InvalidateTransform(Region region)
		{
			Matrix mx = new Matrix();
			mx.Scale(scaleFactor, scaleFactor);
			mx.Translate(-hScrollBar.Value, -vScrollBar.Value);
			region.Transform(mx);
			Invalidate(region);
		}


		private void ValidateNodeEditor()
		{
			
			/*
            if (textEditor.Node != null)
                textEditor.Node.Text = textEditor.Text;
			 */
		}

		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// DiagramContainer
			// 
			this.Size = new System.Drawing.Size(1000, 800);
			this.Resize += new System.EventHandler(this.DiagramContainer_Resize);
			this.ResumeLayout(false);

		}

		private void DiagramContainer_Resize(object sender, EventArgs e)
		{
			SetScrollBarValues();
		}

		private void SetScrollBarValues()
		{

			if (scaleFactor * drawableWidth > Width) {
				hScrollBar.Minimum = 0;
				hScrollBar.Maximum = (int)(scaleFactor * drawableWidth - Width);
				hScrollBar.SmallChange = (int)(scaleFactor * drawableWidth / 20);
				hScrollBar.LargeChange = (int)(scaleFactor * drawableWidth / 10);
				if (this.vScrollBar.Visible) { //step 2
					this.hScrollBar.Maximum += this.vScrollBar.Width;
				}
				hScrollBar.Maximum += this.hScrollBar.LargeChange;//step 3
				hScrollBar.Visible = true;
			} else {
				drawableWidth = Width;
				hScrollBar.Visible = false;
				hScrollBar.Value = 0;
			}

			if (scaleFactor * drawableHeight > Height) {
				vScrollBar.Minimum = 0;
				vScrollBar.Maximum = (int)((scaleFactor * drawableHeight - Height));
				vScrollBar.SmallChange = (int)(scaleFactor * drawableHeight / 20);
				vScrollBar.LargeChange = (int)(scaleFactor * drawableHeight / 10);
				if (this.hScrollBar.Visible) { //step 2
					this.vScrollBar.Maximum += this.hScrollBar.Height;
				}
				vScrollBar.Maximum += this.vScrollBar.LargeChange;//step 3
				vScrollBar.Visible = true;
			} else {
				drawableHeight = Height;
				vScrollBar.Visible = false;
				vScrollBar.Maximum = 0;
			}



		}
		
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			const int WM_KEYDOWN = 0x100;
			const int WM_SYSKEYDOWN = 0x104;
			
			if ((msg.Msg == WM_KEYDOWN) || (msg.Msg == WM_SYSKEYDOWN)) {
				switch (keyData) {
					case Keys.Down:
						this.Parent.Text = "Down Arrow Captured";
						break;
						
					case Keys.Up:
						this.Parent.Text = "Up Arrow Captured";
						break;
						
					case Keys.Tab:
						this.Parent.Text = "Tab Key Captured";
						break;
						
					case Keys.Control | Keys.M:
						this.Parent.Text = "<CTRL> + M Captured";
						break;
						
					case Keys.Alt | Keys.Z:
						this.Parent.Text = "<ALT> + Z Captured";
						break;
				}
			}
			if (nodeEditor.Visible)
				nodeEditor.OnKeyDown(new KeyEventArgs(keyData));
			return base.ProcessCmdKey(ref msg, keyData);
		}
		
	}
}
