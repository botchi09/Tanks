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
using Tanks.Buttons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tanks
{
	class ButtonController
	{
		Dictionary<ButtonType, Button> buttons = new Dictionary<ButtonType, Button>();
		Dictionary<ButtonType, Texture2D> textures;

		UserInterfaceController userInterfaceController;

		public ButtonController(UserInterfaceController userInterfaceController, GraphicsDevice graphicsDevice)
		{
			this.userInterfaceController = userInterfaceController;
			initButtons(graphicsDevice);
		}

		//TODO: Define functions permitting main prog ability to show and hide different button groups

		//Define buttons here.
		private void initButtons(GraphicsDevice graphicsDevice)
		{
			int ingameButtonWidthHeight = 100; //Hardcoding this is bad, but currently the best solution
			int heightOffset = 0;
			int buttonPosition = graphicsDevice.Viewport.Height - ingameButtonWidthHeight + heightOffset;

			Button undoButton = new Button(ButtonType.Undo, ingameButtonWidthHeight, ingameButtonWidthHeight, this, new Vector2((graphicsDevice.Viewport.Width / 2) - 100, buttonPosition));
			Button endTurnButton = new Button(ButtonType.EndTurn, ingameButtonWidthHeight, ingameButtonWidthHeight, this, new Vector2((graphicsDevice.Viewport.Width / 2) + 100, buttonPosition));

			buttons.Add(ButtonType.Undo, undoButton);
			buttons.Add(ButtonType.EndTurn, endTurnButton);
		}

		public void showIngameButtons()
		{
			buttons[ButtonType.Undo].show();
			buttons[ButtonType.EndTurn].show();
		}
		
		public void setButtonTextures(Dictionary<ButtonType, Texture2D> textures)
		{
			this.textures = textures;
		}

		//TODO: BUTTON PRESS ANIMATION
		//Returns true if a button is pressed.
		public bool pushButton(Vector2 point)
		{
			//Buttons will have no overlap, so tank style proximity detection is unneeded.
			foreach (KeyValuePair<ButtonType, Button> entry in buttons)
			{
				if (entry.Value.vectorInTouchRegion(point))
				{
					entry.Value.buttonPressed();
					return true;
				}
			}
			return false;
		}

		//Ideally we would have XML to handle callbacks here. This will suffice for the project's scope.
		public void buttonPressed(ButtonType buttonEnum)
		{
			switch (buttonEnum)
			{
				case ButtonType.Undo:
					userInterfaceController.undoLast();
					System.Diagnostics.Debug.WriteLine("Touched undo button");
					break;
				case ButtonType.EndTurn:
					userInterfaceController.endTurn();
					System.Diagnostics.Debug.WriteLine("Touched end turn button");
					break;
			}
		}

		public void buttonReleased(ButtonType buttonEnum)
		{
			throw new NotImplementedException();
		}



		public void draw(SpriteBatch spriteBatch)
		{
			foreach (KeyValuePair<ButtonType, Button> entry in buttons)
			{
				spriteBatch.Begin();
				spriteBatch.Draw(textures[entry.Key], entry.Value.getPosition(), null, Color.White,
								 0,
								 new Vector2(0, 0),
								 1,
								 SpriteEffects.None, 0f);
				spriteBatch.End();
			}
		}
	}
}