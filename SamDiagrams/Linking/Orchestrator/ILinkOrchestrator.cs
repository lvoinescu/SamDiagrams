using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SamDiagrams.Linking.Orchestrator
{
	public interface ILinkOrchestrator
	{
		void AddLink(DiagramItem source, DiagramItem destination);
		void AddLink(DiagramItem source, DiagramItem destination, Color color);
		List<Link> Links { get; set; }
		LinkStyle LinkStyle { get; set; }
	}
}

