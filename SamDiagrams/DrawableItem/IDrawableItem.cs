/*
 * Created by SharpDevelop.
 * User: L
 * Date: 2/26/2013
 * Time: 10:36 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Drawing;
namespace SamDiagrams
{
	/// <summary>
	/// Description of DrawableItem.
	/// </summary>
	public interface IDrawableItem
	{

        Rectangle Bounds { get;}
        bool Invalidated { get;set;}
		void Draw(Graphics g);
        Size getSize();
        Point getLocation();
	}
}
