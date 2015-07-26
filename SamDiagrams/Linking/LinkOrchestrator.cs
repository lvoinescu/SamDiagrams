using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using SamDiagrams.Linking;
namespace SamDiagrams
{

	public enum LinkStyle
	{
		SingleLine,
		StreightLines
	}

	public partial class LinkOrchestrator : ILinkOrchestrator
	{
		List<DiagramItem> items;
		DiagramContainer dc;
		List<ItemLink> links = new List<ItemLink>();
		public event LinkDirectionChangedHandler LinkDirectionChangedEvent;
		public List<ItemLink> Links {
			get { return links; }
			set { links = value; }
		}
		LinkStyle linkStyle = LinkStyle.SingleLine;
		internal int lPenWidh = 1;
		internal int lsPenWidh = 9;
		public LinkStyle LinkStyle {
			get { return linkStyle; }
			set {
				linkStyle = value;
				dc.Invalidate();
			}
		}

		internal ILinkStrategy linkStrategy;
		
		
		public LinkOrchestrator(DiagramContainer dc)
		{
			items = new List<DiagramItem>();
			linkStrategy = new DefaultLinkStrategy();
			this.dc = dc;
		}

		public void AddLink(DiagramItem source, DiagramItem destination)
		{
			ItemLink l = new ItemLink(source, destination);
			links.Add(l);
			source.Links.Add(l);
			destination.Links.Add(l);
			source.OutputLinkList.Add(l);
			destination.InputLinkList.Add(l);
			source.ItemMoved += new ItemMovedHandler(item_ItemMoved);
			destination.ItemMoved += new ItemMovedHandler(item_ItemMoved);
			if (!items.Contains(source)) {
				items.Add(source);
				source.ItemResized += new ItemResizedHandler(source_ItemResized);
			}
			if (!items.Contains(destination)) {
				items.Add(destination);
				destination.ItemResized += new ItemResizedHandler(source_ItemResized);
			}
			ArangeLinksForItem(source);
			ArangeLinksForItem(destination);
		}

		public void AddLink(DiagramItem source, DiagramItem destination, Color color)
		{
			ItemLink l = new ItemLink(source, destination, color);
			links.Add(l);
			source.OutputLinkList.Add(l);
			destination.InputLinkList.Add(l);
			source.ItemMoved += new ItemMovedHandler(item_ItemMoved);
			destination.ItemMoved += new ItemMovedHandler(item_ItemMoved);
			if (!items.Contains(source)) {
				items.Add(source);
				source.ItemResized += new ItemResizedHandler(source_ItemResized);
			}
			if (!items.Contains(destination)) {
				items.Add(destination);
				destination.ItemResized += new ItemResizedHandler(source_ItemResized);
			}
			ArangeLinksForItem(source);
			ArangeLinksForItem(destination);
		}

		void source_ItemResized(object sender, ItemResizedEventArg e)
		{
			ArangeLinksForItem((DiagramItem)sender);
		}

		void item_ItemMoved(object sender, ItemMovedEventArg e)
		{
			float scaleFactor = (float)dc.ZoomFactor / 100;
			DiagramItem item = e.Item;

			
			foreach(ItemLink link in item.Links){
				LinkDirection direction = linkStrategy.computeDirection(link);
				if (direction != link.Direction) {
					//link.Invalidated = true;
					OnLinkDirectionChanged(link, direction);
					link.Direction = direction;
					ArangeLinksForItem(link.Destination);
					ArangeLinksForItem(link.Source);
				}
			}
			
//			for (int i = 0; i < item.OutputLinkList.Count; i++) {
//
//				ItemLink link = item.OutputLinkList[i];
//				DiagramItem destination = link.Destination;
//				LinkDirection prevDirection = link.Direction;
//				LinkDirection direction = LinkDirection.None;
//				if (item.Location.Y > destination.Location.Y + destination.Size.Height) {
//					direction = LinkDirection.SourceNorthDestinationSouth;
//				} else if (item.Location.Y + item.Size.Height < destination.Location.Y) {
//					direction = LinkDirection.SourceSouthDestinationNorth;
//				} else {
//					if (item.Location.X > destination.Location.X + destination.Size.Width) {
//						direction = LinkDirection.SourceWestDestinationEast;
//					} else if (item.Location.X + item.Size.Width < destination.Location.X) {
//						direction = LinkDirection.SourceEastDestinationWest;
//					} else
//						direction = LinkDirection.None;
//				}
//				if (direction != link.Direction) {
//					//link.Invalidated = true;
//					OnLinkDirectionChanged(link, direction);
//					link.Direction = direction;
//
//				}
//				ArangeLinksForItem(link.Destination);
//			}
//
//
//
//			for (int i = 0; i < item.InputLinkList.Count; i++) {
//				ItemLink link = item.InputLinkList[i];
//				DiagramItem source = link.Source;
//				LinkDirection prevDirection = link.Direction;
//				LinkDirection direction = LinkDirection.None;
//				if (item.Location.Y > source.Location.Y + source.Size.Height) {
//					direction = LinkDirection.SourceSouthDestinationNorth;
//				} else if (item.Location.Y + item.Size.Height < source.Location.Y) {
//					direction = LinkDirection.SourceNorthDestinationSouth;
//				} else {
//					if (item.Location.X > source.Location.X + source.Size.Width) {
//						direction = LinkDirection.SourceEastDestinationWest;
//					} else if (item.Location.X + item.Size.Width < source.Location.X) {
//						direction = LinkDirection.SourceWestDestinationEast;
//					} else
//						direction = LinkDirection.None;
//				}
//				if (direction != link.Direction) {
//					OnLinkDirectionChanged(link, direction);
//					link.Direction = direction;
//				}
//				ArangeLinksForItem(link.Source);
//			}
			ArangeLinksForItem(item);
		}

		internal void ArangeLinksForItem(DiagramItem item)
		{
			int iN = 0, iS = 0, iW = 0, iE = 0;
			List<ItemLink> nLink = new List<ItemLink>();
			for (int i = 0; i < item.InputLinkList.Count; i++) {
				ItemLink link = item.InputLinkList[i];
				switch (link.Direction) {
					case LinkDirection.SourceNorthDestinationSouth:
						iS++;
						link.DestinationPoint.X = item.Location.X + iS * item.Size.Width / (item.LinkPointsSouth.Count + 1);
						link.DestinationPoint.Y = item.Location.Y + item.Size.Height;
						break;
					case LinkDirection.SourceSouthDestinationNorth:
						iN++;
						link.DestinationPoint.X = item.Location.X + iN * item.Size.Width / (item.LinkPointsNorth.Count + 1);
						link.DestinationPoint.Y = item.Location.Y;
						break;
					case LinkDirection.SourceWestDestinationEast:
						iE++;
						link.DestinationPoint.X = item.Location.X + item.Size.Width;
						link.DestinationPoint.Y = item.Location.Y + iE * item.Size.Height / (item.LinkPointsEast.Count + 1);
						break;
					case LinkDirection.SourceEastDestinationWest:
						iW++;
						link.DestinationPoint.X = item.Location.X;
						link.DestinationPoint.Y = item.Location.Y + iW * item.Size.Height / (item.LinkPointsWest.Count + 1);
						break;
					default:
						link.DestinationPoint.X = item.Location.X + item.Size.Width / 2;
						link.DestinationPoint.Y = item.Location.Y + item.Size.Height / 2;
						break;
				}
			}


			for (int i = 0; i < item.OutputLinkList.Count; i++) {
				ItemLink link = item.OutputLinkList[i];
				switch (link.Direction) {
					case LinkDirection.SourceNorthDestinationSouth:
						iN++;
						link.SourcePoint.X = item.Location.X + iN * item.Size.Width / (item.LinkPointsNorth.Count + 1);
						link.SourcePoint.Y = item.Location.Y;
						break;
					case LinkDirection.SourceSouthDestinationNorth:
						iS++;
						link.SourcePoint.X = item.Location.X + iS * item.Size.Width / (item.LinkPointsSouth.Count + 1);
						link.SourcePoint.Y = item.Location.Y + item.Size.Height;
						break;
					case LinkDirection.SourceWestDestinationEast:
						iW++;
						link.SourcePoint.X = item.Location.X;
						link.SourcePoint.Y = item.Location.Y + iW * item.Size.Height / (item.LinkPointsWest.Count + 1);
						break;
					case LinkDirection.SourceEastDestinationWest:
						iE++;
						link.SourcePoint.X = item.Location.X + item.Size.Width;
						link.SourcePoint.Y = item.Location.Y + iE * item.Size.Height / (item.LinkPointsEast.Count + 1);
						break;
					default:
						link.SourcePoint.X = item.Location.X + item.Size.Width / 2;
						link.SourcePoint.Y = item.Location.Y + item.Size.Height / 2;
						break;
				}

			}
			if (item.LinkPointsSouth.Count > 1)
				SortCounterPoints(item.LinkPointsSouth, LinkDirection.SourceSouthDestinationNorth);
			if (item.LinkPointsNorth.Count > 1)
				SortCounterPoints(item.LinkPointsNorth, LinkDirection.SourceNorthDestinationSouth);
			if (item.LinkPointsWest.Count > 1)
				SortCounterPoints(item.LinkPointsWest, LinkDirection.SourceWestDestinationEast);
			if (item.LinkPointsEast.Count > 1)
				SortCounterPoints(item.LinkPointsEast, LinkDirection.SourceEastDestinationWest);
		}

		private void SortCounterPoints(List<LinkPoint> list, LinkDirection direction)
		{

			for (int i = 0; i < list.Count; i++) {
				for (int j = 0; j < list.Count; j++) {
					LinkPoint counterPoint = list[i].GetCounterPoint();
					LinkPoint point = list[i];
					bool counterPointGreater = list[i].GetCounterPoint().IsGreater(list[j].GetCounterPoint(), direction);
					bool pointGreater = list[i].IsGreater(list[j], direction);
					if ((!counterPointGreater && pointGreater) || (counterPointGreater && !pointGreater)) {
						int t = 0;
						t = list[i].X;
						list[i].X = list[j].X;
						list[j].X = t;

						t = list[i].Y;
						list[i].Y = list[j].Y;
						list[j].Y = t;


						//t = list[j + 1].GetCounterPoint().X;
						//list[j + 1].GetCounterPoint().X = list[j].GetCounterPoint().X;
						//list[j].GetCounterPoint().X = t;

						//t = list[j + 1].GetCounterPoint().Y;
						//list[j + 1].GetCounterPoint().Y = list[j].GetCounterPoint().Y;
						//list[j].GetCounterPoint().Y = t;
						//swaped = true;


						//LinkPoint clp1 = list[j].GetCounterPoint();
						//LinkPoint clp2 = list[j+1].GetCounterPoint();

						//list[j].SetCounterPoint(clp2.X, clp2.Y);
						//list[j+1].SetCounterPoint(clp1.X, clp1.Y);

						//LinkPoint aux = list[j];
						//list[j] = list[j + 1];
						//list[j + 1] = aux;

						//LinkPoint temp = list[j];
						//ItemLink aux = list[j].Link;
						//list[j].Link = list[j + 1].Link;
						//list[j + 1].Link = aux;

					}
				}
			}
		}

		private void OnLinkDirectionChanged(ItemLink link, LinkDirection newDirection)
		{
			link.Invalidated = true;
			DiagramItem destination = link.Destination;
			DiagramItem source = link.Source;
			switch (link.Direction) {
				case LinkDirection.None:
					source.LinkPointsNone.Remove(link.SourcePoint);
					destination.LinkPointsNone.Remove(link.DestinationPoint);
					source.LinksNone.Remove(link);
					destination.LinksNone.Remove(link);
					foreach (ItemLink l in source.LinksNone)
						l.Invalidated = true;
					foreach (ItemLink l in destination.LinksNone)
						l.Invalidated = true;
					break;
				case LinkDirection.SourceNorthDestinationSouth:
					source.LinkPointsNorth.Remove(link.SourcePoint);
					destination.LinkPointsSouth.Remove(link.DestinationPoint);
					source.LinksNorth.Remove(link);
					destination.LinksSouth.Remove(link);
					foreach (ItemLink l in source.LinksNorth)
						l.Invalidated = true;
					foreach (ItemLink l in destination.LinksSouth)
						l.Invalidated = true;
					break;
				case LinkDirection.SourceSouthDestinationNorth:
					source.LinkPointsSouth.Remove(link.SourcePoint);
					destination.LinkPointsNorth.Remove(link.DestinationPoint);
					source.LinksSouth.Remove(link);
					destination.LinksNorth.Remove(link);
					foreach (ItemLink l in source.LinksSouth)
						l.Invalidated = true;
					foreach (ItemLink l in destination.LinksNorth)
						l.Invalidated = true;
					break;
				case LinkDirection.SourceWestDestinationEast:
					source.LinkPointsWest.Remove(link.SourcePoint);
					destination.LinkPointsEast.Remove(link.DestinationPoint);
					source.LinksWest.Remove(link);
					destination.LinksEast.Remove(link);
					foreach (ItemLink l in source.LinksWest)
						l.Invalidated = true;
					foreach (ItemLink l in destination.LinksEast)
						l.Invalidated = true;
					break;
				case LinkDirection.SourceEastDestinationWest:
					source.LinkPointsEast.Remove(link.SourcePoint);
					destination.LinkPointsWest.Remove(link.DestinationPoint);
					source.LinksEast.Remove(link);
					destination.LinksWest.Remove(link);
					foreach (ItemLink l in source.LinksEast)
						l.Invalidated = true;
					foreach (ItemLink l in destination.LinksWest)
						l.Invalidated = true;
					break;
			}


			switch (newDirection) {
				case LinkDirection.None:
					source.LinkPointsNone.Add(link.SourcePoint);
					destination.LinkPointsNone.Add(link.DestinationPoint);
					source.LinksNone.Add(link);
					destination.LinksNone.Add(link);
					break;
				case LinkDirection.SourceNorthDestinationSouth:
					source.LinkPointsNorth.Add(link.SourcePoint);
					destination.LinkPointsSouth.Add(link.DestinationPoint);
					source.LinksNorth.Add(link);
					destination.LinksSouth.Add(link);
					foreach (ItemLink l in destination.LinksSouth)
						l.Invalidated = true;
					foreach (ItemLink l in source.LinksNorth)
						l.Invalidated = true;
					break;
				case LinkDirection.SourceSouthDestinationNorth:
					source.LinkPointsSouth.Add(link.SourcePoint);
					destination.LinkPointsNorth.Add(link.DestinationPoint);
					source.LinksSouth.Add(link);
					destination.LinksNorth.Add(link);
					foreach (ItemLink l in destination.LinksNorth)
						l.Invalidated = true;
					foreach (ItemLink l in source.LinksSouth)
						l.Invalidated = true;
					break;
				case LinkDirection.SourceWestDestinationEast:
					source.LinkPointsWest.Add(link.SourcePoint);
					destination.LinkPointsEast.Add(link.DestinationPoint);
					source.LinksWest.Add(link);
					destination.LinksEast.Add(link);
					foreach (ItemLink l in destination.LinksEast)
						l.Invalidated = true;
					foreach (ItemLink l in source.LinksWest)
						l.Invalidated = true;
					break;
				case LinkDirection.SourceEastDestinationWest:
					source.LinkPointsEast.Add(link.SourcePoint);
					destination.LinkPointsWest.Add(link.DestinationPoint);
					source.LinksEast.Add(link);
					destination.LinksWest.Add(link);
					foreach (ItemLink l in destination.LinksWest)
						l.Invalidated = true;
					foreach (ItemLink l in source.LinksEast)
						l.Invalidated = true;
					break;
			}
			if (this.LinkDirectionChangedEvent != null)
				LinkDirectionChangedEvent(link, new LinkDirectionChangedArg(newDirection, link.Direction));
		}

		internal void Draw(Graphics graphics)
		{
			RectangleF rct = graphics.ClipBounds;
			if (linkStyle == LinkStyle.SingleLine)
				graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			else
				graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
			foreach (ItemLink link in links) {
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
				ArangeLinksForItem(item);
		}

	}
}

