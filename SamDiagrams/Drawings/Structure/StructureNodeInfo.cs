/*
 * Created by SharpDevelop.
 * User: L
 * Date: 2/17/2013
 * Time: 12:51 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Windows.Forms;
using SamDiagrams.Drawers;
using SamDiagrams.Drawings;
namespace SamDiagrams
{
	public class StructureNodeInfo
	{
		private Node nod;
		private StructureDrawing structureDrawing;

		public StructureDrawing StructureDrawing {
			get {
				return StructureDrawing;
			}
			set {
				StructureDrawing = value;
			}
		}

		public Node Nod
		{
			get { return nod; }
			set { nod = value; }
		}
		Rectangle boundingRectangle;
	
		public Rectangle BoundingRectangle
		{
			get { return boundingRectangle; }
			set { boundingRectangle = value; }
		}
		public StructureNodeInfo(Node nod, StructureDrawing structureDrawing, Rectangle r)
		{
			this.structureDrawing = structureDrawing;
			this.boundingRectangle = r;
			this.nod = nod;
		}
	}
}
