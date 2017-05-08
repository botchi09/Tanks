using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks;
using Tanks.Explosions;
using TanksUnitTesting.ExplosionTests;
using TanksUnitTesting.TankTesting;

/**
 * Integration testing for Explosion MVC
 * */
namespace TanksUnitTesting
{
	[TestClass]
	public class TankTest
	{
		TankTestHelper helper = new TankTestHelper();

		[TestMethod]
		public void TanksAreAdded()
		{
			TanksController tanksController = helper.generateTanksController();
			List<Tank> tanks = tanksController.getTanks();
			Assert.AreEqual(tanks.Count, 8);
		}

		[TestMethod]
		public void TanksCanBeAccessed()
		{
			TanksController tanksController = helper.generateTanksController();
			List<Tank> tanks = tanksController.getTanks();

			Tank tank = tanks[4];
			
			Assert.AreEqual(tanks[0].getTeam(), TankTeam.ONE);
		}

		[TestMethod]
		public void GetAppropriateTankFromTouchPosition()
		{  
			TanksController tanksController = helper.generateTanksController();
			List<Tank> tanks = tanksController.getTanks();

			Tank foundTank = tanksController.getTankFromTouchPosition(new Vector2(105, 95));

			Assert.AreEqual(foundTank, tanks[2]);
		}
	}
}
