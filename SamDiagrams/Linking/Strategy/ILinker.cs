/*
 * Created by SharpDevelop.
 * User: Sam
 * Date: 7/26/2015
 * Time: 4:47 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace SamDiagrams.Linking.Strategy
{
	public interface ILinker
	{
		/// <summary>
		/// Registers a link to be handled by a link strategy.
		/// </summary>
		/// <param name="link"></param>
		void RegisterLink(Link link);
		
		/// <summary>
		/// Computes the end points of all links associated with an item.
		/// This handled input links and output links.
		/// </summary>
		/// <param name="item"></param>
		void DirectLinks(DiagramItem item);
		
		
	}
}
