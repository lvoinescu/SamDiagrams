using System;
using System.Drawing;

namespace SamDiagrams
{
	public partial class ItemResizedEventArg
	{
		private Size size;

		public Size Size {
			get { return size; }
			set { size = value; }
		}
		
		public ItemResizedEventArg(int width, int height)
		{
			this.size.Width = width;
			this.size.Height = height;
		}
	}
}

