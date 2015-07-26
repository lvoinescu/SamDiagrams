/*
 * Created by SharpDevelop.
 * User: L
 * Date: 2/19/2013
 * Time: 9:08 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace SamDiagrams
{
	/// <summary>
	/// Description of Delegates.
	/// </summary>
	public delegate void SelectedItemsChangeHandler(object sender, SelectedItemsChangeArgs e);
	public delegate void BeforeNodeExpandOrCollapseHandler(object sender, BeforeNodeExpandOrCollapseArg e);
	public delegate void ZoomFactorChangedHandler(object sender, ZoomFactorChangedArg e);
    public delegate void LinkDirectionChangedHandler(object sender, LinkDirectionChangedArg e);
    public delegate void ItemsMovedHandler(object sender, ItemsMovedEventArg e);
    public delegate void ItemMovedHandler(object sender, ItemMovedEventArg e);
    public delegate void ItemResizedHandler(object sender, ItemResizedEventArg e);
}
