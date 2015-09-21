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

namespace SamDiagrams.Drawings.Geometry
{
	/// <summary>
	/// Line characterized by the ecuation:
	/// 
	/// d :   aX + bY + c =0;
	/// </summary>
	public class LineSegment
	{
		internal double a, b, c;
		Point p1;
		Point p2;

		public Point P1 {
			get {
				return p1;
			}
		}

		public Point P2 {
			get {
				return p2;
			}
		}
		
		public LineSegment(Point p1, Point p2)
		{
			this.p1 = p1;
			this.p2 = p2;
			double m = (double)(p2.Y - p1.Y) / (p2.X - p1.X);
			
			a = m;
			b = -1;
			c = p1.Y - m * p1.X;
		}
		
		internal double lineValue(Point p)
		{
			return a * p.X + b * p.Y + c;
		}
		
		public bool Intersects(LineSegment l2)
		{
			return lineValue(l2.p1) * lineValue(l2.p2) < 0 &&
			l2.lineValue(this.p1) * l2.lineValue(this.p2) < 0;
		}
	}
}
