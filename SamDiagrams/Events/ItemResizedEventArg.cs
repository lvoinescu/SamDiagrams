using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SamDiagrams
{
    public partial class ItemResizedEventArg
    {
        private Size size;

        public Size Size
        {
            get { return size; }
            set { size = value; }
        }
        public ItemResizedEventArg(int width, int height)
        {
            this.size.Width = width;
            this.size.Height = height;
        }
    }
}

