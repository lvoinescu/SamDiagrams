/*
 * Created by SharpDevelop.
 * User: Sam
 * Date: 7/29/2015
 * Time: 10:15 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;

namespace SamDiagrams.Linking.Strategy
{
	/// <summary>
	/// Provides ways of drawing links.
	/// </summary>
	public interface ILinkDrawer
	{
	/// <summary>
	/// Draws a link on a specified graphics context with a specified zoomFactor.
	/// </summary>
		void Draw(Link l, System.Drawing.Graphics graphics, float zoomFactor);
	}
}
