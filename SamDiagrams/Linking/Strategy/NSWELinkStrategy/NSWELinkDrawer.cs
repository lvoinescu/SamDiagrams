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

namespace SamDiagrams.Linking.Strategy.NSWELinkStrategy
{
	/// <summary>
	/// Description of NSWELinkDrawer.
	/// </summary>
	public class NSWELinkDrawer : ILinkDrawer
	{
		
		private float lineWidth;
		private float selectedLineWidth;
		private LinkStyle linkStyle;
		
		
		public LinkStyle LinkStyle {
			get { return linkStyle; }
			set { linkStyle = value; }
		}
		
		public NSWELinkDrawer(float lineWidth, float selectedLineWidth, LinkStyle linkStyle)
		{
			this.lineWidth = lineWidth;
			this.selectedLineWidth = selectedLineWidth;
			this.linkStyle = linkStyle;
		}
		
		public void Draw(Link link, System.Drawing.Graphics graphics, float scaleFactor)
		{
			using (Pen lPen = new Pen(link.Color, lineWidth)) {
				Pen sPen = new Pen(Color.FromArgb(70, link.Source.Color), selectedLineWidth);
				lPen.DashPattern = new float[] {8, 3};
				if ((link.Direction == LinkDirection.SourceWestDestinationEast) || (link.Direction == LinkDirection.SourceEastDestinationWest)) {
					if (linkStyle == LinkStyle.StreightLines) {
						int midX = (int)(link.SourcePoint.X + link.DestinationPoint.X) / 2;
						Point[] ps = new Point[] {
							new Point(link.SourcePoint.X, link.SourcePoint.Y),
							new Point(midX, link.SourcePoint.Y),
							new Point(midX, link.DestinationPoint.Y),
							new Point(link.DestinationPoint.X, link.DestinationPoint.Y)
						};
						if (link.Source.IsSelected || link.Destination.IsSelected) {
							graphics.DrawLines(sPen, ps);
						}
						graphics.DrawLines(lPen, ps);

					} else {
						if (link.Source.IsSelected || link.Destination.IsSelected) {
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
						if (link.Source.IsSelected || link.Destination.IsSelected) {
							graphics.DrawLines(sPen, ps);
						}
						graphics.DrawLines(lPen, ps);
					} else {
						if (link.Source.IsSelected || link.Destination.IsSelected) {
							graphics.DrawLine(sPen, link.SourcePoint.X, link.SourcePoint.Y, link.DestinationPoint.X, link.DestinationPoint.Y);
						}
						graphics.DrawLine(lPen, link.SourcePoint.X, link.SourcePoint.Y, link.DestinationPoint.X, link.DestinationPoint.Y);
					}
				}
			}
		}
		
	}
}
