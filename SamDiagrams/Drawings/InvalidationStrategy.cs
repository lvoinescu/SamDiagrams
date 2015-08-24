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
using System.Windows.Forms;
using SamDiagrams.Drawers.Links;
using SamDiagrams.Drawings;
using SamDiagrams.Drawings.Geometry;
using SamDiagrams.Drawings.Link;
using SamDiagrams.Drawings.Selection;
using SamDiagrams.Linking;
using SamDiagrams.Model;

namespace SamDiagrams.Drawers
{
	/// <summary>
	/// Description of InvalidationStrategy.
	/// </summary>
	public class InvalidationStrategy
	{
		
		readonly ContainerDrawer containerDrawer;
		private Rectangle previouslyInvalidatedRectangle;
		
		
		public InvalidationStrategy(ContainerDrawer container)
		{
			this.containerDrawer = container;
			this.containerDrawer.DiagramContainer.MouseDown += OnMouseDown;
			this.containerDrawer.ItemsMoved += new ItemsMovedHandler(OnItemsMoved);
			this.containerDrawer.LinkOrchestrator.linkStrategy.LinkDirectionChangedEvent +=
				new LinkDirectionChangedHandler(OnLinkDirectionChanged);
		}

		void OnMouseDown(object sender, MouseEventArgs e)
		{
			foreach (IDrawing drawer in containerDrawer.Drawings) {
				drawer.Invalidated = true;
			}
		}


		void OnItemsMoved(object sender, ItemsMovedEventArg e)
		{
			foreach (IDrawing drawer in e.Items) {
				InvalidateStructureDrawing(drawer);
			}
		}

		void OnLinkDirectionChanged(object sender, LinkDirectionChangedArg e)
		{
			InvalidateLinkDrawing((LinkDrawing)e.Link);
		}
		
		void InvalidateStructureDrawing(IDrawing drawer)
		{
			InflatableRectangle newRectangleToInvalidate = new InflatableRectangle(drawer.Bounds);
			newRectangleToInvalidate.Inflate(getStructureInvalidatedRegion(drawer));
			Rectangle auxRectangle = newRectangleToInvalidate.Bounds;
			invalidateOverlappingDrawings(previouslyInvalidatedRectangle);
			newRectangleToInvalidate.Inflate(previouslyInvalidatedRectangle);
			previouslyInvalidatedRectangle = auxRectangle;
			
			invalidateOverlappingDrawings(previouslyInvalidatedRectangle);
			
			containerDrawer.DiagramContainer.Invalidate(newRectangleToInvalidate.Bounds);
			containerDrawer.DiagramContainer.Validate();
		}
		
		void InvalidateLinkDrawing(LinkDrawing linkDrawing)
		{
			InflatableRectangle newRectangleToInvalidate = new InflatableRectangle(linkDrawing.Bounds);
			newRectangleToInvalidate.Inflate(getLinkInvalidatedRegion(linkDrawing));
			newRectangleToInvalidate.Inflate(
				getStructureInvalidatedRegion(linkDrawing.SourceDrawing));
			newRectangleToInvalidate.Inflate(
				getStructureInvalidatedRegion(linkDrawing.DestinationDrawing));
			Rectangle auxRectangle = newRectangleToInvalidate.Bounds;
			invalidateOverlappingDrawings(previouslyInvalidatedRectangle);
			newRectangleToInvalidate.Inflate(previouslyInvalidatedRectangle);
			invalidateOverlappingDrawings(newRectangleToInvalidate.Bounds);
			previouslyInvalidatedRectangle = auxRectangle;
			
			containerDrawer.DiagramContainer.Invalidate(newRectangleToInvalidate.Bounds);
			previouslyInvalidatedRectangle = newRectangleToInvalidate.Bounds;
			containerDrawer.DiagramContainer.Validate();
		}
		
		Rectangle getStructureInvalidatedRegion(IDrawing targetDrawing)
		{
			targetDrawing.Invalidated = true;
			InflatableRectangle rectangle = new InflatableRectangle(targetDrawing.Bounds);
			if (targetDrawing is ILinkableDrawing) {
				appendLinksToRegion(rectangle, targetDrawing as ILinkableDrawing);
			}
			for (int i = 0; i < containerDrawer.Drawings.Count; i++) {
				IDrawing drawing = containerDrawer.Drawings[i];
				if (drawing.Bounds.IntersectsWith(rectangle.Bounds) && drawing.Invalidated == false) {
					drawing.Invalidated = true;
					rectangle.Inflate(drawing.Bounds);
				}
			}
			return rectangle.Bounds;
		}
		
		
		Rectangle getLinkInvalidatedRegion(LinkDrawing linkDrawing)
		{
			InflatableRectangle rectangle = new InflatableRectangle(linkDrawing.Bounds);
			linkDrawing.Invalidated = true;
			ILinkableDrawing sourceDrawing = linkDrawing.SourceDrawing;
			ILinkableDrawing destinationDrawing = linkDrawing.DestinationDrawing;
			sourceDrawing.Invalidated = true;
			destinationDrawing.Invalidated = true;
			rectangle.Inflate(sourceDrawing.Bounds);
			rectangle.Inflate(destinationDrawing.Bounds);
			
			
			appendLinksToRegion(rectangle, sourceDrawing);
			appendLinksToRegion(rectangle, destinationDrawing);
			
			
			foreach (StructureDrawing drawing in containerDrawer.Drawings) {
				if (drawing.Bounds.IntersectsWith(rectangle.Bounds)) {
					drawing.Invalidated = true;
					rectangle.Inflate(drawing.Bounds);
				}
			}
			return rectangle.Bounds;
		}

		void appendLinksToRegion(InflatableRectangle rectangle, ILinkableDrawing drawing)
		{
			foreach (LinkDrawing link in drawing.DrawingLinks) {
				rectangle.Inflate(link.Bounds);
			}
		}
		
		void invalidateOverlappingDrawings(Rectangle rectangle)
		{
			for (int i = 0; i < containerDrawer.Drawings.Count; i++) {
				IDrawing drawing = containerDrawer.Drawings[i];
				if (!drawing.Invalidated && drawing.Bounds.IntersectsWith(rectangle)) {
					drawing.Invalidated = true;
				}
			}
		}
		
	}
}
