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
using Microsoft.Xna.Framework;

namespace Tanks
{
	class Line
	{
		private LinkedList<Vector2> points = new LinkedList<Vector2>();

		private void addPoint(Vector2 newPoint)
		{
			points.AddLast(newPoint);
		}

		private LinkedList<Vector2> getPoints()
		{
			return points;
		}

	}
}