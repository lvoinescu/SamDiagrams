using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using SamDiagrams.Model;


namespace SamDiagrams.Linking
{
	public partial class StructureLink:IComparable, Item
	{
		private Color color = Color.Black;

		public Color Color
		{
			get { return color; }
			set { color = value; }
		}
		private Structure source;
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

		private LinkDirection direction = LinkDirection.None ;

		public LinkDirection Direction
		{
			get { return direction; }
			set { direction = value; }
		}
		
		public Structure Source
		{
			get { return source; }
			set { source = value; }
		}
		
		private Structure destination;

		public Structure Destination
		{
			get { return destination; }
			set { destination = value; }
		}
		
		public StructureLink(Structure source, Structure destination)
		{
			this.source = source;
			this.destination = destination;
			this.direction = LinkDirection.None;
			this.sourcePoint = new LinkPoint(this);
			this.destinationPoint = new LinkPoint(this);
		}
		public StructureLink(Structure source, Structure destination, Color color):this(source,destination)
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

		#region IComparable Members

		public int CompareTo(object obj)
		{
			StructureLink l = (StructureLink)obj;
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

