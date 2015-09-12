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

namespace SamDiagrams.Drawings.Link
{
	
	public enum LinkAttachMode
	{
		LEFT,
		RIGHT,
		TOP,
		BOTTOM,
		RANDOM,
		LEFT_RIGHT,
		TOP_BOTTOM,
		ALL

	}
	
	/// <summary>
	/// Description of ILinkableDrawing.
	/// </summary>
	public interface ILinkableDrawing : IDrawing
	{
		LinkAttachMode LinkAttachMode{ get; }
		List<LinkDrawing> DrawingLinks{ get; }
	}
}
