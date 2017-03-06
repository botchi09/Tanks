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
using ClipperLib;
using Microsoft.Xna.Framework;
using Path = System.Collections.Generic.List<ClipperLib.IntPoint>;
using Paths = System.Collections.Generic.List<System.Collections.Generic.List<ClipperLib.IntPoint>>;
using Microsoft.Xna.Framework.Graphics;

namespace Tanks
{
	//Clipper lib deals in IntPoint, functionally identical to MonoGame's Vector2.
	//This class will convert them in accessor functions.
	class Cover
	{
		Line assignedLine = new Line(); //Line for drawing

		//List<IntPoint> points = new List<IntPoint>(); //Point list for dealing with Clipper (no direct type conversion possible)

		public void addPoint(Vector2 point)
		{
			//points.Add(Vector2Ext.ToIntPoint(point));
			assignedLine.addPoint(point);
		}

		public List<Vector2> getPoints()
		{
			/*List<Vector2> convertedPoints = new List<Vector2>();

			//We must iterate in order for correct conversion
			for (int index = 0; index < points.Count; index++)
			{
				convertedPoints.Add(Vector2Ext.ToVector2(points[index]));
			}

			return convertedPoints;*/

			return assignedLine.getPoints();
		}

		public void setPoints(List<Vector2> vector2Points)
		{
			//points.Clear();
			assignedLine = new Line();

			for (int index = 0; index < vector2Points.Count; index++)
			{
				//points.Add(Vector2Ext.ToIntPoint(vector2Points[index]));
				assignedLine.addPoint(vector2Points[index]);

			}

		}

		public Vector2? getLineIntersectionPoint(Line line)
		{
			Clipper clipper = new Clipper();
			PolyTree solution = new PolyTree();

			clipper.AddPath(line.getIntPointsPath(), PolyType.ptSubject, false);
			clipper.AddPath(assignedLine.getIntPointsPath(), PolyType.ptClip, true);

			clipper.Execute(ClipType.ctIntersection, solution);

			if (solution.ChildCount > 0)
			{
				return Vector2Ext.ToVector2(solution.Childs[0].Contour[0]);
			}
			else
			{
				return null;
			}

		}

		public void doesTankCollide(Tank tank)
		{
			//We'd do a line intersection as it's being drawn and simply disallow it, or snap to the nearest clipping point
			throw new NotImplementedException();
		}

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

		//TODO: Delegate to Explosion class, which will fully handle cover clipping. This will permit multiple covers being damaged at once.
		public void ExplosionAt(Vector2 centre, int radius)
		{
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
					assignedLine.addPoint(Vector2Ext.ToVector2(solution[0][index]));
				}

				//One more line to connect it all up...
				//solution[0][solution[0].Count - 1];
				assignedLine.addPoint(assignedLine.getPoints()[0]);
			}

		}

		public void draw(Texture2D lineTexture, SpriteBatch spriteBatch)
		{
			assignedLine.drawLines(lineTexture, spriteBatch);
		}
	}

}