using System;
using SamDiagrams.Drawers.Links;
using SamDiagrams.Linking;

namespace SamDiagrams
{

	public class LinkDirectionChangedArg : EventArgs
	{
		private LinkDirection newDirection;
		private LinkDirection prevDirection;
		private LinkDrawing link;
		
		public LinkDirection NewDirection {
			get { return newDirection; }
			set { newDirection = value; }
		}

		public LinkDirection PrevDirection {
			get { return prevDirection; }
			set { prevDirection = value; }
		}

		public LinkDrawing Link {
			get {
				return link;
			}
		}
		
		public LinkDirectionChangedArg(LinkDrawing link, LinkDirection newDirection, LinkDirection prevDirection)
		{
			this.link = link;
			this.newDirection = newDirection;
			this.prevDirection = prevDirection;
		}

	}
}

