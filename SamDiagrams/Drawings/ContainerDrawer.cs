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
using System.Collections.Generic;
using System.Drawing;
using SamDiagrams.Actions;
using SamDiagrams.Drawings;
using SamDiagrams.Drawings.Geometry;
using SamDiagrams.Drawings.Selection;
using SamDiagrams.Linking.Orchestrator;
using SamDiagrams.Model;

namespace SamDiagrams.Drawers
{
	/// <summary>
	/// Description of ContainerDrawer.
	/// </summary>
	public class ContainerDrawer
	{
		public event ItemsMovedHandler ItemsMoved;
		public event LinkDirectionChangedHandler LinkDirectionChanged;
		
		
		private readonly List<IDrawing> drawings;
		private readonly DiagramContainer diagramContainer;
		private readonly Dictionary<Item, StructureDrawing> modelToDrawer;
		private readonly Dictionary<StructureDrawing,Item > drawerToModel;
		private InvalidationStrategy invalidationStrategy;
		private LinkOrchestrator linkOrchestrator;
		private MoveAction moveAction;
		private SelectionAction selectAction;
		private List<SelectableDrawing> selectableDrawings;
		
		public LinkOrchestrator LinkOrchestrator {
			get {
				return linkOrchestrator;
			}
		}

		public DiagramContainer DiagramContainer {
			get {
				return diagramContainer;
			}
		}
		public List<IDrawing> Drawings {
			get {
				return drawings;
			}
		}

		public List<SelectableDrawing> SelectedDrawing {
			get {
				return selectableDrawings;
			}
			set {
				selectableDrawings = value;
			}
		}
		
		public Dictionary<Item, StructureDrawing> ModelToDrawer {
			get {
				return modelToDrawer;
			}
		}

		public Dictionary<StructureDrawing, Item> DrawerToModel {
			get {
				return drawerToModel;
			}
		}

		public ContainerDrawer(DiagramContainer diagramContainer)
		{
			
			this.diagramContainer = diagramContainer;
			linkOrchestrator = new LinkOrchestrator(this);
			modelToDrawer = new Dictionary<Item, StructureDrawing>();
			
			drawerToModel = new Dictionary<StructureDrawing, Item>();
			moveAction = new MoveAction(diagramContainer);
			
			moveAction.ItemsMoved += new ItemsMovedHandler(OnItemsMoved);
			
			selectAction = new SelectionAction(diagramContainer);
			invalidationStrategy = new InvalidationStrategy(this);

			this.drawings = new List<IDrawing>();
			this.selectableDrawings = new List<SelectableDrawing>();
		}
		
		
		public void Draw(float scaleFactor, Graphics graphics)
		{
			graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

			Point pt = new Point(diagramContainer.HScrollBar.Value, diagramContainer.VScrollBar.Value);
			Rectangle r = new Rectangle(pt, diagramContainer.Size);
			RectangleF clipRectangle = graphics.ClipBounds;
			
			graphics.TranslateTransform(-diagramContainer.HScrollBar.Value, -diagramContainer.VScrollBar.Value);
			graphics.ScaleTransform(scaleFactor, scaleFactor, System.Drawing.Drawing2D.MatrixOrder.Append);
			foreach (IDrawing drawing in drawings) {
				if (drawing.Invalidated && getDrawerScaledBounds(scaleFactor, drawing).IntersectsWith(clipRectangle)) {
					drawing.Draw(graphics);
					drawing.Invalidated = false;
				}
			}
		}
		
		
		private RectangleF getDrawerScaledBounds(float scaleFactor, IDrawing drawer)
		{
			return new RectangleF(
				(float)(drawer.Location.X * scaleFactor),
				(float)(drawer.Location.Y * scaleFactor),
				(float)(drawer.Size.Width * scaleFactor),
				(float)(drawer.Size.Height * scaleFactor)
			);
		}
		
		void OnItemsMoved(object sender, ItemsMovedEventArg e)
		{
			if (ItemsMoved != null)
				this.ItemsMoved(sender, e);
		}

	}
}
