/*
 * Created by SharpDevelop.
 * User: Sam
 * Date: 7/27/2015
 * Time: 12:17 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using SamDiagrams.Drawings;

namespace SamDiagrams.Linking.Strategy.CenterLinkStrategy
{
	/// <summary>
	/// Description of CenterLinkStrategy.
	/// </summary>
	public class CenterLinkStrategy : ILinker
	{
		
		public event LinkDirectionChangedHandler LinkDirectionChangedEvent;
		
		public CenterLinkStrategy()
		{
		}
		
		
		
		public void RegisterLink(StructureLink link)
		{
			throw new NotImplementedException();
		}
		
		public void DirectLinks(StructureDrawing item)
		{
			throw new NotImplementedException();
		}
		
		public void ArangeLinksForItem(Structure item)
		{
			throw new NotImplementedException();
		}
	}
}
