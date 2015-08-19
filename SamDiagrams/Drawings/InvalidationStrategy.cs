/*
 * Created by SharpDevelop.
 * User: Sam
 * Date: 8/6/2015
 * Time: 11:13 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using SamDiagrams.Drawers.Links;
using SamDiagrams.Drawings;
using SamDiagrams.Drawings.Geometry;
using SamDiagrams.Linking;

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
			foreach (Drawing drawer in containerDrawer.Drawings) {
				drawer.Invalidated = true;
			}
		}


		void OnItemsMoved(object sender, ItemsMovedEventArg e)
		{
			foreach (StructureDrawing drawer in e.Items) {
				InvalidateStructureDrawing(drawer);
			}
		}

		void OnLinkDirectionChanged(object sender, LinkDirectionChangedArg e)
		{
			InvalidateLinkDrawing((LinkDrawing)containerDrawer.ModelToDrawer[e.Link]);
		}
		
		void InvalidateStructureDrawing(StructureDrawing drawer)
		{
			InflatableRectangle newRectangleToInvalidate = new InflatableRectangle(drawer.Bounds);
			newRectangleToInvalidate.Inflate(getStructureInvalidatedRegion(drawer));
			Rectangle auxRectangle = newRectangleToInvalidate.Bounds;
			invalidateOverlappingDrawings(previouslyInvalidatedRectangle);
			newRectangleToInvalidate.Inflate(previouslyInvalidatedRectangle);
			previouslyInvalidatedRectangle = auxRectangle;
			
			if (!previouslyInvalidatedRectangle.Contains(newRectangleToInvalidate.Bounds)) {
				invalidateOverlappingDrawings(previouslyInvalidatedRectangle);
			}
			
			containerDrawer.DiagramContainer.Invalidate(newRectangleToInvalidate.Bounds);
			containerDrawer.DiagramContainer.Validate();
		}
		
		void InvalidateLinkDrawing(LinkDrawing drawer)
		{
			InflatableRectangle newRectangleToInvalidate = new InflatableRectangle(drawer.Bounds);
			newRectangleToInvalidate.Inflate(getLinkInvalidatedRegion(drawer.Link));
			newRectangleToInvalidate.Inflate(
				getStructureInvalidatedRegion((StructureDrawing)containerDrawer.ModelToDrawer[drawer.Link.Source]));
			newRectangleToInvalidate.Inflate(
				getStructureInvalidatedRegion((StructureDrawing)containerDrawer.ModelToDrawer[drawer.Link.Destination]));
			Rectangle auxRectangle = newRectangleToInvalidate.Bounds;
			invalidateOverlappingDrawings(previouslyInvalidatedRectangle);
			newRectangleToInvalidate.Inflate(previouslyInvalidatedRectangle);
			invalidateOverlappingDrawings(newRectangleToInvalidate.Bounds);
			previouslyInvalidatedRectangle = auxRectangle;
			
			containerDrawer.DiagramContainer.Invalidate(newRectangleToInvalidate.Bounds);
			previouslyInvalidatedRectangle = newRectangleToInvalidate.Bounds;
			containerDrawer.DiagramContainer.Validate();
		}
		
		Rectangle getStructureInvalidatedRegion(StructureDrawing structureDrawing)
		{
			structureDrawing.Invalidated = true;
			InflatableRectangle rectangle = new InflatableRectangle(structureDrawing.Bounds);
			Structure structure = containerDrawer.DrawerToModel[structureDrawing] as Structure;
			
			appendLinksToRegion(rectangle, structure);
			
			for (int i = 0; i < containerDrawer.Drawings.Count; i++) {
				Drawing drawing = containerDrawer.Drawings[i];
				if (drawing.Bounds.IntersectsWith(rectangle.Bounds) && drawing.Invalidated == false) {
					drawing.Invalidated = true;
					rectangle.Inflate(drawing.Bounds);
				}
			}
			return rectangle.Bounds;
		}
		
		
		Rectangle getLinkInvalidatedRegion(StructureLink structureLink)
		{
			LinkDrawing linkDrawing = (LinkDrawing)containerDrawer.ModelToDrawer[structureLink];
			InflatableRectangle rectangle = new InflatableRectangle(linkDrawing.Bounds);
			linkDrawing.Invalidated = true;
			StructureDrawing sourceDrawing = containerDrawer.ModelToDrawer[structureLink.Source] as StructureDrawing;
			StructureDrawing destinationDrawing = containerDrawer.ModelToDrawer[structureLink.Destination] as StructureDrawing;
			sourceDrawing.Invalidated = true;
			destinationDrawing.Invalidated = true;
			rectangle.Inflate(sourceDrawing.Bounds);
			rectangle.Inflate(destinationDrawing.Bounds);
			
			
			appendLinksToRegion(rectangle, sourceDrawing.Structure);
			appendLinksToRegion(rectangle, destinationDrawing.Structure);
			
			
			for (int i = 0; i < containerDrawer.Drawings.Count; i++) {
				Drawing drawing = containerDrawer.Drawings[i];
				if (drawing.Bounds.IntersectsWith(rectangle.Bounds)) {
					drawing.Invalidated = true;
					rectangle.Inflate(drawing.Bounds);
				}
			}
			return rectangle.Bounds;
		}

		void appendLinksToRegion(InflatableRectangle rectangle, Structure structure)
		{
			foreach (StructureLink link in structure.Links) {
				containerDrawer.ModelToDrawer[link].Invalidated = true;
				rectangle.Inflate(containerDrawer.ModelToDrawer[link].Bounds);
			}
		}
		
		void invalidateOverlappingDrawings(Rectangle rectangle)
		{
			for (int i = 0; i < containerDrawer.Drawings.Count; i++) {
				Drawing drawing = containerDrawer.Drawings[i];
				if (!drawing.Invalidated && drawing.Bounds.IntersectsWith(rectangle)) {
					drawing.Invalidated = true;
				}
			}
		}
		
	}
}
