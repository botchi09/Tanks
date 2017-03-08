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

		TankLineHistory tankLineHistory; //Required for undo button functionality

		//TODO: Define functions permitting main prog ability to show and hide different button groups

		//Define buttons here.
		private void initButtons()
		{
			Button undoButton = new Button(ButtonType.Undo, textures[ButtonType.Undo], this, new Vector2(800, 700));
			Button endTurnButton = new Button(ButtonType.EndTurn, textures[ButtonType.EndTurn], this, new Vector2(800, 900));

			buttons.Add(ButtonType.Undo, undoButton);
			buttons.Add(ButtonType.Undo, endTurnButton);
		}

		public ButtonController(Dictionary<ButtonType, Texture2D> textures, TankLineHistory tankLineHistory)
		{
			this.textures = textures;
			this.tankLineHistory = tankLineHistory;
			initButtons();
		}

		//Returns true if a button is pressed.
		public bool pushButton(Vector2 point)
		{

			return false;
		}

		public void buttonPressed(ButtonType buttonEnum)
		{
			if (buttonEnum == ButtonType.Undo)
			{
				tankLineHistory.undoLast();
			}
		}

		public void buttonReleased(ButtonType buttonEnum)
		{
			throw new NotImplementedException();
		}
	}
}