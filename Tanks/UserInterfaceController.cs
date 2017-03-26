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
	class UserInterfaceController
	{
		private Game1 game;

		//Passing Game is necessary for callbacks to properly function
		public UserInterfaceController(Game1 game)
		{
			this.game = game;
		}

		public void undoLast()
		{
			game.undoLastAction();
		}

		public void endTurn()
		{
			game.endTurn();
		}
	}
}