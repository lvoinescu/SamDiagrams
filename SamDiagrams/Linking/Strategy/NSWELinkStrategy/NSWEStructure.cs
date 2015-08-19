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
