/*
 * Created by SharpDevelop.
 * User: L
 * Date: 2/16/2013
 * Time: 9:03 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using SamDiagrams.DrawableItem.NodeEditor;
using SamDiagrams.Linking.Orchestrator;

namespace SamDiagrams
{
    /// <summary>
    /// Description of DiagramContainer.
    /// </summary>
    public class DiagramContainer : UserControl
    {

        public event DiagramItemClickHandler DiagramItemClick;
        public event SelectedItemsChangeHandler SelectedItemsChanged;
        public event ZoomFactorChangedHandler ZoomFactorChanged;
        public event ItemsMovedHandler ItemsMoved;
        private List<DiagramItem> selectedItems;
        private DiagramItem primarySelectedItem;
        private bool isMouseHoldingForMovement = false;
        private bool isMouseHoldingForResize = false;
        private Point mouseHoldingInitial = new Point(0, 0);
        private Point itemMouseClickLocation = new Point(0, 0);
        internal List<IDrawableItem> DrawableItems;
        List<DiagramItem> diagramItems;
        private int zoomFactor;
        private float scaleFactor = 1;
        private int gridSize = 16;
        private bool snapObjectsToGrid = false;
        private int drawableHeight, drawableWidth;
        private LinkOrchestrator linkOrchestrator;
        private SelectionBorder currentResizeingBorder;
        private bool autoSizeItem = true;
        private Region invalidatedRegion = new Region();
        private Region oldIvalidatedRegion = new Region();
		private NodeTextEditor nodeEditor;
		
		public NodeTextEditor NodeEditor {
			get { return nodeEditor; }
		}


        public bool AutoSizeItem
        {
            get { return autoSizeItem; }
            set
            {
                autoSizeItem = value;
                foreach (DiagramItem di in diagramItems)
                    di.AutoSizeContent();
                //Invalidate();
            }
        }

        public LinkOrchestrator LinkOrchestrator
        {
            get { return linkOrchestrator; }
            set { linkOrchestrator = value; }
        }
        public int DrawableWidth
        {
            get { return drawableWidth; }
            set
            {
                drawableWidth = value;
                SetScrollBarValues();
            }
        }

        public int DrawableHeight
        {
            get { return drawableHeight; }
            set
            {
                drawableHeight = value;
                SetScrollBarValues();
            }
        }

        internal HScrollBar hScrollBar1 = new HScrollBar();
        internal VScrollBar vScrollBar1 = new VScrollBar();

        public bool SnapObjectsToGrid
        {
            get { return snapObjectsToGrid; }
            set { snapObjectsToGrid = value; }
        }
        public int ZoomFactor
        {
            get { return zoomFactor; }
            set
            {
                zoomFactor = value;
                scaleFactor = (float)zoomFactor / 100;
                OnZoomChanged();
            }
        }



        private string tests;
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<DiagramItem> DiagramItems
        {
            get { return diagramItems; }
            set { diagramItems = value; }
        }


        public DiagramContainer()
        {
            zoomFactor = 100;
            DrawableItems = new List<IDrawableItem>();
            diagramItems = new List<DiagramItem>();
            selectedItems = new List<DiagramItem>();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw |
                ControlStyles.DoubleBuffer | ControlStyles.UserPaint, true);
            this.AutoScroll = false;
            this.DoubleBuffered = false;
            hScrollBar1 = new HScrollBar();
            hScrollBar1.Dock = DockStyle.Bottom;
            vScrollBar1 = new VScrollBar();
            vScrollBar1.Dock = DockStyle.Right;
            vScrollBar1.Scroll += new ScrollEventHandler(vScrollBar1_Scroll);
            hScrollBar1.Scroll += new ScrollEventHandler(hScrollBar1_Scroll);
            this.Resize += new EventHandler(DiagramContainer_Resize);
            this.Controls.Add(hScrollBar1);
            this.Controls.Add(vScrollBar1);
            linkOrchestrator = new LinkOrchestrator(this);
            nodeEditor =  new NodeTextEditor(this);
            this.Invalidate();

        }


        protected override void InitLayout()
        {
            base.InitLayout();
            this.drawableHeight = this.Height;
            this.drawableWidth = this.Width;
            SetScrollBarValues();
        }

        void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            this.Invalidate();
        }

        void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            this.Invalidate();
        }

        public void AddItem(IDrawableItem di)
        {
            DrawableItems.Add(di);

        }

        void linkManager_LinkDirectionChangedEvent(object sender, LinkDirectionChangedArg e)
        {
        	
        }


        Random rd = new Random();
        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            //e.Graphics.DrawRectangle(Pens.White, new Rectangle(0, 0, (int)e.Graphics.ClipBounds.Width, (int)e.Graphics.ClipBounds.Height));
            //e.Graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, (int)e.Graphics.ClipBounds.Width, (int)e.Graphics.ClipBounds.Height));
            //if(e.Graphics.ClipBounds.X ==0)
            //e.Graphics.FillRectangle(Brushes.Blue, new Rectangle((int)e.Graphics.ClipBounds.X, (int)e.Graphics.ClipBounds.Y, (int)e.Graphics.ClipBounds.Width, (int)e.Graphics.ClipBounds.Height));
            //else
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            Point pt = new Point(hScrollBar1.Value, vScrollBar1.Value);
            Rectangle r = new Rectangle(pt, this.Size);

            e.Graphics.TranslateTransform(-this.hScrollBar1.Value, -this.vScrollBar1.Value);
            e.Graphics.ScaleTransform(scaleFactor, scaleFactor, System.Drawing.Drawing2D.MatrixOrder.Append);
            //e.Graphics.Save();
            RectangleF rct = e.Graphics.ClipBounds;
            //rct.X = rct.X * scaleFactor;
            //rct.Y = rct.Y * scaleFactor;
            //rct.Width = rct.Width * scaleFactor;
            //rct.Height = rct.Height * scaleFactor;
//e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(rd.Next(255), rd.Next(255), rd.Next(255))), new Rectangle((int)rct.X, (int)rct.Y, (int)rct.Width, (int)rct.Height));
            DrawGrid(e.Graphics);

                        linkOrchestrator.Draw(e.Graphics);
            Rectangle ra = new Rectangle(0, 0, 0, 0);
            Rectangle prevR = new Rectangle(0, 0, 0, 0);
            if (DrawableItems.Count > 0)
            {
                int nr = 0;
                for (int i = 0; i < DrawableItems.Count - 1; i++)
                {
                    IDrawableItem di = DrawableItems[i];
                    
                    Rectangle nextR = new Rectangle(0, 0, 0, 0);
                    bool f = false;
                    for (int j = i + 1; j < DrawableItems.Count &&!f; j++)
                    {
                        IDrawableItem din = DrawableItems[i + 1];
                        if (din is SelectionBorder)
                            continue;
                        else
                        {
                            nextR.X = din.getLocation().X;
                            nextR.Y = din.getLocation().Y;
                            nextR.Width = din.getSize().Width;
                            nextR.Height = din.getSize().Height;

                            f = true;
                        }
                    }
                    ra.X = (int)(di.getLocation().X * scaleFactor);
                    ra.Y = (int)(di.getLocation().Y * scaleFactor);
                    ra.Width = (int)(di.getSize().Width * scaleFactor);
                    ra.Height = (int)(di.getSize().Height * scaleFactor);

                    if (rct.IntersectsWith(ra) && !nextR.Contains(ra))
                    {
                        nr++;
                        di.Draw(e.Graphics);
                        prevR.X = di.getLocation().X;
                        prevR.Y = di.getLocation().Y;
                        prevR.Size = di.getSize();
                      
                    }
                    di.Invalidated = false;
                }
                if (DrawableItems.Count > 0)
                {
                    IDrawableItem dif = DrawableItems[DrawableItems.Count - 1];
                    dif.Draw(e.Graphics);
                }
            }
            
        	nodeEditor.Draw(e.Graphics);
            
            e.Graphics.DrawString(tests, this.Font, Brushes.Black, 0, 0);
            Pen p = new Pen(Color.Black, 4);
        }

        protected void OnZoomChanged()
        {
            SetScrollBarValues();
            if (ZoomFactorChanged != null)
                this.ZoomFactorChanged(this, new ZoomFactorChangedArg(zoomFactor));
            //this.textEditor.Hide();
            linkOrchestrator.ArrangeAllLinks();
            invalidatedRegion = new Region(new Rectangle(this.Location, this.Size));
        }

        protected void DrawGrid(Graphics g)
        {
            Pen p = new Pen(Color.FromArgb(100, 200, 200, 200));
            float step = gridSize;
            if (step < 1)
                step = 1;
            for (float i = 0; i < this.drawableWidth; i += step)
            {
                g.DrawLine(p, i, 0, i, this.drawableHeight);
            }
            for (float j = 0; j < this.drawableHeight; j += step)
            {
                g.DrawLine(p, 0, j, this.drawableWidth, j);
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            for (int i = DrawableItems.Count - 1; i >= 0; i--)
            {
                IDrawableItem dii = DrawableItems[i];
                if (dii is DiagramItem)
                {
                    DiagramItem di = (DiagramItem)dii;
                    Rectangle r = new Rectangle((int)(di.Location.X * scaleFactor), (int)(di.Location.Y * scaleFactor), (int)(di.Size.Width * scaleFactor), (int)(di.Size.Height * scaleFactor));
                    Point p = e.Location;
                    p.Offset(hScrollBar1.Value, vScrollBar1.Value);
                    if (r.Contains(p))
                    {
                        Point pInside = new Point(e.Location.X, e.Location.Y);
                        pInside.Offset(hScrollBar1.Value, vScrollBar1.Value);
                        int x = (int)(pInside.X - di.Location.X * scaleFactor);
                        int y = (int)(pInside.Y - di.Location.Y * scaleFactor);
                        di.OnClick(new MouseEventArgs(e.Button, e.Clicks, x, y, e.Delta), scaleFactor);
                        if (this.DiagramItemClick !=null)
                        	DiagramItemClick(di, e);
                        return;
                    }
                }

            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            for (int i = DrawableItems.Count - 1; i >= 0; i--)
            {
                IDrawableItem dii = DrawableItems[i];
                if (dii is DiagramItem)
                {
                    DiagramItem di = (DiagramItem)dii;
                    Rectangle r = new Rectangle(di.Location.X, di.Location.Y, di.Size.Width, di.Size.Height);
                    Point p = new Point((int)(e.Location.X / scaleFactor), (int)(e.Location.Y / scaleFactor));
                    p.Offset(hScrollBar1.Value, vScrollBar1.Value);
                    if (r.Contains(p))
                    {
                    	di.OnDblClick(e, scaleFactor);
                            return;
                        
                        
                        //nodeEditor.ShowOnNode(diin.Nod);
                    }
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
        	
        	this.Focus();
            bool found = false;
            for (int i = DrawableItems.Count - 1; i >= 0 && !found; i--)
            {
                IDrawableItem dii = DrawableItems[i];
                if (dii is DiagramItem)
                {
                    DiagramItem di = (DiagramItem)DrawableItems[i];
                    Rectangle r = new Rectangle(di.Location.X, di.Location.Y, di.Size.Width, di.Size.Height);
                    Point p = new Point((int)(e.Location.X / scaleFactor), (int)(e.Location.Y / scaleFactor));
                    p.Offset(hScrollBar1.Value, vScrollBar1.Value);
                    if (r.Contains(p))
                    {
                   	
                        mouseHoldingInitial = new Point(e.X, e.Y);
                        found = true;
                        if(!nodeEditor.Visible)
                       		isMouseHoldingForMovement = true;
                        else
                        	checkEditorInsideMouseDown(e,scaleFactor);
                        DrawableItems.Remove(di);
                        DrawableItems.Add(di);
                        Point pInside = new Point(e.Location.X, e.Location.Y);
                        int x = (int)(pInside.X - di.Location.X * scaleFactor);
                        int y = (int)(pInside.Y - di.Location.Y * scaleFactor);
                        itemMouseClickLocation.X = x;
                        itemMouseClickLocation.Y = y;
                        primarySelectedItem = di;
                        di.InitialSelectedLocation = di.Location;
                        if (Control.ModifierKeys == Keys.Control)
                        {
                            if (!di.IsSelected && !selectedItems.Contains(di))
                            {
                                di.IsSelected = true;
                                selectedItems.Add(di);
                                invalidatedRegion = new Region(di.selectionBorder.Bounds);

                            }
                            else
                                if (selectedItems.Contains(di))
                                {
                                    di.IsSelected = false;
                                    selectedItems.Remove(di);
                                }
                        }
                        else
                        {
                            if (!di.IsSelected)
                            {
                                foreach (DiagramItem dir in selectedItems)
                                    if (dir != di)
                                    {
                                        InvalidateItem(dir);
                                        dir.selectionBorder.Invalidated = true;
                                        Region re = CalculateInvalidatedRegion(dir.selectionBorder.Bounds);
                                        Invalidate(re);
                                        dir.IsSelected = false;
                                        oldIvalidatedRegion = re;

                                    }
                                selectedItems.Clear();
                                selectedItems.Add(di);
                                di.IsSelected = true;
                                InvalidateItem(di);
                                di.selectionBorder.Invalidated = true;
                                Region red = CalculateInvalidatedRegion(di.selectionBorder.Bounds);
                                Invalidate(red);
                                Update();

                            }
                        }
                        di.Invalidated = true;
                        oldIvalidatedRegion = invalidatedRegion;


                        if (this.SelectedItemsChanged != null)
                            this.SelectedItemsChanged(this, new SelectedItemsChangeArgs(selectedItems));
                        break;
                    }
                    
                }
                if (dii is SelectionBorder)
                {
                    SelectionBorder border = (SelectionBorder)dii;
                    DiagramItem item = border.Item;
                    Rectangle r = new Rectangle(item.Location.X, item.Location.Y, item.Size.Width, item.Size.Height);
                    Point p = new Point((int)(e.Location.X / scaleFactor), (int)(e.Location.Y / scaleFactor));
                    p.Offset(hScrollBar1.Value, vScrollBar1.Value);
                    Rectangle ro = new Rectangle(r.X, r.Y, r.Width, r.Height);

                    if (!r.Contains(p) && ro.Contains(p) && !autoSizeItem)
                    {
                        isMouseHoldingForResize = true;
                        border.setReziseDirection(p);
                        currentResizeingBorder = border;
                        currentResizeingBorder.initialSize = border.Item.Size;
                        mouseHoldingInitial = new Point(e.X, e.Y);
                        found = true;

                    }

                }
            }
             
            if (!found)
            {
                // no item clicked => hide the editor;
                //nodeEditor.Invalidated = true;
               
                checkEditorInsideMouseDown(e, scaleFactor);
                
                
                foreach (DiagramItem dir in selectedItems)
                    dir.IsSelected = false;
                selectedItems.Clear();
                primarySelectedItem = null;
                if(this.SelectedItemsChanged!=null)
                	this.SelectedItemsChanged(this, new SelectedItemsChangeArgs(selectedItems));

            }
            Update();
            oldIvalidatedRegion = invalidatedRegion;
            Invalidate();
        }

        private bool checkEditorInsideMouseDown(MouseEventArgs e, double scaleFactor){
        	 if(nodeEditor.Visible){
                    Point p = new Point((int)(e.Location.X / scaleFactor), (int)(e.Location.Y / scaleFactor));
                    p.Offset(hScrollBar1.Value, vScrollBar1.Value);
                    if (!nodeEditor.Bounds.Contains(p))
                    {
                        nodeEditor.Visible = false;
                    	nodeEditor.currentNode.Nod.DiagramItem.Invalidated = true;
                    	return true;
                    }
                    nodeEditor.onMouseDown(nodeEditor, e, scaleFactor);
                }
        	return false;
        }
        
        protected override void OnMouseUp(MouseEventArgs e)
        {
            isMouseHoldingForMovement = false;
            isMouseHoldingForResize = false;
            foreach (DiagramItem di in this.selectedItems)
            {
                if (di.IsSelected)
                {
                    di.InitialSelectedLocation = di.Location;
                    Rectangle rf = di.selectionBorder.Bounds;
                    rf.Inflate(1, 1);
                    invalidatedRegion = new Region(rf);
                    InvalidateTransform(invalidatedRegion);
                }


            }
            if (this.isMouseHoldingForResize)
            {
                currentResizeingBorder.initialLocation = currentResizeingBorder.Item.Location;
                currentResizeingBorder.initialSize = currentResizeingBorder.Item.Size;
            }
            
            if(nodeEditor.Visible)
            	nodeEditor.onMouseUp(nodeEditor, e, scaleFactor);

            invalidatedRegion = new Region();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            int maxX, maxY;
            maxX = this.Width;
            maxY = this.Height;
            if (isMouseHoldingForMovement)
            {
                InvalidateTransform(oldIvalidatedRegion);
                int dx = (int)((e.X - mouseHoldingInitial.X) / scaleFactor);
                int dy = (int)((e.Y - mouseHoldingInitial.Y) / scaleFactor);
                foreach (DiagramItem di in this.selectedItems)
                {
                    int x = (int)(di.InitialSelectedLocation.X + dx);
                    int y = (int)(di.InitialSelectedLocation.Y + dy);
                    tests = x.ToString() + "  " + y.ToString() + " |         dx=" + dx.ToString() + "   dy=" + dy.ToString();
                    Point pInside = new Point(e.Location.X, e.Location.Y);

                    int inf = SelectionBorder.squareSize + SelectionBorder.inflate;
                    if (snapObjectsToGrid)
                    {
                        x = ((int)x / gridSize) * gridSize;
                        y = ((int)y / gridSize) * gridSize;
                    }
                    if (x < 0)
                        x = 0;
                    if (x > this.drawableWidth - di.Size.Width)
                        x = (int)(this.drawableWidth - di.Size.Width);
                    if (y < 0)
                        y = 0;
                    if (y > this.DrawableHeight - di.Size.Height)
                        y = (int)(this.DrawableHeight - di.Size.Height);
                    di.SetLocation(x, y);
                }
                if(this.ItemsMoved!=null)
                    	ItemsMoved(this, new ItemsMovedEventArg(this.selectedItems,dx, dy));
                if (selectedItems.Count > 0)
                {
                    //Invalidate(oldIvalidatedRegion);
                    for (int i = 0; i < selectedItems.Count; i++)
                    {
                        InvalidateItem(selectedItems[i]);
                    }
                    Rectangle r = selectedItems[0].selectionBorder.Bounds;
                    r.Inflate(1,1);
                    
                    //invalidate the region delimited by the rectangle bounds of the invalidated region !!!
                    invalidatedRegion = new Region(CalculateInvalidatedRegion(r).GetBounds(this.CreateGraphics()));
                }

				oldIvalidatedRegion = invalidatedRegion;

                //Update();
                
                
                //invalidatedRegion.Translate(-hScrollBar1.Value, -vScrollBar1.Value);
                //RectangleF rf = invalidatedRegion.GetBounds(this.CreateGraphics());
                ////Rectangle ri = new Rectangle((int)(rf.X * scaleFactor), (int)(rf.Y * scaleFactor), (int)(rf.Width * scaleFactor), (int)(rf.Height * scaleFactor));
                //Rectangle ri = new Rectangle((int)(rf.X ), (int)(rf.Y ), (int)(rf.Width), (int)(rf.Height));
                //Invalidate(invalidatedRegion);
                //Update();
               
                //oldIvalidatedRegion = invalidatedRegion;
                //RectangleF re = invalidatedRegion.GetBounds(this.CreateGraphics());
                //RectangleF rfo = oldIvalidatedRegion.GetBounds(this.CreateGraphics());
                //Rectangle rio = new Rectangle((int)(rfo.X * scaleFactor), (int)(rfo.Y * scaleFactor), (int)(rfo.Width * scaleFactor), (int)(rfo.Height * scaleFactor));
                //this.Invalidate(rio);

            }
            if (this.isMouseHoldingForResize)
            {
                int dx = (int)((e.X - mouseHoldingInitial.X) / scaleFactor);
                int dy = (int)((e.Y - mouseHoldingInitial.Y) / scaleFactor);

                DiagramItem di = currentResizeingBorder.Item;
                int x = (int)(di.InitialSelectedLocation.X + dx);
                int y = (int)(di.InitialSelectedLocation.Y + dy);

                Size s = new Size();
                s.Width = di.Size.Width;
                switch (currentResizeingBorder.resizeDirection)
                {
                    case ResizeDirection.N:
                        di.SetLocation(di.Location.X, currentResizeingBorder.initialLocation.Y + dy);
                        s = new Size(currentResizeingBorder.initialSize.Width, currentResizeingBorder.initialSize.Height - dy);
                        break;
                    case ResizeDirection.S:
                        s = new Size(currentResizeingBorder.initialSize.Width, currentResizeingBorder.initialSize.Height + dy);
                        break;
                    case ResizeDirection.W:
                        di.SetLocation(x, currentResizeingBorder.initialLocation.Y);
                        s = new Size(currentResizeingBorder.initialSize.Width - dx, currentResizeingBorder.initialSize.Height);
                        break;
                    case ResizeDirection.E:
                        s = new Size(currentResizeingBorder.initialSize.Width + (int)(dx), currentResizeingBorder.initialSize.Height);
                        break;
                    case ResizeDirection.NW:
                        di.SetLocation(currentResizeingBorder.initialLocation.X + dx, currentResizeingBorder.initialLocation.Y + dy);
                        s = new Size(currentResizeingBorder.initialSize.Width - dx, currentResizeingBorder.initialSize.Height - dy);
                        break;
                    case ResizeDirection.NE:
                        di.SetLocation(di.Location.X, currentResizeingBorder.initialLocation.Y + dy);
                        s = new Size(currentResizeingBorder.initialSize.Width + dx, currentResizeingBorder.initialSize.Height - dy);
                        break;
                    case ResizeDirection.SW:
                        di.SetLocation(currentResizeingBorder.initialLocation.X + dx, di.Location.Y);
                        s = new Size(currentResizeingBorder.initialSize.Width - dx, currentResizeingBorder.initialSize.Height + dy);
                        break;
                    case ResizeDirection.SE:
                        di.SetLocation(currentResizeingBorder.initialLocation.X, di.Location.Y);
                        s = new Size(currentResizeingBorder.initialSize.Width + dx, currentResizeingBorder.initialSize.Height + dy);
                        break;
                }
                // invalidableRegion = di.selectionBorder.Bounds;
                //invalidableRegion.Inflate(1, 1);
                if (s.Height < di.MinHeight)
                    s.Height = di.MinHeight;
                di.SetSize(s.Width, s.Height);
                //linkOrchestrator.linkStrategy.ArangeLinksForItem(di);
                //Invalidate(invalidableRegion, false);
            }
            
            if(nodeEditor.Visible){
            	nodeEditor.onMouseMove(nodeEditor, e, scaleFactor);
            }
            //Update();

        }

        protected void InvalidateItem(DiagramItem di)
        {
            di.Invalidated = true;
            if (di.IsSelected)
                di.selectionBorder.Invalidated = true;
//            for (int j = 0; j < di.OutputLinkList.Count; j++)
//            {
//                di.OutputLinkList[j].Invalidated = true;
//                di.OutputLinkList[j].Destination.Invalidated = true;
//            }
//            for (int j = 0; j < di.InputLinkList.Count; j++)
//            {
//                di.InputLinkList[j].Invalidated = true;
//                di.InputLinkList[j].Destination.Invalidated = true;
//            }
        }

        protected Region CalculateInvalidatedRegion(Rectangle initialRect)
        {
            //Rectangle rs = new Rectangle((int)(initialRect.X * scaleFactor), (int)(initialRect.Y * scaleFactor), (int)(initialRect.Width * scaleFactor), (int)(initialRect.Height * scaleFactor));
            Region r = new Region(initialRect);
            
                        for (int i = 0; i < linkOrchestrator.Links.Count; i++)
            {
                if (linkOrchestrator.Links[i].Invalidated)
                {
                	linkOrchestrator.Links[i].Destination.Invalidated = true;
                	linkOrchestrator.Links[i].Source.Invalidated = true;
                    RectangleF rl = linkOrchestrator.Links[i].Bounds;
                    rl.Inflate( linkOrchestrator.lsPenWidth/2 +1, linkOrchestrator.lsPenWidth/2 +1);
                    r.Union(rl);
                }
            }
            
            for (int i = 0; i < DrawableItems.Count; i++)
                if (DrawableItems[i].Invalidated)
                {
                    Rectangle rdr = DrawableItems[i].Bounds;
                    //rdr.Inflate(1, 1);
                    r.Union(rdr);
                }

            RectangleF r2 = r.GetBounds(this.CreateGraphics());
            //Matrix mx = new Matrix();
            //mx.Scale(scaleFactor, scaleFactor);
            //r.Transform(mx);
            //RectangleF r3 = r.GetBounds(this.CreateGraphics());

            
            //return new Region(r3);
            return new Region(r.GetRegionData());
            //return r;
        }

        protected void OnItemsMoved(List<DiagramItem> items, int dx,int dy)
        {
            //if (ItemsMoved != null)
            //   ItemsMoved(this, new ItemsMovedEventArg(items, dx, dy));
        }

        private void textEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ValidateNodeEditor();
                //textEditor.Hide();
            }
        }

        private void textEditor_LostFocus(object sender, EventArgs e)
        {


        }

        private void InvalidateTransform(Region region)
        {
            Matrix mx = new Matrix();
            mx.Scale(scaleFactor, scaleFactor);
            mx.Translate(-hScrollBar1.Value, -vScrollBar1.Value);
            region.Transform(mx);
            Invalidate(region);
        }


        private void ValidateNodeEditor()
        {
        	
        	/*
            if (textEditor.Node != null)
                textEditor.Node.Text = textEditor.Text;
            */
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DiagramContainer
            // 
            this.Size = new System.Drawing.Size(1000, 800);
            this.Resize += new System.EventHandler(this.DiagramContainer_Resize);
            this.ResumeLayout(false);

        }

        private void DiagramContainer_Resize(object sender, EventArgs e)
        {
            SetScrollBarValues();
        }

        private void SetScrollBarValues()
        {

            if (scaleFactor * drawableWidth > Width)
            {
                hScrollBar1.Minimum = 0;
                hScrollBar1.Maximum = (int)(scaleFactor * drawableWidth - Width);
                hScrollBar1.SmallChange = (int)(scaleFactor * drawableWidth / 20);
                hScrollBar1.LargeChange = (int)(scaleFactor * drawableWidth / 10);
                if (this.vScrollBar1.Visible) //step 2
                {
                    this.hScrollBar1.Maximum += this.vScrollBar1.Width;
                }
                hScrollBar1.Maximum += this.hScrollBar1.LargeChange;//step 3
                hScrollBar1.Visible = true;
            }
            else
            {
                drawableWidth = Width;
                hScrollBar1.Visible = false;
                hScrollBar1.Value = 0;
            }

            if (scaleFactor * drawableHeight > Height)
            {
                vScrollBar1.Minimum = 0;
                vScrollBar1.Maximum = (int)((scaleFactor * drawableHeight - Height));
                vScrollBar1.SmallChange = (int)(scaleFactor * drawableHeight / 20);
                vScrollBar1.LargeChange = (int)(scaleFactor * drawableHeight / 10);
                if (this.hScrollBar1.Visible) //step 2
                {
                    this.vScrollBar1.Maximum += this.hScrollBar1.Height;
                }
                vScrollBar1.Maximum += this.vScrollBar1.LargeChange;//step 3
                vScrollBar1.Visible = true;
            }
            else
            {
                drawableHeight = Height;
                vScrollBar1.Visible = false;
                vScrollBar1.Maximum = 0;
            }



        }
        
 
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)	
		{
        	const int WM_KEYDOWN = 0x100;
			const int WM_SYSKEYDOWN = 0x104;
			
			if ((msg.Msg == WM_KEYDOWN) || (msg.Msg == WM_SYSKEYDOWN))
			{
			  switch(keyData)
			  {
			     case Keys.Down:
			        this.Parent.Text="Down Arrow Captured";
			        break;
			  
			     case Keys.Up:
			        this.Parent.Text="Up Arrow Captured";
			        break;
			
			     case Keys.Tab:
			        this.Parent.Text="Tab Key Captured";
			        break;
			
			     case Keys.Control | Keys.M:
			        this.Parent.Text="<CTRL> + M Captured";
			        break;
			
			     case Keys.Alt | Keys.Z:
			        this.Parent.Text="<ALT> + Z Captured";
			        break;
			  }				
			}
			if(nodeEditor.Visible)
				nodeEditor.OnKeyDown(new KeyEventArgs(keyData));
			return base.ProcessCmdKey(ref msg,keyData);
		}
        
    }
}
