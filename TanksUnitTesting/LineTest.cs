using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tanks;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using ClipperLib;

namespace TanksUnitTesting
{
	[TestClass]
	public class LineTest
	{
		[TestMethod]
		public void LinePointsAddedCorrectly()
		{
			Line line = new Line();
			line.addPoint(new Vector2(100, 100));
			line.addPoint(new Vector2(200, 100));
			line.addPoint(new Vector2(300, 100));
			var points = line.getPoints();

			Assert.AreEqual(points[0], new Vector2(100, 100));
			Assert.AreEqual(points[1], new Vector2(200, 100));
			Assert.AreEqual(points[2], new Vector2(300, 100));

		}

		[TestMethod]
		public void LineSetPointsCorrectly()
		{
			Line line = new Line();
			List<Vector2> points = new List<Vector2>();
			points.Add(new Vector2(100, 100));
			points.Add(new Vector2(200, 100));
			points.Add(new Vector2(300, 100));
			line.setPoints(points);

			points = line.getPoints();

			Assert.AreEqual(points[0], new Vector2(100, 100));
			Assert.AreEqual(points[1], new Vector2(200, 100));
			Assert.AreEqual(points[2], new Vector2(300, 100));
		}

		[TestMethod]
		public void LineSetIntPointsCorrectly()
		{
			Line line = new Line();
			List<IntPoint> intPoints = new List<IntPoint>();
			intPoints.Add(new IntPoint(100, 100));
			line.setIntPointsPath(intPoints);

			Assert.AreEqual(line.getPoints()[0], new Vector2(100,100));
		}
	}
}
