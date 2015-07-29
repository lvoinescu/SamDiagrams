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
namespace SamDiagrams
{
	/// <summary>
	/// Description of DiagramItem.
	/// </summary>
	/// 
	
	
	public delegate void DiagramItemClickHandler(object sender, EventArgs e);
	[Serializable]
	public class DiagramItem : IDrawableItem
	{

		public event BeforeNodeExpandOrCollapseHandler BeforeNodeExpandOrCollapse;
		public event ItemResizedHandler ItemResized;
		public event ItemMovedHandler ItemMoved;
		
		private DiagramContainer diagramContainer;
		private Point location;
		private Size size;
		
		private string title;
		private DiagramContainer parentContainer;
		private Color color = Color.LightSteelBlue;
		private Color _ClassStartColor = Color.LightGray;
		private Color _ClassEndColor = Color.White;

		private Color _ClassBorderColor = Color.SteelBlue;
		private int scaledCornerRadius = 10;
		private int CornerRadius = 10;
		private int rowScaledHeight=10;
		private Font rowFont;
		private Font titleFont;
		private static int destination = 6;
		private static int titleHeight = 26;
		private static int expanderSize = 10;
		private static int tabNodWidth =10;
		private static int leftPadding = 4;
		private static int titleOffset =50;
		private SolidBrush b_Black = new SolidBrush(Color.Black);
		private SolidBrush b_White = new SolidBrush(Color.White);
		private SolidBrush b_Border = new SolidBrush(Color.SteelBlue);
		private Pen contur ;
		private float scaleFactor = 1;
		private int crtDrawingRow =0;
		private int nodsYOffset = 0;
		private int  yScaledOffset;
		private bool invalidated = true;
		private bool isSelected = false;
		private int titleWidth;
		private int crtExpanderCheckRow = 0;
		private int nodDblX, nodDblY;
		private int nrOfDisplayedRows =0;
		private int crtNodCheck = 0;
		
		private List<DiagramItemNode> nodes ;
		private Point initialSelectedLocation;
		
		private Image titleImage;
		internal SelectionBorder selectionBorder = null;
		internal Font titleScaledFont;
		internal Font rowScaledFont;
		
		
		public Color Color
		{
			get { return color; }
			set { color = value; }
		}
		
		public DiagramContainer DiagramContainer {
			get { return diagramContainer; }
			set { diagramContainer = value; }
		}

		
		public string Title {
			get { return title; }
			set { title = value; }
		}

		public Size Size {
			get { return size; }
		}
		
		public bool Invalidated{
			get { return invalidated; }
			set { invalidated = value;
			}
		}
		
		public Image TitleImage {
			get { return titleImage; }
			set { titleImage = value; }
		}
		
		internal Point InitialSelectedLocation {
			get { return initialSelectedLocation; }
			set { initialSelectedLocation = value; }
		}
		public bool IsSelected {
			get { return isSelected; }
			set {
				isSelected = value;
				if (value)
				{
					int i = parentContainer.DrawableItems.IndexOf(this);
					SelectionBorder border = new SelectionBorder(this);
					parentContainer.DrawableItems.Add(border);
					selectionBorder = border;
				}
				else
				{
					foreach (IDrawableItem idi in parentContainer.DrawableItems)
					{
						if (idi is SelectionBorder)
						{
							SelectionBorder s = (SelectionBorder)idi;
							if (s.Item == this)
							{
								parentContainer.DrawableItems.Remove(idi);
								selectionBorder = null;
								return;
							}
						}
					}
				}
			}
		}


		

		private Rectangle bounds = new Rectangle();

		public Rectangle Bounds
		{
			get { return bounds; }
		}

		public List<DiagramItemNode> Nodes {
			get { return nodes; }
			set { nodes = value;
				AutoSizeContent();
				
			}
		}
		public Point Location {
			get { return location; }
		}

		internal int MinHeight
		{
			get
			{
				return 2* scaledCornerRadius + titleHeight + destination;
			}
		}
		internal int MinWidth
		{
			get
			{
				return 100;
			}
		}
		public DiagramItem(DiagramContainer p, String title, Point location, Size s)
		{
			invalidated = true;
			this.location =  location;
			this.size= s;
			this.parentContainer = p;
			this.title = title;
			contur = new Pen(b_Border, 1F);
			this.rowFont = new Font(this.parentContainer.Font.FontFamily, 9.0F);
			this.titleFont = new Font(this.parentContainer.Font.FontFamily,9.0F, FontStyle.Bold);
			nodes = new List<DiagramItemNode>();
			scaleFactor =  (float)p.ZoomFactor/100;
			p.ZoomFactorChanged+=new ZoomFactorChangedHandler(OnZoomFactorChanged);
			//scaledLocation = new Point((int)(location.X), (int)(location.Y));
			//this.scaledSize = new Size(s.Width,s.Height);
			AutoSizeContent();
		}
		
		public void SetLocation(int x, int y)
		{
			int dx = x - this.location.X;
			int dy = y - this.location.Y;
			this.location.X =x;
			this.location.Y =y;
			bounds.X = x;
			bounds.Y = y;
			bounds.Width = size.Width;
			bounds.Height = size.Height;
			if (ItemMoved != null)
			{
				ItemMoved(this,new ItemMovedEventArg(this,dx,dy));
			}
		}

		public void SetSize(int widht, int height)
		{
			size.Width = widht;
			size.Height = height;
			
		}

		public void Draw()
		{
			using(Graphics g = parentContainer.CreateGraphics())
				Draw(g);
		}
		
		public void Draw(Graphics g){

			//float scaleFactor = 1.0f;
			
			GraphicsPath path = new GraphicsPath();

			//header
			path.AddArc(location.X, location.Y, scaledCornerRadius, scaledCornerRadius, 180, 90);
			path.AddArc(location.X + size.Width - scaledCornerRadius, location.Y, scaledCornerRadius, scaledCornerRadius, 270, 90);
			path.AddLine(location.X + size.Width, location.Y + (int)(scaledCornerRadius / 2), location.X + size.Width, location.Y + (int)(scaledCornerRadius / 2) + titleHeight + titleOffset);
			path.AddLine(location.X, location.Y + (int)(scaledCornerRadius / 2) + titleHeight + titleOffset, location.X, location.Y + (int)(scaledCornerRadius / 2));
			path.CloseAllFigures();


			
			
			//title background
			// LinearGradientBrush linBrush = new LinearGradientBrush(new Rectangle(scaledLocation,scaledSize), _ClassStartColor, _ClassEndColor, LinearGradientMode.Horizontal);
			g.FillPath(new SolidBrush(color), path);
			g.DrawPath(contur,path);

			//title
			int titlePosX =  location.X+(size.Width - titleWidth)/2;
			g.DrawString(this.title, titleScaledFont, b_Black, new PointF(titlePosX, location.Y));
			if(titleImage!=null)
				g.DrawImage(titleImage, new Rectangle(((int)((this.location.X+4))),((int)((this.location.Y+4))),((int)(16 )),((int)(16 ))));


			int xscaledOffset = (int)((this.location.X + leftPadding) );
			yScaledOffset = location.Y + titleHeight + (int)(scaledCornerRadius / 2);
			int mainHeightScaled = rowScaledHeight * nrOfDisplayedRows + titleOffset;
			if (!parentContainer.AutoSizeItem)
			{
				mainHeightScaled = this.size.Height - titleOffset - rowScaledHeight - scaledCornerRadius;
			}
			Rectangle lineMainR = new Rectangle(location.X, yScaledOffset, size.Width, mainHeightScaled);
			g.FillRectangle(Brushes.White,lineMainR );
			g.DrawRectangle(contur,lineMainR);

			crtDrawingRow = 0;
			for(int i=0;i<nodes.Count;i++)
			{
				RecursiveDraw(g, rowScaledFont, scaleFactor, nodes[i],xscaledOffset,yScaledOffset + titleOffset);
				crtDrawingRow++;
			}
			
			path = new GraphicsPath();
			path.AddArc(location.X, lineMainR.Y + lineMainR.Height - (int)(scaledCornerRadius / 2), scaledCornerRadius, scaledCornerRadius, 180, -90);
			path.AddArc(location.X + size.Width - scaledCornerRadius, lineMainR.Y + lineMainR.Height - (int)(scaledCornerRadius / 2), scaledCornerRadius, scaledCornerRadius, 90, -90);
			path.CloseAllFigures();
			g.FillPath(new SolidBrush(color), path);
			g.DrawPath(contur, path);
			
			g.DrawLine(Pens.Black, location.X,  yScaledOffset, location.X + 40,   yScaledOffset);

		}
		

		public Size getSize()
		{
			return this.size;
		}

		public Point getLocation()
		{
			return this.location;
		}

		public void AddOnDiagram(DiagramContainer d, Color c)
		{
			this.diagramContainer = d;
			scaleFactor = (float)parentContainer.ZoomFactor / 100;
			rowScaledHeight = CalculateRowHeight();
			this.color = c;
			this.AutoSizeContent();

			d.AddItem(this);
			d.DiagramItems.Add(this);
		}
		
		
		internal void OnClick(MouseEventArgs e, float scaleFactor)
		{
			crtExpanderCheckRow = 0;
			for(int i=0;i<nodes.Count;i++)
			{
				DiagramItemNode nod = RecursiveExpanderCheck(nodes[i],(int)(leftPadding ),nodsYOffset, scaleFactor, e.X, e.Y, nodes[i].IsExpanded);
				if(nod!=null)
				{
					if(BeforeNodeExpandOrCollapse!=null){
						this.BeforeNodeExpandOrCollapse(this, new BeforeNodeExpandOrCollapseArg(nod));
					}
					nod.IsExpanded =!nod.IsExpanded;
					AutoSizeContent();
					parentContainer.Invalidate(new Rectangle(location,size));
					return;
				}
				crtExpanderCheckRow++;
			}
		}
		
		
		internal void OnDblClick(MouseEventArgs e, float scaleFactor)
		{
			Point pInside = new Point((int)(e.Location.X / scaleFactor), (int)(e.Location.Y / scaleFactor));
			pInside.Offset(diagramContainer.hScrollBar1.Value, diagramContainer.vScrollBar1.Value);
			int x = (int)(pInside.X - this.Location.X);
			int y = (int)(pInside.Y - this.Location.Y);
			DiagramInfoItemNode diin = this.GetNodAtXY(x, y);
			if (diin != null && diin.Nod.Editable)
			{
				diagramContainer.NodeEditor.ShowOnNode(diin);
			}
		}

		public DiagramInfoItemNode GetNodAtXY(int x, int y)
		{
			crtNodCheck= 0;
			for (int i = 0; i < nodes.Count; i++)
			{
				DiagramItemNode nod = GetNodAtXYRec(nodes[i],(int)(leftPadding) , nodsYOffset,x,y, nodes[i].IsExpanded);
				if (nod != null)
				{
					return new DiagramInfoItemNode(nod, new Rectangle(nodDblX,nodDblY,    this.location.X +  this.size.Width - nodDblX,this.rowScaledHeight));
				}
				crtNodCheck++;
			}
			return null;
		}
		
		Random r = new Random(255);
		private void RecursiveDraw(Graphics g, Font  titleScaledFont, float scaleFactor,DiagramItemNode nod, int xOffset, int yOffset)
		{

			int cY = yOffset  + rowScaledHeight *crtDrawingRow;
			int cX =xOffset;
			int expanderScaledSize = expanderSize;


			if (!parentContainer.AutoSizeItem && cY > this.location.Y + this.size.Height - scaledCornerRadius - rowScaledHeight)
				return;

			if(!nod.IsLeaf)
			{
				//g.FillRectangle(Brushes.AliceBlue, this.location.X,cY,this.size.Width, lineSpaceing);
				g.DrawRectangle(Pens.Black, cX ,cY ,expanderScaledSize,expanderScaledSize);
				g.DrawLine(Pens.Black, cX,cY+expanderScaledSize/2  ,cX +expanderScaledSize, cY+expanderScaledSize/2  );
				if(!nod.IsExpanded)
				{
					g.DrawLine(Pens.Black, cX +expanderScaledSize/2,cY  ,cX+expanderScaledSize/2 , cY+ +expanderScaledSize);
				}
			}
			int imgSpace = 14;
			int space = 0;
			int imgPadding = 4;
			for (int i = 0; i < nod.Images.Count; i++)
			{
				g.DrawImage(nod.Images[i], cX + expanderScaledSize + space, cY + (int)(imgPadding / 2), rowScaledHeight - imgPadding, rowScaledHeight - imgPadding);
				space += (int)(imgSpace) - imgPadding/2;
			}
			//g.FillRectangle(new SolidBrush(Color.FromArgb(r.Next(255),r.Next(255),r.Next(255))),
			//               new Rectangle( new Point(cX + space + expanderScaledSize, cY), new Size(100, rowScaledHeight)));
			g.DrawString(nod.Text, titleScaledFont, b_Black, new PointF(cX + space + expanderScaledSize, cY));
			if((!nod.IsLeaf) && nod.IsExpanded)
			{
				xOffset += tabNodWidth;
				for(int i=0;i<nod.Count;i++)
				{
					crtDrawingRow++;
					RecursiveDraw(g,titleScaledFont, scaleFactor, nod[i],  xOffset, yOffset);
				}
			}
		}
		
		private DiagramItemNode RecursiveExpanderCheck(DiagramItemNode nod,int x, int y,float scaleFactor, int mouseX, int mouseY , bool parentIsExpanded)
		{
			int cY = (int)(y + rowScaledHeight * crtExpanderCheckRow);
			int cX =(int)(x );
			int expanderScaledSize = (int)(expanderSize );
			if(!nod.IsLeaf)
			{
				float yT = cY;
				RectangleF r = new RectangleF(cX ,yT,expanderScaledSize,expanderScaledSize);
				if(r.Contains(mouseX, mouseY))
					return nod;
				if(nod.IsExpanded)
				{
					x += (int)(tabNodWidth );
				}
				for(int i=0;i<nod.Count;i++)
				{
					DiagramItemNode ndr = null;
					if(parentIsExpanded)
						crtExpanderCheckRow++;
					ndr = RecursiveExpanderCheck( nod[i],  x, y, scaleFactor, mouseX, mouseY, parentIsExpanded && nod[i].IsExpanded);
					if(ndr!=null)
						return ndr;
				}
			}
			
			return null;
		}

		private DiagramItemNode GetNodAtXYRec(DiagramItemNode nod, int x, int y,  int mouseX, int mouseY, bool parentIsExpanded)
		{
			int cY = (int)(y + rowScaledHeight * crtNodCheck);
			int cX = (int)(x );
			float yT = cY;
			RectangleF r = new RectangleF(cX, yT, this.size.Width ,rowScaledHeight);
			if (r.Contains(mouseX, mouseY))
			{
				nodDblX = this.location.X + (int)(x) + (int)(expanderSize );
				nodDblY = this.location.Y + (int)(nodsYOffset) + (int)(crtNodCheck * rowScaledHeight);
				return nod;
			}
			if (nod.IsExpanded)
			{
				x += (int)(tabNodWidth );
			}
			for (int i = 0; i < nod.Count; i++)
			{
				DiagramItemNode ndr = null;
				if (parentIsExpanded)
					crtNodCheck++;
				ndr = GetNodAtXYRec(nod[i], x, y,  mouseX, mouseY, parentIsExpanded && nod[i].IsExpanded);
				if (ndr != null)
				{
					nodDblX = this.location.X + (int)(x ) + (int)(leftPadding );
					nodDblY = this.location.Y + (int)(nodsYOffset) + (int)(crtNodCheck * rowScaledHeight );
					return ndr;
				}
			}
			return null;
		}

		private int CalculateRowHeight()
		{
			Font rowScaledFont = new Font(this.rowFont.FontFamily,(float)((rowFont.Size-0)));
			Size sT = TextRenderer.MeasureText("TEST",rowScaledFont);
			return (int)(sT.Height);
		}
		
		internal void AutoSizeContent()
		{
			invalidated = true;
			int nr = 0;
			for(int i=0;i<nodes.Count;i++)
			{
				nr+=CalculateDisplayedRows(nodes[i], nodes[i].IsExpanded );
			}
			this.rowScaledHeight = CalculateRowHeight();
			scaledCornerRadius = (int)(CornerRadius );
			titleOffset = (int)(destination );
			if (scaledCornerRadius < 1)
				scaledCornerRadius = 1;
			if (parentContainer.AutoSizeItem)
				this.nrOfDisplayedRows = nr;
			this.size.Height = rowScaledHeight * nr + titleOffset + titleHeight + (int)(scaledCornerRadius);
			//this.size.Height = (int)(this.scaledSize.Height/scaleFactor);
			//this.scaledSize.Width = (int)(this.size.Width );
			titleScaledFont = new Font(this.titleFont.FontFamily,(float)(titleFont.Size),FontStyle.Bold);
			rowScaledFont = new Font(this.rowFont.FontFamily,rowFont.Size);
			Size sT = TextRenderer.MeasureText(title,titleFont);
			titleHeight = sT.Height;
			titleWidth = sT.Width;

			//important
			nodsYOffset = sT.Height + titleOffset + (int)(scaledCornerRadius/2);
			if(ItemResized!=null)
				ItemResized(this, new ItemResizedEventArg(this.size.Width, this.size.Height));

		}
		
		private int CalculateDisplayedRows(DiagramItemNode nod, bool parentIsExpanded )
		{
			int nr = 0;
			if(parentIsExpanded)
			{
				for(int i=0;i<nod.Count;i++)
				{
					nr += CalculateDisplayedRows(nod[i], parentIsExpanded && nod[i].IsExpanded );
				}
			}
			nr++;
			return nr;
		}
		
		private void OnZoomFactorChanged(object sender, ZoomFactorChangedArg e)
		{
			scaleFactor = (float)e.ZoomLevel/100;
			AutoSizeContent();
		}

		

	}


	public class DiagramInfoItemNode
	{
		private DiagramItemNode nod;

		public DiagramItemNode Nod
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
		public DiagramInfoItemNode(DiagramItemNode nod, Rectangle r)
		{
			this.boundingRectangle = r;
			this.nod = nod;
		}
	}
	
}
