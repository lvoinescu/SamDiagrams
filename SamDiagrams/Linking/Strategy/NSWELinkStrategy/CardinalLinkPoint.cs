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
using SamDiagrams.Drawers.Links;

namespace SamDiagrams.Linking.Strategy.NSWELinkStrategy
{
	/// <summary>
	/// Description of CardinalPoint.
	/// </summary>
	public class CardinalLinkPoint : LinkPoint, IComparable
	{
		private CardinalDirection direction;
		
		public CardinalDirection Direction {
			get {
				return direction;
			}
			set {
				direction = value;
			}
		}
		
		public CardinalLinkPoint(LinkDrawing linkDrawing)
			: base(linkDrawing)
		{
			this.direction = CardinalDirection.None;
		}

		public int CompareTo(object obj)
		{
			if (obj == null)
				return 1;
			CardinalLinkPoint compararisonPoint = obj as CardinalLinkPoint;
			switch (compararisonPoint.direction) {
				case CardinalDirection.North:
				case CardinalDirection.South:
					return this.X - compararisonPoint.X;
				case CardinalDirection.East:
				case CardinalDirection.West:
					return this.Y - compararisonPoint.Y;
			}
			return 0;
		}
		
		internal CardinalLinkPoint GetCounterPoint()
		{
			if (linkDrawing.DestinationPoint == this)
				return linkDrawing.SourcePoint;
			return linkDrawing.DestinationPoint;
		}
		
		public override string ToString()
		{
			return string.Format("[CardinalLinkPoint Direction={0}, (x,y) = ({1},{2})]", direction, this.X, this.Y);
		}

	}
}
