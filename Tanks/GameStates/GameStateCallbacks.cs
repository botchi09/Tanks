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
using Tanks.Messages;

namespace Tanks
{
	class GameStateCallbacks
	{
		private MessageController messageController;

		public GameStateCallbacks(MessageController messageController)
		{
			this.messageController = messageController;
		}

		public void matchComplete(TankTeam victor)
		{
			switch (victor)
			{
				case TankTeam.ONE:
					messageController.dispatchMessage(GamePhase.P1_WIN);
					break;
				case TankTeam.TWO:
					messageController.dispatchMessage(GamePhase.P2_WIN);
					break;
			}
		}
	}
}