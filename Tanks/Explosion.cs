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
		private List<Cover> coverList = new List<Cover>();
		private Vector2 centre;
		private CoverController coverController;

		

		//Clamps an int between two values
		//http://stackoverflow.com/a/3040551
		private int Clamp(int value, int min, int max)
		{
			return (value < min) ? min : (value > max) ? max : value;
		}

		//TODO: Decide if we should have radial explosions also damage tanks.
		public Explosion(Vector2 centre, int radius, CoverController coverController, TanksController tanksController)
		{
			this.coverList = coverController.getCoverList();
			this.centre = centre;
			this.radius = radius;
			this.coverController = coverController;
		}

		//Required as constructors cannot return values
		public List<Cover> Explode()
		{
			return ExplosionAt(centre, radius, coverList);
		}

		

		//TODO: MUST NOT MODIFY LINES INDIVIDUALLY! PASS COVER AS GROUP TO CLIPPER!
		private List<Cover> ExplosionAt(Vector2 centre, int radius, List<Cover> allCover)
		{
			List<Cover> newCoverList = new List<Cover>();

			allCover.ForEach(delegate (Cover cover)
			{
				Line assignedLine = cover.getAssignedLine();

				int optimalNumberOfPoints = Clamp(radius / 10, 10, 100);
				Path circle = new Circle().createCircleOfPoints(optimalNumberOfPoints, radius, Vector2Ext.ToIntPoint(centre));

				Clipper clipper = new Clipper();
				Paths solution = new Paths();

				clipper.AddPath(circle, PolyType.ptClip, true);
				clipper.AddPath(assignedLine.getIntPointsPath(), PolyType.ptSubject, true);


				clipper.Execute(ClipType.ctDifference, solution);

				if (solution.Count > 0)
				{
					assignedLine.getPoints().Clear();

					solution.ForEach(delegate (List<IntPoint> coverItem)
					{
						Cover newCover = coverController.convertIntPointsToCover(coverItem);

						newCoverList.Add(newCover);
					});
					
				}

			});

			return newCoverList;
		}

		
	}

}