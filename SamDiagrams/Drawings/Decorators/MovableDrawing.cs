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

namespace SamDiagrams.Drawings
{
	/// <summary>
	/// A decorator-wrapper that represents a drawing that can be moved.
	/// </summary>
	public class MovableDrawing : IDrawing
	{
		private IDrawing drawing;
		private Point initialLocation;

		public Point InitialLocation {
			get {
				return initialLocation;
			}
			set {
				initialLocation = value;
			}
		}

		public Color Color {
			get {
				return drawing.Color;
			}
			set {
				drawing.Color = value;
			}
		}
		public IDrawing Drawing {
			get {
				return drawing;
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

		public MovableDrawing(IDrawing drawing)
		{
			this.drawing = drawing;
		}

		public void Draw(System.Drawing.Graphics graphics)
		{
			drawing.Draw(graphics);
		}

		public Rectangle InvalidatedRegion {
			get {
				return drawing.InvalidatedRegion;
			}
		}

		public SamDiagrams.Model.Item Item {
			get {
				return drawing.Item;
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

		public bool Movable {
			get {
				return drawing.Movable;
			}
			set {
				drawing.Movable = value;
			}
		}


		public Point Location {
			get {
				return drawing.Location;
			}
			set {
				drawing.Location = value;
			}
		}

		public Size Size {
			get {
				return drawing.Size;
			}
		}

		public Rectangle Bounds {
			get {
				return drawing.Bounds;
			}
		}
	}
}
