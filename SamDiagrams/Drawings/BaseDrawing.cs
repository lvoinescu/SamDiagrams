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

namespace SamDiagrams.Drawings
{
	/// <summary>
	/// Description of BaseDrawing.
	/// </summary>
	public abstract class BaseDrawing : IBoundedShape
	{
		
		protected Size size;
		protected Point location;
		protected bool invalidated;
		
		public BaseDrawing()
		{
		}

		#region Drawing implementation

		public abstract void Draw(Graphics graphics);
		

		public Point Location {
			get {
				return this.location;
			}
			set {
				this.location = value;
			}
		}

		public Size Size {
			get {
				return this.size;
			}
		}

		public Rectangle Bounds {
			get {
				return new Rectangle(this.location, this.size);
			}
		}

		public bool Invalidated {
			get {
				return this.invalidated;
			}
			set {
				this.invalidated = value;
			}
		}

		#endregion
	}
}
