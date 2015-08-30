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
using SamDiagrams.Drawings;
using SamDiagrams.Drawings.Link;
using SamDiagrams.Linking;
using SamDiagrams.Model;

namespace SamDiagrams.Drawers.Links
{
	/// <summary>
	/// Description of NSWELinkDrawer.
	/// </summary>
	public class LinkDrawing : IDrawing
	{
		private ILink link;
		private float lineWidth;
		private float selectedLineWidth;
		private LinkStyle linkStyle;
		private bool invalidated;
		private ILinkableDrawing sourceDrawing;
		private ILinkableDrawing destinationDrawing;
		private LinkPoint sourcePoint, destinationPoint;
		private LinkDirection direction = LinkDirection.None;


		public Item Item {
			get {
				return this.link;
			}
		}

		public  ILinkableDrawing SourceDrawing {
			get {
				return (ILinkableDrawing)this.link.Source.Drawing;
			}
		}

		public  ILinkableDrawing DestinationDrawing {
			get {
				return (ILinkableDrawing)this.link.Destination.Drawing;
			}
		}

		public bool Movable {
			get {
				return false;
			}
			set {
				throw new NotImplementedException();
			}
		}
		public LinkDirection Direction {
			get { return direction; }
			set { direction = value; }
		}
		
		public bool Invalidated {
			get {
				return invalidated;
			}
			set {
				invalidated = value;
			}
		}
		public LinkStyle LinkStyle {
			get { return linkStyle; }
			set { linkStyle = value; }
		}
		
		public LinkPoint DestinationPoint {
			get { return destinationPoint; }
			set { destinationPoint = value; }
		}


		public LinkPoint SourcePoint {
			get { return sourcePoint; }
			set { sourcePoint = value; }
		}

		public Rectangle InvalidatedRegion {
			get {
				return this.Bounds;
			}
		}

		public int CompareTo(object obj)
		{
			LinkDrawing l = (LinkDrawing)obj;
			switch (l.Direction) {
				case LinkDirection.SourceNorthDestinationSouth:
				case LinkDirection.SourceSouthDestinationNorth:
					return this.SourcePoint.X - l.SourcePoint.X;
				case LinkDirection.SourceWestDestinationEast:
				case LinkDirection.SourceEastDestinationWest:
					return this.SourcePoint.Y - l.SourcePoint.Y;

			}
			return 0;
		}
		public LinkDrawing(ILink link, float lineWidth, float selectedLineWidth, LinkStyle linkStyle)
		{
			this.link = (ILink)link;
			this.lineWidth = lineWidth;
			this.selectedLineWidth = selectedLineWidth;
			this.linkStyle = linkStyle;
			this.sourcePoint = new LinkPoint(this);
			this.destinationPoint = new LinkPoint(this);
			this.sourceDrawing = (ILinkableDrawing)(link.Source.Drawing);
			this.destinationDrawing = (ILinkableDrawing)(link.Destination.Drawing);
		}
		
		public void Draw(Graphics graphics)
		{
			using (Pen linePen = new Pen(link.Color, lineWidth)) {
				Pen selectionPen = new Pen(Color.FromArgb(70, sourceDrawing.Item.Color), selectedLineWidth);
				linePen.DashPattern = new float[] { 8, 3 };
				if ((direction == LinkDirection.SourceWestDestinationEast) || (direction == LinkDirection.SourceEastDestinationWest)) {
					if (linkStyle == LinkStyle.StreightLines) {
						int midX = (int)(sourcePoint.X + destinationPoint.X) / 2;
						Point[] ps = new Point[] {
							new Point(sourcePoint.X, sourcePoint.Y),
							new Point(midX, sourcePoint.Y),
							new Point(midX, destinationPoint.Y),
							new Point(destinationPoint.X, destinationPoint.Y)
						};
						
						if (((sourceDrawing is ISelectable) && (sourceDrawing as ISelectable).Selected) ||
						    ((destinationDrawing is StructureDrawing) && (destinationDrawing as StructureDrawing).Selected)) {
							graphics.DrawLines(selectionPen, ps);
						}
						graphics.DrawLines(linePen, ps);

					} else {
						if (((sourceDrawing is StructureDrawing) && (sourceDrawing as StructureDrawing).Selected) ||
						    ((destinationDrawing is StructureDrawing) && (destinationDrawing as StructureDrawing).Selected)) {
							graphics.DrawLine(selectionPen, sourcePoint.X, sourcePoint.Y, destinationPoint.X, destinationPoint.Y);
						}
						graphics.DrawLine(linePen, sourcePoint.X, sourcePoint.Y, destinationPoint.X, destinationPoint.Y);
					}

				} else {
					if (linkStyle == LinkStyle.StreightLines) {
						int midY = (int)(sourcePoint.Y + destinationPoint.Y) / 2;
						Point[] ps = new Point[] {
							new Point((int)(sourcePoint.X), (int)(sourcePoint.Y)),
							new Point((int)(sourcePoint.X), (int)(midY)),
							new Point((int)(destinationPoint.X), (int)(midY)),
							new Point((int)(destinationPoint.X), (int)(destinationPoint.Y))
						};
						if (((sourceDrawing is ISelectable) && (sourceDrawing as ISelectable).Selected) ||
						    ((destinationDrawing is ISelectable) && (destinationDrawing as ISelectable).Selected)) {
							graphics.DrawLines(selectionPen, ps);
						}
						graphics.DrawLines(linePen, ps);
					} else {
						if (((sourceDrawing is StructureDrawing) && (sourceDrawing as StructureDrawing).Selected) ||
						    ((destinationDrawing is StructureDrawing) && (destinationDrawing as StructureDrawing).Selected)) {
							graphics.DrawLine(selectionPen, sourcePoint.X, sourcePoint.Y, destinationPoint.X, destinationPoint.Y);
						}
						graphics.DrawLine(linePen, sourcePoint.X, sourcePoint.Y, destinationPoint.X, destinationPoint.Y);
					}
				}
			}
		}
		
		public Point Location {
			get {
				return new Point(Math.Min(sourcePoint.X, destinationPoint.X), Math.Min(sourcePoint.Y, destinationPoint.Y));
			}
			set {
				throw new Exception("Cannot set location for a link.");
			}
		}
		public Size Size {
			get {
				return new Size(
					Math.Abs(sourcePoint.X - destinationPoint.X),
					Math.Abs(sourcePoint.Y - destinationPoint.Y));
			}
		}
		public Rectangle Bounds {
			get {
				Rectangle bounds = new Rectangle(Location, Size);
				bounds.Inflate((int)Math.Floor(selectedLineWidth / 2), (int)Math.Floor(selectedLineWidth / 2));
				return bounds;
			}
		}

		
		public override string ToString()
		{
			return string.Format("[LinkDrawer Invalidated={0}, Location={1}, Size={2}]",
				invalidated, Location, Size);
		}

	}
}
