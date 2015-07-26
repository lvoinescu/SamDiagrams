using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SamDiagrams
{
    public partial class ItemMovedEventArg : EventArgs
    {
        DiagramItem item;

        public DiagramItem Item
        {
            get { return item; }
            set { item = value; }
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
        public ItemMovedEventArg(DiagramItem item, int dx, int dy)
        {
            this.item = item;
            this.dx = dx;
            this.dy = dy;
        }
    }
}

