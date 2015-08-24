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
using SamDiagrams.Model;

namespace SamDiagrams.Linking.Strategy.NSWELinkStrategy
{
	/// <summary>
	/// Description of NSWEDiagramItem.
	/// </summary>
	public class NSWEDrawing : IDrawing
	{
		
		private List<LinkDrawing> linksNorth, linksSouth, linksWest, linksEast, linksNone;
		private List<LinkPoint> linkPointsSouth, linkPointsNorth, linkPointsWest, linkPointsEast, linkPointsNone;
		private List<LinkDrawing> inputLinkList, outputLinkList, links;
		private readonly IDrawing structureDrawing;

		#region IDrawing implementation
		public void Draw(System.Drawing.Graphics graphics)
		{
			structureDrawing.Draw(graphics);
		}
		public Item Item {
			get {
				return this.structureDrawing.Item;
			}
		}
		public bool Invalidated {
			get {
				throw new NotImplementedException();
			}
			set {
				throw new NotImplementedException();
			}
		}
		#endregion
		#region IBoundedShape implementation
		public System.Drawing.Point Location {
			get {
				return this.structureDrawing.Location;
			}
			set {
				this.structureDrawing.Location = value;
			}
		}
		public System.Drawing.Size Size {
			get {
				return this.structureDrawing.Size;
			}
		}
		public System.Drawing.Rectangle Bounds {
			get {
				return this.structureDrawing.Bounds;
			}
		}
		#endregion
		public List<LinkDrawing> Links {
			get { return links; }
			set { links = value; }
		}
		
		internal List<LinkDrawing> InputLinkList {
			get { return inputLinkList; }
			set { inputLinkList = value; }
		}

		internal List<LinkDrawing> OutputLinkList {
			get { return outputLinkList; }
			set { outputLinkList = value; }
		}

		public List<LinkDrawing> LinksNone {
			get { return linksNone; }
			set { linksNone = value; }
		}

		internal List<LinkDrawing> LinksEast {
			get { return linksEast; }
			set { linksEast = value; }
		}

		internal List<LinkDrawing> LinksWest {
			get { return linksWest; }
			set { linksWest = value; }
		}

		internal List<LinkDrawing> LinksSouth {
			get { return linksSouth; }
			set { linksSouth = value; }
		}

		internal List<LinkDrawing> LinksNorth {
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
		
		public NSWEDrawing(IDrawing structureDrawing)
		{
			this.structureDrawing = structureDrawing;
			links = new List<LinkDrawing>();
			
			inputLinkList = new List<LinkDrawing>();
			outputLinkList = new List<LinkDrawing>();
			
			linksNone = new List<LinkDrawing>();
			linksNorth = new List<LinkDrawing>();
			linksSouth = new List<LinkDrawing>();
			linksWest = new List<LinkDrawing>();
			linksEast = new List<LinkDrawing>();
			
			linkPointsNone = new List<LinkPoint>();
			linkPointsNorth = new List<LinkPoint>();
			linkPointsSouth = new List<LinkPoint>();
			linkPointsWest = new List<LinkPoint>();
			LinkPointsEast = new List<LinkPoint>();
		}
	}
}
