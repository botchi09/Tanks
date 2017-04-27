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

namespace Tanks.Explosions
{
	class ExplosionController
	{
		private ExplosionModel explosionModel;
		private CoverController coverController;
		private TanksController tanksController;

		public ExplosionController(ExplosionModel explosionModel, TanksController tanksController, CoverController coverController)
		{
			this.explosionModel = explosionModel;
			this.tanksController = tanksController;
			this.coverController = coverController;
		}

		public void Explosion(Vector2 centre, int radius)
		{
			Explosion explosion = new Explosion(explosionModel.explosionRecord.Count + 1, centre, radius, coverController, tanksController, this);
			List<Cover> damagedCover = explosion.Explode();
			coverController.setCoverList(damagedCover);
			explosionModel.explosionRecord.Add(explosion);
		}

		public void clearExplosions() 
		{
			explosionModel.explosionRecord = new List<Explosion>();
		}

		public List<Explosion> getExplosionRecord()
		{
			return explosionModel.explosionRecord;
		}

		public int getMaxExplosions()
		{
			return explosionModel.maxExplosions;
		}

		//TODO: Push to explosion model w/ explosion view to draw and animate all explosions-o
	}
}