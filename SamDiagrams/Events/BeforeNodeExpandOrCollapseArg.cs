/*
 * Created by SharpDevelop.
 * User: L
 * Date: 2/25/2013
 * Time: 2:45 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace SamDiagrams
{
	/// <summary>
	/// Description of BeforeNodeExpandOrCollapseArg.
	/// </summary>
	public class BeforeNodeExpandOrCollapseArg :EventArgs
	{
		private Node nod;
		
		public Node Nod {
			get { return nod; }
			set { nod = value; }
		}
		
		public BeforeNodeExpandOrCollapseArg(Node n)
		{
			this.nod = n;
		}
	}
}
