/*
 * Created by SharpDevelop.
 * User: L
 * Date: 2/17/2013
 * Time: 11:08 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using SamDiagrams.Model;
namespace SamDiagrams
{
	/// <summary>
	/// Description of DiagramItemNode.
	/// </summary>
	public class Node : CollectionBase, Item
	{
		private String nodeText;
		private Hashtable asoc = new Hashtable();
		private bool isExpanded = true;
		private object tag;
		private bool editable = true;
		private List<Image> images;
		private Structure item;
		private Node parent;
		
		public Node Parent {
			get { return parent; }
			set { parent = value; }
		}
		
		public List<Image> Images
		{
			get { return images; }
			set { images = value; }
		}
		public bool Editable
		{
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
			get { return this.List.Count<1; }
		}
		public string Text {
			get { return nodeText; }
			set { nodeText = value; }
		}
		public Structure DiagramItem{
			get {return item; }
		}
		
		public Node(string itemText, Structure item)
		{
			images = new List<Image>();
			this.item = item;
			this.nodeText = itemText;
		}
		public Node(string itemText, bool editable, Structure item):  this(itemText, item)
		{
			this.editable = editable;
		}

		public Node(string itemText, bool editable,Image img, Structure item):this(itemText,editable,item)
		{
			this.images.Add(img);
		}
		public Node this[int Index]
		{
			get
			{
				return (Node)List[Index];
			}
			set
			{
				List.IndexOf(value);
				List[Index] = value;
			}
		}

		public Node this[string name]
		{
			get
			{
				if(asoc.ContainsKey(name))
					return (Node)asoc[name];
				return null;
			}
			
		}

		public void AddNode(Node nod)
		{
			this.List.Add(nod);
			nod.parent = this;
			asoc.Add(nod.Text, nod);
		}
		
		public IList getNodes() {
			return this.List;
		}
		
	}
}
