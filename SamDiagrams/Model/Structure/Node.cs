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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using SamDiagrams.Drawings;
using SamDiagrams.Model;
namespace SamDiagrams
{
	public delegate void NodeAddedHandler(object sender, EventArgs e);
	
	
	/// <summary>
	/// Description of DiagramItemNode.
	/// </summary>
	public class Node : Item, ILinkable
	{
		public event ListChangedEventHandler NodesChanged;
		
		private String nodeText;
		private Hashtable asoc = new Hashtable();
		private bool isExpanded = true;
		private object tag;
		private bool editable = true;
		private List<Image> images;
		private Structure item;
		private Node parent;
		private IDrawing nodeDrawing;
		private readonly BindingList<Node> nodes;
		private List<ILink> links = new List<ILink>();
		public IDrawing Drawing {
			get {
				return nodeDrawing;
			}
			set {
				this.nodeDrawing = value;
			}
		}

		public BindingList<Node> Nodes {
			get {
				return nodes;
			}
		}
		
		public Node Parent {
			get { return parent; }
			set { parent = value; }
		}

		public Color Color {
			get {
				throw new NotImplementedException();
			}
			set {
				throw new NotImplementedException();
			}
		}
		public List<Image> Images {
			get { return images; }
			set { images = value; }
		}

		public string Name {
			get {
				throw new NotImplementedException();
			}
			set {
				throw new NotImplementedException();
			}
		}

		public bool Editable {
			get { return editable; }
			set { editable = value; }
		}
		public object Tag {
			get { return tag; }
			set { tag = value; }
		}
		public bool IsExpanded {
			get { return isExpanded; }
			set { isExpanded = value; }
		}
		public bool IsLeaf {
			get { return this.Nodes.Count < 1; }
		}
		public string Text {
			get { return nodeText; }
			set { nodeText = value; }
		}
		public Structure DiagramItem {
			get { return item; }
		}

		public List<ILink> Links {
			get {
				return this.links;
			}
		}
		public Node(string itemText, Structure item)
		{
			images = new List<Image>();
			nodes = new BindingList<Node>();
			nodes.ListChanged += new ListChangedEventHandler(OnListChanged);
			this.item = item;
			this.nodeText = itemText;
		}
		public Node(string itemText, bool editable, Structure item)
			: this(itemText, item)
		{
			this.editable = editable;
		}

		public Node(string itemText, bool editable, Image img, Structure item)
			: this(itemText, editable, item)
		{
			this.images.Add(img);
		}

		void OnListChanged(object sender, ListChangedEventArgs e)
		{
			if (NodesChanged != null) {
				this.NodesChanged(sender, e);
			}
		}

		public int Level {
			get {
				return recursiveComputeLevel(this);
			}
		}
		
		private int recursiveComputeLevel(Node node)
		{
			if (node.parent != null)
				return recursiveComputeLevel(node.Parent) + 1;
			else
				return 0;
		}
		
		public Node this[string name] {
			get {
				if (asoc.ContainsKey(name))
					return (Node)asoc[name];
				return null;
			}
			
		}

		public Node AddNode(Node nod)
		{
			this.Nodes.Add(nod);
			nod.parent = this;
			asoc.Add(nod.Text, nod);
			return nod;
		}
		
		public IList getNodes()
		{
			return this.Nodes;
		}
		
		public override String ToString()
		{
			return this.Text;
		}
		
	}
}
