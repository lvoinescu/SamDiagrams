using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SamDiagrams
{

    public class LinkDirectionChangedArg : EventArgs
    {
        private LinkDirection newDirection;

        public LinkDirection NewDirection
        {
            get { return newDirection; }
            set { newDirection = value; }
        }
        private LinkDirection prevDirection;

        public LinkDirection PrevDirection
        {
            get { return prevDirection; }
            set { prevDirection = value; }
        }

        public LinkDirectionChangedArg(LinkDirection newDirection, LinkDirection prevDirection)
        {
            this.newDirection = newDirection;
            this.prevDirection = prevDirection;
        }

    }
}

