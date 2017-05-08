using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks;

namespace TanksUnitTesting.TankTesting
{
	class TankTestHelper
	{
		//Create a TanksController and add 4 tanks from each team
		public TanksController generateTanksController()
		{
			TanksModel tanksModel = new TanksModel();

			TanksController tanksController = new TanksController(tanksModel);
			CoverController coverController = new CoverController(tanksModel);

			TankLineHistory tankLineHistory = new TankLineHistory();

			Tank tank3 = new Tank(tankLineHistory, TankTeam.ONE);

			tank3.setPosition(new Vector2(100, 100));
			tanksController.addTank(new Tank(tankLineHistory, TankTeam.ONE));
			tanksController.addTank(new Tank(tankLineHistory, TankTeam.ONE));
			tanksController.addTank(tank3);
			tanksController.addTank(new Tank(tankLineHistory, TankTeam.ONE));

			tanksController.addTank(new Tank(tankLineHistory, TankTeam.TWO));
			tanksController.addTank(new Tank(tankLineHistory, TankTeam.TWO));
			tanksController.addTank(new Tank(tankLineHistory, TankTeam.TWO));
			tanksController.addTank(new Tank(tankLineHistory, TankTeam.TWO));

			return tanksController;
		}


	}
}
