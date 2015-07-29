using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;


namespace SamDiagrams.Linking
{
	public partial class Link:IComparable, IDrawableItem
	{
		private Color color = Color.Black;

		public Color Color
		{
			get { return color; }
			set { color = value; }
		}
		private DiagramItem source;
		private LinkPoint sourcePoint, destinationPoint;

		bool invalidated = true;

		public bool Invalidated
		{
			get { return invalidated; }
			set { invalidated = value; }
		}
		public LinkPoint DestinationPoint
		{
			get { return destinationPoint; }
			set { destinationPoint = value; }
		}


		public LinkPoint SourcePoint
		{
			get { return sourcePoint; }
			set { sourcePoint = value; }
		}


		public Rectangle Bounds
		{
			get
			{
				Rectangle r = new Rectangle(Math.Min(sourcePoint.X, destinationPoint.X), Math.Min(sourcePoint.Y, destinationPoint.Y),
				                            Math.Abs(sourcePoint.X - destinationPoint.X), Math.Abs(sourcePoint.Y - destinationPoint.Y));
				return r;
			}
		}

		private LinkDirection direction = LinkDirection.None ;

		public LinkDirection Direction
		{
			get { return direction; }
			set { direction = value; }
		}
		
		public DiagramItem Source
		{
			get { return source; }
			set { source = value; }
		}
		
		private DiagramItem destination;

		public DiagramItem Destination
		{
			get { return destination; }
			set { destination = value; }
		}
		
		public Link(DiagramItem source, DiagramItem destination)
		{
			this.source = source;
			this.destination = destination;
			this.Direction = LinkDirection.None;
			this.sourcePoint = new LinkPoint(this);
			this.destinationPoint = new LinkPoint(this);
		}
		public Link(DiagramItem source, DiagramItem destination, Color color):this(source,destination)
		{
			this.color = color;
		}

		public Size getSize()
		{
			return new Size(Math.Abs(sourcePoint.X-destinationPoint.X), Math.Abs(sourcePoint.Y- destinationPoint.Y));
		}

		public Point getLocation()
		{
			return new Point(Math.Min(sourcePoint.X, destinationPoint.X), Math.Min(sourcePoint.Y, destinationPoint.Y));
		}

		public void Draw(Graphics g)
		{
		}




		#region IComparable Members

		public int CompareTo(object obj)
		{
			Link l = (Link)obj;
			switch(l.direction)
			{
				case LinkDirection.SourceNorthDestinationSouth:
				case LinkDirection.SourceSouthDestinationNorth:
					return this.SourcePoint.X - l.SourcePoint.X;
				case LinkDirection.SourceWestDestinationEast:
				case LinkDirection.SourceEastDestinationWest:
					return this.SourcePoint.Y - l.SourcePoint.Y;

			}
			return 0;
		}

		#endregion
	}
}

