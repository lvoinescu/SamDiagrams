/*
 * Created by SharpDevelop.
 * User: L
 * Date: 2/19/2013
 * Time: 9:09 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
namespace SamDiagrams
{
	/// <summary>
	/// Description of SelectedItemsChangeArgs.
	/// </summary>
	public class SelectedItemsChangeArgs:EventArgs
	{

		private List<DiagramItem> selectedItems;
		
		public List<DiagramItem> SelectedItems {
			get { return selectedItems; }
		}
		public SelectedItemsChangeArgs( List<DiagramItem> items)
		{
			this.selectedItems = items;
		}

	}
	
}
