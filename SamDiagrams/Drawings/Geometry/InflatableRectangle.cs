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

namespace SamDiagrams.Drawings.Geometry
{
	/// <summary>
	/// Represents an rectangle that can be "inflatable"
	/// </summary>
	public class InflatableRectangle
	{
		private Rectangle r1;
		
		public InflatableRectangle(Rectangle rectangle)
		{
			this.r1 = rectangle;
		}

		public Rectangle Bounds {
			get {
				return r1;
			}
		}
		
		public void Inflate(Rectangle r2)
		{
			if (r2.Width == 0 || r2.Height == 0)
				return;
			
			int maxX = Math.Max(r1.X + r1.Width, r2.X + r2.Width + 1);
			int maxY = Math.Max(r1.Y + r1.Height, r2.Y + r2.Height + 1);
			int minX = Math.Min(r1.X, r2.X);
			int minY = Math.Min(r1.Y, r2.Y);
			
			r1.Location = new Point(minX, minY);
			
			r1.Width = maxX - minX;
			r1.Height = maxY - minY;
		}
		
		public override string ToString()
		{
			return string.Format("[InflatableRectangle R1={0}]", r1);
		}

		
	}
}
