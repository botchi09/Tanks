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
using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;

namespace Tanks
{
	//Clipper lib deals in IntPoint, functionally identical to MonoGame's Vector2.
	//This class will convert them in accessor functions.
	class Cover
	{
		Line assignedLine = new Line(); //Line for drawing

		public void addPoint(Vector2 point)
		{
			assignedLine.addPoint(point);
		}

		public List<Vector2> getPoints()
		{
			return assignedLine.getPoints();
		}

		public void setPoints(List<Vector2> vector2Points)
		{
			assignedLine = new Line();

			for (int index = 0; index < vector2Points.Count; index++)
			{
				assignedLine.addPoint(vector2Points[index]);

			}

		}

		public Line getAssignedLine()
		{
			return assignedLine;
		}

		public Vector2? getLineIntersectionPoint(Line line)
		{
			List<Cover> coverList = new List<Cover>();
			coverList.Add(this);
			return line.getCoverIntersectionPoint(coverList);

		}

		public void doesTankCollide(Tank tank)
		{
			//We'd do a line intersection as it's being drawn and simply disallow it, or snap to the nearest clipping point
			throw new NotImplementedException();
		}



		public void draw(Texture2D lineTexture, SpriteBatch spriteBatch)
		{
			assignedLine.drawLines(lineTexture, spriteBatch);
		}
	}

}