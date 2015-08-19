/*
 * Created by SharpDevelop.
 * User: Sam
 * Date: 8/9/2015
 * Time: 3:15 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using SamDiagrams.Drawings;

namespace SamDiagrams.Actions
{
	/// <summary>
	/// Description of MoveAction.
	/// </summary>
	public class MoveAction : MouseAction
	{
		
		public event ItemsMovedHandler ItemsMoved;
		
		readonly DiagramContainer container;
		private Point startMovePoint;
		private bool actionStarted;
		
		public MoveAction(DiagramContainer container)
		{
			actionStarted = false;
			this.container = container;
			this.container.MouseDown += new System.Windows.Forms.MouseEventHandler(OnMouseDown);
			this.container.MouseUp += new System.Windows.Forms.MouseEventHandler(OnMouseUp);
			this.container.MouseMove += new System.Windows.Forms.MouseEventHandler(OnMouseMove);
		}

		public void OnMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			float scaleFactor = (float)container.ZoomFactor / 100;
			
			foreach (Drawing drawing in container.ContainerDrawer.Drawings) {
				
				if (!(drawing is Drawing)) {
					continue;
				}
				
				Point p = new Point((int)(e.Location.X / scaleFactor), (int)(e.Location.Y / scaleFactor));
				p.Offset(container.HScrollBar.Value, container.VScrollBar.Value);
				if (drawing.Bounds.Contains(e.Location)) {
					startMovePoint = new Point(e.Location.X, e.Location.Y);
					actionStarted = true;
					break;
				}
			}
		}

		public void OnMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			actionStarted = false;
			foreach (Drawing drawing in container.ContainerDrawer.Drawings) {
				
				if (!(drawing is StructureDrawing)) {
					continue;
				}
				StructureDrawing structureDrawing = drawing as StructureDrawing;
				structureDrawing.InitialSelectedLocation = structureDrawing.Location;
			}
		}

		public void OnMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (!actionStarted) {
				return;
			}
			float scaleFactor = (float)container.ZoomFactor / 100;
			int dx = (int)((e.X - startMovePoint.X) / scaleFactor);
			int dy = (int)((e.Y - startMovePoint.Y) / scaleFactor);
			List<StructureDrawing> movedStructures = new List<StructureDrawing>();
			foreach (Drawing drawing in container.ContainerDrawer.Drawings) {
				
				if (!(drawing is StructureDrawing) || !drawing.Selected) {
					continue;
				}
				
				StructureDrawing structureDrawing = drawing as StructureDrawing;
				int x = (int)(structureDrawing.InitialSelectedLocation.X + dx);
				int y = (int)(structureDrawing.InitialSelectedLocation.Y + dy);

				int inf = SelectionBorder.squareSize + SelectionBorder.inflate;
				if (x < 0)
					x = 0;
				if (x > container.Width - structureDrawing.Size.Width)
					x = (int)(container.Width - structureDrawing.Size.Width);
				if (y < 0)
					y = 0;
				if (y > container.Height - structureDrawing.Size.Height)
					y = (int)(container.Height - structureDrawing.Size.Height);
				structureDrawing.Location = new Point(x, y);
				movedStructures.Add(structureDrawing);
			}
			
			if (ItemsMoved != null) {
				ItemsMoved(this, new ItemsMovedEventArg(movedStructures, dx, dy));
			}
		}

	}
}
