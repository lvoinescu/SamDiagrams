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
using System.Collections.Generic;
using SamDiagrams.Drawings;
using SamDiagrams.Drawings.Selection;
using SamDiagrams.Model;
namespace SamDiagrams
{

	[Serializable]
	public class Structure : ILinkable
	{
		
		private Color color = Color.LightSteelBlue;
		private List<Node> nodes;
		private DiagramContainer diagramContainer;
		private string title;
		private IDrawing drawing;
		
		private bool invalidated;
		internal List<ILink> links;
		private Image titleImage;
		internal SelectionBorder selectionBorder = null;
		
		
		public Color Color {
			get { return color; }
			set { color = value; }
		}

		public string Name {
			get {
				throw new NotImplementedException();
			}
			set {
				throw new NotImplementedException();
			}
		}

		public IDrawing Drawing {
			get {
				return this.drawing;
			}
			set {
				this.drawing = value;
			}
		}
		
		public  List<ILink> Links {
			get {
				return links;
			}
		}
		public DiagramContainer DiagramContainer {
			get { return diagramContainer; }
			set { diagramContainer = value; }
		}

		
		public string Title {
			get { return title; }
			set { title = value; }
		}

 
		public bool Invalidated {
			get { return invalidated; }
			set {
				invalidated = value;
			}
		}
		
		public Image TitleImage {
			get { return titleImage; }
			set { titleImage = value; }
		}



		public List<Node> Nodes {
			get { return nodes; }
			set {
				nodes = value;
			}
		}

		
		public Structure(DiagramContainer p, String title)
		{
			invalidated = true;
			this.diagramContainer = p;
			this.title = title;
			nodes = new List<Node>();
			links = new  List<ILink>();
		}
		

		public void AddOnDiagram(DiagramContainer container, Color color)
		{
			this.diagramContainer = container;
			this.color = color;
			container.AddStructure(this, color);
		}
		
		public override string ToString()
		{
			return string.Format("[Structure Title={0}]", title);
		}

	}
}
