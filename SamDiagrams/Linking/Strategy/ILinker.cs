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
using SamDiagrams.Drawings;

namespace SamDiagrams.Linking.Strategy
{
	public interface ILinker
	{
		
		event LinkDirectionChangedHandler LinkDirectionChangedEvent;
		
		/// <summary>
		/// Registers a link to be handled by a link strategy.
		/// </summary>
		/// <param name="link"></param>
		void RegisterLink(StructureLink link);
		
		/// <summary>
		/// Computes the end points of all links associated with an item.
		/// This handled input links and output links.
		/// </summary>
		/// <param name="item"></param>
		void DirectLinks(StructureDrawing item);
		
		
	}
}
