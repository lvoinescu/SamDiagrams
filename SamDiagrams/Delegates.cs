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

namespace SamDiagrams
{
	/// <summary>
	/// Description of Delegates.
	/// </summary>
	public delegate void SelectedItemsChangedHandler(object sender, SelectedItemsChangedArgs e);
	public delegate void BeforeNodeExpandOrCollapseHandler(object sender, BeforeNodeExpandOrCollapseArg e);
	public delegate void ZoomFactorChangedHandler(object sender, ZoomFactorChangedArg e);
	public delegate void LinkDirectionChangedHandler(object sender, LinkDirectionChangedArg e);
	public delegate void ItemsMovedHandler(object sender, ItemsMovedEventArg e);
	public delegate void ItemMovedHandler(object sender, ItemMovedEventArg e);
	public delegate void ItemResizedHandler(object sender, ItemResizedEventArg e);
	public delegate void DiagramItemClickHandler(object sender, EventArgs e);
}
