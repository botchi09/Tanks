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
using Microsoft.Xna.Framework.Graphics;

namespace Tanks
{
	class MessageView
	{
		ScreenMessage screenMessage;

		public MessageView(ScreenMessage screenMessage)
		{
			this.screenMessage = screenMessage;
		}

		//Translate GamePhase into string. Normally would be handled by localisation lib.
		public void showMessage(GamePhase message)
		{
			switch(message)
			{
				case GamePhase.P1_DRAW:
					screenMessage.show("Player One: Draw Phase");
					break;
				case GamePhase.P2_DRAW:
					screenMessage.show("Player Two: Draw Phase");
					break;
				case GamePhase.P1_FIGHT:
					screenMessage.show("Player One: Fight Phase");
					break;
				case GamePhase.P2_FIGHT:
					screenMessage.show("Player Two: Fight Phase");
					break;
			}
		}

		public void draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, SpriteFont font)
		{
			screenMessage.draw(graphicsDevice, spriteBatch, font);
		}

		public void update(double timeStep)
		{
			screenMessage.update(timeStep);
		}
	}
}