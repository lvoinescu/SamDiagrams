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
using SamDiagrams.Linking;

namespace SamDiagrams
{
	/// <summary>
	/// A simple class describing the end point of a link described
	/// by a cartesian pair (x, y).
	/// </summary>
	public class LinkPoint
	{
		private int x = 0;
		private int y = 0;
		private StructureLink link;
		
		public StructureLink Link {
			get { return link; }
			set { link = value; }
		}
		
		public int X {
			get { return x; }
			set { x = value; }
		}

		public int Y {
			get { return y; }
			set { y = value; }
		}

		public LinkPoint(StructureLink link)
		{
			this.link = link;
		}

		internal LinkPoint GetCounterPoint()
		{
			if (link.DestinationPoint == this)
				return link.SourcePoint;
			return link.DestinationPoint;
		}

		internal void SetCounterPoint(int x, int y)
		{
			LinkPoint pc = GetCounterPoint();
			pc.x = x;
			pc.y = y;
		}

		internal bool IsGreater(LinkPoint point, LinkDirection direction)
		{
			switch (direction) {
				case LinkDirection.SourceNorthDestinationSouth:
				case LinkDirection.SourceSouthDestinationNorth:
					return this.x > point.x;
				case LinkDirection.SourceEastDestinationWest:
				case LinkDirection.SourceWestDestinationEast:
					return this.y > point.y;
			}
			return false;
		}

		public static void Swap(LinkPoint p1, LinkPoint p2)
		{
			int t = 0;
			t = p1.X;
			p1.X = p2.X;
			p2.X = t;

			t = p1.Y;
			p1.Y = p2.Y;
			p2.y = t;
		}

	}
}
