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
using System.Drawing;

namespace SamDiagrams.Model.Link
{
	/// <summary>
	/// Description of Link.
	/// </summary>
	public class Link : ILink
	{

		private ILinkable source;
		private ILinkable destination;
		private String name;
		private Color color;
		
		public Item Source {
			get {
				return source;
			}
		}

		public Item Destination {
			get {
				return destination;
			}
		}

		public string Name {
			get {
				return this.name;
			}
			set {
				this.name = value;
			}
		}

		public Color Color {
			get {
				return color;
			}
			set {
				color = value;
			}
		}

		public Link(ILinkable source, ILinkable destination, Color color)
		{
			this.source = source;
			this.destination = destination;
			this.color = color;
		}
	}
}
