using System;
using System.Collections.Generic;
using System.Drawing;

using System.Windows.Forms;
using SamDiagrams.Actions;
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
		List<Drawing> items;
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
			items = new List<Drawing>();
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
			LinkDrawing linkDrawer = new LinkDrawing(link, lineWidth, selectedLineWidth, LinkStyle.SingleLine);
			containerDrawer.ModelToDrawer[link] = linkDrawer;
			containerDrawer.Drawings.Add(linkDrawer);
			
			StructureDrawing sourceDrawing = (StructureDrawing)containerDrawer.ModelToDrawer[link.Source];
			StructureDrawing destinationDrawing = (StructureDrawing)containerDrawer.ModelToDrawer[link.Destination];
			
			if (!items.Contains(sourceDrawing)) {
				items.Add(sourceDrawing);
			}
			
			if (!items.Contains(destinationDrawing)) {
				items.Add(destinationDrawing);
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
			foreach (StructureDrawing item in items)
				linkStrategy.DirectLinks(item);
		}

	}
}

