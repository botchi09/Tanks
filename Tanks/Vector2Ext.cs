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

namespace Tanks
{

	//Simple conversion between Vector2 and IntPoint
	public static class Vector2Ext
	{
		public static Vector2 ToVector2(this IntPoint point)
		{
			return new Vector2(point.X, point.Y);
		}

		public static IntPoint ToIntPoint(this Vector2 point)
		{
			return new IntPoint(point.X, point.Y);
		}


	}
}