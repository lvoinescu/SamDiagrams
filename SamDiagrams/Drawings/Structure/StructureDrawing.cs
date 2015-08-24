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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SamDiagrams.Drawers.Links;
using SamDiagrams.Drawings.Geometry;
using SamDiagrams.Drawings.Link;
using SamDiagrams.Drawings.Selection;
using SamDiagrams.Model;

namespace SamDiagrams.Drawings
{
	/// <summary>
	/// Description of ComponentDrawer.
	/// </summary>
	public class StructureDrawing : BaseDrawing, ILinkableDrawing
	{
		
		public event BeforeNodeExpandOrCollapseHandler BeforeNodeExpandOrCollapse;
		
		private static SolidBrush CONTOUR_BRUSH = new SolidBrush(Color.SteelBlue);
		private const int SCALED_CORNER_RADIUS = 10;
		private const int DEFAULT_WIDTH = 100;
		private const int CORNER_RADIUS = 10;
		private const int LEFT_PADDING = 4;
		private const int TITLE_OFFSET = 5;
		private const int TAB_NOD_SIZE = 10;
		private const int EXPANDER_SIZE = 10;
		private static Color BORDER_COLOR = Color.SteelBlue;
		private static SolidBrush TITLE_BRUSH = new SolidBrush(Color.Black);
		
		private Structure structure;
		private Color color = Color.LightSteelBlue;
		

		private int rowScaledHeight = 10;
		private Font rowFont;
		private Font titleFont;
		private int titleHeight = 26;
		private Pen contur;
		private float scaleFactor = 1;
		private int crtDrawingRow = 0;
		private int nodsYOffset = 0;
		private int yScaledOffset;
		private int titleWidth;
		private int nodDblX, nodDblY;
		private int nrOfDisplayedRows = 0;
		private int crtNodCheck = 0;
		private bool selected;

		private int crtExpanderCheckRow = 0;
		internal Font titleScaledFont;
		internal Font rowScaledFont;
		private List<LinkDrawing> links;
		public Item Item {
			get {
				return this.structure;
			}
		}
		
		public StructureDrawing(Structure Structure)
		{
			this.Structure = Structure;
			this.invalidated = true;
			contur = new Pen(CONTOUR_BRUSH, 1F);
			this.selected = false;
			this.rowFont = new Font(this.Structure.DiagramContainer.Font.FontFamily, 9.0F);
			this.titleFont = new Font(this.Structure.DiagramContainer.Font.FontFamily, 9.0F, FontStyle.Bold);
			this.structure.DiagramContainer.ZoomFactorChanged += new ZoomFactorChangedHandler(OnZoomFactorChanged);
			rowScaledHeight = CalculateRowHeight();
			this.size.Width = DEFAULT_WIDTH;
			AutoSizeContent();
		}

		public List<LinkDrawing> DrawingLinks {
			get {
				return this.links;
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

		public Point InitialSelectedLocation {
			get {
				throw new NotImplementedException();
			}
			set {
				throw new NotImplementedException();
			}
		}
		
		public bool Selected {
			get {
				return selected;
			}
			set {
				selected = value;
			}
		}

			
		public override void Draw(Graphics graphics)
		{
			GraphicsPath path = new GraphicsPath();

			//header
			path.AddArc(location.X, location.Y, SCALED_CORNER_RADIUS, SCALED_CORNER_RADIUS, 180, 90);
			path.AddArc(location.X + size.Width - SCALED_CORNER_RADIUS, location.Y, SCALED_CORNER_RADIUS, SCALED_CORNER_RADIUS, 270, 90);
			path.AddLine(location.X + size.Width, location.Y + (int)(SCALED_CORNER_RADIUS / 2), location.X + size.Width, location.Y + (int)(SCALED_CORNER_RADIUS / 2) + titleHeight + TITLE_OFFSET);
			path.AddLine(location.X, location.Y + (int)(SCALED_CORNER_RADIUS / 2) + titleHeight + TITLE_OFFSET, location.X, location.Y + (int)(SCALED_CORNER_RADIUS / 2));
			path.CloseAllFigures();

			
			//title background
			graphics.FillPath(new SolidBrush(color), path);
			graphics.DrawPath(contur, path);

			//title
			int titlePosX = location.X + (size.Width - titleWidth) / 2;
			graphics.DrawString(this.Structure.Title, titleScaledFont, TITLE_BRUSH, new PointF(titlePosX, location.Y));
			if (Structure.TitleImage != null)
				graphics.DrawImage(Structure.TitleImage, new Rectangle(((int)((this.location.X + 4))), ((int)((this.location.Y + 4))), ((int)(16)), ((int)(16))));


			int xscaledOffset = (int)((this.location.X + LEFT_PADDING));
			yScaledOffset = location.Y + titleHeight + (int)(SCALED_CORNER_RADIUS / 2);
			int mainHeightScaled = rowScaledHeight * nrOfDisplayedRows + TITLE_OFFSET;
			if (!Structure.DiagramContainer.AutoSizeItem) {
				mainHeightScaled = this.size.Height - TITLE_OFFSET - rowScaledHeight - SCALED_CORNER_RADIUS;
			}
			Rectangle lineMainR = new Rectangle(location.X, yScaledOffset, size.Width, mainHeightScaled);
			graphics.FillRectangle(Brushes.White, lineMainR);
			graphics.DrawRectangle(contur, lineMainR);

			crtDrawingRow = 0;
			foreach (Node node in Structure.Nodes) {
				RecursiveDraw(graphics, rowScaledFont, scaleFactor, node, xscaledOffset, yScaledOffset + TITLE_OFFSET);
				crtDrawingRow++;
			}
			
			path = new GraphicsPath();
			path.AddArc(location.X, lineMainR.Y + lineMainR.Height - (int)(SCALED_CORNER_RADIUS / 2), SCALED_CORNER_RADIUS, SCALED_CORNER_RADIUS, 180, -90);
			path.AddArc(location.X + size.Width - SCALED_CORNER_RADIUS, lineMainR.Y + lineMainR.Height - (int)(SCALED_CORNER_RADIUS / 2), SCALED_CORNER_RADIUS, SCALED_CORNER_RADIUS, 90, -90);
			path.CloseAllFigures();
			graphics.FillPath(new SolidBrush(color), path);
			graphics.DrawPath(contur, path);
			
		}
		
		private void OnZoomFactorChanged(object sender, ZoomFactorChangedArg e)
		{
			scaleFactor = (float)e.ZoomLevel / 100;
			AutoSizeContent();
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
			Font rowScaledFont = new Font(this.rowFont.FontFamily, (float)((rowFont.Size - 0)));
			Size sT = TextRenderer.MeasureText("TEST", rowScaledFont);
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
			rowScaledHeight = CalculateRowHeight();
			titleScaledFont = new Font(this.titleFont.FontFamily, (float)(titleFont.Size), FontStyle.Bold);
			rowScaledFont = new Font(this.rowFont.FontFamily, rowFont.Size);
			Size sT = TextRenderer.MeasureText(structure.Title, titleFont);
			titleHeight = sT.Height;
			titleWidth = sT.Width;
			size.Height = rowScaledHeight * nr + TITLE_OFFSET + titleHeight + (int)(SCALED_CORNER_RADIUS);

		}
		
		Random r = new Random(255);
		private void RecursiveDraw(Graphics g, Font  titleScaledFont, float scaleFactor, Node nod, int xOffset, int yOffset)
		{

			int cY = yOffset + rowScaledHeight * crtDrawingRow;
			int cX = xOffset;
			int expanderScaledSize = EXPANDER_SIZE;


			if (!Structure.DiagramContainer.AutoSizeItem && cY >
			    location.Y + size.Height - SCALED_CORNER_RADIUS - rowScaledHeight)
				return;

			if (!nod.IsLeaf) {
				//g.FillRectangle(Brushes.AliceBlue, location.X,cY,this.size.Width, lineSpaceing);
				g.DrawRectangle(Pens.Black, cX, cY, expanderScaledSize, expanderScaledSize);
				g.DrawLine(Pens.Black, cX, cY + expanderScaledSize / 2, cX + expanderScaledSize, cY + expanderScaledSize / 2);
				if (!nod.IsExpanded) {
					g.DrawLine(Pens.Black, cX + expanderScaledSize / 2, cY, cX + expanderScaledSize / 2, cY + +expanderScaledSize);
				}
			}
			int imgSpace = 14;
			int space = 0;
			int imgPadding = 4;
			for (int i = 0; i < nod.Images.Count; i++) {
				g.DrawImage(nod.Images[i], cX + expanderScaledSize + space, cY + (int)(imgPadding / 2), rowScaledHeight - imgPadding, rowScaledHeight - imgPadding);
				space += (int)(imgSpace) - imgPadding / 2;
			}
			//g.FillRectangle(new SolidBrush(Color.FromArgb(r.Next(255),r.Next(255),r.Next(255))),
			//               new Rectangle( new Point(cX + space + expanderScaledSize, cY), new Size(100, rowScaledHeight)));
			g.DrawString(nod.Text, titleScaledFont, TITLE_BRUSH, new PointF(cX + space + expanderScaledSize, cY));
			if ((!nod.IsLeaf) && nod.IsExpanded) {
				xOffset += TAB_NOD_SIZE;
				for (int i = 0; i < nod.Count; i++) {
					crtDrawingRow++;
					RecursiveDraw(g, titleScaledFont, scaleFactor, nod[i], xOffset, yOffset);
				}
			}
		}
		
		private Node GetNodAtXYRec(Node nod, int x, int y, int mouseX, int mouseY, bool parentIsExpanded)
		{
			int cY = (int)(y + rowScaledHeight * crtNodCheck);
			int cX = (int)(x);
			float yT = cY;
			RectangleF r = new RectangleF(cX, yT, size.Width, rowScaledHeight);
			if (r.Contains(mouseX, mouseY)) {
				nodDblX = location.X + (int)(x) + (int)(EXPANDER_SIZE);
				nodDblY = location.Y + (int)(nodsYOffset) + (int)(crtNodCheck * rowScaledHeight);
				return nod;
			}
			if (nod.IsExpanded) {
				x += (int)(TAB_NOD_SIZE);
			}
			for (int i = 0; i < nod.Count; i++) {
				Node ndr = null;
				if (parentIsExpanded)
					crtNodCheck++;
				ndr = GetNodAtXYRec(nod[i], x, y, mouseX, mouseY, parentIsExpanded && nod[i].IsExpanded);
				if (ndr != null) {
					nodDblX = location.X + (int)(x) + (int)(LEFT_PADDING);
					nodDblY = location.Y + (int)(nodsYOffset) + (int)(crtNodCheck * rowScaledHeight);
					return ndr;
				}
			}
			return null;
		}
		
		internal void OnDblClick(MouseEventArgs e, float scaleFactor)
		{
			Point pInside = new Point((int)(e.Location.X / scaleFactor), (int)(e.Location.Y / scaleFactor));
			pInside.Offset(structure.DiagramContainer.HScrollBar.Value, structure.DiagramContainer.VScrollBar.Value);
			int x = (int)(pInside.X - this.Location.X);
			int y = (int)(pInside.Y - this.Location.Y);
			StructureNodeInfo diin = this.GetNodAtXY(x, y);
			if (diin != null && diin.Nod.Editable) {
				structure.DiagramContainer.NodeEditor.ShowOnNode(diin);
			}
		}
		
		
		public void OnClick(MouseEventArgs e, float scaleFactor)
		{
			crtExpanderCheckRow = 0;
			for (int i = 0; i < structure.Nodes.Count; i++) {
				Node nod = RecursiveExpanderCheck(structure.Nodes[i], (int)(LEFT_PADDING), nodsYOffset, scaleFactor, e.X, e.Y, structure.Nodes[i].IsExpanded);
				if (nod != null) {
					if (BeforeNodeExpandOrCollapse != null) {
						this.BeforeNodeExpandOrCollapse(this, new BeforeNodeExpandOrCollapseArg(nod));
					}
					nod.IsExpanded = !nod.IsExpanded;
					structure.DiagramContainer.Invalidate(new Rectangle(Location, size));
					return;
				}
				crtExpanderCheckRow++;
			}
		}
		
		
		private Node RecursiveExpanderCheck(Node nod, int x, int y, float scaleFactor, int mouseX, int mouseY, bool parentIsExpanded)
		{
			int cY = (int)(y + rowScaledHeight * crtExpanderCheckRow);
			int cX = (int)(x);
			const int expanderScaledSize = (int)(EXPANDER_SIZE);
			if (!nod.IsLeaf) {
				float yT = cY;
				RectangleF r = new RectangleF(cX, yT, expanderScaledSize, expanderScaledSize);
				if (r.Contains(mouseX, mouseY))
					return nod;
				if (nod.IsExpanded) {
					x += (int)(TAB_NOD_SIZE);
				}
				for (int i = 0; i < nod.Count; i++) {
					Node ndr = null;
					if (parentIsExpanded)
						crtExpanderCheckRow++;
					ndr = RecursiveExpanderCheck(nod[i], x, y, scaleFactor, mouseX, mouseY, parentIsExpanded && nod[i].IsExpanded);
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
				Node nod = GetNodAtXYRec(structure.Nodes[i], (int)(LEFT_PADDING), nodsYOffset, x, y, structure.Nodes[i].IsExpanded);
				if (nod != null) {
					return new StructureNodeInfo(nod, this, new Rectangle(nodDblX, nodDblY, this.Location.X + this.size.Width - nodDblX, this.rowScaledHeight));
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
