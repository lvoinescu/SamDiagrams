using System;

namespace SamDiagrams
{
	public partial class ItemMovedEventArg : EventArgs
	{
		Structure item;

		public Structure Item {
			get { return item; }
			set { item = value; }
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
		public ItemMovedEventArg(Structure item, int dx, int dy)
		{
			this.item = item;
			this.dx = dx;
			this.dy = dy;
		}
	}
}

