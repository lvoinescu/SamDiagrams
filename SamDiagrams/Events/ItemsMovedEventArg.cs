using System;
using System.Collections.Generic;
using SamDiagrams.Drawings;
using SamDiagrams.Drawings.Geometry;
using SamDiagrams.Drawings.Selection;

namespace SamDiagrams
{
	public partial class ItemsMovedEventArg : EventArgs
	{
		List<IDrawing> items;

		public List<IDrawing> Items {
			get { return items; }
			set { items = value; }
		}
		int dx, dy;

		public int Dy {
			get { return dy; }
			set { dy = value; }
		}

		public int Dx {
			get { return dx; }
			set { dx = value; }
		}
		public ItemsMovedEventArg(List<IDrawing> items, int dx, int dy)
		{
			this.items = items;
			this.dx = dx;
			this.dy = dy;
		}
	}
}

