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
using SamDiagrams.Drawings.Geometry;
using SamDiagrams.Drawings.Selection;

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
		private List<MovableDrawing> drawingsToMove;

		public  List<MovableDrawing> DrawingsToMove {
			get {
				return drawingsToMove;
			}
		}
		
		public MoveAction(DiagramContainer container)
		{
			actionStarted = false;
			this.container = container;
			drawingsToMove = new List<MovableDrawing>();
		}
		
		public void ClearDrawing()
		{
			drawingsToMove.Clear();
		}
		
		public void AddDrawing(IDrawing drawing)
		{
			drawingsToMove.Add(new MovableDrawing(drawing));
		}
		
		public void RemoveDrawing(IDrawing drawing)
		{
			drawingsToMove.Remove(new MovableDrawing(drawing));
		}
 
		public void OnMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			float scaleFactor = (float)container.ZoomFactor / 100;
			foreach (MovableDrawing drawing in drawingsToMove) {
				Point mouseDownScaledLocation = new Point((int)((double)e.Location.X / scaleFactor), 
					                                (int)((double)e.Location.Y / scaleFactor));
				mouseDownScaledLocation.Offset(container.HScrollBar.Value, container.VScrollBar.Value);
				
				if (drawing.Bounds.Contains(mouseDownScaledLocation)) {
					startMovePoint = new Point(e.X, e.Y);
					actionStarted = true;
				}
				
				drawing.InitialLocation = drawing.Drawing.Location;
			}
		}

		public void OnMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			actionStarted = false;
			startMovePoint = new Point(e.Location.X, e.Location.Y);
		}

		public void OnMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (!actionStarted) {
				return;
			}
			double scaleFactor = (float)container.ZoomFactor / 100;
			int dx = (int)((double)(e.X - startMovePoint.X) / scaleFactor);
			int dy = (int)((double)(e.Y - startMovePoint.Y) / scaleFactor);
			List<IDrawing> movedDrawing = new List<IDrawing>();
			foreach (MovableDrawing movableDrawing in drawingsToMove) {
				int x = (int)(movableDrawing.InitialLocation.X + dx);
				int y = (int)(movableDrawing.InitialLocation.Y + dy);

				if (x < 0)
					x = 0;
				
				if (x > container.Width - movableDrawing.Size.Width)
					x = (int)(container.Width - movableDrawing.Size.Width);
				
				if (y < 0)
					y = 0;
				
				if (y > container.Height - movableDrawing.Size.Height)
					y = (int)(container.Height - movableDrawing.Size.Height);
				movedDrawing.Add(movableDrawing.Drawing);
				movableDrawing.Location = new Point(x, y);
			}
			
			
			if ((dx != 0 || dy != 0) && ItemsMoved != null) {
				ItemsMoved(this, new ItemsMovedEventArg(movedDrawing, dx, dy));
			}
		}

	}
}
