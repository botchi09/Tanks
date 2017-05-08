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
	class InkMonitor
	{
		private int maxInk = 500;
		private int ink;
		private Texture2D texture;

		public InkMonitor()
		{
			resetInk();
		}

		public void resetInk()
		{
			ink = maxInk;
		}

		public int getInk()
		{
			return ink;
		}

		public void spendInk(int inkSpent)
		{
			ink = ink - inkSpent;
			System.Diagnostics.Debug.WriteLine("ink left: " + ink);
		}

		public bool canSpendInk(int inkSpent)
		{
			return (ink - inkSpent) > 0;
		}

		public float getInkPercent()
		{
			return ((float)ink / (float)maxInk);
		}

		public void draw(SpriteBatch spriteBatch, Vector2 offset)
		{

		}
	}
}