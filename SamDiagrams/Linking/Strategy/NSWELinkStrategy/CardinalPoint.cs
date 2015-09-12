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

namespace SamDiagrams.Linking.Strategy.NSWELinkStrategy
{
	/// <summary>
	/// Description of CardinalDirection.
	/// </summary>
	public enum CardinalPoint
	{
		North,
		South,
		West,
		East,
		None}
	;
	
	
	public static class CardinalPointUtils
	{
		public static bool AreOpposite(CardinalPoint c1, CardinalPoint c2)
		{
			return (
			    (c1 == CardinalPoint.West && c2 == CardinalPoint.East) ||
			    (c2 == CardinalPoint.West && c1 == CardinalPoint.East) ||
			    (c1 == CardinalPoint.North && c2 == CardinalPoint.South) ||
			    (c2 == CardinalPoint.North && c1 == CardinalPoint.South));
		}
	}
}
