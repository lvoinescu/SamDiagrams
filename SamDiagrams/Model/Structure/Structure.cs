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
using System.ComponentModel;
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
		public event ListChangedEventHandler NodesChanged;
		
		private Color color = Color.LightSteelBlue;
		private readonly BindingList<Node> nodes;
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



		public BindingList<Node> Nodes {
			get { return nodes; }
		}

		
		public Structure(DiagramContainer p, String title)
		{
			invalidated = true;
			this.diagramContainer = p;
			this.title = title;
			nodes = new BindingList<Node>();
			nodes.ListChanged += new ListChangedEventHandler(OnListChanged);
			links = new  List<ILink>();
		}

		public void AddNode(Node node)
		{
			node.NodesChanged += OnListChanged;
			nodes.Add(node);
		}
		void OnListChanged(object sender, ListChangedEventArgs e)
		{
			if (NodesChanged != null) {
				this.NodesChanged(sender, e);
			}
		}
		
		public void IterateDrawings(Action<NodeDrawing> action){
			foreach(Node node in nodes){
				RecursiveTraverse(node.Drawing as NodeDrawing, action);
			}
		}
		
		private void RecursiveTraverse(NodeDrawing nodeDrawing, Action<NodeDrawing> action)
		{
			action(nodeDrawing);
			if (!(nodeDrawing.Item as Node).IsLeaf && (nodeDrawing.Item as Node).IsExpanded) {
				foreach (Node node in (nodeDrawing.Item as Node).Nodes) {
					RecursiveTraverse(node.Drawing as NodeDrawing, action);
				}
			}
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
