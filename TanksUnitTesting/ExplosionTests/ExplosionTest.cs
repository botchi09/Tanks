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

/**
 * Integration testing for Explosion MVC
 * */
namespace TanksUnitTesting
{
	[TestClass]
	public class ExplosionTest
	{
		ExplosionTestHelper helper = new ExplosionTestHelper();

		[TestMethod]
		public void ExplosionIsCreated()
		{
			List<Explosion> record = helper.generateExplosionRecord();

			Assert.AreEqual(record[0].getRadius(), 10);
			Assert.AreEqual(record[4].getRadius(), 1000);
			Assert.AreEqual(record[2].getPosition(), new Vector2(500, 50));
		}

		[TestMethod]
		public void IdsConformToMaxExplosions()
		{
			List<Explosion> record = helper.generateExplosionRecord();
			int maxExplosions = helper.getExplosionController().getMaxExplosions();
			Assert.AreEqual(5, record[4].getId());
			Assert.AreEqual(3, record[2].getId());
		}

		[TestMethod]
		public void ExplosionRecordClears()
		{
			ExplosionController controller = helper.getExplosionController();
			controller.Explosion(new Vector2(5, 0), 0);
			Assert.AreEqual(controller.getExplosionRecord()[0].getPosition(), new Vector2(5, 0));
			controller.clearExplosions();
			Assert.AreEqual(controller.getExplosionRecord().Count, 0);
		}
	}
}
