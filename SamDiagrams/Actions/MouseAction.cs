/*
 * Created by SharpDevelop.
 * User: Sam
 * Date: 8/5/2015
 * Time: 10:56 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;

namespace SamDiagrams.Actions
{
	/// <summary>
	/// Description of IMouseHandler.
	/// </summary>
	public interface MouseAction
	{
		void OnMouseDown(Object sender, MouseEventArgs e);
		void OnMouseUp(Object sender, MouseEventArgs e);
		void OnMouseMove(Object sender, MouseEventArgs e);
	}
}
