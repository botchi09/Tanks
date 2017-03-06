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
using ClipperLib;
using Path = System.Collections.Generic.List<ClipperLib.IntPoint>;


namespace Tanks
{
	class Line
	{
		private List<Vector2> points;

		public Line()
		{
			points = new List<Vector2>();
		}

		public void addPoint(Vector2 newPoint)
		{
			points.Add(newPoint);
		}

		public List<Vector2> getPoints()
		{
			return points;
		}

		//Compatibility with Clipper lib
		public Path getIntPointsPath()
		{
			Path convertedPoints = new Path();

			//We must iterate in order for correct conversion
			for (int index = 0; index < points.Count; index++)
			{

				convertedPoints.Add(Vector2Ext.ToIntPoint(points[index]));
			}

			return convertedPoints;
		}

		//http://gamedev.stackexchange.com/a/26027
		private void DrawLine(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 end)
		{
			spriteBatch.Draw(texture, start, null, Color.White,
							 (float)Math.Atan2(end.Y - start.Y, end.X - start.X),
							 new Vector2(0f, (float)texture.Height / 2),
							 new Vector2(Vector2.Distance(start, end), 1f),
							 SpriteEffects.None, 0f);
		}

		public void drawLines(Texture2D lineTexture, SpriteBatch spriteBatch)
		{
			spriteBatch.Begin();


			//Begin at one as we must ignore the first point in order to connect lines between 2 vectors
			for (var i = 1; i < points.Count; i++)
			{
				DrawLine(spriteBatch, lineTexture, points[i-1], points[i]);
			}
			spriteBatch.End();
		}
	}
}