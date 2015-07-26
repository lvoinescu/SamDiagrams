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
	/// Description of DiagramPoin.
	/// </summary>
	public class LinkPoint
	{
        private int x = 0;
        private ItemLink link;

        public ItemLink Link
        {
            get { return link; }
            set { link = value; }
        }
        public int X
        {
            get { return x; }
            set { x = value; }
        }
        private int y = 0;

        public int Y
        {
            get { return y; }
            set { y = value; }
        }
		public LinkPoint(ItemLink link)
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
            //LinkPoint paux = p1;
            //if (p1.link.DestinationPoint == p1)
            //    p1.link.DestinationPoint = p2;
            //else
            //    p1.link.SourcePoint = p1;

            //if (p2.link.DestinationPoint == p2)
            //    p1.link.DestinationPoint = p1;
            //else
            //    p1.link.SourcePoint = p1;


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
