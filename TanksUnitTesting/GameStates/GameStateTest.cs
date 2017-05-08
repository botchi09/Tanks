using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks;
using TanksUnitTesting.GameStates;

namespace GameStates
{

	[TestClass]
	public class GameStateTest
	{
		GameStateTestHelper helper = new GameStateTestHelper();

		[TestMethod]
		public void GameStateIsProperlyIncremented()
		{
			GameStateController gameStateController = helper.generateGameStateController();

			gameStateController.endTurn();
			gameStateController.endTurn();
			gameStateController.endTurn();

			Assert.AreEqual(gameStateController.getGamePhase(), GamePhase.P2_FIGHT);

		}
	}
}
