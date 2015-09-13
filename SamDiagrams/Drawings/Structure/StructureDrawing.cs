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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SamDiagrams.Drawers.Links;
using SamDiagrams.Drawings.Geometry;
using SamDiagrams.Events;

namespace SamDiagrams.Drawings
{
	/// <summary>
	/// Default Drawing implementation associated with a Structure.
	/// The drawing represents a tree of nodes.
	/// </summary>
	public class StructureDrawing : DiagramComponent, IClickable, IResizable
	{
		
		public event BeforeNodeExpandOrCollapseHandler BeforeNodeExpandOrCollapse;
		public event DrawingResizedHandler DrawingResized;
		
		public const int LEFT_PADDING = 4;
		public const int IMAGE_SPACE = 14;
		public const int IMAGE_PADDING = 4;
		public const int IMAGE_SIZE = 16;
		private const int TITLE_OFFSET = 5;
		private const int TAB_NOD_SIZE = 10;
		private const int EXPANDER_SIZE = 10;
		private const int CORNER_RADIUS = 10;
		private const int DEFAULT_WIDTH = 100;
		private static Color BORDER_COLOR = Color.SteelBlue;
		private static SolidBrush CONTOUR_BRUSH = new SolidBrush(Color.SteelBlue);
		private static SolidBrush TITLE_BRUSH = new SolidBrush(Color.Black);
		
		private Structure structure;

		internal int rowHeight = 10;
		internal Font rowFont;
		private Font titleFont;
		private int titleHeight = 26;
		private Pen conturPen;
		private int crtDrawingRow = 0;
		private int nodesDrawingTopPosition;
		private int nodesInnerDrawingTopPosition;
		private int titleWidth;
		private int nodDblX, nodDblY;
		private int nrOfDisplayedRows = 0;
		private int crtNodCheck = 0;


		public StructureDrawing(Structure structure, Color color)
			: this(structure)
		{
			this.color = color;
		}
		
		public StructureDrawing(Structure structure)
			: base(structure)
		{
			this.structure = structure;
			CreateNodeDrawings();
			this.structure.NodesChanged += new ListChangedEventHandler(OnListChanged);
			this.invalidated = true;
			conturPen = new Pen(CONTOUR_BRUSH, 1F);
			this.selected = false;
			this.rowFont = new Font(this.structure.DiagramContainer.Font.FontFamily, 9.0F);
			this.titleFont = new Font(this.structure.DiagramContainer.Font.FontFamily, 9.0F, FontStyle.Bold);
			rowHeight = ComputeRowHeight();
			this.size.Width = DEFAULT_WIDTH;
			AutoSizeContent();
		}
	
		public int RowHeight {
			get {
				return rowHeight;
			}
		}
		
		public Structure Structure {
			get {
				return structure;
			}
			set {
				structure = value;
			}
		}

		public override Point Location {
			get {
				return this.location;
			}
			set {
				this.location = value;
				ComputeDrawingRows();
			}
		}
		
		public override bool Invalidated {
			get { 
				return this.invalidated;
			}
			set { 
				this.invalidated = value;
			}
		}
		
		void CreateNodeDrawings()
		{
			structure.Iterate(new Action<Node>(CreateNodeDrawings));
			ComputeDrawingRows();
		}

		void CreateNodeDrawings(Node node)
		{
			NodeDrawing nodeDrawing = new NodeDrawing(this, node);
			node.Drawing = nodeDrawing;
			Components.Add(nodeDrawing);
		}
		
		void OnListChanged(object sender, ListChangedEventArgs e)
		{
			ComputeDrawingRows();
		}
		
		public override Rectangle InvalidatedRegion {
			get {
				MergeableRectangle rectangle = new MergeableRectangle(this.Bounds);
				foreach (LinkDrawing link in this.DrawingLinks) {
					rectangle.Merge(link.Bounds);
				}
				
				foreach (IDrawing subcomponent in Components) {
					rectangle.Merge(subcomponent.InvalidatedRegion);
				}
				return rectangle.Bounds;
			}
		}
		
		public Point NodesDrawingStart {
			get {
				return new Point(this.location.X, nodesDrawingTopPosition + TITLE_OFFSET);
			}
		}
		
		public override void Draw(Graphics graphics)
		{
			GraphicsPath path = new GraphicsPath();

			path.AddArc(location.X, location.Y, 
				CORNER_RADIUS, CORNER_RADIUS, 180, 90);
			path.AddArc(location.X + size.Width - CORNER_RADIUS, location.Y, 
				CORNER_RADIUS, CORNER_RADIUS, 270, 90);
			path.AddLine(location.X + size.Width, location.Y + (int)(CORNER_RADIUS / 2), 
				location.X + size.Width, location.Y + (int)(CORNER_RADIUS / 2) + titleHeight + TITLE_OFFSET);
			path.AddLine(location.X, location.Y + (int)(CORNER_RADIUS / 2) + titleHeight + TITLE_OFFSET, 
				location.X, location.Y + (int)(CORNER_RADIUS / 2));
			path.CloseAllFigures();

			
			graphics.FillPath(new SolidBrush(color), path);
			graphics.DrawPath(conturPen, path);

			int titlePosX = location.X + (size.Width - titleWidth) / 2;
			graphics.DrawString(this.Structure.Title, titleFont, TITLE_BRUSH, new PointF(titlePosX, location.Y));
			if (Structure.TitleImage != null)
				graphics.DrawImage(Structure.TitleImage, 
					new Rectangle(this.location.X + IMAGE_PADDING, this.location.Y + IMAGE_PADDING, 
						IMAGE_SIZE, IMAGE_SIZE));


			int xOffset = this.location.X + LEFT_PADDING;
			nodesDrawingTopPosition = location.Y + titleHeight + (int)(Math.Ceiling((double)CORNER_RADIUS / 2));

			int mainHeight = rowHeight * nrOfDisplayedRows + TITLE_OFFSET;
			if (!Structure.DiagramContainer.AutoSizeItem) {
				mainHeight = this.size.Height - TITLE_OFFSET - rowHeight - CORNER_RADIUS;
			}
			Rectangle lineMainR = new Rectangle(location.X, nodesDrawingTopPosition, size.Width, mainHeight);
			graphics.FillRectangle(Brushes.White, lineMainR);
			graphics.DrawRectangle(conturPen, lineMainR);

			path = new GraphicsPath();
			
			path.AddArc(location.X, lineMainR.Y + lineMainR.Height - (int)(CORNER_RADIUS / 2),
				CORNER_RADIUS, CORNER_RADIUS, 180, -90);
			path.AddArc(location.X + size.Width - CORNER_RADIUS, 
				lineMainR.Y + lineMainR.Height - (int)(CORNER_RADIUS / 2),
				CORNER_RADIUS, CORNER_RADIUS, 90, -90);
			
			path.CloseAllFigures();
			graphics.FillPath(new SolidBrush(color), path);
			graphics.DrawPath(conturPen, path);
			
			DrawNodes(graphics);
			crtDrawingRow = 0;
			
		}

		private int ComputeDisplayedRows(Node nod, bool parentIsExpanded)
		{
			int nr = 0;
			if (parentIsExpanded) {
				for (int i = 0; i < nod.Nodes.Count; i++) {
					nr += ComputeDisplayedRows(nod.Nodes[i], parentIsExpanded && nod.Nodes[i].IsExpanded);
				}
			}
			nr++;
			return nr;
		}
		
		private int ComputeRowHeight()
		{
			rowFont = new Font(this.rowFont.FontFamily, (float)((rowFont.Size - 0)));
			Size sT = TextRenderer.MeasureText("TEST", rowFont);
			return sT.Height;
		}
		
		internal void AutoSizeContent()
		{
			int nr = 0;
			foreach (Node node in structure.Nodes) {
				nr += ComputeDisplayedRows(node, node.IsExpanded);
			}
			if (structure.DiagramContainer.AutoSizeItem) {
				nrOfDisplayedRows = nr;
			}
			rowHeight = ComputeRowHeight();
			titleFont = new Font(this.titleFont.FontFamily, (float)(titleFont.Size), FontStyle.Bold);
			rowFont = new Font(this.rowFont.FontFamily, rowFont.Size);
			
			Size sT = TextRenderer.MeasureText(structure.Title, titleFont);
			titleHeight = sT.Height;
			titleWidth = sT.Width;
			size.Height = rowHeight * nr + TITLE_OFFSET + titleHeight + CORNER_RADIUS;
			nodesInnerDrawingTopPosition = titleHeight + (int)(Math.Ceiling((double)CORNER_RADIUS / 2)) + TITLE_OFFSET;
			ComputeDrawingRows();
		}
		
		private void DrawNodes(Graphics graphics)
		{
			foreach (Node node in structure.Nodes) {
				RecursiveDrawNodeDrawings(node, graphics);
			}
		}
		
		private void RecursiveDrawNodeDrawings(Node node, Graphics graphics)
		{
			node.Drawing.Draw(graphics);
			if ((!node.IsLeaf) && node.IsExpanded) {
				for (int i = 0; i < node.Nodes.Count; i++) {
					RecursiveDrawNodeDrawings(node.Nodes[i], graphics);
				}
			}
		}
		
		private void ComputeDrawingRows()
		{
			crtDrawingRow = 0;
			structure.Iterate(new Action<Node>(ComputeNodeDrawingPosition));
		}

		private void ComputeNodeDrawingPosition(Node nod)
		{
			int currentNodeInnerTop = nodesInnerDrawingTopPosition + rowHeight * crtDrawingRow;
			int currentNodeInnerLeft = nod.Level * TAB_NOD_SIZE + LEFT_PADDING;
			NodeDrawing nodDrawing = nod.Drawing as NodeDrawing;
			if (nod.Parent != null) {
				if (nod.Parent.IsExpanded && (nod.Parent.Drawing as NodeDrawing).Visible) {
					nodDrawing.InnerLocation = new Point(currentNodeInnerLeft, currentNodeInnerTop);
					crtDrawingRow++;
					nodDrawing.Visible = true;
				} else {
					nodDrawing.InnerLocation = (nod.Parent.Drawing as NodeDrawing).InnerLocation;
					nodDrawing.Visible = false;
				}
			} else {
				nodDrawing.Visible = true;
				nodDrawing.InnerLocation = new Point(currentNodeInnerLeft, currentNodeInnerTop);
				crtDrawingRow++;
			}
			
			nodDrawing.Location = new Point(location.X + nodDrawing.InnerLocation.X,
				location.Y + nodDrawing.InnerLocation.Y);
			nodDrawing.Size = new Size(this.size.Width - currentNodeInnerLeft, rowHeight);
			nodDrawing.DrawingPadding = currentNodeInnerLeft;
		}
		
		private Node GetNodAtXYRec(Node nod, int x, int y, int mouseX, int mouseY, bool parentIsExpanded)
		{
			int cY = y + rowHeight * crtNodCheck;
			int cX = x;
			float yT = cY;
			RectangleF r = new RectangleF(cX, yT, size.Width, rowHeight);
			if (r.Contains(mouseX, mouseY)) {
				nodDblX = location.X + x + EXPANDER_SIZE;
				nodDblY = location.Y + nodesDrawingTopPosition + crtNodCheck * rowHeight;
				return nod;
			}
			if (nod.IsExpanded) {
				x += TAB_NOD_SIZE;
			}
			for (int i = 0; i < nod.Nodes.Count; i++) {
				Node ndr = null;
				if (parentIsExpanded)
					crtNodCheck++;
				ndr = GetNodAtXYRec(nod.Nodes[i], x, y, mouseX, mouseY, parentIsExpanded && nod.Nodes[i].IsExpanded);
				if (ndr != null) {
					nodDblX = location.X + x + LEFT_PADDING;
					nodDblY = location.Y + nodesDrawingTopPosition + crtNodCheck * rowHeight;
					return ndr;
				}
			}
			return null;
		}
		
		internal void OnDblClick(MouseEventArgs e)
		{
			Point pInside = e.Location;
			pInside.Offset(structure.DiagramContainer.HScrollBar.Value, structure.DiagramContainer.VScrollBar.Value);
			int x = pInside.X - this.Location.X;
			int y = pInside.Y - this.Location.Y;
			StructureNodeInfo diin = this.GetNodAtXY(x, y);
			if (diin != null && diin.Nod.Editable) {
				structure.DiagramContainer.NodeEditor.ShowOnNode(diin);
			}
		}
		
		public void OnInsideClick(MouseEventArgs e)
		{
			int crtExpanderRow = 0;
			foreach (Node node in  structure.Nodes) {
				
				Point insideClickPoint = e.Location;
				Node nodeToToggle = RecursiveExpanderCheck(node, 0, ref crtExpanderRow, insideClickPoint, node.IsExpanded);
				
				if (nodeToToggle != null) {
					if (BeforeNodeExpandOrCollapse != null) {
						BeforeNodeExpandOrCollapse(this, new BeforeNodeExpandOrCollapseArg(nodeToToggle));
					}
					int previousHeight = size.Height;
					int previousWidth = size.Width;
					
					nodeToToggle.IsExpanded = !nodeToToggle.IsExpanded;
					Rectangle previousBounds = new Rectangle(location, size);
					AutoSizeContent();
					if (DrawingResized != null) {
						DrawingResized(this, new DrawingResizedEventArgs(this, previousBounds, Bounds));
					}
					ComputeDrawingRows();
					return;
				}
				crtExpanderRow++;
			}
		}


		/// <summary>
		/// Method that find the node for which the expander was triggered
		/// </summary>
		/// <param name="node"> the node to be checked</param>
		/// <param name="nodeLevel"> the level of the node</param>
		/// <param name="crtExpanderRow"> the row associated with the node</param>
		/// <param name="insideClickPoint">the location inside the structure where de mouse was clicked</param>
		/// <param name="parentIsExpanded">true if parent node is expanded, false otherwhise</param>
		/// <returns></returns>
		private Node RecursiveExpanderCheck(Node node, int nodeLevel, ref int crtExpanderRow,
			Point insideClickPoint, bool parentIsExpanded)
		{
			int cY = rowHeight * crtExpanderRow + nodesDrawingTopPosition - location.Y + TITLE_OFFSET;
			RectangleF rowRectangleToCheck = new RectangleF(LEFT_PADDING, cY, EXPANDER_SIZE, EXPANDER_SIZE);
			
			if (!node.IsLeaf) {
				
				if (node.Parent != null && node.Parent.IsExpanded) {
					rowRectangleToCheck.Offset(nodeLevel * TAB_NOD_SIZE, 0);
				}
				
				if (rowRectangleToCheck.Contains(insideClickPoint))
					return node;
				
				foreach (Node childNode in node.Nodes) {
					Node ndr = null;
					if (parentIsExpanded) {
						crtExpanderRow++;
					}
					ndr = RecursiveExpanderCheck(childNode, nodeLevel + 1, ref crtExpanderRow, 
						insideClickPoint, parentIsExpanded && childNode.IsExpanded);
					if (ndr != null)
						return ndr;
				}
			}
			return null;
		}

		
		public StructureNodeInfo GetNodAtXY(int x, int y)
		{
			crtNodCheck = 0;
			for (int i = 0; i < structure.Nodes.Count; i++) {
				Node nod = GetNodAtXYRec(structure.Nodes[i], LEFT_PADDING, nodesDrawingTopPosition, x, y, structure.Nodes[i].IsExpanded);
				if (nod != null) {
					return new StructureNodeInfo(nod, this, new Rectangle(nodDblX, nodDblY, 
						this.Location.X + this.size.Width - nodDblX, this.rowHeight));
				}
				crtNodCheck++;
			}
			return null;
		}
		
		public override string ToString()
		{
			return string.Format("[StructureDrawer Title={0}, Selected={1}, Location={2}, Size={3}]",
				structure.Title, selected, location, size);
		}

	}
}
