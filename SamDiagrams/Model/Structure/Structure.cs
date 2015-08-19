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
using System.Collections.Generic;
using SamDiagrams.Linking;
using SamDiagrams.Model;
namespace SamDiagrams
{

	[Serializable]
	public class Structure : Item
	{
		
		private Color color = Color.LightSteelBlue;
		private List<Node> nodes;
		private DiagramContainer diagramContainer;
		private string title;
		
		private bool invalidated;
		internal List<StructureLink> links;
		private Image titleImage;
		internal SelectionBorder selectionBorder = null;
		
		
		public Color Color {
			get { return color; }
			set { color = value; }
		}

		public List<StructureLink> Links {
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
			links = new List<StructureLink>();
		}
		

		public void AddOnDiagram(DiagramContainer container, Color c)
		{
			this.diagramContainer = container;
			this.color = c;
			container.AddStructure(this);
		}
		
		public override string ToString()
		{
			return string.Format("[Structure Title={0}]", title);
		}

		
		


	}
}
