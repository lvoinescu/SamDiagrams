/*
 * Created by SharpDevelop.
 * User: Sam
 * Date: 8/6/2015
 * Time: 9:01 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;

namespace SamDiagrams.Drawings.Grid
{
	/// <summary>
	/// Description of GridDrawer.
	/// </summary>
	public class GridDrawer : IDrawing
	{
		
		private const int DEFAULT_GRID_SIZE = 16;
		
		private int gridSize = DEFAULT_GRID_SIZE;
		private bool invalidated = true;
		private readonly DiagramContainer diagramContainer;
		
		public GridDrawer(DiagramContainer diagramContainer)
		{
			this.diagramContainer = diagramContainer;
		}

		public SamDiagrams.Model.Item Item {
			get {
				throw new NotImplementedException();
			}
		}
		public GridDrawer(DiagramContainer diagramContainer, int gridSize)
			: this(diagramContainer)
		{
			this.gridSize = gridSize;
		}

		#region IDrawer implementation

		public void Draw(System.Drawing.Graphics graphics)
		{
			Pen p = new Pen(Color.FromArgb(100, 200, 200, 200));
			float step = gridSize;
			if (step < 1)
				step = 1;
			for (float i = 0; i < diagramContainer.Width; i += step) {
				graphics.DrawLine(p, i, 0, i, diagramContainer.Height);
			}
			for (float j = 0; j < diagramContainer.Height; j += step) {
				graphics.DrawLine(p, 0, j, diagramContainer.Width, j);
			}
		}

		public int GridSize {
			get {
				return gridSize;
			}
			set {
				gridSize = value;
			}
		}
		public Point Location {
			get {
				return new Point(0, 0);
			}
			set {
				throw new Exception("Cannot set location for grid.");
			}
		}

		public Size Size {
			get {
				return new Size(diagramContainer.Width, diagramContainer.Height);
			}
		}

		public Rectangle Bounds {
			get {
				return new Rectangle(Location, Size);
			}
			
		}

		public bool Selected {
			get {
				throw new NotImplementedException();
			}
			set {
				throw new NotImplementedException();
			}
		}
		
		public bool Invalidated {
			get {
				return false;
			}
			set {
				this.invalidated = false;
			}
		}

		#endregion
	}
}
