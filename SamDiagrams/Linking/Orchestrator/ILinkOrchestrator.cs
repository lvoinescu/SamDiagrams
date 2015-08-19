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
		void AddLink(Structure source, Structure destination);
		
		void AddLink(Structure source, Structure destination, Color color);
		
		List<StructureLink> Links { get; set; }
		
		LinkStyle LinkStyle { get; set; }
	}
}

