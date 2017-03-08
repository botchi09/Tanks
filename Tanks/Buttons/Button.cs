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
using Microsoft.Xna.Framework;
using Tanks.Buttons;
using Microsoft.Xna.Framework.Graphics;

namespace Tanks
{
	class Button
	{
		private bool active = true;
		private bool hidden = false;
		private Vector2 position;
		private int width;
		private int height;
		private ButtonController buttonController;
		private ButtonType buttonEnum;
		private Texture2D texture;

		public void setActive(bool active)
		{
			this.active = active;
		}

		//TODO: Consider if unhidden buttons should be auto-active.
		public void show()
		{
			hidden = false;
			active = true;
		}

		public void hide()
		{
			hidden = true;
			active = false;
		}

		//TODO: Decide if w/h should be texture2d agnostic
		public Button(ButtonType buttonEnum, Texture2D texture, ButtonController buttonController, Vector2 position)
		{
			this.position = position;
			//this.width = width;
			//this.height = height;
			this.width = texture.Width;
			this.height = texture.Height;
			this.buttonEnum = buttonEnum;
			this.texture = texture;
			this.buttonController = buttonController;
		}

		public bool vectorInTouchRegion(Vector2 vector)
		{
			return (position.X >= vector.X &&
				position.X <= vector.X + width &&
				position.Y >= vector.Y &&
				position.Y <= vector.Y + height);
		}

		public void buttonPressed()
		{
			buttonController.buttonPressed(buttonEnum);
		}

		public void buttonReleased()
		{
			buttonController.buttonReleased(buttonEnum);
		}

		public void draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Begin();
			spriteBatch.Draw(this.texture, position, null, Color.White,
							 0,
							 new Vector2(0, 0),
							 1,
							 SpriteEffects.None, 0f);
			spriteBatch.End();
		}
	}
}