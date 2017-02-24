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
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;

namespace Tanks
{

	class DetectedGesture
	{
		//These are directly accessible to mirror GestureSample's functionality

		public GestureType GestureType;
		public Vector2 Delta;
		public Vector2 Delta2;
		public Vector2 Position;
		public Vector2 Position2;
		public TimeSpan Timestamp;

	}
}