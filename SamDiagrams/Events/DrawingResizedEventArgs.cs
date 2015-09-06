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
using SamDiagrams.Drawings;

namespace SamDiagrams.Events
{
	/// <summary>
	/// Description of DrawingResizedEventArgs.
	/// </summary>
	public class DrawingResizedEventArgs
	{
		private readonly IDrawing drawing;
		private readonly Rectangle previousBounds;
		private readonly Rectangle newBounds;

		public IDrawing Drawing {
			get {
				return drawing;
			}
		}

		public Rectangle PreviousBounds {
			get {
				return previousBounds;
			}
		}

		public Rectangle NewBounds {
			get {
				return newBounds;
			}
		}
		
		public DrawingResizedEventArgs(IDrawing drawing, Rectangle previousBounds, Rectangle newBounds)
		{
			this.drawing = drawing;
			this.previousBounds = previousBounds;
			this.newBounds = newBounds;
		}
	}
}
