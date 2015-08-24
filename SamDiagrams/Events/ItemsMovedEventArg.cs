using System;
using System.Collections.Generic;
using SamDiagrams.Drawings;
using SamDiagrams.Drawings.Geometry;
using SamDiagrams.Drawings.Selection;

namespace SamDiagrams
{
	public partial class ItemsMovedEventArg : EventArgs
	{
		List<SelectableDrawing> items;

		public List<SelectableDrawing> Items {
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
		public ItemsMovedEventArg(List<SelectableDrawing> items, int dx, int dy)
		{
			this.items = items;
			this.dx = dx;
			this.dy = dy;
		}
	}
}

