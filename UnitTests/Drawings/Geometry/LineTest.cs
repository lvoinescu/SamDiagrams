/*
 *   SamDiagrams - diagram component for .NET
 *   Copyright (C) 2011  Lucian Voinescu
 *
 *   This file is part of SamDiagrams
 *
 *   SamDiagrams is free software: you can redistribute it and/or modify
 *   it under the terms of the GNU Lesser General Public License as published by
 *   the Free Software Foundation, either version 3 of the License, or
 *   (at your option) any later version.
 *
 *   SamDiagrams is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU Lesser General Public License for more details.
 *
 *   You should have received a copy of the GNU Lesser General Public License
 *   along with SamDiagrams. If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Drawing;
using NUnit.Framework;
using SamDiagrams.Drawings.Geometry;

namespace UnitTests.Drawings.Geometry
{
	/// <summary>
	/// Description of LineTest.
	/// </summary>
	[TestFixture]
	public class LineTest
	{
		[Test]
		public void TestIntersection1()
		{
			LineSegment ls1 = new LineSegment(new Point(0, 0), new Point(100, 100));
			LineSegment ls2 = new LineSegment(new Point(0, 100), new Point(100, 0));
			Assert.IsTrue(ls1.Intersects(ls2));
		}
		
		[Test]
		public void TestNonIntersection1()
		{
			LineSegment ls1 = new LineSegment(new Point(0, 0), new Point(100, 100));
			LineSegment ls2 = new LineSegment(new Point(100, 0), new Point(100, 50));
			Assert.IsFalse(ls1.Intersects(ls2));
		}
		
		[Test]
		public void TesIntersection2()
		{
			LineSegment ls1 = new LineSegment(new Point(20, 10), new Point(30, 60));
			LineSegment ls2 = new LineSegment(new Point(10, 10), new Point(100, 30));
			Assert.IsTrue(ls1.Intersects(ls2));
		}
	}
}
