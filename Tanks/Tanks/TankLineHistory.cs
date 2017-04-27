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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Tanks
{
	class TankLineHistory
	{
		private class TankWaypoints
		{
			public Tank tank;
			public List<Vector2> completedWaypoints;
			private bool canUndo = true;

			public TankWaypoints(Tank tank, List<Vector2> completedWaypoints)
			{
				this.tank = tank;
				this.completedWaypoints = completedWaypoints;
			}

			public void disableUndo()
			{
				canUndo = false;
			}

			public bool isUndoable()
			{
				return canUndo;
			}

		}

		private List<TankWaypoints> previousTankWaypoints = new List<TankWaypoints>();

		public void undoLast()
		{
			if (previousTankWaypoints.Count > 0 && previousTankWaypoints[0].completedWaypoints.Count > 0) //Previous waypoints must exist
			{
				if (previousTankWaypoints[0].isUndoable())
				{
					previousTankWaypoints[0].tank.clearWaypoints();

					Vector2 tankStartPoint = previousTankWaypoints[0].completedWaypoints[0]; //The first waypoint
					previousTankWaypoints[0].tank.setPosition(tankStartPoint); //Reset to original start point
					previousTankWaypoints[0].tank.faceVector(previousTankWaypoints[0].completedWaypoints[0]); //Face the first point- we can't get back the original rotation so compromise.
					previousTankWaypoints.RemoveAt(0);
				}
			}
		}

		//Add last completed line and tank assigned with it
		public void addTankAndWaypoints(Tank tank, List<Vector2> waypoints)
		{
			previousTankWaypoints.Insert(0, new TankWaypoints(tank, waypoints)); //FIFO, stack behaviour for correct history function.
		}

		public void draw(Texture2D lineTexture, SpriteBatch spriteBatch)
		{
			//TODO: Add some kind of fading effect to very old lines.
			previousTankWaypoints.ForEach(delegate (TankWaypoints tankWaypoints)
			{
				Line oldWaypoints = new Line();
				oldWaypoints.setPoints(tankWaypoints.completedWaypoints);
				oldWaypoints.drawLines(lineTexture, spriteBatch);
			});
		}

		public void tankMoveComplete(Tank tank, List<Vector2> completedWaypoints)
		{
			addTankAndWaypoints(tank, completedWaypoints);
		}

		//Each new turn disables previous undos and flags them for slow deletion.
		public void disableCurrentUndoables()
		{
			previousTankWaypoints.ForEach(delegate (TankWaypoints tankWaypoints)
			{
				tankWaypoints.disableUndo();
			});
		}

		public void disableTankUndo(Tank tank)
		{
			previousTankWaypoints.ForEach(delegate (TankWaypoints tankWaypoints)
			{
				if (tankWaypoints.tank.Equals(tank))
				{
					tankWaypoints.disableUndo();
				}
			});
		}
	}

}