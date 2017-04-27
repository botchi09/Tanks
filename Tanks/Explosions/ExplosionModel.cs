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

namespace Tanks.Explosions
{
	class ExplosionModel
	{
		public List<Explosion> explosionRecord { get; set; }
		public int maxExplosions { get; set; }

		public ExplosionModel()
		{
			explosionRecord = new List<Explosion>();
			maxExplosions = 10; //This will likely be how many different explosion decals we have
		}
	}
}