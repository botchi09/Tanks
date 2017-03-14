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
using Microsoft.Xna.Framework;

namespace Tanks
{
	class Explosion
	{
		private int radius;
		private List<Cover> coverList = new List<Cover>;
		private Vector2 centre;

		//http://stackoverflow.com/a/5301049
		private Path createCircleOfPoints(int points, double radius, IntPoint centre)
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

		//Clamps an int between two values
		//http://stackoverflow.com/a/3040551
		private int Clamp(int value, int min, int max)
		{
			return (value < min) ? min : (value > max) ? max : value;
		}

		public Explosion(Vector2 centre, int radius, Cover cover)
		{
			coverList.Add(cover);
			
			
		}

		public Explosion(Vector2 centre, int radius, List<Cover> allCover)
		{
			this.centre = centre;
			this.radius = radius;
		}

		//Required as constructors cannot return values
		public List<Cover> Explode()
		{
			return ExplosionAt(centre, radius, coverList);
		}

		private List<Cover> ExplosionAt(Vector2 centre, int radius, List<Cover> allCover)
		{
			List<Cover> newCoverList = new List<Cover>();

			allCover.ForEach(delegate (Cover cover)
			{
				Line assignedLine = cover.getAssignedLine();

				int optimalNumberOfPoints = Clamp(radius / 10, 10, 100);
				Path circle = createCircleOfPoints(optimalNumberOfPoints, radius, Vector2Ext.ToIntPoint(centre));

				Clipper clipper = new Clipper();
				Paths solution = new Paths();

				clipper.AddPath(circle, PolyType.ptClip, true);
				clipper.AddPath(assignedLine.getIntPointsPath(), PolyType.ptSubject, true);


				clipper.Execute(ClipType.ctDifference, solution);


				if (solution.Count > 0)
				{
					assignedLine.getPoints().Clear();
					for (int index = 0; index < solution[0].Count; index++)
					{
						assignedLine.addPoint(Vector2Ext.ToVector2(solution[0][index])); //TODO: Splitting cover into two results in only one cover. Mass mysteriously disappears.
					}

					//One more line to connect it all up...
					assignedLine.addPoint(assignedLine.getPoints()[0]);
				}

			});
		}
	}

}