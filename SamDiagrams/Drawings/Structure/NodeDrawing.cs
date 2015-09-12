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
using SamDiagrams.Drawers.Links;
using SamDiagrams.Drawings.Geometry;
using SamDiagrams.Drawings.Link;
using SamDiagrams.Model;

namespace SamDiagrams.Drawings
{
	/// <summary>
	/// Description of NodeDrawing.
	/// </summary>
	public class NodeDrawing : BaseDrawing, ILinkableDrawing
	{
		private const int EXPANDER_SIZE = 10;
		private const int IMAGE_SPACE = 14;
		private const int IMAGE_PADDING = 4;
		private static SolidBrush TITLE_BRUSH = new SolidBrush(Color.Black);
		
		private int drawingRow;
		private int drawingPadding;
		private bool visible;
		private Font rowFont;
		StructureDrawing structureDrawing;
		readonly Node node;
		List<LinkDrawing> drawingLinks;

		public NodeDrawing(StructureDrawing structureDrawing, Node node)
		{
			this.node = node;
			this.structureDrawing = structureDrawing;
			this.rowFont = new Font(this.structureDrawing.Structure.DiagramContainer.Font.FontFamily, 9.0F);
			drawingLinks = new List<LinkDrawing>();
		}

		public int DrawingPadding {
			get {
				return drawingPadding;
			}
			set {
				drawingPadding = value;
			}
		}
		
		public int DrawingRow {
			get {
				return drawingRow;
			}
			set {
				drawingRow = value;
			}
		}

		public bool Visible {
			get {
				return visible;
			}
			set {
				visible = value;
			}
		}

		public Item Item {
			get {
				return this.node;
			}
		}

		public Rectangle InvalidatedRegion {
			get {
				MergeableRectangle rectangle = new MergeableRectangle(this.Bounds);
				foreach (LinkDrawing link in this.DrawingLinks) {
					rectangle.Merge(link.Bounds);
				}
				return rectangle.Bounds;
			}
		}
		
		public List<LinkDrawing> DrawingLinks {
			get {
				return drawingLinks;
			}
		}
				
		public List<IDrawing> Components {
			get {
				return new List<IDrawing>();
			}
		}
		
		public LinkAttachMode LinkAttachMode {
			get {
				return LinkAttachMode.LEFT_RIGHT;
			}
		}
		
		public override void Draw(Graphics graphics)
		{
//			if (!visible) {
//				return;
//			}
			
			int cY = this.location.Y;
			int cX = this.location.X;
			
			if (!node.IsLeaf) {
				graphics.DrawRectangle(Pens.Black, cX, cY, EXPANDER_SIZE, EXPANDER_SIZE);
				graphics.DrawLine(Pens.Black, cX, cY + EXPANDER_SIZE / 2, cX + EXPANDER_SIZE, cY + EXPANDER_SIZE / 2);
				if (!node.IsExpanded) {
					graphics.DrawLine(Pens.Black, cX + EXPANDER_SIZE / 2, cY, cX + EXPANDER_SIZE / 2, cY + +EXPANDER_SIZE);
				}
			}

			int space = 0;

			for (int i = 0; i < node.Images.Count; i++) {
				graphics.DrawImage(node.Images[i], cX + EXPANDER_SIZE + space, cY + (int)(IMAGE_PADDING / 2),
					structureDrawing.RowHeight - IMAGE_PADDING, structureDrawing.RowHeight - IMAGE_PADDING);
				space += IMAGE_SPACE - IMAGE_PADDING / 2;
			}
			
			graphics.DrawString(node.Text, rowFont, TITLE_BRUSH, new PointF(cX + space + EXPANDER_SIZE, cY));
		}
	}
}
