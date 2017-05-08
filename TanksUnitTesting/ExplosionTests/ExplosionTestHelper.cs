using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks;
using Tanks.Explosions;
using Microsoft.Xna.Framework;

namespace TanksUnitTesting.ExplosionTests
{
	class ExplosionTestHelper
	{
		public ExplosionController getExplosionController()
		{
			TanksModel tanksModel = new TanksModel();
			ExplosionModel explosionModel = new ExplosionModel();

			TanksController tanksController = new TanksController(tanksModel);
			CoverController coverController = new CoverController(tanksModel);

			ExplosionController explosionController = new ExplosionController(explosionModel, tanksController, coverController);

			return explosionController;

		}

		public List<Explosion> generateExplosionRecord()
		{
			ExplosionController explosionController = getExplosionController();
			explosionController.Explosion(new Vector2(100, 100), 10);
			explosionController.Explosion(new Vector2(200, 200), 30);
			explosionController.Explosion(new Vector2(500, 50), 100);
			explosionController.Explosion(new Vector2(0, 0), 1);
			explosionController.Explosion(new Vector2(5000, 5000), 1000);



			List<Explosion> record = explosionController.getExplosionRecord();

			return record;
		}

	}
}
