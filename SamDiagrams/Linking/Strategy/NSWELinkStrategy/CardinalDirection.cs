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
	public enum CardinalDirection
	{
		North,
		South,
		West,
		East,
		None,
	}
	
	public static class CardinalDirectionUtils
	{
		public static bool AreOpposite(CardinalDirection c1, CardinalDirection c2)
		{
			return (
			    (c1 == CardinalDirection.West && c2 == CardinalDirection.East) ||
			    (c2 == CardinalDirection.West && c1 == CardinalDirection.East) ||
			    (c1 == CardinalDirection.North && c2 == CardinalDirection.South) ||
			    (c2 == CardinalDirection.North && c1 == CardinalDirection.South));
		}
	}
}


