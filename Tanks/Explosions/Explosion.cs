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
using Tanks.Explosions;

namespace Tanks
{
	class Explosion
	{
		private int radius;
		private List<Cover> coverList = new List<Cover>();
		private Vector2 centre;
		private CoverController coverController;
		private TanksController tanksController;
		private ExplosionController explosionController;
		private int id;

		//Custom numerical identifier used to identify explosion.
		public int getId()
		{
			return id;
		}

		//Clamps an int between two values
		//http://stackoverflow.com/a/3040551
		private int Clamp(int value, int min, int max)
		{
			return (value < min) ? min : (value > max) ? max : value;
		}

		public Explosion(int id, Vector2 centre, int radius, CoverController coverController, TanksController tanksController, ExplosionController explosionController)
		{
			this.id = id;
			this.centre = centre;
			this.radius = radius;
			this.coverController = coverController;
			this.tanksController = tanksController;
			this.explosionController = explosionController;

		}

		public Vector2 getPosition()
		{
			return centre;
		}

		public int getRadius()
		{
			return radius;
		}

		//Required as constructors cannot return values
		public List<Cover> Explode()
		{
			return ExplosionAt(centre, radius, coverController.getCoverList());
		}

		//Finds all tanks in radius of centre, then calls blow up.
		//TODO: Fix this. Right now it's an infinite loop!
		private void blowUpTanksInArea(Vector2 centre, int radius)
		{
			List<Tank> affectedTanks = new List<Tank>();
			tanksController.getTanks().ForEach(delegate (Tank tank)
			{
				if (Vector2.Distance(tank.getPosition(), centre) <= radius && tank.getAlive())
				{
					tank.blowUp(explosionController);
				}
			});
		}

		//Creates an explosion at specified coordinates of radius size, returning deformed allCover
		private List<Cover> ExplosionAt(Vector2 centre, int radius, List<Cover> allCover)
		{

			//blowUpTanksInArea(centre, 30);

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