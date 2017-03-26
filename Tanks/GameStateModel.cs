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
	class GameStateModel
	{
		//TODO: Information about game state.
		/*Cover drawing should actually be here, not TanksModel
		 * Not for user configured options.
		 * Game phases decided here too.
		 */
		public bool coverDrawingMode { get; set; }

		public GameStateModel()
		{
			coverDrawingMode = false;
		}

	}
}