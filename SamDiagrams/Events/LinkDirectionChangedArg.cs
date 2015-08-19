using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SamDiagrams.Linking;

namespace SamDiagrams
{

	public class LinkDirectionChangedArg : EventArgs
	{
		private LinkDirection newDirection;
		private LinkDirection prevDirection;
		private StructureLink link;
		
		public LinkDirection NewDirection {
			get { return newDirection; }
			set { newDirection = value; }
		}

		public LinkDirection PrevDirection {
			get { return prevDirection; }
			set { prevDirection = value; }
		}

		public StructureLink Link {
			get {
				return link;
			}
		}
		
		public LinkDirectionChangedArg(StructureLink link, LinkDirection newDirection, LinkDirection prevDirection)
		{
			this.link = link;
			this.newDirection = newDirection;
			this.prevDirection = prevDirection;
		}

	}
}

