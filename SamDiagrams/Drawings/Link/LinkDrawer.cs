/*
 * Created by SharpDevelop.
 * User: Sam
 * Date: 7/29/2015
 * Time: 10:15 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using SamDiagrams.Drawings;
using SamDiagrams.Linking;

namespace SamDiagrams.Drawers.Links
{
	/// <summary>
	/// Description of NSWELinkDrawer.
	/// </summary>
	public class LinkDrawing : Drawing
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
			using (Pen lPen = new Pen(link.Color, lineWidth)) {
				
				ContainerDrawer containerDrawer = link.Source.DiagramContainer.ContainerDrawer;
				Drawing sourceDrawing = (Drawing)containerDrawer.ModelToDrawer[link.Source];
				Drawing destinationDrawing = (Drawing)containerDrawer.ModelToDrawer[link.Destination];
				Pen sPen = new Pen(Color.FromArgb(70, link.Source.Color), selectedLineWidth);
				lPen.DashPattern = new float[] { 8, 3 };
				if ((link.Direction == LinkDirection.SourceWestDestinationEast) || (link.Direction == LinkDirection.SourceEastDestinationWest)) {
					if (linkStyle == LinkStyle.StreightLines) {
						int midX = (int)(link.SourcePoint.X + link.DestinationPoint.X) / 2;
						Point[] ps = new Point[] {
							new Point(link.SourcePoint.X, link.SourcePoint.Y),
							new Point(midX, link.SourcePoint.Y),
							new Point(midX, link.DestinationPoint.Y),
							new Point(link.DestinationPoint.X, link.DestinationPoint.Y)
						};
						if (sourceDrawing.Selected || destinationDrawing.Selected) {
							graphics.DrawLines(sPen, ps);
						}
						graphics.DrawLines(lPen, ps);

					} else {
						if (sourceDrawing.Selected || destinationDrawing.Selected) {
							graphics.DrawLine(sPen, link.SourcePoint.X, link.SourcePoint.Y, link.DestinationPoint.X, link.DestinationPoint.Y);
						}
						graphics.DrawLine(lPen, link.SourcePoint.X, link.SourcePoint.Y, link.DestinationPoint.X, link.DestinationPoint.Y);
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
						if (sourceDrawing.Selected || destinationDrawing.Selected) {
							graphics.DrawLines(sPen, ps);
						}
						graphics.DrawLines(lPen, ps);
					} else {
						if (sourceDrawing.Selected || destinationDrawing.Selected) {
							graphics.DrawLine(sPen, link.SourcePoint.X, link.SourcePoint.Y, link.DestinationPoint.X, link.DestinationPoint.Y);
						}
						graphics.DrawLine(lPen, link.SourcePoint.X, link.SourcePoint.Y, link.DestinationPoint.X, link.DestinationPoint.Y);
					}
				}
			}
		}
		
		public Point Location {
			get {
				return link.getLocation();
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
				bounds.Inflate((int)Math.Floor(selectedLineWidth/2), (int)Math.Floor(selectedLineWidth/2));
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
