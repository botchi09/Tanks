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
using Microsoft.Xna.Framework.Graphics;

namespace Tanks
{
	class TanksController
	{
		TanksModel tanksModel;

		public TanksController(TanksModel tanksModel)
		{
			this.tanksModel = tanksModel;
		}

		public void setTanks(List<Tank> tanks)
		{
			this.tanksModel.tanks = tanks;
		}

		public List<Tank> getTanks()
		{
			return tanksModel.tanks;
		}

		public void addTank(Tank tank)
		{
			tanksModel.tanks.Add(tank);
		}

		public void createTank(Vector2 position, TankLineHistory tankLineHistory)
		{
			Tank tank = new Tank(tankLineHistory);
			tank.setPosition(position);

			addTank(tank);
		}



		private int tankTouchRadius = 200;

		//Determines if vector (i.e. generated by user input) is in sufficient proximity to tank
		//http://stackoverflow.com/a/11555445
		public Tank getTankFromTouchPosition(Vector2 point)
		{
			List<Tank> possibleTanks = new List<Tank>();

			getTanks().ForEach(delegate (Tank tank)
			{
				if (Vector2.DistanceSquared(tank.getPosition(), point) < (tankTouchRadius * tankTouchRadius))
				{
					possibleTanks.Add(tank);
				}
			});

			//Find the closest of possible tanks to touch position. Sort closest to First.
			//https://msdn.microsoft.com/en-us/library/b0zbh7b6(v=vs.110).aspx
			possibleTanks.Sort(delegate (Tank tankOne, Tank tankTwo)
			{
				if (Vector2.DistanceSquared(point, tankOne.getPosition()) > Vector2.DistanceSquared(point, tankTwo.getPosition()))
				{
					return 1;
				}
				else
				{
					return -1;
				}
			});

			if (possibleTanks.Count > 0)
			{
				return possibleTanks.First();
			}
			else
			{
				return null;
			}
		}

		public void update(float timeStep)
		{
			getTanks().ForEach(delegate (Tank tank)
			{
				tank.update(timeStep);
			});
		}

		public void draw(Texture2D tankTexture, Texture2D tankOldLineTexture, SpriteBatch spriteBatch)
		{
			getTanks().ForEach(delegate (Tank tank)
			{
				tank.draw(tankTexture, tankOldLineTexture, spriteBatch);
			});
		}
	}
}