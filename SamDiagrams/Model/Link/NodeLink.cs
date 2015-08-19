using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using SamDiagrams.Model;


namespace SamDiagrams.Linking
{
	public partial class NodeLink:IComparable, Item
	{
		private Color color = Color.Black;

		public Color Color
		{
			get { return color; }
			set { color = value; }
		}
		private Node source;
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
				Rectangle r = new Rectangle(Math.Min(sourcePoint.X, destinationPoint.X), 
				                            Math.Min(sourcePoint.Y, destinationPoint.Y),
				                            Math.Abs(sourcePoint.X - destinationPoint.X),
				                            Math.Abs(sourcePoint.Y - destinationPoint.Y));
				return r;
			}
		}

		private LinkDirection direction = LinkDirection.None ;

		public LinkDirection Direction
		{
			get { return direction; }
			set { direction = value; }
		}
		
		public Node Source
		{
			get { return source; }
			set { source = value; }
		}
		
		private Node destination;

		public Node Destination
		{
			get { return destination; }
			set { destination = value; }
		}
		
		public NodeLink(Node source, Node destination)
		{
			this.source = source;
			this.destination = destination;
			this.Direction = LinkDirection.None;
 
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
			return 0;
		}

		#endregion
	}
}

