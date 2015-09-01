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
using SamDiagrams.Drawings.Geometry;
using SamDiagrams.Model;

namespace SamDiagrams.Drawings.Selection
{
	/// <summary>
	/// A Decorator that handles drawing the selection border
	/// </summary>
	public class SelectableDrawing : IDrawing, IClickable
	{
		
		readonly ISelectable drawing;
		SelectionBorder selectionBorder;
		Point initialSelectedLocation;
		
		public IDrawing Drawing {
			get {
				return drawing;
			}
		}

		public Item Item {
			get {
				return this.drawing.Item;
			}
		}
		public Point Location {
			get {
				return selectionBorder.Bounds.Location;
			}
			set {
				Point p = new Point(value.X, value.Y);
				int dx = (selectionBorder.Bounds.Width - this.drawing.Bounds.Width) / 2;
				int dy = (selectionBorder.Bounds.Height - this.drawing.Bounds.Height) / 2;
				p.Offset(dx, dy);
				drawing.Location = p;
			}
		}
		
		public Size Size {
			get {
				return selectionBorder.Bounds.Size;
			}
		}
		
		public bool Invalidated {
			get {
				return drawing.Invalidated;
			}
			set {
				drawing.Invalidated = value;
			}
		}

		public Rectangle Bounds {
			get {
				return selectionBorder.Bounds;
			}
		}

		public bool Movable {
			get {
				return drawing.Movable;
			}
			set {
				drawing.Movable = value;
			}
		}
		
		public Rectangle InvalidatedRegion {
			get {
				InflatableRectangle rectangle = new InflatableRectangle(selectionBorder.Bounds);
				rectangle.Inflate(this.drawing.InvalidatedRegion);
				return rectangle.Bounds;
			}
		}
		
		public bool Selected {
			get {
				return this.drawing.Selected;
			}
			set {
				this.drawing.Selected = value;
			}
		}
		
		public Point InitialSelectedLocation {
			get {
				return initialSelectedLocation;
			}
			set {
				this.initialSelectedLocation = value;
			}
		}
		
		public SelectableDrawing(ISelectable drawing)
		{
			this.drawing = drawing;
			this.selectionBorder = new SelectionBorder(drawing);
		}
		
		public void Draw(Graphics graphics)
		{
			this.drawing.Draw(graphics);
			if (drawing.Selected) {
				this.selectionBorder.Draw(graphics);
			}
		}

		public void OnClick(System.Windows.Forms.MouseEventArgs e)
		{
			if(drawing is IClickable)
			{
				(drawing as IClickable).OnClick(e);
			}
		}
		
		public override String ToString()
		{
			return "SelectableDrawing; " + drawing.ToString();
		}
		
	}
}
