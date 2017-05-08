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
using Microsoft.Xna.Framework;

namespace Tanks
{
	class MessageView
	{
		ScreenMessage screenMessage;
		String messageToShow;

		public MessageView(ScreenMessage screenMessage)
		{
			this.screenMessage = screenMessage;
		}

		//Translate GamePhase into string. Normally would be handled by localisation lib.
		public void showMessage(GamePhase message)
		{
			switch (message)
			{
				case GamePhase.P1_DRAW:
					messageToShow = "Player One: Draw Phase";
					break;
				case GamePhase.P2_DRAW:
					messageToShow = "Player Two: Draw Phase";
					break;
				case GamePhase.P1_FIGHT:
					messageToShow = "Player One: Fight Phase";
					break;
				case GamePhase.P2_FIGHT:
					messageToShow = "Player Two: Fight Phase";
					break;
				case GamePhase.P1_WIN:
					messageToShow = "Match complete! Player one wins!";
					break;
				case GamePhase.P2_WIN:
					messageToShow = "Match complete! Player two wins!";
					break;


			}
			screenMessage.show(messageToShow);
		}

		public void draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, SpriteFont font)
		{
			screenMessage.draw(graphicsDevice, spriteBatch, font);

			spriteBatch.Begin();
			Vector2 fontPos = new Vector2(graphicsDevice.Viewport.Width / 2,
						0 + 50);
			Vector2 FontOrigin = font.MeasureString(messageToShow) / 2;
			// Draw the string
			spriteBatch.DrawString(font, messageToShow, fontPos, Color.Black,
				0, FontOrigin, 0.5f, SpriteEffects.None, 1.0f);
			spriteBatch.End();

		}

		public void update(double timeStep)
		{
			screenMessage.update(timeStep);
		}
	}
}