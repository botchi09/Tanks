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
using Tanks.Messages;

namespace Tanks
{
	class GameStateController
	{
		private GameStateCallbacks gameStateCallbacks;
		private GameStateModel gameStateModel;
		private TanksController tanksController;
		private int tanksPerTeam = 4; //Should really be in model but fine for now.
		private MessageController messageController;

		public GameStateController(GameStateCallbacks gameStateCallbacks, GameStateModel gameStateModel, TanksController tanksController, MessageController messageController)
		{
			this.gameStateCallbacks = gameStateCallbacks;
			this.gameStateModel = gameStateModel;
			this.tanksController = tanksController;
			this.messageController = messageController;
		}

		private void resetTanks(TankTeam team)
		{
			tanksController.getTanks().ForEach(delegate (Tank tank)
			{
				if (tank.getTeam() == team)
				{
					tank.resetEndOfTurn();
				}
			});
		}

		//Returns enum of winning team.
		//TODO: Refactor to allow any number of tanks per team.
		private TankTeam? checkVictor()
		{
			Dictionary<TankTeam, int> deadCount = new Dictionary<TankTeam, int>();

			deadCount[TankTeam.ONE] = 0;
			deadCount[TankTeam.TWO] = 0;

			tanksController.getTanks().ForEach(delegate (Tank tank)
			{
				if (!tank.getAlive())
				{
					deadCount[tank.getTeam()]++;
				}
			});

			bool p1Dead = false;
			bool p2Dead = false;

			if (deadCount[TankTeam.ONE] == tanksPerTeam)
			{
				p1Dead = true;
			}

			if (deadCount[TankTeam.TWO] == tanksPerTeam)
			{
				p2Dead = true;
			}

			if (p1Dead && !p2Dead)
			{
				return TankTeam.ONE;
			}

			if (p2Dead && !p1Dead)
			{
				return TankTeam.TWO;
			}

			return null;
		}

		//Initially called when game is set up into phase 1.
		public void setupGameState()
		{
			gameStateModel.coverDrawingMode = true;
			gameStateModel.gamePhase = GamePhase.P1_DRAW;
			messageController.dispatchMessage(GamePhase.P1_DRAW);

		}

		//First check if player won turn, then increment game state
		public void endTurn()
		{
			TankTeam? victor = checkVictor();
			if (victor == null)
			{
				incrementGameState();
			}
			else
			{
				gameStateModel.victor = (TankTeam)victor;
				gameStateCallbacks.matchComplete();
				System.Diagnostics.Debug.WriteLine("Match complete!");

			}
		}

		private void switchDrawingMode()
		{
			if (gameStateModel.coverDrawingMode)
			{
				gameStateModel.coverDrawingMode = false;
				tanksController.setLastSelectedTank(null);

			}
			else
			{
				gameStateModel.coverDrawingMode = true;
			}
		}

		private void fightTurnEnd(TankTeam team)
		{
			resetTanks(team);
		}

		/* More than 2 players out of spec and un-needed
		 * 1: P1 Cover draw phase
		 * 2: P2 Cover draw phase
		 * 3: P1 Fight phase
		 * 4: P2 Fight phase
		 * 5: Goto 3 until P1 or P2 are neutralised.
		 * */
		public void incrementGameState()
		{
			switch (gameStateModel.gamePhase)
			{
				case GamePhase.P1_DRAW:
					gameStateModel.gamePhase = GamePhase.P2_DRAW;
					System.Diagnostics.Debug.WriteLine("P2 draw phase");
					messageController.dispatchMessage(GamePhase.P2_DRAW);
					break;
				case GamePhase.P2_DRAW:
					gameStateModel.gamePhase = GamePhase.P1_FIGHT;
					System.Diagnostics.Debug.WriteLine("P1 fight phase");
					gameStateModel.coverDrawingMode = false;
					messageController.dispatchMessage(GamePhase.P1_FIGHT);
					break;
				case GamePhase.P1_FIGHT:
					gameStateModel.gamePhase = GamePhase.P2_FIGHT;
					System.Diagnostics.Debug.WriteLine("P2 fight phase");
					messageController.dispatchMessage(GamePhase.P2_FIGHT);
					fightTurnEnd(TankTeam.ONE);

					break;
				case GamePhase.P2_FIGHT:
					gameStateModel.gamePhase = GamePhase.P1_FIGHT;
					System.Diagnostics.Debug.WriteLine("P1 fight phase");
					messageController.dispatchMessage(GamePhase.P1_FIGHT);
					fightTurnEnd(TankTeam.TWO);

					break;
			}
		}
	}
}