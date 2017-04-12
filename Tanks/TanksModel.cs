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
	class TanksModel
	{
		public Tank lastSelectedTank { get; set; }
		public Line tankFollowLine { get; set; }
		public List<Cover> coverList { get; set; }
		public Line coverLine { get; set; }
		public TankLineHistory tankLineHistory { get; set; }
		public List<Tank> tanks { get; set; }
		public InkMonitor inkMonitor { get; set; }

		public TanksModel()
		{
			//Initialize default variables
			lastSelectedTank = null;
			tankFollowLine = new Line();
			coverList = new List<Cover>();
			coverLine = new Line();
			tanks = new List<Tank>();
		}

	}
}