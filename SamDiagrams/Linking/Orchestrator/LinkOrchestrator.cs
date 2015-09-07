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

using SamDiagrams.Drawers;
using SamDiagrams.Drawers.Links;
using SamDiagrams.Drawings;
using SamDiagrams.Drawings.Link;
using SamDiagrams.Linking.Strategy;
using SamDiagrams.Linking.Strategy.NSWELinkStrategy;
using SamDiagrams.Model;

namespace SamDiagrams.Linking.Orchestrator
{



	public class LinkOrchestrator : ILinkOrchestrator
	{
		ContainerDrawer containerDrawer;
		LinkStyle linkStyle = LinkStyle.SingleLine;
		readonly List<StructureDrawing> sturctureDrawings;
		List<LinkDrawing> links = new List<LinkDrawing>();
		internal int lineWidth = 1;
		internal int selectedLineWidth = 9;
		internal ILinker linkStrategy;
		
		
		public LinkStyle LinkStyle {
			get { return linkStyle; }
			set {
				linkStyle = value;
			}
		}

		public List<LinkDrawing> Links {
			get { return links; }
			set { links = value; }
		}
		
		
		public LinkOrchestrator(ContainerDrawer containerDrawer)
		{
			sturctureDrawings = new List<StructureDrawing>();
			linkStrategy = new NSWELinkStrategy();
			this.containerDrawer = containerDrawer;
			containerDrawer.ActionListener.ItemsMoved += new ItemsMovedHandler(OnItemsMoved);
		}

		public void AddLink(ILink link)
		{
			LinkDrawing linkDrawing = new LinkDrawing(link, lineWidth, selectedLineWidth, LinkStyle.StreightLines);
			link.Drawing = linkDrawing;
			RegisterLink(linkDrawing);
		}
		
		public void AddLinkDrawing(LinkDrawing linkDrawing)
		{
			RegisterLink(linkDrawing);
		}		
		
		
		private void RegisterLink(LinkDrawing linkDrawing)
		{
			ILinkableDrawing sourceDrawing = linkDrawing.SourceDrawing;
			ILinkableDrawing destinationDrawing = linkDrawing.DestinationDrawing;

			
			sourceDrawing.DrawingLinks.Add(linkDrawing);
			destinationDrawing.DrawingLinks.Add(linkDrawing);
			
			containerDrawer.Drawings.Insert(0, linkDrawing);
			linkStrategy.RegisterLink(linkDrawing);
			links.Add(linkDrawing);

			linkStrategy.DirectLinks(sourceDrawing);
			linkStrategy.DirectLinks(destinationDrawing);
		}

		private void OnItemResized(object sender, ItemResizedEventArg e)
		{
			linkStrategy.DirectLinks((StructureDrawing)sender);
		}

		private void OnItemsMoved(object sender, ItemsMovedEventArg e)
		{
			foreach (IDrawing drawing in e.Items) {
				linkStrategy.DirectLinks(drawing);
			}
		}

		internal void ArrangeAllLinks()
		{
			foreach (StructureDrawing item in sturctureDrawings)
				linkStrategy.DirectLinks(item);
		}

	}
}

