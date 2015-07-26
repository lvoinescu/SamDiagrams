/*
 * Created by SharpDevelop.
 * User: Sam
 * Date: 7/26/2015
 * Time: 4:47 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace SamDiagrams.Linking
{
	public interface ILinkStrategy
	{
		LinkDirection computeDirection(ItemLink link);
	}
}
