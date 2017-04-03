using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Path = System.Collections.Generic.List<ClipperLib.IntPoint>;
using Paths = System.Collections.Generic.List<System.Collections.Generic.List<ClipperLib.IntPoint>>;
using ClipperLib;

namespace Tanks
{
	class Circle
	{
		//http://stackoverflow.com/a/5301049
		public Path createCircleOfPoints(int points, double radius, IntPoint centre)
		{
			Path circlePoints = new Path();

			double slice = 2 * Math.PI / points;
			for (int i = 0; i < points; i++)
			{
				double angle = slice * i;
				double newX = (centre.X + radius * Math.Cos(angle));
				double newY = (centre.Y + radius * Math.Sin(angle));

				circlePoints.Add(new IntPoint(newX, newY));
			}

			return circlePoints;
		}
	}
}