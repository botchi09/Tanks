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
using ClipperLib;
using Path = System.Collections.Generic.List<ClipperLib.IntPoint>;
using Paths = System.Collections.Generic.List<System.Collections.Generic.List<ClipperLib.IntPoint>>;

namespace Tanks
{
	class CoverCollision
	{

		//TODO: Fully delegate intersection from Cover
		//Returns a safe position should a projected line intersect with cover.
		public CoverCollisionResult getSafeIntersectionPoint(Line intersectionLine, List<Cover> coverList)
		{

			bool wasModified = false;

			Line safeLine = new Line();
			safeLine.setPoints(intersectionLine.getPoints());
			if (coverList.Count > 0)
			{
				Clipper clipper = new Clipper();
				PolyTree solution = new PolyTree();

				clipper.AddPath(safeLine.getIntPointsPath(), PolyType.ptSubject, false);

				coverList.ForEach(delegate (Cover cover)
				{
					clipper.AddPath(cover.getAssignedLine().getIntPointsPath(), PolyType.ptClip, true);

				});

				clipper.Execute(ClipType.ctIntersection, solution);

				if (solution.ChildCount > 0)
				{
					Vector2 contourOne = Vector2Ext.ToVector2(solution.Childs[0].Contour[0]);
					Vector2 contourTwo = Vector2Ext.ToVector2(solution.Childs[0].Contour[1]);

					/*Vector2 safePoint = Vector2Ext.ToVector2(solution.Childs[0].Contour[0]);

					//Now do a small unit vector offset to prevent wall stickiness.
					//intersectionLine.getPoints().First();
					Vector2 direction = Vector2.Subtract(safeLine.getPoints().First(), safePoint);
					Vector2 unit = Vector2.Normalize(direction);

					safePoint = Vector2.Add(safePoint, unit * 15);*/

					Vector2 direction = Vector2.Subtract(contourOne, contourTwo);
					Vector2 unit = Vector2.Normalize(direction);

					Vector2 safePoint = Vector2.Add(contourOne, unit * 15);

					safeLine.getPoints().RemoveAt(safeLine.getPoints().Count - 1); //Remove the last point on this line
					//safeLine.getPoints().RemoveAt(0); //Remove the last point on this line


					safeLine.addPoint(safePoint);
					wasModified = true;
				}
			}

			return new CoverCollisionResult(safeLine, wasModified);
		}
	}
}