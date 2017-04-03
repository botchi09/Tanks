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
	class TanksView
	{
		private TanksModel tanksModel;
		private GameStateModel gameStateModel;

		private TanksController tanksController;
		private ButtonController buttonController;
		private CoverController coverController;

		private Texture2D lineTexture;
		private Texture2D oldLineTexture;
		private Texture2D coverTexture;

		private Dictionary<TankTeam, Texture2D> teamTextures;

		public TanksView(TanksModel tanksModel, GameStateModel gameStateModel, TanksController tanksController, CoverController coverController, ButtonController buttonController)
		{
			this.tanksModel = tanksModel;
			this.gameStateModel = gameStateModel;
			this.tanksController = tanksController;
			this.coverController = coverController;
			this.buttonController = buttonController;

		}

		public void setDrawTextures(Texture2D lineTexture, Texture2D oldLineTexture, Dictionary<TankTeam, Texture2D> teamTextures, Texture2D coverTexture)
		{
			this.lineTexture = lineTexture;
			this.oldLineTexture = oldLineTexture;
			this.teamTextures = teamTextures;
			this.coverTexture = coverTexture;
		}

		public void draw(SpriteBatch spriteBatch, GameTime gameTime)
		{

			

			if (!gameStateModel.coverDrawingMode)
			{
				tanksModel.tankFollowLine.drawLines(lineTexture, spriteBatch);
			}
			else
			{
				tanksModel.coverLine.drawLines(lineTexture, spriteBatch);
			}

			coverController.getCoverList().ForEach(delegate (Cover coverItem)
			{
				coverItem.draw(coverTexture, spriteBatch);
			});

			tanksController.draw(teamTextures, oldLineTexture, spriteBatch);
			tanksModel.tankLineHistory.draw(oldLineTexture, spriteBatch);

			buttonController.draw(spriteBatch);

		}
	}
}