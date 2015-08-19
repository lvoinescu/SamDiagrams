/*
 * Created by SharpDevelop.
 * User: Sam
 * Date: 8/1/2015
 * Time: 5:30 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;

namespace SamDiagrams.Drawings
{
	/// <summary>
	/// Description of IDrawer.
	/// </summary>
	public interface Drawing
	{
		Point Location { get; }
		Size Size { get; }
		Rectangle Bounds { get; }
		bool Invalidated { get; set; }
		bool Selected { get; set; }
		void Draw(Graphics graphics);
	}
}
