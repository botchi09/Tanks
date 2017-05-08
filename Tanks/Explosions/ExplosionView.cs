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
using Android.Graphics;
using Microsoft.Xna.Framework;

namespace Tanks.Explosions
{
	class ExplosionView
	{
		private ExplosionController explosionController;
		private List<Texture2D> explosionTextures;

		public ExplosionView(ExplosionController explosionController)
		{
			this.explosionController = explosionController;
		}

		public void setDrawTextures(List<Texture2D> explosionTextures)
		{
			this.explosionTextures = explosionTextures;
		}

		public void draw(SpriteBatch spriteBatch)
		{
			List<Explosion> explosions = explosionController.getExplosionRecord();

			//Draw every explosion
			for (int i=0;i<explosions.Count;i++)
			{
				//Ensure we don't try to access Explosion14 or so, when we only have textures up to 8
				int decal = i % explosionController.getMaxExplosions();
				spriteBatch.Begin();
				int halfRadius = explosions[i].getRadius() / 2;
				int scale = 2;
				//TODO:Modulo operation using Max Explosions for consistent drawing
				spriteBatch.Draw(explosionTextures[decal], explosions[i].getPosition(), null, Microsoft.Xna.Framework.Color.White,
							 0,
							 new Vector2(halfRadius, halfRadius),
							 scale,
							 SpriteEffects.None, 0f);
				spriteBatch.End();

			}
		}
	}
}