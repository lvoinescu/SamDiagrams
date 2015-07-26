/*
 * Created by SharpDevelop.
 * User: Sam
 * Date: 10/4/2014
 * Time: 3:37 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SamDiagrams.DrawableItem.NodeEditor
{
	public interface INodeEditor
	{
		void ShowOnNode(DiagramInfoItemNode nodeInfo);
		void Draw(Graphics g);
		Size getSize();
		Point getLocation();
		Rectangle Bounds { get; }
		void onMouseDown(object sender, MouseEventArgs e, double scaleFactor);
		void onMouseUp(object sender, MouseEventArgs e, double scaleFactor);
		void onMouseMove(object sender, MouseEventArgs e, double scaleFactor);
	}
}
