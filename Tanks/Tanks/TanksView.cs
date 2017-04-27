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

		private GraphicsDevice graphics;

		private Dictionary<TankTeam, Texture2D> teamTextures;

		private Texture2D inkTexture;

		public TanksView(GraphicsDevice graphics, TanksModel tanksModel, GameStateModel gameStateModel, TanksController tanksController, CoverController coverController, ButtonController buttonController)
		{
			this.graphics = graphics;
			this.tanksModel = tanksModel;
			this.gameStateModel = gameStateModel;
			this.tanksController = tanksController;
			this.coverController = coverController;
			this.buttonController = buttonController;

			inkTexture = new Texture2D(graphics, 1, 1, false, SurfaceFormat.Color);
			inkTexture.SetData<Color>(new Color[] { Color.White });

		}
		
		public void setDrawTextures(Texture2D lineTexture, Texture2D oldLineTexture, Dictionary<TankTeam, Texture2D> teamTextures, Texture2D coverTexture)
		{
			this.lineTexture = lineTexture;
			this.oldLineTexture = oldLineTexture;
			this.teamTextures = teamTextures;
			this.coverTexture = coverTexture;
		}

		private void drawTanks(Texture2D inkTexture, Dictionary<TankTeam, Texture2D> teamTextures, Texture2D tankOldLineTexture, SpriteBatch spriteBatch)
		{
			tanksController.getTanks().ForEach(delegate (Tank tank)
			{
				tank.draw(teamTextures[tank.getTeam()], tankOldLineTexture, spriteBatch);

				//Only draw the ink monitor if we're actively moving tank
				if (tanksModel.selectedTank != null && tanksModel.selectedTank.Equals(tank))
				{
					tank.drawInkMonitor(inkTexture, spriteBatch);
				}
			});
		}

		public void draw(SpriteBatch spriteBatch)
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

			drawTanks(inkTexture, teamTextures, oldLineTexture, spriteBatch);
			tanksModel.tankLineHistory.draw(oldLineTexture, spriteBatch);

			buttonController.draw(spriteBatch);

		}
	}
}