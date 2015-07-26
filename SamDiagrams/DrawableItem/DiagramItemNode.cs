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
namespace SamDiagrams
{
	/// <summary>
	/// Description of DiagramItemNode.
	/// </summary>
	public class DiagramItemNode : CollectionBase
	{
		private String nodeText;
		private Hashtable asoc = new Hashtable();
		private bool isExpanded = true;
		private object tag;
		private bool editable = true;
		private List<Image> images;
		private DiagramItem item;
		private DiagramItemNode parent;
		
		public DiagramItemNode Parent {
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
		public DiagramItem DiagramItem{
			get {return item; }
		}
		
		public DiagramItemNode(string itemText, DiagramItem item)
		{
			images = new List<Image>();
			this.item = item;
			this.nodeText = itemText;
		}
		public DiagramItemNode(string itemText, bool editable, DiagramItem item):  this(itemText, item)
		{
			this.editable = editable;
		}

		public DiagramItemNode(string itemText, bool editable,Image img, DiagramItem item):this(itemText,editable,item)
		{
			this.images.Add(img);
		}
		public DiagramItemNode this[int Index]
		{
			get
			{
				return (DiagramItemNode)List[Index];
			}
			set
			{
				List.IndexOf(value);
				List[Index] = value;
			}
		}

		public DiagramItemNode this[string name]
		{
			get
			{
				if(asoc.ContainsKey(name))
					return (DiagramItemNode)asoc[name];
				return null;
			}
			
		}

		public void AddNode(DiagramItemNode nod)
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
