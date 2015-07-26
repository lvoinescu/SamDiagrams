using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SamDiagrams
{
    public partial class ItemsMovedEventArg : EventArgs
    {
        List<DiagramItem> items;

        public List<DiagramItem> Items
        {
            get { return items; }
            set { items = value; }
        }
        int dx, dy;

        public int Dy
        {
            get { return dy; }
            set { dy = value; }
        }

        public int Dx
        {
            get { return dx; }
            set { dx = value; }
        }
        public ItemsMovedEventArg(List<DiagramItem> items, int dx, int dy)
        {
            this.items = items;
            this.dx = dx;
            this.dy = dy;
        }
    }
}

