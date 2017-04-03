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

namespace Tanks
{
	class InkMonitor
	{
		private int maxInk = 100;
		private int ink;

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

		public void spendInk()
		{
			ink--;
		}
	}
}