/*
 *   SamDiagrams - diagram component for .NET
 *   Copyright (C) 2011  Lucian Voinescu
 *
 *   This file is part of SamDiagrams
 *
 *   SamDiagrams is free software: you can redistribute it and/or modify
 *   it under the terms of the GNU Lesser General Public License as published by
 *   the Free Software Foundation, either version 3 of the License, or
 *   (at your option) any later version.
 *
 *   SamDiagrams is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU Lesser General Public License for more details.
*
 *   You should have received a copy of the GNU Lesser General Public License
 *   along with SamDiagrams. If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SamDiagrams.Drawings;

namespace SamDiagrams.DiagramItem.NodeEditor
{
	/// <summary>
	/// Description of NodeEditor.
	/// </summary>
	public class NodeTextEditor : IDrawing, INodeEditor
	{
		DiagramContainer container;
		private bool visible = false;
		internal StructureNodeInfo currentNode = null;
		private Timer timer;
		private int cursorBlinkPeriod = 300;
		private bool cursorVisible = true;
		private int cursorPosition = 0;
		private int cursorXposition = 0;
		private string currentText = "";
		private bool mousePressed = false;
		private Point selectionLocation;
		private Size selectionSize;
		public bool Visible {
			get { return visible; }
			set {
				visible = value;
				timer.Stop();
			}
		}

		public SamDiagrams.Model.Item Item {
			get {
				throw new NotImplementedException();
			}
		}

		public bool Movable {
			get {
				return false;
			}
			set {
				throw new NotImplementedException();
			}
		}
		public Rectangle InvalidatedRegion {
			get {
				throw new NotImplementedException();
			}
		}
		
		public NodeTextEditor(DiagramContainer container)
		{
			this.container = container;
			timer = new Timer();
			timer.Interval = cursorBlinkPeriod;
			timer.Tick += new EventHandler(timer_Tick);

		}

		public void timer_Tick(object sender, EventArgs e)
		{
			cursorVisible = !cursorVisible;
			currentNode.Nod.DiagramItem.Invalidated = true;
			float scaleFactor = (float)this.currentNode.Nod.DiagramItem.DiagramContainer.ZoomFactor / 100;
			Rectangle r = new Rectangle((int)(currentNode.BoundingRectangle.Location.X * scaleFactor), (int)(currentNode.BoundingRectangle.Location.Y * scaleFactor), (int)(currentNode.BoundingRectangle.Size.Width * scaleFactor), (int)(currentNode.BoundingRectangle.Size.Height * scaleFactor));
			container.Invalidate(r);
		}


		public void ShowOnNode(StructureNodeInfo nodeInfo)
		{
			currentNode = nodeInfo;
			currentText = nodeInfo.Nod.Text;
			this.visible = true;
			timer.Start();
		}


		public System.Drawing.Rectangle Bounds {
			get { return currentNode.BoundingRectangle; }
		}

		public bool Invalidated {
			get {
				throw new NotImplementedException();
			}
			set {
				throw new NotImplementedException();
			}
		}

		public void Draw(System.Drawing.Graphics g)
		{
			if (visible) {
				Pen p = new Pen(Color.Black, 1);
				Rectangle rec = new Rectangle(currentNode.BoundingRectangle.Location, currentNode.BoundingRectangle.Size);
				//rec.Inflate(new Size(2,1));
				g.DrawRectangle(p, rec);
				g.FillRectangle(Brushes.White, currentNode.BoundingRectangle);
				g.FillRectangle(Brushes.Blue, selectionLocation.X, selectionLocation.Y, selectionSize.Width, selectionSize.Height);
				if (cursorVisible) {

					string currentCaretString = currentNode.Nod.Text.Substring(0, cursorPosition);
					SizeF stringSize = g.MeasureString(currentCaretString, currentNode.StructureDrawing.rowFont);
					if (stringSize.Width == 0)
						stringSize.Width = 2;
					cursorXposition = currentNode.BoundingRectangle.X + (int)stringSize.Width - 1;
					g.DrawLine(p, cursorXposition, currentNode.BoundingRectangle.Y, cursorXposition, currentNode.BoundingRectangle.Y + currentNode.BoundingRectangle.Height);
					

				}
				
				g.DrawString(currentNode.Nod.Text, currentNode.StructureDrawing.rowFont, Brushes.Black, new PointF(currentNode.BoundingRectangle.X, currentNode.BoundingRectangle.Y));

				p.Dispose();


			}
		}

		public Point Location {
			get {
				throw new NotImplementedException();
			}
			set {
				throw new NotImplementedException();
			}
		}
		public Size Size {
			get {
				throw new NotImplementedException();
			}
		}
		public bool Selected {
			get {
				throw new NotImplementedException();
			}
			set {
				throw new NotImplementedException();
			}
		}
		internal void OnKeyDown(KeyEventArgs e)
		{
			switch (e.KeyCode) {
				case Keys.Right:
					if (cursorPosition < currentNode.Nod.Text.Length)
						cursorPosition++;
					break;
				case Keys.Left:
					if (cursorPosition > 0)
						cursorPosition--;
					break;
			}
		}

		public System.Drawing.Size getSize()
		{
			throw new NotImplementedException();
		}

		public System.Drawing.Point getLocation()
		{
			throw new NotImplementedException();
		}
		
		public void onMouseDown(object sender, MouseEventArgs e, double scaleFactor)
		{

			Font rowScaledFont = new Font(currentNode.StructureDrawing.rowFont.FontFamily, (float)((currentNode.StructureDrawing.rowFont.Size - 0)));
			
			cursorPosition = 0;		
			Size sT = new Size(0, 0);
			for (int i = 0; i < currentText.Length; i++) {
				sT = TextRenderer.MeasureText(currentText.Substring(0, i), rowScaledFont);
				if (currentNode.BoundingRectangle.X + sT.Width + 6 > e.X)
					break;
				cursorPosition++;
			}
			

			
			selectionLocation = new Point(cursorXposition, currentNode.BoundingRectangle.Y);
			Rectangle r = new Rectangle((int)(currentNode.BoundingRectangle.Location.X * scaleFactor), (int)(currentNode.BoundingRectangle.Location.Y * scaleFactor), (int)(currentNode.BoundingRectangle.Size.Width * scaleFactor), (int)(currentNode.BoundingRectangle.Size.Height * scaleFactor));
			container.Invalidate(r);
			mousePressed = true;
		}
		
		public void onMouseUp(object sender, MouseEventArgs e, double scaleFactor)
		{
			mousePressed = false;
		}
		
		public void onMouseMove(object sender, MouseEventArgs e, double scaleFactor)
		{
			if (mousePressed) {
				selectionSize.Width = e.X - selectionLocation.X;
				selectionSize.Height = currentNode.BoundingRectangle.Height;
				Rectangle r = new Rectangle((int)(currentNode.BoundingRectangle.Location.X * scaleFactor), (int)(currentNode.BoundingRectangle.Location.Y * scaleFactor), (int)(currentNode.BoundingRectangle.Size.Width * scaleFactor), (int)(currentNode.BoundingRectangle.Size.Height * scaleFactor));
				container.Invalidate(r);
			}
		}
	}
}
