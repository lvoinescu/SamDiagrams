/*
 * Created by SharpDevelop.
 * User: L
 * Date: 2/27/2013
 * Time: 10:19 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace SamDiagrams
{
	/// <summary>
	/// Description of ZoomFactorChangedArg.
	/// </summary>
	public class ZoomFactorChangedArg
	{
		private int zoomLevel;
		
		public int ZoomLevel {
			get { return zoomLevel; }
		}
		public ZoomFactorChangedArg(int zoom)
		{
			zoomLevel = zoom;
		}
	}
}
