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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SamDiagrams.Drawers.Links;
using SamDiagrams.Drawings.Geometry;
using SamDiagrams.Events;

namespace SamDiagrams.Drawings
{
	/// <summary>
	/// Default Drawing implementation associated with a Structure.
	/// The drawing represents a tree of nodes.
	/// </summary>
	public class StructureDrawing : DiagramComponent, IClickable
	{
		
		public event BeforeNodeExpandOrCollapseHandler BeforeNodeExpandOrCollapse;
		public event DrawingResizedHandler DrawingResized;
		
		private static SolidBrush CONTOUR_BRUSH = new SolidBrush(Color.SteelBlue);
		private const int CORNER_RADIUS = 10;
		private const int DEFAULT_WIDTH = 100;
		private const int LEFT_PADDING = 4;
		private const int TITLE_OFFSET = 5;
		private const int TAB_NOD_SIZE = 10;
		private const int EXPANDER_SIZE = 10;
		private const int IMAGE_SPACE = 14;
		private const int IMAGE_PADDING = 4;
		private const int IMAGE_SIZE = 16;
		private static Color BORDER_COLOR = Color.SteelBlue;
		private static SolidBrush TITLE_BRUSH = new SolidBrush(Color.Black);
		
		private Structure structure;
		private Color color = Color.LightSteelBlue;
		

		internal int rowHeight = 10;
		internal Font rowFont;
		private Font titleFont;
		private int titleHeight = 26;
		private Pen conturPen;
		private int crtDrawingRow = 0;
		private int nodesDrawingTopPosition;
		private int titleWidth;
		private int nodDblX, nodDblY;
		private int nrOfDisplayedRows = 0;
		private int crtNodCheck = 0;


		public StructureDrawing(Structure structure):base(structure)
		{
			this.structure = structure;
			this.invalidated = true;
			conturPen = new Pen(CONTOUR_BRUSH, 1F);
			this.selected = false;
			this.rowFont = new Font(this.structure.DiagramContainer.Font.FontFamily, 9.0F);
			this.titleFont = new Font(this.structure.DiagramContainer.Font.FontFamily, 9.0F, FontStyle.Bold);
			rowHeight = CalculateRowHeight();
			this.size.Width = DEFAULT_WIDTH;
			AutoSizeContent();
		}
	
		public Structure Structure {
			get {
				return structure;
			}
			set {
				structure = value;
			}
		}

		public override Rectangle InvalidatedRegion {
			get {
				MergableRectangle rectangle = new MergableRectangle(this.Bounds);
				foreach (LinkDrawing link in this.DrawingLinks) {
					rectangle.Merge(link.Bounds);
				}
				return rectangle.Bounds;
			}
		}
		
	
		public override void Draw(Graphics graphics)
		{
			GraphicsPath path = new GraphicsPath();

			//header
			path.AddArc(location.X, location.Y, 
				CORNER_RADIUS, CORNER_RADIUS, 180, 90);
			path.AddArc(location.X + size.Width - CORNER_RADIUS, location.Y, 
				CORNER_RADIUS, CORNER_RADIUS, 270, 90);
			path.AddLine(location.X + size.Width, location.Y + (int)(CORNER_RADIUS / 2), 
				location.X + size.Width, location.Y + (int)(CORNER_RADIUS / 2) + titleHeight + TITLE_OFFSET);
			path.AddLine(location.X, location.Y + (int)(CORNER_RADIUS / 2) + titleHeight + TITLE_OFFSET, 
				location.X, location.Y + (int)(CORNER_RADIUS / 2));
			path.CloseAllFigures();

			
			//title background
			graphics.FillPath(new SolidBrush(color), path);
			graphics.DrawPath(conturPen, path);

			//title
			int titlePosX = location.X + (size.Width - titleWidth) / 2;
			graphics.DrawString(this.Structure.Title, titleFont, TITLE_BRUSH, new PointF(titlePosX, location.Y));
			if (Structure.TitleImage != null)
				graphics.DrawImage(Structure.TitleImage, 
					new Rectangle(this.location.X + IMAGE_PADDING, this.location.Y + IMAGE_PADDING, IMAGE_SIZE, IMAGE_SIZE));


			int xOffset = this.location.X + LEFT_PADDING;
			nodesDrawingTopPosition = location.Y + titleHeight + (int)(Math.Ceiling((double)CORNER_RADIUS / 2));
			int mainHeight = rowHeight * nrOfDisplayedRows + TITLE_OFFSET;
			if (!Structure.DiagramContainer.AutoSizeItem) {
				mainHeight = this.size.Height - TITLE_OFFSET - rowHeight - CORNER_RADIUS;
			}
			Rectangle lineMainR = new Rectangle(location.X, nodesDrawingTopPosition, size.Width, mainHeight);
			graphics.FillRectangle(Brushes.White, lineMainR);
			graphics.DrawRectangle(conturPen, lineMainR);

			crtDrawingRow = 0;
			foreach (Node node in Structure.Nodes) {
				RecursiveDraw(graphics, rowFont, node, xOffset, nodesDrawingTopPosition + TITLE_OFFSET);
				crtDrawingRow++;
			}
			
			path = new GraphicsPath();
			path.AddArc(location.X, lineMainR.Y + lineMainR.Height - (int)(CORNER_RADIUS / 2), 
				CORNER_RADIUS, CORNER_RADIUS, 180, -90);
			path.AddArc(location.X + size.Width - CORNER_RADIUS, lineMainR.Y + lineMainR.Height - (int)(CORNER_RADIUS / 2),
				CORNER_RADIUS, CORNER_RADIUS, 90, -90);
			path.CloseAllFigures();
			graphics.FillPath(new SolidBrush(color), path);
			graphics.DrawPath(conturPen, path);
			
		}
		
		private int CalculateDisplayedRows(Node nod, bool parentIsExpanded)
		{
			int nr = 0;
			if (parentIsExpanded) {
				for (int i = 0; i < nod.Count; i++) {
					nr += CalculateDisplayedRows(nod[i], parentIsExpanded && nod[i].IsExpanded);
				}
			}
			nr++;
			return nr;
		}
		
		private int CalculateRowHeight()
		{
			rowFont = new Font(this.rowFont.FontFamily, (float)((rowFont.Size - 0)));
			Size sT = TextRenderer.MeasureText("TEST", rowFont);
			return (int)(sT.Height);
		}
		
		internal void AutoSizeContent()
		{
			invalidated = true;
			int nr = 0;
			foreach (Node node in structure.Nodes) {
				nr += CalculateDisplayedRows(node, node.IsExpanded);
			}
			if (structure.DiagramContainer.AutoSizeItem)
				nrOfDisplayedRows = nr;
			rowHeight = CalculateRowHeight();
			titleFont = new Font(this.titleFont.FontFamily, (float)(titleFont.Size), FontStyle.Bold);
			rowFont = new Font(this.rowFont.FontFamily, rowFont.Size);
			Size sT = TextRenderer.MeasureText(structure.Title, titleFont);
			titleHeight = sT.Height;
			titleWidth = sT.Width;
			size.Height = rowHeight * nr + TITLE_OFFSET + titleHeight + CORNER_RADIUS;

		}
		
		Random r = new Random(255);
		private void RecursiveDraw(Graphics g, Font  nodeFont, Node nod, int xOffset, int yOffset)
		{

			int cY = yOffset + rowHeight * crtDrawingRow;
			int cX = xOffset;

			if (!Structure.DiagramContainer.AutoSizeItem && cY >
			    location.Y + size.Height - CORNER_RADIUS - rowHeight)
				return;

			if (!nod.IsLeaf) {
				g.DrawRectangle(Pens.Black, cX, cY, EXPANDER_SIZE, EXPANDER_SIZE);
				g.DrawLine(Pens.Black, cX, cY + EXPANDER_SIZE / 2, cX + EXPANDER_SIZE, cY + EXPANDER_SIZE / 2);
				if (!nod.IsExpanded) {
					g.DrawLine(Pens.Black, cX + EXPANDER_SIZE / 2, cY, cX + EXPANDER_SIZE / 2, cY + +EXPANDER_SIZE);
				}
			}

			int space = 0;

			for (int i = 0; i < nod.Images.Count; i++) {
				g.DrawImage(nod.Images[i], cX + EXPANDER_SIZE + space, cY + (int)(IMAGE_PADDING / 2),
					rowHeight - IMAGE_PADDING, rowHeight - IMAGE_PADDING);
				space += IMAGE_SPACE - IMAGE_PADDING / 2;
			}
			
			g.DrawString(nod.Text, nodeFont, TITLE_BRUSH, new PointF(cX + space + EXPANDER_SIZE, cY));
			if ((!nod.IsLeaf) && nod.IsExpanded) {
				xOffset += TAB_NOD_SIZE;
				for (int i = 0; i < nod.Count; i++) {
					crtDrawingRow++;
					RecursiveDraw(g, nodeFont, nod[i], xOffset, yOffset);
				}
			}
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
			for (int i = 0; i < nod.Count; i++) {
				Node ndr = null;
				if (parentIsExpanded)
					crtNodCheck++;
				ndr = GetNodAtXYRec(nod[i], x, y, mouseX, mouseY, parentIsExpanded && nod[i].IsExpanded);
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
					AutoSizeContent();
					if (DrawingResized != null) {
						DrawingResized(this,
							new DrawingResizedEventArgs(this, 
								size.Height - previousHeight, size.Width - previousWidth));
					}
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
				
				foreach (Node childNode in node) {
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
