using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using SamDiagrams.Linking.Strategy;
using SamDiagrams.Linking.Strategy.NSWELinkStrategy;
namespace SamDiagrams.Linking.Orchestrator
{



	public partial class LinkOrchestrator : ILinkOrchestrator
	{
		DiagramContainer dc;
		LinkStyle linkStyle = LinkStyle.SingleLine;
		List<DiagramItem> items;
		List<Link> links = new List<Link>();
		internal int lPenWidh = 1;
		internal int lsPenWidh = 9;
		internal ILinkStrategy linkStrategy;
		
		public LinkStyle LinkStyle {
			get { return linkStyle; }
			set {
				linkStyle = value;
				dc.Invalidate();
			}
		}

		public List<Link> Links {
			get { return links; }
			set { links = value; }
		}
		
		
		public LinkOrchestrator(DiagramContainer dc)
		{
			items = new List<DiagramItem>();
			linkStrategy = new NSWELinkStrategy();
			this.dc = dc;
		}

		public void AddLink(DiagramItem source, DiagramItem destination)
		{
			Link link = new Link(source, destination);
			RegisterLink(link);
		}

		public void AddLink(DiagramItem source, DiagramItem destination, Color color)
		{
			Link link = new Link(source, destination, color);
			RegisterLink(link);
		}
		
		private void RegisterLink(Link link){
			DiagramItem source = link.Source;
			DiagramItem destination = link.Destination;
			linkStrategy.RegisterLink(link);
			links.Add(link);
//			link.Source.OutputLinkList.Add(link);
//			link.Destination.InputLinkList.Add(link);
//			source.Links.Add(link);
//			destination.Links.Add(link);
			source.ItemMoved += new ItemMovedHandler(OnItemMoved);
			destination.ItemMoved += new ItemMovedHandler(OnItemMoved);
			if (!items.Contains(source)) {
				items.Add(source);
				source.ItemResized += new ItemResizedHandler(OnItemResized);
			}
			if (!items.Contains(destination)) {
				items.Add(destination);
				destination.ItemResized += new ItemResizedHandler(OnItemResized);
			}
			linkStrategy.ArangeLinksForItem(source);
			linkStrategy.ArangeLinksForItem(destination);
		}

		private void OnItemResized(object sender, ItemResizedEventArg e)
		{
			linkStrategy.ArangeLinksForItem((DiagramItem)sender);
		}

		private void OnItemMoved(object sender, ItemMovedEventArg e)
		{
			float scaleFactor = (float)dc.ZoomFactor / 100;
			DiagramItem item = e.Item;

			linkStrategy.DirectLinks(item);
			
			linkStrategy.ArangeLinksForItem(item);
		}

		internal void Draw(Graphics graphics)
		{
			RectangleF rct = graphics.ClipBounds;
			if (linkStyle == LinkStyle.SingleLine)
				graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			else
				graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
			foreach (Link link in links) {
				//RectangleF rf = new RectangleF(link.getLocation().X-5, link.getLocation().Y-5,
				//    link.getSize().Width+10, link.getSize().Height+10);
				if (link.Invalidated || rct.IntersectsWith(link.Bounds)) {
					using (Pen lPen = new Pen(link.Color, lPenWidh)) {
						Pen sPen = new Pen(Color.FromArgb(70, link.Source.Color), lsPenWidh);
						lPen.DashPattern = new float[] {
							8,
							3
						};
						float scaleFactor = (float)dc.ZoomFactor / 100;
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
					link.Invalidated = false;
				}
			}

		}

		internal void ArrangeAllLinks()
		{
			foreach (DiagramItem item in items)
				linkStrategy.ArangeLinksForItem(item);
		}

	}
}

