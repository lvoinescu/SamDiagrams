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
using SamDiagrams.Drawings;
using SamDiagrams.Linking.Strategy;

namespace SamDiagrams.Linking.Strategy.NSWELinkStrategy
{
	/// <summary>
	/// Description of LinkStrategy.
	/// </summary>
	public class NSWELinkStrategy : ILinker
	{
		
		public event LinkDirectionChangedHandler LinkDirectionChangedEvent;

		private Dictionary<Drawing, NSWEStructure> virtualMapping;
		
		public NSWELinkStrategy()
		{
			virtualMapping = new Dictionary<Drawing, NSWEStructure>();
		}

		
		public void DirectLinks(StructureDrawing structureDrawing)
		{
			if (!virtualMapping.ContainsKey(structureDrawing)) {
				return;
			}
			NSWEStructure nsweItem = virtualMapping[structureDrawing];
			foreach (StructureLink link in nsweItem.Links) {
				link.Invalidated = true;
				Drawing destinationDrawing = structureDrawing.Structure.DiagramContainer.ContainerDrawer.ModelToDrawer[link.Destination];
				Drawing sourceDrawing = structureDrawing.Structure.DiagramContainer.ContainerDrawer.ModelToDrawer[link.Source];
				
				LinkDirection prevDirection = link.Direction;
				LinkDirection direction = LinkDirection.None;
				if (sourceDrawing.Location.Y > destinationDrawing.Location.Y + destinationDrawing.Size.Height) {
					direction = LinkDirection.SourceNorthDestinationSouth;
				} else if (sourceDrawing.Location.Y + sourceDrawing.Size.Height < destinationDrawing.Location.Y) {
					direction = LinkDirection.SourceSouthDestinationNorth;
				} else {
					if (sourceDrawing.Location.X > destinationDrawing.Location.X + destinationDrawing.Size.Width) {
						direction = LinkDirection.SourceWestDestinationEast;
					} else if (sourceDrawing.Location.X + sourceDrawing.Size.Width < destinationDrawing.Location.X) {
						direction = LinkDirection.SourceEastDestinationWest;
					} else
						direction = LinkDirection.None;
				}
				
				if (direction != link.Direction) {
					link.Invalidated = true;
					OnLinkDirectionChanged(link, direction);
					link.Direction = direction;
				}
				
				ArangeLinksForItem((Drawing)destinationDrawing);
				ArangeLinksForItem((Drawing)sourceDrawing);
			}
			arrangeConnectionPoints(structureDrawing);
		}
		
		public void ArangeLinksForItem(Drawing structureDrawing)
		{
			arrangeConnectionPoints(structureDrawing);
			
			arrangeLinks(structureDrawing);
		}
		
		public void RegisterLink(StructureLink link)
		{
			NSWEStructure nsweDestination;
			NSWEStructure nsweSource;
			
			Drawing sourceDrawing = (Drawing)link.Source.DiagramContainer.ContainerDrawer.ModelToDrawer[link.Source];
			Drawing destinationDrawing = (Drawing)link.Destination.DiagramContainer.ContainerDrawer.ModelToDrawer[link.Destination];
			if (!virtualMapping.ContainsKey(destinationDrawing))
				nsweDestination = new NSWEStructure(link.Destination);
			else
				nsweDestination = virtualMapping[destinationDrawing];
			
			if (!virtualMapping.ContainsKey(sourceDrawing))
				nsweSource = new NSWEStructure(link.Source);
			else
				nsweSource = virtualMapping[sourceDrawing];
			
			nsweSource.OutputLinkList.Add(link);
			nsweDestination.InputLinkList.Add(link);

			nsweSource.Links.Add(link);
			nsweDestination.Links.Add(link);
			
			virtualMapping[sourceDrawing] = nsweSource;
			virtualMapping[destinationDrawing] = nsweDestination;
		}
		
		private void arrangeLinks(Drawing structure)
		{
			Drawing item = (Drawing)structure;
			int iN = 0, iS = 0, iW = 0, iE = 0;
			NSWEStructure virtualItem = virtualMapping[structure];
			foreach (StructureLink link in virtualItem.InputLinkList) {
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


			foreach (StructureLink link in virtualItem.OutputLinkList) {
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
			arrangeConnectionPoints(structure);
		}
		
		private void OnLinkDirectionChanged(StructureLink link, LinkDirection newDirection)
		{
			
			Drawing sourceDrawing = (Drawing)link.Source.DiagramContainer.ContainerDrawer.ModelToDrawer[link.Source];
			Drawing destinationDrawing = (Drawing)link.Destination.DiagramContainer.ContainerDrawer.ModelToDrawer[link.Destination];
			
			link.Invalidated = true;
			NSWEStructure source = virtualMapping[sourceDrawing];
			NSWEStructure destination = virtualMapping[destinationDrawing];
			switch (link.Direction) {
				case LinkDirection.None:
					source.LinkPointsNone.Remove(link.SourcePoint);
					destination.LinkPointsNone.Remove(link.DestinationPoint);
					source.LinksNone.Remove(link);
					destination.LinksNone.Remove(link);
					foreach (StructureLink l in source.LinksNone)
						l.Invalidated = true;
					foreach (StructureLink l in destination.LinksNone)
						l.Invalidated = true;
					break;
				case LinkDirection.SourceNorthDestinationSouth:
					source.LinkPointsNorth.Remove(link.SourcePoint);
					destination.LinkPointsSouth.Remove(link.DestinationPoint);
					source.LinksNorth.Remove(link);
					destination.LinksSouth.Remove(link);
					foreach (StructureLink l in source.LinksNorth)
						l.Invalidated = true;
					foreach (StructureLink l in destination.LinksSouth)
						l.Invalidated = true;
					break;
				case LinkDirection.SourceSouthDestinationNorth:
					source.LinkPointsSouth.Remove(link.SourcePoint);
					destination.LinkPointsNorth.Remove(link.DestinationPoint);
					source.LinksSouth.Remove(link);
					destination.LinksNorth.Remove(link);
					foreach (StructureLink l in source.LinksSouth)
						l.Invalidated = true;
					foreach (StructureLink l in destination.LinksNorth)
						l.Invalidated = true;
					break;
				case LinkDirection.SourceWestDestinationEast:
					source.LinkPointsWest.Remove(link.SourcePoint);
					destination.LinkPointsEast.Remove(link.DestinationPoint);
					source.LinksWest.Remove(link);
					destination.LinksEast.Remove(link);
					foreach (StructureLink l in source.LinksWest)
						l.Invalidated = true;
					foreach (StructureLink l in destination.LinksEast)
						l.Invalidated = true;
					break;
				case LinkDirection.SourceEastDestinationWest:
					source.LinkPointsEast.Remove(link.SourcePoint);
					destination.LinkPointsWest.Remove(link.DestinationPoint);
					source.LinksEast.Remove(link);
					destination.LinksWest.Remove(link);
					foreach (StructureLink l in source.LinksEast)
						l.Invalidated = true;
					foreach (StructureLink l in destination.LinksWest)
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
					foreach (StructureLink l in destination.LinksSouth)
						l.Invalidated = true;
					foreach (StructureLink l in source.LinksNorth)
						l.Invalidated = true;
					break;
				case LinkDirection.SourceSouthDestinationNorth:
					source.LinkPointsSouth.Add(link.SourcePoint);
					destination.LinkPointsNorth.Add(link.DestinationPoint);
					source.LinksSouth.Add(link);
					destination.LinksNorth.Add(link);
					foreach (StructureLink l in destination.LinksNorth)
						l.Invalidated = true;
					foreach (StructureLink l in source.LinksSouth)
						l.Invalidated = true;
					break;
				case LinkDirection.SourceWestDestinationEast:
					source.LinkPointsWest.Add(link.SourcePoint);
					destination.LinkPointsEast.Add(link.DestinationPoint);
					source.LinksWest.Add(link);
					destination.LinksEast.Add(link);
					foreach (StructureLink l in destination.LinksEast)
						l.Invalidated = true;
					foreach (StructureLink l in source.LinksWest)
						l.Invalidated = true;
					break;
				case LinkDirection.SourceEastDestinationWest:
					source.LinkPointsEast.Add(link.SourcePoint);
					destination.LinkPointsWest.Add(link.DestinationPoint);
					source.LinksEast.Add(link);
					destination.LinksWest.Add(link);
					foreach (StructureLink l in destination.LinksWest)
						l.Invalidated = true;
					foreach (StructureLink l in source.LinksEast)
						l.Invalidated = true;
					break;
			}
			if (this.LinkDirectionChangedEvent != null)
				LinkDirectionChangedEvent(link, new LinkDirectionChangedArg(link, newDirection, link.Direction));
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
		
		private void arrangeConnectionPoints(Drawing diagramItem)
		{
			
			NSWEStructure item = virtualMapping[diagramItem];
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