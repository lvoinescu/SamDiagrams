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
using SamDiagrams.Drawings.Link;
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
		
		
		public static LinkDirection Create(IDrawing sourceDrawing, IDrawing destinationDrawing)
		{
			
			CardinalDirection from = CardinalDirection.None;
			CardinalDirection to = CardinalDirection.None;
			if (sourceDrawing.Location.Y > destinationDrawing.Location.Y + destinationDrawing.Size.Height) {
				from = CardinalDirection.South;
				to = CardinalDirection.North;
			} else if (sourceDrawing.Location.Y + sourceDrawing.Size.Height < destinationDrawing.Location.Y) {
				from = CardinalDirection.North;
				to = CardinalDirection.South;
			} else if (sourceDrawing.Location.X > destinationDrawing.Location.X + destinationDrawing.Size.Width) {
				from = CardinalDirection.East;
				to = CardinalDirection.West;
			} else if (sourceDrawing.Location.X + sourceDrawing.Size.Width < destinationDrawing.Location.X) {
				from = CardinalDirection.West;
				to = CardinalDirection.East;
			}
			return new LinkDirection(from, to);
		}
		
		
		public static LinkDirection GetClosestDirectionPoints(LinkDrawing linkDrawing)
		{
			ILinkableDrawing destinationDrawing = linkDrawing.DestinationDrawing;
			ILinkableDrawing sourceDrawing = linkDrawing.SourceDrawing;
			CardinalDirection[] directionsArray = new CardinalDirection[] {
				CardinalDirection.North,
				CardinalDirection.South,
				CardinalDirection.West,
				CardinalDirection.East
			};
			
			List<CardinalDirection> directions = new List<CardinalDirection>(directionsArray);
			
			CardinalDirection from = CardinalDirection.None;
			CardinalDirection to = CardinalDirection.None;
			
			if (destinationDrawing.LinkAttachMode == LinkAttachMode.LEFT_RIGHT) {
				if (destinationDrawing.Location.X > sourceDrawing.Location.X + sourceDrawing.Size.Width) {
					to = CardinalDirection.West;
				} else if (destinationDrawing.Location.X + destinationDrawing.Size.Width < sourceDrawing.Location.X) {
					to = CardinalDirection.East;
				}
			} else if (destinationDrawing.LinkAttachMode == LinkAttachMode.ALL) {
				if (destinationDrawing.Location.Y > sourceDrawing.Location.Y + sourceDrawing.Size.Height) {
					to = CardinalDirection.South;
				} else if (destinationDrawing.Location.Y + destinationDrawing.Size.Height < sourceDrawing.Location.Y) {
					to = CardinalDirection.North;
				} else if (destinationDrawing.Location.X > sourceDrawing.Location.X + sourceDrawing.Size.Width) {
					to = CardinalDirection.East;
				} else if (destinationDrawing.Location.X + destinationDrawing.Size.Width < sourceDrawing.Location.X) {
					to = CardinalDirection.West;
				}
			}
			
			
			if (sourceDrawing.LinkAttachMode == LinkAttachMode.ALL) {
				if (sourceDrawing.Location.Y > destinationDrawing.Location.Y + destinationDrawing.Size.Height) {
					from = CardinalDirection.South;
				} else if (sourceDrawing.Location.Y + sourceDrawing.Size.Height < destinationDrawing.Location.Y) {
					from = CardinalDirection.North;
				} else if (sourceDrawing.Location.X > destinationDrawing.Location.X + destinationDrawing.Size.Width) {
					from = CardinalDirection.East;
				} else if (sourceDrawing.Location.X + sourceDrawing.Size.Width < destinationDrawing.Location.X) {
					from = CardinalDirection.West;
				}
			} else if (sourceDrawing.LinkAttachMode == LinkAttachMode.LEFT_RIGHT) {
				if (sourceDrawing.Location.X > destinationDrawing.Location.X + destinationDrawing.Size.Width) {
					from = CardinalDirection.East;
				} else if (sourceDrawing.Location.X + sourceDrawing.Size.Width < destinationDrawing.Location.X) {
					from = CardinalDirection.West;
				}
			}
			return new LinkDirection(from, to);
		}
	}
}


