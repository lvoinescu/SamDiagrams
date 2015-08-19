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
using System.Drawing;

using SamDiagrams.Drawers;
using SamDiagrams.Drawers.Links;
using SamDiagrams.Drawings;
using SamDiagrams.Linking.Strategy;
using SamDiagrams.Linking.Strategy.NSWELinkStrategy;

namespace SamDiagrams.Linking.Orchestrator
{



	public partial class LinkOrchestrator : ILinkOrchestrator
	{
		ContainerDrawer containerDrawer;
		LinkStyle linkStyle = LinkStyle.SingleLine;
		readonly List<StructureDrawing> sturctureDrawings;
		List<StructureLink> links = new List<StructureLink>();
		internal int lineWidth = 1;
		internal int selectedLineWidth = 9;
		internal ILinker linkStrategy;
		Dictionary<Structure, List<StructureLink>> structureLinks;
		
		
		public LinkStyle LinkStyle {
			get { return linkStyle; }
			set {
				linkStyle = value;
			}
		}

		public List<StructureLink> Links {
			get { return links; }
			set { links = value; }
		}
		
		
		public LinkOrchestrator(ContainerDrawer containerDrawer)
		{
			sturctureDrawings = new List<StructureDrawing>();
			linkStrategy = new NSWELinkStrategy();
			structureLinks = new Dictionary<Structure, List<StructureLink>>();
			this.containerDrawer = containerDrawer;
			containerDrawer.ItemsMoved+= new ItemsMovedHandler(OnItemsMoved);
		}

		public void AddLink(Structure source, Structure destination)
		{
			StructureLink link = new StructureLink(source, destination);
			RegisterLink(link);
		}

		public void AddLink(Structure source, Structure destination, Color color)
		{
			StructureLink link = new StructureLink(source, destination, color);
			RegisterLink(link);
		}
		
		private void RegisterLink(StructureLink link)
		{
			Structure source = link.Source;
			Structure destination = link.Destination;
			
			source.links.Add(link);
			destination.links.Add(link);
			
			linkStrategy.RegisterLink(link);
			links.Add(link);
			LinkDrawing linkDrawer = new LinkDrawing(link, lineWidth, selectedLineWidth, LinkStyle.StreightLines);
			containerDrawer.ModelToDrawer[link] = linkDrawer;
			containerDrawer.Drawings.Add(linkDrawer);
			
			StructureDrawing sourceDrawing = (StructureDrawing)containerDrawer.ModelToDrawer[link.Source];
			StructureDrawing destinationDrawing = (StructureDrawing)containerDrawer.ModelToDrawer[link.Destination];
			
			if (!sturctureDrawings.Contains(sourceDrawing)) {
				sturctureDrawings.Add(sourceDrawing);
			}
			
			if (!sturctureDrawings.Contains(destinationDrawing)) {
				sturctureDrawings.Add(destinationDrawing);
			}
			
			linkStrategy.DirectLinks(sourceDrawing);
			linkStrategy.DirectLinks(destinationDrawing);
		}

		private void OnItemResized(object sender, ItemResizedEventArg e)
		{
			linkStrategy.DirectLinks((StructureDrawing)sender);
		}

		public void OnItemsMoved(object sender, ItemsMovedEventArg e)
		{
			foreach (StructureDrawing structureDrawing in e.Items) {
				linkStrategy.DirectLinks(structureDrawing);
			}
		}

		internal void ArrangeAllLinks()
		{
			foreach (StructureDrawing item in sturctureDrawings)
				linkStrategy.DirectLinks(item);
		}

	}
}

