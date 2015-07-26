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
	/// <summary>
	/// Description of LinkStrategy.
	/// </summary>
	public class DefaultLinkStrategy : ILinkStrategy
	{
		public DefaultLinkStrategy()
		{
		}


		public LinkDirection computeDirection(ItemLink link)
		{
			DiagramItem destinationItem = link.Destination;
			DiagramItem sourceItem = link.Source;
			
			LinkDirection prevDirection = link.Direction;
			LinkDirection direction = LinkDirection.None;
			if (sourceItem.Location.Y > destinationItem.Location.Y + destinationItem.Size.Height) {
				direction = LinkDirection.SourceNorthDestinationSouth;
			} else if (sourceItem.Location.Y + sourceItem.Size.Height < destinationItem.Location.Y) {
				direction = LinkDirection.SourceSouthDestinationNorth;
			} else {
				if (sourceItem.Location.X > destinationItem.Location.X + destinationItem.Size.Width) {
					direction = LinkDirection.SourceWestDestinationEast;
				} else if (sourceItem.Location.X + sourceItem.Size.Width < destinationItem.Location.X) {
					direction = LinkDirection.SourceEastDestinationWest;
				} else
					direction = LinkDirection.None;
			}
			return direction;
		}
	}
	
//	public class OutputLinkStrategy : ILinkStrategy
//	{
//		public OutputLinkStrategy()
//		{
//		}
//
//
//		public LinkDirection computeDirection(ItemLink link)
//		{
//			DiagramItem sourceItem = link.Source;
//			DiagramItem destinationItem = link.Destination;
//			LinkDirection prevDirection = link.Direction;
//			LinkDirection direction = LinkDirection.None;
//			if (destinationItem.Location.Y > sourceItem.Location.Y + sourceItem.Size.Height) {
//				direction = LinkDirection.SourceSouthDestinationNorth;
//			} else if (destinationItem.Location.Y + destinationItem.Size.Height < sourceItem.Location.Y) {
//				direction = LinkDirection.SourceNorthDestinationSouth;
//			} else {
//				if (destinationItem.Location.X > sourceItem.Location.X + sourceItem.Size.Width) {
//					direction = LinkDirection.SourceEastDestinationWest;
//				} else if (destinationItem.Location.X + destinationItem.Size.Width < sourceItem.Location.X) {
//					direction = LinkDirection.SourceWestDestinationEast;
//				} else
//					direction = LinkDirection.None;
//			}
//			return direction;
//		}
//	}
	
}
