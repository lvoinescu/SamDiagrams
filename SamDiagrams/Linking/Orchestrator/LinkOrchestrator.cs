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
		internal int lPenWidth = 1;
		internal int lsPenWidth = 9;
		internal ILinker linkStrategy;
		internal ILinkDrawer linkDrawer;
		
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
			linkDrawer = new NSWELinkDrawer(lPenWidth, lsPenWidth, LinkStyle.StreightLines);
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
			
			if (!items.Contains(source)) {
				items.Add(source);
				source.ItemResized += new ItemResizedHandler(OnItemResized);
				source.ItemMoved += new ItemMovedHandler(OnItemMoved);
			}
			
			if (!items.Contains(destination)) {
				items.Add(destination);
				destination.ItemResized += new ItemResizedHandler(OnItemResized);
				destination.ItemMoved += new ItemMovedHandler(OnItemMoved);
			}
			
			linkStrategy.DirectLinks(source);
			linkStrategy.DirectLinks(destination);
		}

		private void OnItemResized(object sender, ItemResizedEventArg e)
		{
			linkStrategy.DirectLinks((DiagramItem)sender);
		}

		private void OnItemMoved(object sender, ItemMovedEventArg e)
		{
			float scaleFactor = (float)dc.ZoomFactor / 100;
			DiagramItem item = e.Item;

			linkStrategy.DirectLinks(item);
			
			linkStrategy.DirectLinks(item);
		}

		internal void Draw(Graphics graphics)
		{
			RectangleF rct = graphics.ClipBounds;
			float zoomFactor = (float)dc.ZoomFactor/100;
			if (linkStyle == LinkStyle.SingleLine)
				graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			else
				graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
			foreach (Link link in links) {
				if (link.Invalidated || rct.IntersectsWith(link.Bounds)) {
					linkDrawer.Draw(link, graphics, zoomFactor);
				}
				link.Invalidated = false;
			}
		}


		internal void ArrangeAllLinks()
		{
			foreach (DiagramItem item in items)
				linkStrategy.DirectLinks(item);
		}

	}
}

