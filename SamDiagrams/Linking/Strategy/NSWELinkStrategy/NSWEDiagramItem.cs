/*
 * Created by SharpDevelop.
 * User: Sam
 * Date: 7/27/2015
 * Time: 8:05 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace SamDiagrams.Linking.Strategy.NSWELinkStrategy
{
	/// <summary>
	/// Description of NSWEDiagramItem.
	/// </summary>
	public class NSWEDiagramItem
	{
		
		private List<Link> linksNorth, linksSouth, linksWest, linksEast, linksNone;
		private List<LinkPoint> linkPointsSouth, linkPointsNorth, linkPointsWest, linkPointsEast, linkPointsNone;
		private List<Link> inputLinkList, outputLinkList, links;
		
		private DiagramItem diagramItem;

		public List<Link> Links {
			get { return links; }
			set { links = value; }
		}
		internal List<Link> InputLinkList
		{
			get { return inputLinkList; }
			set { inputLinkList = value; }
		}

		internal List<Link> OutputLinkList
		{
			get { return outputLinkList; }
			set { outputLinkList = value; }
		}

		public List<Link> LinksNone
		{
			get { return linksNone; }
			set { linksNone = value; }
		}

		internal List<Link> LinksEast
		{
			get { return linksEast; }
			set { linksEast = value; }
		}

		internal List<Link> LinksWest
		{
			get { return linksWest; }
			set { linksWest = value; }
		}

		internal List<Link> LinksSouth
		{
			get { return linksSouth; }
			set { linksSouth = value; }
		}

		internal List<Link> LinksNorth
		{
			get { return linksNorth; }
			set { linksNorth = value; }
		}

		public List<LinkPoint> LinkPointsNone
		{
			get { return linkPointsNone; }
			set { linkPointsNone = value; }
		}

		internal List<LinkPoint> LinkPointsEast
		{
			get { return linkPointsEast; }
			set { linkPointsEast = value; }
		}

		internal List<LinkPoint> LinkPointsWest
		{
			get { return linkPointsWest; }
			set { linkPointsWest = value; }
		}

		internal List<LinkPoint> LinkPointsNorth
		{
			get { return linkPointsNorth; }
			set { linkPointsNorth = value; }
		}

		
		internal List<LinkPoint> LinkPointsSouth
		{
			get { return linkPointsSouth; }
			set { linkPointsSouth = value; }
		}
		public NSWEDiagramItem(DiagramItem item)
		{
			this.diagramItem = item;
			links = new List<Link>();
			
			inputLinkList = new List<Link>();
			outputLinkList = new List<Link>();
			
			linksNone = new List<Link>();
			linksNorth = new List<Link>();
			linksSouth = new List<Link>();
			linksWest = new List<Link>();
			linksEast = new List<Link>();
			
			linkPointsNone = new List<LinkPoint>();
			linkPointsNorth = new List<LinkPoint>();
			linkPointsSouth = new List<LinkPoint>();
			linkPointsWest = new List<LinkPoint>();
			LinkPointsEast = new List<LinkPoint>();
		}
	}
}
