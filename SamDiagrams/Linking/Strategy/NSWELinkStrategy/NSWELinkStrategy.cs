/*
 * Created by SharpDevelop.
 * User: Sam
 * Date: 7/26/2015
 * Time: 4:47 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using SamDiagrams.Linking.Strategy;

namespace SamDiagrams.Linking.Strategy.NSWELinkStrategy
{
	/// <summary>
	/// Description of LinkStrategy.
	/// </summary>
	public class NSWELinkStrategy : ILinker
	{
		
		public event LinkDirectionChangedHandler LinkDirectionChangedEvent;

		private Dictionary<DiagramItem, NSWEDiagramItem> virtualMapping;
		
		public NSWELinkStrategy()
		{
			virtualMapping  = new Dictionary<DiagramItem, NSWEDiagramItem>();
		}

		
		public void DirectLinks(DiagramItem item)
		{
			
			NSWEDiagramItem nsweItem = virtualMapping[item];
			foreach(Link link in nsweItem.Links){
				link.Invalidated = true;
				DiagramItem destinationItem = link.Destination;
				DiagramItem sourceItem = link.Source;
				
				LinkDirection prevDirection = link.Direction;
				LinkDirection direction = LinkDirection.None;
				if (sourceItem.Location.Y > destinationItem.Location.Y + destinationItem.Size.Height) {
					direction = LinkDirection.SourceNorthDestinationSouth;
				} else if (sourceItem.Location.Y + sourceItem.Size.Height < destinationItem.Location.Y) {
					direction = LinkDirection.SourceSouthDestinationNorth;
				} else {
					if (sourceItem.Location.X > destinationItem.Location.X + destinationItem.Size.Width) {
						direction = LinkDirection.SourceWestDestinationEast;
					} else if (sourceItem.Location.X + sourceItem.Size.Width < destinationItem.Location.X) {
						direction = LinkDirection.SourceEastDestinationWest;
					} else
						direction = LinkDirection.None;
				}
				
				if (direction != link.Direction) {
					link.Invalidated = true;
					OnLinkDirectionChanged(link, direction);
					link.Direction = direction;
					ArangeLinksForItem(link.Destination);
					ArangeLinksForItem(link.Source);
				}
				
				arrangeLinks(link.Source);
				arrangeLinks(link.Destination);
			}
			arrangeConnectionPoints(item);
		}
		
		public void ArangeLinksForItem(DiagramItem item)
		{
			arrangeConnectionPoints(item);
			
			arrangeLinks(item);
		}
		
		public void RegisterLink(Link link)
		{
			NSWEDiagramItem nsweDestination;
			NSWEDiagramItem nsweSource;
			if(!virtualMapping.ContainsKey(link.Destination))
				nsweDestination = new NSWEDiagramItem(link.Destination);
			else
				nsweDestination = virtualMapping[link.Destination];
			
			if(!virtualMapping.ContainsKey(link.Source))
				nsweSource = new NSWEDiagramItem(link.Source);
			else
				nsweSource = virtualMapping[link.Source];
			
			nsweSource.OutputLinkList.Add(link);
			nsweDestination.InputLinkList.Add(link);

			nsweSource.Links.Add(link);
			nsweDestination.Links.Add(link);
			
			virtualMapping[link.Source] = nsweSource;
			virtualMapping[link.Destination] = nsweDestination;
		}
		
		private void arrangeLinks(DiagramItem item){
			int iN = 0, iS = 0, iW = 0, iE = 0;
			NSWEDiagramItem virtualItem = virtualMapping[item];
			foreach (Link link in virtualItem.InputLinkList) {
				switch (link.Direction) {
					case LinkDirection.SourceNorthDestinationSouth:
						iS++;
						link.DestinationPoint.X = item.Location.X + iS * item.Size.Width / (virtualItem.LinkPointsSouth.Count + 1);
						link.DestinationPoint.Y = item.Location.Y + item.Size.Height;
						break;
					case LinkDirection.SourceSouthDestinationNorth:
						iN++;
						link.DestinationPoint.X = item.Location.X + iN * item.Size.Width / (virtualItem.LinkPointsNorth.Count + 1);
						link.DestinationPoint.Y = item.Location.Y;
						break;
					case LinkDirection.SourceWestDestinationEast:
						iE++;
						link.DestinationPoint.X = item.Location.X + item.Size.Width;
						link.DestinationPoint.Y = item.Location.Y + iE * item.Size.Height / (virtualItem.LinkPointsEast.Count + 1);
						break;
					case LinkDirection.SourceEastDestinationWest:
						iW++;
						link.DestinationPoint.X = item.Location.X;
						link.DestinationPoint.Y = item.Location.Y + iW * item.Size.Height / (virtualItem.LinkPointsWest.Count + 1);
						break;
					default:
						link.DestinationPoint.X = item.Location.X + item.Size.Width / 2;
						link.DestinationPoint.Y = item.Location.Y + item.Size.Height / 2;
						break;
				}
			}


			foreach (Link link in virtualItem.OutputLinkList) {
				switch (link.Direction) {
					case LinkDirection.SourceNorthDestinationSouth:
						iN++;
						link.SourcePoint.X = item.Location.X + iN * item.Size.Width / (virtualItem.LinkPointsNorth.Count + 1);
						link.SourcePoint.Y = item.Location.Y;
						break;
					case LinkDirection.SourceSouthDestinationNorth:
						iS++;
						link.SourcePoint.X = item.Location.X + iS * item.Size.Width / (virtualItem.LinkPointsSouth.Count + 1);
						link.SourcePoint.Y = item.Location.Y + item.Size.Height;
						break;
					case LinkDirection.SourceWestDestinationEast:
						iW++;
						link.SourcePoint.X = item.Location.X;
						link.SourcePoint.Y = item.Location.Y + iW * item.Size.Height / (virtualItem.LinkPointsWest.Count + 1);
						break;
					case LinkDirection.SourceEastDestinationWest:
						iE++;
						link.SourcePoint.X = item.Location.X + item.Size.Width;
						link.SourcePoint.Y = item.Location.Y + iE * item.Size.Height / (virtualItem.LinkPointsEast.Count + 1);
						break;
					default:
						link.SourcePoint.X = item.Location.X + item.Size.Width / 2;
						link.SourcePoint.Y = item.Location.Y + item.Size.Height / 2;
						break;
				}

			}
			arrangeConnectionPoints(item);
		}
		
		private void OnLinkDirectionChanged(Link link, LinkDirection newDirection)
		{
			link.Invalidated = true;
			NSWEDiagramItem source = virtualMapping[link.Source];
			NSWEDiagramItem destination = virtualMapping[link.Destination];
			switch (link.Direction) {
				case LinkDirection.None:
					source.LinkPointsNone.Remove(link.SourcePoint);
					destination.LinkPointsNone.Remove(link.DestinationPoint);
					source.LinksNone.Remove(link);
					destination.LinksNone.Remove(link);
					foreach (Link l in source.LinksNone)
						l.Invalidated = true;
					foreach (Link l in destination.LinksNone)
						l.Invalidated = true;
					break;
				case LinkDirection.SourceNorthDestinationSouth:
					source.LinkPointsNorth.Remove(link.SourcePoint);
					destination.LinkPointsSouth.Remove(link.DestinationPoint);
					source.LinksNorth.Remove(link);
					destination.LinksSouth.Remove(link);
					foreach (Link l in source.LinksNorth)
						l.Invalidated = true;
					foreach (Link l in destination.LinksSouth)
						l.Invalidated = true;
					break;
				case LinkDirection.SourceSouthDestinationNorth:
					source.LinkPointsSouth.Remove(link.SourcePoint);
					destination.LinkPointsNorth.Remove(link.DestinationPoint);
					source.LinksSouth.Remove(link);
					destination.LinksNorth.Remove(link);
					foreach (Link l in source.LinksSouth)
						l.Invalidated = true;
					foreach (Link l in destination.LinksNorth)
						l.Invalidated = true;
					break;
				case LinkDirection.SourceWestDestinationEast:
					source.LinkPointsWest.Remove(link.SourcePoint);
					destination.LinkPointsEast.Remove(link.DestinationPoint);
					source.LinksWest.Remove(link);
					destination.LinksEast.Remove(link);
					foreach (Link l in source.LinksWest)
						l.Invalidated = true;
					foreach (Link l in destination.LinksEast)
						l.Invalidated = true;
					break;
				case LinkDirection.SourceEastDestinationWest:
					source.LinkPointsEast.Remove(link.SourcePoint);
					destination.LinkPointsWest.Remove(link.DestinationPoint);
					source.LinksEast.Remove(link);
					destination.LinksWest.Remove(link);
					foreach (Link l in source.LinksEast)
						l.Invalidated = true;
					foreach (Link l in destination.LinksWest)
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
					foreach (Link l in destination.LinksSouth)
						l.Invalidated = true;
					foreach (Link l in source.LinksNorth)
						l.Invalidated = true;
					break;
				case LinkDirection.SourceSouthDestinationNorth:
					source.LinkPointsSouth.Add(link.SourcePoint);
					destination.LinkPointsNorth.Add(link.DestinationPoint);
					source.LinksSouth.Add(link);
					destination.LinksNorth.Add(link);
					foreach (Link l in destination.LinksNorth)
						l.Invalidated = true;
					foreach (Link l in source.LinksSouth)
						l.Invalidated = true;
					break;
				case LinkDirection.SourceWestDestinationEast:
					source.LinkPointsWest.Add(link.SourcePoint);
					destination.LinkPointsEast.Add(link.DestinationPoint);
					source.LinksWest.Add(link);
					destination.LinksEast.Add(link);
					foreach (Link l in destination.LinksEast)
						l.Invalidated = true;
					foreach (Link l in source.LinksWest)
						l.Invalidated = true;
					break;
				case LinkDirection.SourceEastDestinationWest:
					source.LinkPointsEast.Add(link.SourcePoint);
					destination.LinkPointsWest.Add(link.DestinationPoint);
					source.LinksEast.Add(link);
					destination.LinksWest.Add(link);
					foreach (Link l in destination.LinksWest)
						l.Invalidated = true;
					foreach (Link l in source.LinksEast)
						l.Invalidated = true;
					break;
			}
			if (this.LinkDirectionChangedEvent != null)
				LinkDirectionChangedEvent(link, new LinkDirectionChangedArg(newDirection, link.Direction));
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

					}
				}
			}
		}
		
		private void arrangeConnectionPoints(DiagramItem diagramItem)
		{
			
			NSWEDiagramItem item = virtualMapping[diagramItem];
			if (item.LinkPointsSouth.Count > 1)
				SortCounterPoints(item.LinkPointsSouth, LinkDirection.SourceSouthDestinationNorth);
			if (item.LinkPointsNorth.Count > 1)
				SortCounterPoints(item.LinkPointsNorth, LinkDirection.SourceNorthDestinationSouth);
			if (item.LinkPointsWest.Count > 1)
				SortCounterPoints(item.LinkPointsWest, LinkDirection.SourceWestDestinationEast);
			if (item.LinkPointsEast.Count > 1)
				SortCounterPoints(item.LinkPointsEast, LinkDirection.SourceEastDestinationWest);
		}
		
	}
}