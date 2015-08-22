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
using SamDiagrams.Drawings.Selection;

namespace SamDiagrams.Actions
{
	/// <summary>
	/// Description of MouseHandler.
	/// </summary>
	public class SelectionAction : MouseAction
	{
		DiagramContainer container;
		private List<ISelectableDrawing> selectedDrawings;
		
		public SelectionAction(DiagramContainer container)
		{
			this.container = container;
			selectedDrawings = new List<ISelectableDrawing>();
			this.container.MouseDown += new System.Windows.Forms.MouseEventHandler(OnMouseDown);
			this.container.MouseUp += new System.Windows.Forms.MouseEventHandler(OnMouseUp);
		}

		public void OnMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			bool drawingFound = false;
			float scaleFactor = (float)container.ZoomFactor / 100;
			
			for (int i = 0; i < container.ContainerDrawer.SelectedDrawings.Count && !drawingFound; i++) {
				ISelectableDrawing selectableDrawing = container.ContainerDrawer.SelectedDrawings[i];

				Point p = new Point((int)(e.Location.X / scaleFactor), (int)(e.Location.Y / scaleFactor));
				p.Offset(container.HScrollBar.Value, container.VScrollBar.Value);
				if (selectableDrawing.Bounds.Contains(e.Location)) {
					drawingFound = true;
					moveLast(selectableDrawing);
					if (Control.ModifierKeys != Keys.Control) {
						clearSelections();
					}
					toggleSelection(selectableDrawing);
				} else {
					removeSelected(selectableDrawing);
				}
			}
			
			if (!drawingFound) {
				selectedDrawings.Clear();
			}
		}

		private void clearSelections()
		{
			foreach (ISelectableDrawing drawing in selectedDrawings) {
				drawing.Selected = false;
			}
			selectedDrawings.Clear();
		}
		
		private void toggleSelection(ISelectableDrawing selectableDrawing)
		{
			if (!selectedDrawings.Contains(selectableDrawing)) {
				selectableDrawing.Selected = true;
				selectedDrawings.Add(selectableDrawing);
			} else {
				selectableDrawing.Selected = false;
				selectedDrawings.Remove(selectableDrawing);
			}
		}
		
		
		private void removeSelected(ISelectableDrawing selectedDrawing)
		{
			if (selectedDrawings.Contains(selectedDrawing)) {
				selectedDrawing.Selected = false;
				selectedDrawings.Remove(selectedDrawing);
			}
		}
		
		private void moveLast(ISelectableDrawing drawing)
		{
			container.ContainerDrawer.Drawings.Remove((IDrawing)drawing);
			container.ContainerDrawer.Drawings.Add((IDrawing)drawing);
		}
		
		public void OnMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
		}

		public void OnMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
		}

	}
}
