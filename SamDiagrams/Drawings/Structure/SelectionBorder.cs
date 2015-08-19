using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SamDiagrams
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

	public partial class SelectionBorder : IDrawableItem
	{
		internal static int squareSize = 8;
		internal static int inflate = 2;
		Structure item;
		internal Point initialLocation;
		internal Size initialSize;

		bool invalidated = true;

		public bool Invalidated {
			get { return invalidated; }
			set { invalidated = value; }
		}
		public Structure Item {
			get { return item; }
			set { item = value; }
		}
		public SelectionBorder(Structure item)
		{
			this.item = item;
//            initialSize = item.Size;
//            initialLocation = item.Location;
		}

		#region IDrawableItem Members

		public void Draw(Graphics g)
		{
			using (Pen p = new Pen(Color.FromArgb(80, 70, 70, 70), 1)) {
				float[] dashValues = { 6, 3 };
				p.DashPattern = dashValues;
				Rectangle r = new Rectangle(item.DiagramContainer.ContainerDrawer.ModelToDrawer[item].Location, item.DiagramContainer.ContainerDrawer.ModelToDrawer[item].Size);
				r.Inflate(new Size(inflate, inflate));
				g.DrawRectangle(p, r);
				g.FillRectangle(Brushes.White, new Rectangle(r.Location.X - squareSize, r.Location.Y - squareSize, squareSize, squareSize));
				g.DrawRectangle(Pens.Blue, new Rectangle(r.Location.X - squareSize, r.Location.Y - squareSize, squareSize, squareSize));

				g.FillRectangle(Brushes.White, new Rectangle(r.Location.X + r.Width, r.Location.Y - squareSize, squareSize, squareSize));
				g.DrawRectangle(Pens.Blue, new Rectangle(r.Location.X + r.Width, r.Location.Y - squareSize, squareSize, squareSize));

				g.FillRectangle(Brushes.White, new Rectangle(r.Location.X + (r.Width - squareSize) / 2, r.Location.Y - squareSize, squareSize, squareSize));
				g.DrawRectangle(Pens.Blue, new Rectangle(r.Location.X + (r.Width - squareSize) / 2, r.Location.Y - squareSize, squareSize, squareSize));



				g.FillRectangle(Brushes.White, new Rectangle(r.Location.X - squareSize, r.Location.Y + r.Size.Height, squareSize, squareSize));
				g.DrawRectangle(Pens.Blue, new Rectangle(r.Location.X - squareSize, r.Location.Y + r.Size.Height, squareSize, squareSize));


				g.FillRectangle(Brushes.White, new Rectangle(r.Location.X - squareSize, r.Location.Y + (r.Size.Height - squareSize) / 2, squareSize, squareSize));
				g.DrawRectangle(Pens.Blue, new Rectangle(r.Location.X - squareSize, r.Location.Y + (r.Size.Height - squareSize) / 2, squareSize, squareSize));

				g.FillRectangle(Brushes.White, new Rectangle(r.Location.X + r.Width, r.Location.Y + r.Size.Height, squareSize, squareSize));
				g.DrawRectangle(Pens.Blue, new Rectangle(r.Location.X + r.Width, r.Location.Y + r.Size.Height, squareSize, squareSize));

				g.FillRectangle(Brushes.White, new Rectangle(r.Location.X + (r.Width - squareSize) / 2, r.Location.Y + r.Size.Height, squareSize, squareSize));
				g.DrawRectangle(Pens.Blue, new Rectangle(r.Location.X + (r.Width - squareSize) / 2, r.Location.Y + r.Size.Height, squareSize, squareSize));

				g.FillRectangle(Brushes.White, new Rectangle(r.Location.X + r.Size.Width, r.Location.Y + (r.Size.Height - squareSize) / 2, squareSize, squareSize));
				g.DrawRectangle(Pens.Blue, new Rectangle(r.Location.X + r.Size.Width, r.Location.Y + (r.Size.Height - squareSize) / 2, squareSize, squareSize));
				//g.Save();
			}
		}

		public Size getSize()
		{
//            Rectangle r = new Rectangle(item.Location, item.Size);
			Rectangle r = new Rectangle(0, 0, 0, 0);
			r.Inflate(new Size(inflate + squareSize, inflate + squareSize));
			return r.Size;
		}


		public Rectangle Bounds {
			get {
//                Rectangle r = new Rectangle(item.Location, item.Size);
				Rectangle r = new Rectangle(0, 0, 0, 0);
				r.Inflate(new Size(inflate + squareSize, inflate + squareSize));
				return r;
			}
		}

		public Point getLocation()
		{
			return new Point();
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
		#endregion
	}
}

