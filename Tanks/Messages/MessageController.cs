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

namespace Tanks.Messages
{
	class MessageController
	{
		private MessageView messageView;

		public MessageController(MessageView messageView)
		{
			this.messageView = messageView;
		}

		public void dispatchMessage(GamePhase message)
		{
			messageView.showMessage(message);
		}

		public void update(double timeStep)
		{
			messageView.update(timeStep);
		}
	}
}