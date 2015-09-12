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

namespace SamDiagrams.Linking.Strategy.CenterLinkStrategy
{
	/// <summary>
	/// Description of CenterLinkStrategy.
	/// </summary>
	public class CenterLinkStrategy : ILinkStrategy
	{
		
		public event LinkDirectionChangedHandler LinkDirectionChangedEvent;
		
		public CenterLinkStrategy()
		{
		}

		public void RegisterLink(SamDiagrams.Drawers.Links.LinkDrawing link)
		{
			throw new NotImplementedException();
		}
		public void DirectLinks(IDrawing item)
		{
			throw new NotImplementedException();
		}		
		
		public void ArangeLinksForItem(IDrawing item)
		{
			throw new NotImplementedException();
		}

		public SamDiagrams.Drawers.Links.LinkDrawing CreateLink(SamDiagrams.Model.ILink link, int lineWidth, int selectedLineWidth, LinkStyle streightLines)
		{
			throw new NotImplementedException();
		}
	}
}
