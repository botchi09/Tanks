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

		public void doesLineCollide(Line line)
		{
			Clipper clipper = new Clipper();
			PolyTree solution = new PolyTree();

			clipper.AddPath(line.getIntPointsPath(), PolyType.ptSubject, false);
			clipper.AddPath(assignedLine.getIntPointsPath(), PolyType.ptClip, true);

			clipper.Execute(ClipType.ctIntersection, solution);
			//solution.Childs[0].Contour
			//TODO: Return intersection status.
			System.Diagnostics.Debug.WriteLine(solution.ChildCount);


		}

		public void doesTankCollide(Tank tank)
		{
			throw new NotImplementedException();
		}

		
	
	}

	public class Tank
	{
	}
}