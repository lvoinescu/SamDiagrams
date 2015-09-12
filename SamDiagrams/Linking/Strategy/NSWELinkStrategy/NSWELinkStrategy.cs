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
using System.Collections.Generic;
using SamDiagrams.Drawers.Links;
using SamDiagrams.Drawings;
using SamDiagrams.Drawings.Link;
using SamDiagrams.Drawings.Selection;
using SamDiagrams.Linking.Strategy;
using SamDiagrams.Model;

namespace SamDiagrams.Linking.Strategy.NSWELinkStrategy
{
	/// <summary>
	/// Description of LinkStrategy.
	/// </summary>
	public class NSWELinkStrategy : ILinkStrategy
	{
		
		public event LinkDirectionChangedHandler LinkDirectionChangedEvent;

		private Dictionary<IDrawing, NSWEDrawing> virtualMapping;

 
		public NSWELinkStrategy()
		{
			virtualMapping = new Dictionary<IDrawing, NSWEDrawing>();
		}

		public LinkDrawing CreateLink(ILink link, int lineWidth, int selectedLineWidth, LinkStyle linkStyle)
		{
			LinkDrawing linkDrawing = new LinkDrawing(link, lineWidth, selectedLineWidth, linkStyle);
			return linkDrawing;
		}
		
		public void DirectLinks(IDrawing drawing)
		{
 
			foreach (IDrawing subcomponent in drawing.Components) {
				DirectLinks(subcomponent);
			}
			
			IDrawing linkableDrawing = drawing;
			if (drawing is SelectableDrawing)
				linkableDrawing = (drawing as SelectableDrawing).Drawing;
			
			if (!virtualMapping.ContainsKey(linkableDrawing))
				return;
			
			NSWEDrawing nsweItem = virtualMapping[linkableDrawing];
			foreach (LinkDrawing link in nsweItem.Links) {
				LinkDirection prevDirection = link.Direction;
				LinkDirection newDirection = CardinalDirectionUtils.GetClosestDirectionPoints(link);
				
				if (!newDirection.Equals(link.Direction)) {
					link.Invalidated = true;
					OnLinkDirectionChanged(link, link.Direction, newDirection);
					link.Direction = newDirection;
					link.DestinationPoint.Direction = newDirection.From;
					link.SourcePoint.Direction = newDirection.To;
				}
				
				ArangeLinksForItem(link.DestinationDrawing);
				ArangeLinksForItem(link.SourceDrawing);
			}
			arrangeConnectionPoints(linkableDrawing);
		}
		
		public void ArangeLinksForItem(IDrawing structureDrawing)
		{
			arrangeConnectionPoints(structureDrawing);
			arrangeLinkPoints(structureDrawing);
		}
		
		public void RegisterLink(LinkDrawing link)
		{
			
			NSWEDrawing nsweDestination = registerDrawing(link.DestinationDrawing);
			NSWEDrawing nsweSource = registerDrawing(link.SourceDrawing);
		
			nsweSource.OutputLinkList.Add(link);
			nsweDestination.InputLinkList.Add(link);

			nsweSource.Links.Add(link);
			nsweDestination.Links.Add(link);
		}
		
		private NSWEDrawing registerDrawing(ILinkableDrawing drawing)
		{
			NSWEDrawing nsweDrawing;
			if (!virtualMapping.ContainsKey(drawing)) {
				nsweDrawing = new NSWEDrawing(drawing);
				virtualMapping[drawing] = nsweDrawing;
			} else
				nsweDrawing = virtualMapping[drawing];
			return nsweDrawing;
		}
		
		/// <summary>
		/// This method arrange all the points of the links attached to the IDrawing
		/// </summary>
		/// <param name="item"></param>
		private void arrangeLinkPoints(IDrawing item)
		{
			int drawingNorthPoints = 0, drawingSouthPoints = 0, drawingWestPoints = 0, drawingEastPoints = 0;
			NSWEDrawing virtualItem = virtualMapping[item];
			
			foreach (LinkDrawing link in virtualItem.InputLinkList) {
				switch (link.Direction.To) {
					case CardinalDirection.North:
						drawingSouthPoints++;
						link.DestinationPoint.X = item.Location.X
						+ drawingSouthPoints * item.Size.Width / (virtualItem.LinkPointsSouth.Count + 1);
						link.DestinationPoint.Y = item.Location.Y + item.Size.Height;
						break;
					case CardinalDirection.South:
						drawingNorthPoints++;
						link.DestinationPoint.X = item.Location.X
						+ drawingNorthPoints * item.Size.Width / (virtualItem.LinkPointsNorth.Count + 1);
						link.DestinationPoint.Y = item.Location.Y;
						break;
					case CardinalDirection.West:
						drawingEastPoints++;
						link.DestinationPoint.X = item.Location.X + item.Size.Width;
						link.DestinationPoint.Y = item.Location.Y
						+ drawingEastPoints * item.Size.Height / (virtualItem.LinkPointsEast.Count + 1);
						break;
					case CardinalDirection.East:
						drawingWestPoints++;
						link.DestinationPoint.X = item.Location.X;
						link.DestinationPoint.Y = item.Location.Y
						+ drawingWestPoints * item.Size.Height / (virtualItem.LinkPointsWest.Count + 1);
						break;
					default:
						link.DestinationPoint.X = item.Location.X + item.Size.Width / 2;
						link.DestinationPoint.Y = item.Location.Y + item.Size.Height / 2;
						break;
				}
			}

			foreach (LinkDrawing link in virtualItem.OutputLinkList) {
				switch (link.Direction.From) {
					case CardinalDirection.South:
						drawingNorthPoints++;
						link.SourcePoint.X = item.Location.X
						+ drawingNorthPoints * item.Size.Width / (virtualItem.LinkPointsNorth.Count + 1);
						link.SourcePoint.Y = item.Location.Y;
						break;
					case CardinalDirection.North:
						drawingSouthPoints++;
						link.SourcePoint.X = item.Location.X
						+ drawingSouthPoints * item.Size.Width / (virtualItem.LinkPointsSouth.Count + 1);
						link.SourcePoint.Y = item.Location.Y + item.Size.Height;
						break;
					case CardinalDirection.East:
						drawingWestPoints++;
						link.SourcePoint.X = item.Location.X;
						link.SourcePoint.Y = item.Location.Y
						+ drawingWestPoints * item.Size.Height / (virtualItem.LinkPointsWest.Count + 1);
						break;
					case CardinalDirection.West:
						drawingEastPoints++;
						link.SourcePoint.X = item.Location.X + item.Size.Width;
						link.SourcePoint.Y = item.Location.Y
						+ drawingEastPoints * item.Size.Height / (virtualItem.LinkPointsEast.Count + 1);
						break;
					default:
						link.SourcePoint.X = item.Location.X + item.Size.Width / 2;
						link.SourcePoint.Y = item.Location.Y + item.Size.Height / 2;
						break;
				}

			}
			arrangeConnectionPoints(item);
		}
		
		private void OnLinkDirectionChanged(LinkDrawing link, LinkDirection previousDirection, LinkDirection newDirection)
		{
			
			IDrawing sourceDrawing = link.SourceDrawing;
			IDrawing destinationDrawing = link.DestinationDrawing;
			
			link.Invalidated = true;
			NSWEDrawing source = virtualMapping[sourceDrawing];
			NSWEDrawing destination = virtualMapping[destinationDrawing];
			
			switch (previousDirection.From) {
				case CardinalDirection.None:
					source.LinkPointsNone.Remove(link.SourcePoint);
					source.LinksNone.Remove(link);
					break;
				case CardinalDirection.North:
					source.LinkPointsSouth.Remove(link.SourcePoint);
					source.LinksSouth.Remove(link);
					break;
				case CardinalDirection.South:
					source.LinkPointsNorth.Remove(link.SourcePoint);
					source.LinksNorth.Remove(link);
					break;
				case CardinalDirection.West:
					source.LinkPointsEast.Remove(link.SourcePoint);
					source.LinksEast.Remove(link);
					break;
				case CardinalDirection.East:
					source.LinkPointsWest.Remove(link.SourcePoint);
					source.LinksWest.Remove(link);
					break;
			}
			
			switch (previousDirection.To) {
				case CardinalDirection.None:
					destination.LinkPointsNone.Remove(link.DestinationPoint);
					destination.LinksNone.Remove(link);
					break;
				case CardinalDirection.South:
					destination.LinkPointsNorth.Remove(link.DestinationPoint);
					destination.LinksNorth.Remove(link);
					break;
				case CardinalDirection.North:
					destination.LinkPointsSouth.Remove(link.DestinationPoint);
					destination.LinksSouth.Remove(link);
					break;
				case CardinalDirection.East:
					destination.LinkPointsWest.Remove(link.DestinationPoint);
					destination.LinksWest.Remove(link);
					break;
				case CardinalDirection.West:
					destination.LinkPointsEast.Remove(link.DestinationPoint);
					destination.LinksEast.Remove(link);
					break;
			}


			switch (newDirection.From) {
				case CardinalDirection.None:
					source.LinkPointsNone.Add(link.SourcePoint);
					source.LinksNone.Add(link);
					break;
				case CardinalDirection.North:
					source.LinkPointsSouth.Add(link.SourcePoint);
					source.LinksSouth.Add(link);
					break;
				case CardinalDirection.South:
					source.LinkPointsNorth.Add(link.SourcePoint);
					source.LinksNorth.Add(link);
					break;
				case CardinalDirection.West:
					source.LinkPointsEast.Add(link.SourcePoint);
					source.LinksEast.Add(link);
					break;
				case CardinalDirection.East:
					source.LinkPointsWest.Add(link.SourcePoint);
					source.LinksWest.Add(link);
					break;
			}
			
			switch (newDirection.To) {
				case CardinalDirection.None:
					destination.LinkPointsNone.Add(link.DestinationPoint);
					destination.LinksNone.Add(link);
					break;
				case CardinalDirection.North:
					destination.LinkPointsSouth.Add(link.DestinationPoint);
					destination.LinksSouth.Add(link);
					break;
				case CardinalDirection.South:
					destination.LinkPointsNorth.Add(link.DestinationPoint);
					destination.LinksNorth.Add(link);
					break;
				case CardinalDirection.West:
					destination.LinkPointsEast.Add(link.DestinationPoint);
					destination.LinksEast.Add(link);
					break;
				case CardinalDirection.East:
					destination.LinkPointsWest.Add(link.DestinationPoint);
					destination.LinksWest.Add(link);
					break;
			}
			
			if (this.LinkDirectionChangedEvent != null)
				LinkDirectionChangedEvent(link, new LinkDirectionChangedArg(link, newDirection, link.Direction));
		}
		
		private void SortCounterPoints(List<CardinalLinkPoint> list)
		{

			for (int i = 0; i < list.Count; i++) {
				for (int j = 0; j < list.Count; j++) {
					LinkPoint counterPoint = list[i].GetCounterPoint();
					LinkPoint point = list[i];
					bool counterPointGreater = list[i].GetCounterPoint()
						.CompareTo(list[j].GetCounterPoint()) > 0;
					bool pointGreater = list[i].CompareTo(list[j]) > 0;
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
		
		private void arrangeConnectionPoints(IDrawing diagramItem)
		{
			
			NSWEDrawing item = virtualMapping[diagramItem];
			if (item.LinkPointsSouth.Count > 1)
				SortCounterPoints(item.LinkPointsSouth);
			if (item.LinkPointsNorth.Count > 1)
				SortCounterPoints(item.LinkPointsNorth);
			if (item.LinkPointsWest.Count > 1)
				SortCounterPoints(item.LinkPointsWest);
			if (item.LinkPointsEast.Count > 1)
				SortCounterPoints(item.LinkPointsEast);
		}
		
	}
}