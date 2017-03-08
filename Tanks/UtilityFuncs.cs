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

namespace Tanks
{
	public static class UtilityFuncs
	{
		public static double DistanceSquared(Vector2 pos1, Vector2 pos2)
		{
			return (Math.Pow(pos1.X - pos2.X, 2) + Math.Pow(pos1.Y - pos2.Y, 2));
		}

		public static int Clamp(int value, int min, int max)
		{
			throw new NotImplementedException();
		}

		internal static bool DistanceSquared(Vector2 vector2)
		{
			throw new NotImplementedException();
		}
	}
}