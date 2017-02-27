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
	class Line
	{
		private LinkedList<Vector2> points;

		public Line()
		{
			points = new LinkedList<Vector2>();
		}

		private void addPoint(Vector2 newPoint)
		{
			points.AddLast(newPoint);
		}

		private LinkedList<Vector2> getPoints()
		{
			return points;
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
			DrawLine(spriteBatch, lineTexture, new Vector2(20, 20), new Vector2(120, 120));
			DrawLine(spriteBatch, lineTexture, new Vector2(120, 20), new Vector2(220, 60));
			DrawLine(spriteBatch, lineTexture, new Vector2(20, 240), new Vector2(220, 100));
			spriteBatch.End();
		}
	}
}