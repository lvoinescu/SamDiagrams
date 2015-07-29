/*
 * Created by SharpDevelop.
 * User: L
 * Date: 3/11/2013
 * Time: 11:06 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

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
		private Link link;
		
		public Link Link
		{
			get { return link; }
			set { link = value; }
		}
		
		public int X
		{
			get { return x; }
			set { x = value; }
		}

		public int Y
		{
			get { return y; }
			set { y = value; }
		}

		public LinkPoint(Link link)
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
			switch (direction)
			{
				case LinkDirection.SourceNorthDestinationSouth:
				case LinkDirection.SourceSouthDestinationNorth:
					return this.x> point.x;
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
