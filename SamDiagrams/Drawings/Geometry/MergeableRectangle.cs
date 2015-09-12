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
	/// Represents an rectangle that can be merged with a Rectangle.
	/// </summary>
	public class MergeableRectangle
	{
		private Rectangle rectangle;
		
		public MergeableRectangle(Rectangle rectangle)
		{
			this.rectangle = rectangle;
		}

		public Rectangle Bounds {
			get {
				return rectangle;
			}
		}
		
		public void Merge(Rectangle target)
		{
			if (target.Width == 0 || target.Height == 0)
				return;
			
			int maxX = Math.Max(rectangle.X + rectangle.Width, target.X + target.Width);
			int maxY = Math.Max(rectangle.Y + rectangle.Height, target.Y + target.Height);
			int minX = Math.Min(rectangle.X, target.X);
			int minY = Math.Min(rectangle.Y, target.Y);
			
			rectangle.Location = new Point(minX, minY);
			
			rectangle.Width = maxX - minX;
			rectangle.Height = maxY - minY;
		}
		
		public void Inflate(int ammount)
		{
			this.rectangle.Inflate(ammount, ammount);
		}
		
		public override string ToString()
		{
			return string.Format("[MergableRectangle R1={0}]", rectangle);
		}

		
	}
}
