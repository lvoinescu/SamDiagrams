﻿/*
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
	/// Description of MergeblaRectangleTest.
	/// </summary>
	[TestFixture]
	public class MergeblaRectangleTest
	{
			
		[Test]
		public void TestMethod()
		{
			MergeableRectangle mRect = new MergeableRectangle(new Rectangle(100, 100, 200, 300));
			mRect.Merge(new Rectangle(150, 150, 500, 600));
			
			Assert.AreEqual(new Rectangle(100, 100, 550, 650), mRect.Bounds);
			
		}
	}
}