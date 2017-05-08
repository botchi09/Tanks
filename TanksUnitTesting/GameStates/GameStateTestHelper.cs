using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks;
using Tanks.Messages;

namespace TanksUnitTesting.GameStates
{
	class GameStateTestHelper
	{
		public GameStateController generateGameStateController()
		{
			TanksModel tanksModel = new TanksModel();
			TanksController tanksController = new TanksController(tanksModel);

			GameStateModel gameStateModel = new GameStateModel();
			ScreenMessage screenMessage = new ScreenMessage();
			MessageView messageView = new MessageView(screenMessage);
			MessageController messageController = new MessageController(messageView);
			GameStateCallbacks gameStateCallbacks = new GameStateCallbacks(messageController);

			GameStateController gameStateController = new GameStateController(gameStateCallbacks, gameStateModel, tanksController, messageController);

			gameStateController.setupGameState();

			return gameStateController;
		}
	}
}
