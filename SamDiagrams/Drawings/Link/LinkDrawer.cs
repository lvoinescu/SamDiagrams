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
using SamDiagrams.Drawings.Geometry;
using SamDiagrams.Linking;

namespace SamDiagrams.Drawers.Links
{
	/// <summary>
	/// Description of NSWELinkDrawer.
	/// </summary>
	public class LinkDrawing : IDrawing, IBoundedShape
	{
		private StructureLink link;
		private float lineWidth;
		private float selectedLineWidth;
		private LinkStyle linkStyle;
		private bool invalidated;

		public StructureLink Link {
			get {
				return link;
			}
			set {
				link = value;
			}
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
		
		public LinkDrawing(StructureLink link, float lineWidth, float selectedLineWidth, LinkStyle linkStyle)
		{
			this.link = link;
			this.lineWidth = lineWidth;
			this.selectedLineWidth = selectedLineWidth;
			this.linkStyle = linkStyle;
		}
		
		public void Draw(Graphics graphics)
		{
			using (Pen linePen = new Pen(link.Color, lineWidth)) {
				
				ContainerDrawer containerDrawer = link.Source.DiagramContainer.ContainerDrawer;
				IDrawing sourceDrawing = containerDrawer.ModelToDrawer[link.Source];
				IDrawing destinationDrawing = containerDrawer.ModelToDrawer[link.Destination];
				Pen selectionPen = new Pen(Color.FromArgb(70, link.Source.Color), selectedLineWidth);
				linePen.DashPattern = new float[] { 8, 3 };
				if ((link.Direction == LinkDirection.SourceWestDestinationEast) || (link.Direction == LinkDirection.SourceEastDestinationWest)) {
					if (linkStyle == LinkStyle.StreightLines) {
						int midX = (int)(link.SourcePoint.X + link.DestinationPoint.X) / 2;
						Point[] ps = new Point[] {
							new Point(link.SourcePoint.X, link.SourcePoint.Y),
							new Point(midX, link.SourcePoint.Y),
							new Point(midX, link.DestinationPoint.Y),
							new Point(link.DestinationPoint.X, link.DestinationPoint.Y)
						};
						
						if (((sourceDrawing is StructureDrawing) && (sourceDrawing as StructureDrawing).Selected) ||
						    ((destinationDrawing is StructureDrawing) && (destinationDrawing as StructureDrawing).Selected)) {
							graphics.DrawLines(selectionPen, ps);
						}
						graphics.DrawLines(linePen, ps);

					} else {
						if (((sourceDrawing is StructureDrawing) && (sourceDrawing as StructureDrawing).Selected) ||
						    ((destinationDrawing is StructureDrawing) && (destinationDrawing as StructureDrawing).Selected)) {
							graphics.DrawLine(selectionPen, link.SourcePoint.X, link.SourcePoint.Y, link.DestinationPoint.X, link.DestinationPoint.Y);
						}
						graphics.DrawLine(linePen, link.SourcePoint.X, link.SourcePoint.Y, link.DestinationPoint.X, link.DestinationPoint.Y);
					}

				} else {
					if (linkStyle == LinkStyle.StreightLines) {
						int midY = (int)(link.SourcePoint.Y + link.DestinationPoint.Y) / 2;
						Point[] ps = new Point[] {
							new Point((int)(link.SourcePoint.X), (int)(link.SourcePoint.Y)),
							new Point((int)(link.SourcePoint.X), (int)(midY)),
							new Point((int)(link.DestinationPoint.X), (int)(midY)),
							new Point((int)(link.DestinationPoint.X), (int)(link.DestinationPoint.Y))
						};
						if (((sourceDrawing is StructureDrawing) && (sourceDrawing as StructureDrawing).Selected) ||
						    ((destinationDrawing is StructureDrawing) && (destinationDrawing as StructureDrawing).Selected)) {
							graphics.DrawLines(selectionPen, ps);
						}
						graphics.DrawLines(linePen, ps);
					} else {
						if (((sourceDrawing is StructureDrawing) && (sourceDrawing as StructureDrawing).Selected) ||
						    ((destinationDrawing is StructureDrawing) && (destinationDrawing as StructureDrawing).Selected)) {
							graphics.DrawLine(selectionPen, link.SourcePoint.X, link.SourcePoint.Y, link.DestinationPoint.X, link.DestinationPoint.Y);
						}
						graphics.DrawLine(linePen, link.SourcePoint.X, link.SourcePoint.Y, link.DestinationPoint.X, link.DestinationPoint.Y);
					}
				}
			}
		}
		
		public Point Location {
			get {
				return link.getLocation();
			}
			set { 
				throw new Exception("Cannot set location for a link.");
			}
		}
		public Size Size {
			get {
				return link.getSize();
			}
		}
		public Rectangle Bounds {
			get {
				Rectangle bounds = new Rectangle(link.getLocation(), link.getSize());
				bounds.Inflate((int)Math.Floor(selectedLineWidth / 2), (int)Math.Floor(selectedLineWidth / 2));
				return bounds;
			}
		}

		public bool Selected {
			get {
				throw new NotImplementedException();
			}
			set {
				throw new NotImplementedException();
			}
		}
		
		public override string ToString()
		{
			return string.Format("[LinkDrawer Invalidated={0}, Location={1}, Size={2}]",
				invalidated, Location, Size);
		}

	}
}
