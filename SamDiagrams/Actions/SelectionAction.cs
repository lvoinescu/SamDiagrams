/*
 * Created by SharpDevelop.
 * User: Sam
 * Date: 8/5/2015
 * Time: 10:56 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SamDiagrams.Drawings;

namespace SamDiagrams.Actions
{
	/// <summary>
	/// Description of MouseHandler.
	/// </summary>
	public class SelectionAction : MouseAction
	{
		DiagramContainer container;
		private List<Drawing> selectedDrawings;
		
		public SelectionAction(DiagramContainer container)
		{
			this.container = container;
			selectedDrawings = new List<Drawing>();
			this.container.MouseDown += new System.Windows.Forms.MouseEventHandler(OnMouseDown);
			this.container.MouseUp += new System.Windows.Forms.MouseEventHandler(OnMouseUp);
		}

		public void OnMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			bool drawingFound = false;
			float scaleFactor = (float)container.ZoomFactor / 100;
			
			for (int i = 0; i < container.ContainerDrawer.Drawings.Count && !drawingFound; i++) {
				Drawing drawing = container.ContainerDrawer.Drawings[i];
				
				if (!(drawing is StructureDrawing)) {
					continue;
				}
				Point p = new Point((int)(e.Location.X / scaleFactor), (int)(e.Location.Y / scaleFactor));
				p.Offset(container.HScrollBar.Value, container.VScrollBar.Value);
				if (drawing.Bounds.Contains(e.Location)) {
					drawingFound = true;
					moveLast(container.ContainerDrawer.Drawings, drawing);
					if (Control.ModifierKeys != Keys.Control) {
						clearSelections();
					}
					toggleSelection(drawing);
				} else {
					removeSelected(drawing);
				}
			}
			
			if (!drawingFound) {
				selectedDrawings.Clear();
			}
		}

		private void clearSelections()
		{
			foreach (Drawing drawing in selectedDrawings) {
				drawing.Selected = false;
			}
			selectedDrawings.Clear();
		}
		
		private void toggleSelection(Drawing drawing)
		{
			if (!selectedDrawings.Contains(drawing)) {
				drawing.Selected = true;
				selectedDrawings.Add(drawing);
			} else {
				drawing.Selected = false;
				selectedDrawings.Remove(drawing);
			}
		}
		
		private void addSelected(Drawing drawing)
		{
			if (!selectedDrawings.Contains(drawing)) {
				drawing.Selected = true;
				selectedDrawings.Add(drawing);
			}
		}
		
		private void removeSelected(Drawing drawing)
		{
			if (selectedDrawings.Contains(drawing)) {
				drawing.Selected = false;
				selectedDrawings.Remove(drawing);
			}
		}
		
		private void moveLast(List<Drawing> drawings, Drawing drawing)
		{
			drawings.Remove(drawing);
			drawings.Add(drawing);
		}
		
		public void OnMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
		}

		public void OnMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
		}

	}
}
