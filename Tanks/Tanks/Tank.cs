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

	class Tank
	{
		public Vector2 position = new Vector2(300, 300);
		private float rotation = 0; //Rotation in radians. Use accessor functions instead.
		private int speed = 200;
		private List<Vector2> waypoints = new List<Vector2>();
		private List<Vector2> completedWaypoints = new List<Vector2>();
		private Line lastShot;
		private TankTeam team;
		private InkMonitor inkMonitor;

		private bool movementEnabled = true;
		private bool engineDisabled = false;
		private bool gunsDisabled = false;
		private bool isAlive = true;

		private TankLineHistory moveCompleteHistoryCallback;

		public Tank(TankLineHistory tankLineHistory, TankTeam team)
		{
			moveCompleteHistoryCallback = tankLineHistory;
			this.team = team;
			this.inkMonitor = new InkMonitor();
		}

		public float getInkLevel()
		{
			return inkMonitor.getInkPercent();
		}

		public TankTeam getTeam()
		{
			return this.team;
		}

		public bool getAlive()
		{
			return isAlive;
		}

		public bool canMove()
		{
			return (movementEnabled && !engineDisabled);
		}

		public void disableEngine()
		{
			engineDisabled = true;
		}

		public void disableGuns()
		{
			gunsDisabled = true;
		}

		public void blowUp()
		{
			disableEngine();
			disableGuns();
			isAlive = false;
		}

		public void setMovementEnabled(bool enabled)
		{
			movementEnabled = enabled;
		}

		public void setPosition(Vector2 position)
		{
			this.position = position;
		}

		public Vector2 getPosition()
		{
			return position;
		}

		public void resetHealth()
		{
			movementEnabled = true;
			gunsDisabled = false;
			isAlive = true;
		}

		public void resetMove()
		{
			inkMonitor.resetInk();
		}

		public void setRotation(int degrees)
		{
			rotation = (float)(degrees * Math.PI) / (180);
		}

		public void faceVector(Vector2 vector)
		{

			rotation = (float)Math.Atan2(vector.Y - position.Y, vector.X - position.X);
		}

		public void drawInkMonitor(Texture2D inkTexture, SpriteBatch spriteBatch)
		{

			spriteBatch.Draw(inkTexture, new Rectangle((int)position.X, (int)position.Y, 100, (int)(300 * getInkLevel())), Color.Black);
		}

		public void draw(Texture2D tankTexture, Texture2D tankOldLineTexture, SpriteBatch spriteBatch)
		{
			Line currentOldWaypointLine = new Line();
			currentOldWaypointLine.setPoints(completedWaypoints);
			currentOldWaypointLine.drawLines(tankOldLineTexture, spriteBatch);

			spriteBatch.Begin();
			spriteBatch.Draw(tankTexture, position, null, Color.White,
							 rotation,
							 new Vector2(tankTexture.Width / 2, tankTexture.Height / 2),
							 1,
							 SpriteEffects.None, 0f);
			spriteBatch.End();

			if (lastShot != null)
			{
				lastShot.drawLines(tankOldLineTexture, spriteBatch);
			}
		}

		public int getNewWaypointCost(Vector2 newWaypoint)
		{
			return (int)Vector2.Distance(waypoints.Last(), newWaypoint); //DistanceSquared will give us squared progression
		}

		public void addWaypoint(Vector2 waypoint)
		{
			if (waypoints.Count > 0)
			{
				int cost = getNewWaypointCost(waypoint);
				if (inkMonitor.canSpendInk(cost)) //Ensure that we have enough movement to create new waypoint
				{
					if (Vector2.DistanceSquared(waypoints.Last(), waypoint) > 20) //Ignore points added too clustered together.
					{
						inkMonitor.spendInk(cost);
						waypoints.Add(waypoint);
					}
				}
			}
			else
			{
				waypoints.Add(waypoint);
			}
		}

		private void makeCallback()
		{
			moveCompleteHistoryCallback.tankMoveComplete(this, completedWaypoints.GetRange(0, completedWaypoints.Count));
			completedWaypoints.Clear();
		}

		public void clearWaypoints()
		{
			setMovementEnabled(false);
			waypoints.Clear();

		}

		public void saveCurrentWaypoints()
		{
			if (completedWaypoints.Count > 0)
			{
				makeCallback();
			}
		}

		public void saveAndClearWaypoints()
		{
			saveCurrentWaypoints();
			clearWaypoints();
		}

		//By default, C# passes by ref. We must clone each item.
		public void setWaypoints(List<Vector2> newWaypoints)
		{
			waypoints = newWaypoints.GetRange(0, newWaypoints.Count);
		}

		private bool hasSavedWaypoints = true;

		public void update(double elapsedTimeStep)
		{
			//Move towards waypoints here
			if (waypoints.Count > 0 && canMove())
			{
				hasSavedWaypoints = false;
				bool waypointMoveSuccess = moveTowardsPoint(waypoints[0], (float)elapsedTimeStep);
				if (waypointMoveSuccess)
				{
					completedWaypoints.Add(new Vector2(waypoints[0].X, waypoints[0].Y));
					waypoints.RemoveAt(0);
				}
				else
				{
					faceVector(waypoints[0]);
				}
			}
			if (waypoints.Count == 0 && !hasSavedWaypoints)
			{
				hasSavedWaypoints = true;
				saveCurrentWaypoints();
			}
		}

		//Moves tank one speed unit toward goal, or back to prevent overshot
		//http://gamedev.stackexchange.com/a/28337
		private bool moveTowardsPoint(Vector2 goal, float elapsed)
		{
			// If we're already at the goal return immediatly
			if (position == goal) return true;

			// Find direction from current position to goal
			Vector2 direction = Vector2.Normalize(goal - position);

			// Move in that direction
			position += direction * speed * elapsed;

			// If we moved PAST the goal, move it back to the goal
			if (Math.Abs(Vector2.Dot(direction, Vector2.Normalize(goal - position)) + 1) < 0.1f)
				position = goal;

			// Return whether we've reached the goal or not
			return position == goal;
		}

		public void shoot(Vector2 direction, CoverController coverController, TanksController tanksController)
		{
			if (!gunsDisabled)
			{
				List<Cover> allCover = coverController.getCoverList();
				CoverCollision coverCollision = new CoverCollision();
				TankCollision tankCollision = new TankCollision();

				Line shotLine = new Tanks.Line();
				shotLine.addPoint(position);
				shotLine.addPoint(position + direction);

				//Use tank collision detection to find if tank shell collides and explodes.
				Vector2 explosionPoint = coverCollision.getSafeIntersectionPoint(shotLine, allCover).getLine().getPoints()[1];
				TankCollisionResult shotTraceResult = tankCollision.getShotTankCollision(shotLine, tanksController.getTanks(), this);

				if (shotTraceResult.getTankHit() != null)
				{
					if (shotTraceResult.getDisabled())
					{
						shotTraceResult.getTankHit().disableEngine();
						System.Diagnostics.Debug.WriteLine("Tank disabled!");
					}
					else
					{
						shotTraceResult.getTankHit().blowUp();
						System.Diagnostics.Debug.WriteLine("Tank wrecked!");
					}
				}

				Explosion explosion = new Explosion(explosionPoint, 100, coverController, tanksController);
				lastShot = shotLine;
				System.Diagnostics.Debug.WriteLine("Shooting guns!");
				coverController.setCoverList(explosion.Explode());
			}

		}
	}
}