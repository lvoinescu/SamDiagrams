﻿/*
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
using SamDiagrams.Drawings.Selection;

namespace SamDiagrams.Actions
{
	/// <summary>
	/// Description of ActionListener.
	/// </summary>
	public class ActionListener
	{
		private MoveAction moveAction;
		private SelectionAction selectAction;
		private DiagramContainer container;
		
		public event ItemsMovedHandler ItemsMoved;
		public event SelectedItemsChangedHandler SelectionChanged;
		
		public ActionListener(DiagramContainer container)
		{
			this.container = container;
			moveAction = new MoveAction(container);
			moveAction.ItemsMoved += new ItemsMovedHandler(OnItemsMoved);
			selectAction = new SelectionAction(container);
			selectAction.SelectedItemsChanged += new SelectedItemsChangedHandler(OnSelectionChanged);
			this.container.MouseDown += new System.Windows.Forms.MouseEventHandler(OnMouseDown);
			this.container.MouseUp += new System.Windows.Forms.MouseEventHandler(OnMouseUp);
			this.container.MouseMove += new System.Windows.Forms.MouseEventHandler(OnMouseMove);
		}

		void OnSelectionChanged(object sender, SelectedItemsChangedArgs e)
		{
			moveAction.ClearDrawing();
			foreach (SelectableDrawing selectedDrawing in e.SelectedDrawings) {
				moveAction.AddDrawing(selectedDrawing);
			}
			SelectionChanged(this, e);
		}
		
		void OnMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			selectAction.OnMouseDown(sender, e);
			moveAction.OnMouseDown(sender, e);
		}

		void OnMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			selectAction.OnMouseUp(sender, e);
			moveAction.OnMouseUp(sender, e);
		}

		void OnMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			moveAction.OnMouseMove(sender, e);
		}

		void OnItemsMoved(object sender, ItemsMovedEventArg e)
		{
			if (ItemsMoved != null)
				ItemsMoved(sender, e);
		}
	}
}
