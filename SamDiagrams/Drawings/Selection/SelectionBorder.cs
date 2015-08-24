using System;
using System.Drawing;
using SamDiagrams.Drawings;
using SamDiagrams.Drawings.Geometry;

namespace SamDiagrams.Drawings.Selection
{
	public enum ResizeDirection
	{
		NONE,
		N,
		S,
		W,
		E,
		NW,
		NE,
		SW,
		SE}

	;

	public partial class SelectionBorder : IBoundedShape
	{
		internal static int squareSize = 8;
		internal static int inflate = 2;
		IDrawing drawing;
		internal Point initialLocation;
		internal Size initialSize;

		bool invalidated = true;

		
		public bool Invalidated {
			get { return invalidated; }
			set { invalidated = value; }
		}
		
		public IDrawing Drawing {
			get { return drawing; }
			set { drawing = value; }
		}
		
		public SelectionBorder(IDrawing item)
		{
			this.drawing = item;
		}

		public void Draw(Graphics graphics)
		{
			using (Pen p = new Pen(Color.FromArgb(80, 70, 70, 70), 1)) {
				float[] dashValues = { 6, 3 };
				p.DashPattern = dashValues;
				Rectangle r = new Rectangle(drawing.Location, drawing.Size);
				r.Inflate(new Size(inflate, inflate));
				graphics.DrawRectangle(p, r);
				graphics.FillRectangle(Brushes.White, new Rectangle(r.Location.X - squareSize, r.Location.Y - squareSize,	squareSize, squareSize));
				graphics.DrawRectangle(Pens.Blue, new Rectangle(r.Location.X - squareSize, r.Location.Y - squareSize, squareSize, squareSize));

				graphics.FillRectangle(Brushes.White, new Rectangle(r.Location.X + r.Width, r.Location.Y - squareSize, squareSize, squareSize));
				graphics.DrawRectangle(Pens.Blue, new Rectangle(r.Location.X + r.Width, r.Location.Y - squareSize, squareSize, squareSize));

				graphics.FillRectangle(Brushes.White, new Rectangle(r.Location.X + (r.Width - squareSize) / 2, r.Location.Y - squareSize, squareSize, squareSize));
				graphics.DrawRectangle(Pens.Blue, new Rectangle(r.Location.X + (r.Width - squareSize) / 2, r.Location.Y - squareSize, squareSize, squareSize));



				graphics.FillRectangle(Brushes.White, new Rectangle(r.Location.X - squareSize, r.Location.Y + r.Size.Height, squareSize, squareSize));
				graphics.DrawRectangle(Pens.Blue, new Rectangle(r.Location.X - squareSize, r.Location.Y + r.Size.Height, squareSize, squareSize));


				graphics.FillRectangle(Brushes.White, new Rectangle(r.Location.X - squareSize, r.Location.Y + (r.Size.Height - squareSize) / 2, squareSize, squareSize));
				graphics.DrawRectangle(Pens.Blue, new Rectangle(r.Location.X - squareSize, r.Location.Y + (r.Size.Height - squareSize) / 2, squareSize, squareSize));

				graphics.FillRectangle(Brushes.White, new Rectangle(r.Location.X + r.Width, r.Location.Y + r.Size.Height, squareSize, squareSize));
				graphics.DrawRectangle(Pens.Blue, new Rectangle(r.Location.X + r.Width, r.Location.Y + r.Size.Height, squareSize, squareSize));

				graphics.FillRectangle(Brushes.White, new Rectangle(r.Location.X + (r.Width - squareSize) / 2, r.Location.Y + r.Size.Height, squareSize, squareSize));
				graphics.DrawRectangle(Pens.Blue, new Rectangle(r.Location.X + (r.Width - squareSize) / 2, r.Location.Y + r.Size.Height, squareSize, squareSize));

				graphics.FillRectangle(Brushes.White, new Rectangle(r.Location.X + r.Size.Width, r.Location.Y + (r.Size.Height - squareSize) / 2, squareSize, squareSize));
				graphics.DrawRectangle(Pens.Blue, new Rectangle(r.Location.X + r.Size.Width, r.Location.Y + (r.Size.Height - squareSize) / 2, squareSize, squareSize));
			}
		}

		public Point Location {
			get {
				return Bounds.Location;
			}
			set {
				throw new NotImplementedException();
			}
		}
		public Size Size {
			get {
				return Bounds.Size;
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


		public Rectangle Bounds {
			get {
				Rectangle r = new Rectangle(this.drawing.Location, this.drawing.Size);
				r.Inflate(new Size(inflate + squareSize, inflate + squareSize));
				return r;
			}
		}


		internal ResizeDirection resizeDirection;

		internal void setReziseDirection(Point p)
		{
//			initialLocation = this.item.Location;
//			initialSize = item.Size;
//			Rectangle r = new Rectangle(item.Location, item.Size);
			Rectangle r = new Rectangle(0, 0, 0, 0);
			r.Inflate(new Size(2, 2));
			Rectangle r1 = new Rectangle(r.Location.X - squareSize, r.Location.Y - squareSize, squareSize, squareSize);
			if (r1.Contains(p)) {
				resizeDirection = ResizeDirection.NW;
				return;
			}
			Rectangle r2 = new Rectangle(r.Location.X + r.Width, r.Location.Y - squareSize, squareSize, squareSize);
			if (r2.Contains(p)) {
				resizeDirection = ResizeDirection.NE;
				return;
			}          
			Rectangle r3 = new Rectangle(r.Location.X + (r.Width - squareSize) / 2, r.Location.Y - squareSize, squareSize, squareSize);
			if (r3.Contains(p)) {
				resizeDirection = ResizeDirection.N;
				return;
			} 
			Rectangle r4 = new Rectangle(r.Location.X - squareSize, r.Location.Y + r.Size.Height, squareSize, squareSize);
			if (r4.Contains(p)) {
				resizeDirection = ResizeDirection.SW;
				return;
			} 
			Rectangle r5 = new Rectangle(r.Location.X - squareSize, r.Location.Y + (r.Size.Height - squareSize) / 2, squareSize, squareSize);
			if (r5.Contains(p)) {
				resizeDirection = ResizeDirection.W;
				return;
			} 
			Rectangle r6 = new Rectangle(r.Location.X + r.Width, r.Location.Y + r.Size.Height, squareSize, squareSize);
			if (r6.Contains(p)) {
				resizeDirection = ResizeDirection.SE;
				return;
			}
			Rectangle r7 = new Rectangle(r.Location.X + (r.Width - squareSize) / 2, r.Location.Y + r.Size.Height, squareSize, squareSize);
			if (r7.Contains(p)) {
				resizeDirection = ResizeDirection.S;
				return;
			}
			Rectangle r8 = new Rectangle(r.Location.X + r.Size.Width, r.Location.Y + (r.Size.Height - squareSize) / 2, squareSize, squareSize);
			if (r8.Contains(p)) {
				resizeDirection = ResizeDirection.E;
				return;
			}

			resizeDirection = ResizeDirection.NONE;
		}
	}
}

