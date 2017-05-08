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
	//Prevents all touch actions while displaying a short image with text.
	class ScreenMessage
	{

		private double lastShown = 0;
		private int secondsToShow = 2;
		private String showingText = "";
		private bool shouldShow = false;
		private double currentTime = 0;

		public ScreenMessage()
		{

		}

		public void show(String text)
		{
			showingText = text;
			lastShown = currentTime;
		}

		public void draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, SpriteFont font)
		{
			if (shouldShow) //Only draw popup when required
			{
				spriteBatch.Begin();
				Vector2 fontPos = new Vector2(graphicsDevice.Viewport.Width / 2,
						graphicsDevice.Viewport.Height / 2);
				Vector2 FontOrigin = font.MeasureString(showingText) / 2;
				// Draw the string
				spriteBatch.DrawString(font, showingText, fontPos, Color.Black,
					0, FontOrigin, 1.0f, SpriteEffects.None, 1.0f);
				spriteBatch.End();
			}
		}

		public void update(double totalTime)
		{
			currentTime = totalTime;
			if (totalTime < lastShown + (secondsToShow * 1000))
			{
				shouldShow = true;
			}
			else
			{
				shouldShow = false;
			}
		}

	}
}