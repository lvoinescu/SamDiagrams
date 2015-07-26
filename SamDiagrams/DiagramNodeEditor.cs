using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SamDiagrams
{
    public partial class DiagramNodeEditor : TextBox
    {
        private DiagramItemNode nod;



        public DiagramItemNode Node
        {
            get { return nod; }
            set { nod = value; }
        }
        private DiagramItem diagramItem;

        public DiagramItem DiagramItem
        {
            get { return diagramItem; }
            set { diagramItem = value; }
        }
        public DiagramNodeEditor():base()
        {
        }

        public DiagramNodeEditor(DiagramItem item, DiagramItemNode nod)
        {
            this.diagramItem = item;
            this.nod = nod;
        }

 
    }
}

