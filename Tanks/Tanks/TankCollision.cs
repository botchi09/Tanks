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
using Path = System.Collections.Generic.List<ClipperLib.IntPoint>;
using Paths = System.Collections.Generic.List<System.Collections.Generic.List<ClipperLib.IntPoint>>;

namespace Tanks
{
	class TankCollision
	{
		private Path makeCircleAroundTank(Tank tank, int radius)
		{
			return new Circle().createCircleOfPoints(20, radius, Vector2Ext.ToIntPoint(tank.getPosition()));
		}

		private bool doesLineIntersectWithTank(Line line, Tank tank, int radius)
		{

			Clipper clipper = new Clipper();
			PolyTree solution = new PolyTree();

			clipper.AddPath(line.getIntPointsPath(), PolyType.ptSubject, false);

			Path disabledCircle = makeCircleAroundTank(tank, radius);

			clipper.AddPath(disabledCircle, PolyType.ptClip, true);

			clipper.Execute(ClipType.ctIntersection, solution);

			return solution.ChildCount > 0;
		}

		public TankCollisionResult getShotTankCollision(Line intersectionLine, List<Tank> tanks, Tank shootingTank)
		{

			Tank tankHit = null;
			bool disabled = false;
			int disabledRadius = 28; //TODO: Refine these.
			int destroyedRadius = 18;

			if (tanks.Count > 0)
			{
				tanks.ForEach(delegate (Tank tank)
				{
					if (!tank.Equals(shootingTank))
					{
						//TODO: Why is this being called twice?
						bool wasDisabled = doesLineIntersectWithTank(intersectionLine, tank, disabledRadius);
						bool wasDestroyed = doesLineIntersectWithTank(intersectionLine, tank, destroyedRadius);

						if (wasDisabled || wasDestroyed)
						{
							tankHit = tank;
							if (wasDisabled && !wasDestroyed)
							{
								disabled = true;
							}
						}
					}
				});
				

			}

			return new TankCollisionResult(tankHit, disabled);
		}
	}
}