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
	class TankCollisionResult
	{
		private Tank tankHit = null; //Assigned value if tank was shot
		private bool disabled = false; //If tank was shot and disabled

		public Tank getTankHit()
		{
			return tankHit;
		}

		public bool getDisabled()
		{
			return disabled;
		}

		public TankCollisionResult(Tank tankHit, bool disabled)
		{
			this.tankHit = tankHit;
			this.disabled = disabled;
		}
	}
}