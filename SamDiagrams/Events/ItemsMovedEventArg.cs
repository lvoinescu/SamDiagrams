using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SamDiagrams.Drawers;
using SamDiagrams.Drawings;

namespace SamDiagrams
{
	public partial class ItemsMovedEventArg : EventArgs
	{
		List<StructureDrawing> items;

		public List<StructureDrawing> Items {
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
		public ItemsMovedEventArg(List<StructureDrawing> items, int dx, int dy)
		{
			this.items = items;
			this.dx = dx;
			this.dy = dy;
		}
	}
}

