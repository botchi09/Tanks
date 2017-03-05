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
		private List<Vector2> waypoints;


		public void setPosition(Vector2 position)
		{
			this.position = position;
		}

		public Vector2 getPosition()
		{
			return position;
		}

		public void setRotation(int degrees)
		{
			rotation = (float)(degrees * Math.PI) / (180);
		}

		public void faceVector(Vector2 vector)
		{

			rotation = (float)Math.Atan2(vector.Y - position.Y, vector.X - position.X);
		}

		public void draw(Texture2D texture, SpriteBatch spriteBatch)
		{

			spriteBatch.Begin();
			spriteBatch.Draw(texture, position, null, Color.White,
							 rotation,
							 new Vector2(texture.Width / 2, texture.Height / 2),
							 1,
							 SpriteEffects.None, 0f);
			spriteBatch.End();
		}

		public void addWaypoint(Vector2 waypoint)
		{
			waypoints.Add(waypoint);
		}

		public void clearWaypoints()
		{
			waypoints.Clear();
		}

		//By default, C# passes by ref. We must clone each item.
		public void setWaypoints(List<Vector2> newWaypoints)
		{
			waypoints = newWaypoints.GetRange(0, newWaypoints.Count);
		}

		public void update(float timeStep)
		{
			//Move towards waypoints here
			if (waypoints.Count > 0)
			{
				bool waypointMoveSuccess = moveTowardsPoint(waypoints[0], timeStep);
				if (waypointMoveSuccess)
				{
					waypoints.RemoveAt(0);
				}
				else
				{
					faceVector(waypoints[0]);
				}
			}
		}

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
	}
}