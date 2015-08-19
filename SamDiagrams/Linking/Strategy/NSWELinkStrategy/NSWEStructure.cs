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
	public class NSWEStructure
	{
		
		private List<StructureLink> linksNorth, linksSouth, linksWest, linksEast, linksNone;
		private List<LinkPoint> linkPointsSouth, linkPointsNorth, linkPointsWest, linkPointsEast, linkPointsNone;
		private List<StructureLink> inputLinkList, outputLinkList, links;
		
		private Structure structure;

		public List<StructureLink> Links {
			get { return links; }
			set { links = value; }
		}
		
		internal List<StructureLink> InputLinkList {
			get { return inputLinkList; }
			set { inputLinkList = value; }
		}

		internal List<StructureLink> OutputLinkList {
			get { return outputLinkList; }
			set { outputLinkList = value; }
		}

		public List<StructureLink> LinksNone {
			get { return linksNone; }
			set { linksNone = value; }
		}

		internal List<StructureLink> LinksEast {
			get { return linksEast; }
			set { linksEast = value; }
		}

		internal List<StructureLink> LinksWest {
			get { return linksWest; }
			set { linksWest = value; }
		}

		internal List<StructureLink> LinksSouth {
			get { return linksSouth; }
			set { linksSouth = value; }
		}

		internal List<StructureLink> LinksNorth {
			get { return linksNorth; }
			set { linksNorth = value; }
		}

		public List<LinkPoint> LinkPointsNone {
			get { return linkPointsNone; }
			set { linkPointsNone = value; }
		}

		internal List<LinkPoint> LinkPointsEast {
			get { return linkPointsEast; }
			set { linkPointsEast = value; }
		}

		internal List<LinkPoint> LinkPointsWest {
			get { return linkPointsWest; }
			set { linkPointsWest = value; }
		}

		internal List<LinkPoint> LinkPointsNorth {
			get { return linkPointsNorth; }
			set { linkPointsNorth = value; }
		}

		
		internal List<LinkPoint> LinkPointsSouth {
			get { return linkPointsSouth; }
			set { linkPointsSouth = value; }
		}
		public NSWEStructure(Structure item)
		{
			this.structure = item;
			links = new List<StructureLink>();
			
			inputLinkList = new List<StructureLink>();
			outputLinkList = new List<StructureLink>();
			
			linksNone = new List<StructureLink>();
			linksNorth = new List<StructureLink>();
			linksSouth = new List<StructureLink>();
			linksWest = new List<StructureLink>();
			linksEast = new List<StructureLink>();
			
			linkPointsNone = new List<LinkPoint>();
			linkPointsNorth = new List<LinkPoint>();
			linkPointsSouth = new List<LinkPoint>();
			linkPointsWest = new List<LinkPoint>();
			LinkPointsEast = new List<LinkPoint>();
		}
	}
}
