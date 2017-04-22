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
using Microsoft.Xna.Framework.Input.Touch;

namespace Tanks
{
	class GestureController
	{
		private GestureDetect gestureDetect;
		private ButtonController buttonController;
		private TanksController tanksController;
		private CoverController coverController;
		private TanksModel tanksModel;
		private GameStateModel gameStateModel;
		private Vector2 lastTouchPosition;
		private Vector2? lastSafePosition;

		private int minDragDist = 20;

		//TODO: Figure out if this efficiency function is needed.
		//Minimise number of points by requiring a minimum drag distance before new waypoint is created
		private bool sufficientDragDistance(Vector2 point1, Vector2 point2)
		{
			//TODO: Fix unresponsiveness as result of this code
			//return true;
			float dist = Vector2.DistanceSquared(point1, point2);

			return dist > (minDragDist * minDragDist);
		}

		//TODO: Given an origin, returns a point safely out of collision.
		private Vector2 ensureSafePoint(Vector2 originPoint, Vector2 destinationPoint)
		{
			CoverCollision coverCollision = new CoverCollision();
			Line intersect = new Line();

			if (lastSafePosition != null)
			{
				intersect.addPoint((Vector2)lastSafePosition);
			}
			else
			{
				intersect.addPoint(originPoint);
			}
			intersect.addPoint(destinationPoint);

			CoverCollisionResult result = coverCollision.getSafeIntersectionPoint(intersect, coverController.getCoverList());

			Line safeLine = result.getLine();
			Vector2 lastSafeLinePoint = safeLine.getPoints().Last();

			if (!result.getWasModified())
			{
				lastSafePosition = lastSafeLinePoint;
			}

			return lastSafeLinePoint;
		}

		public GestureController(TanksModel tanksModel, GameStateModel gameStateModel,
			TanksController tanksController, CoverController coverController, ButtonController buttonController, GestureDetect gestureDetect)
		{
			this.tanksModel = tanksModel;
			this.gameStateModel = gameStateModel;
			this.tanksController = tanksController;
			this.coverController = coverController;
			this.buttonController = buttonController;
			this.gestureDetect = gestureDetect;

		}

		private bool authorisedToMoveTank(Tank tank)
		{
			switch(tank.getTeam())
			{
				case TankTeam.ONE:
					return gameStateModel.gamePhase == GamePhase.P1_FIGHT;
				case TankTeam.TWO:
					return gameStateModel.gamePhase == GamePhase.P2_FIGHT;
			}
			return false;
		}

		public void update(double timeStep)
		{
			DetectedGesture detectedGesture = gestureDetect.getGesture(timeStep); //May be null before game space is fully initialized

			if (detectedGesture.GestureType != GestureType.None)
			{
				bool pushSuccess = buttonController.pushButton(detectedGesture.Position);
				if (!pushSuccess) //Ensure button presses are not picked up by gesture detection
				{
					Tank selectedTank = null;
					if (!gameStateModel.coverDrawingMode)
					{
						//Find tank nearest to touch position.
						//rOnly set selected tank to proposed tank selection if player is allowed to control that tank.
						Tank proposedTank = tanksController.getTankFromTouchPosition(detectedGesture.Position);
						if (authorisedToMoveTank(proposedTank))
						{
							selectedTank = proposedTank;
						}
					}

					switch (detectedGesture.GestureType)
					{
						case GestureType.Tap:
							if (!gameStateModel.coverDrawingMode)
							{
								if (tanksModel.lastSelectedTank != null)
								{
									tanksModel.lastSelectedTank.saveAndClearWaypoints();
								}
								tanksModel.tankFollowLine = new Line(); //Clear the line on tap}
							}
							else
							{

							}
							break;
						case GestureType.Flick:
							if (!gameStateModel.coverDrawingMode && selectedTank != null)
							{
								Vector2 direction = Vector2.Multiply(detectedGesture.Delta, 6);

								selectedTank.shoot(direction, coverController, tanksController);

							}
							break;
						case GestureType.FreeDrag:

							//Ensure the user must drag out from the tank to draw. Each new drag creates a new line.
							if (detectedGesture.firstDetection)
							{
								List<Vector2> firstPositions = detectedGesture.detectedGestureList;

								if (!gameStateModel.coverDrawingMode)
								{

									if (selectedTank != null && sufficientDragDistance(selectedTank.getPosition(), firstPositions.Last()))
									{
										System.Diagnostics.Debug.WriteLine("First draw detection");

										lastSafePosition = null;

										tanksModel.tankFollowLine = new Line();
										tanksModel.tankFollowLine.addPoint(selectedTank.getPosition()); //Create illusion of tank closely following line

										selectedTank.saveAndClearWaypoints();
										selectedTank.addWaypoint(selectedTank.getPosition()); //Necessary for Undo to place in correct pos

										firstPositions.ForEach(delegate (Vector2 position)
										{

											Vector2 safePoint = ensureSafePoint(selectedTank.getPosition(), position);
											tanksModel.tankFollowLine.addPoint(safePoint);

											selectedTank.addWaypoint(safePoint);

										});

										tanksModel.lastSelectedTank = selectedTank;
										lastTouchPosition = firstPositions.Last();

										selectedTank.setMovementEnabled(false);

									}
									else
									{
										tanksModel.lastSelectedTank = null;
									}
								}
								else
								{
									tanksModel.coverLine = new Tanks.Line();

									firstPositions.ForEach(delegate (Vector2 position)
									{
										tanksModel.coverLine.addPoint(position);
									});
								}
							}

							Vector2 gesturePosition = detectedGesture.Position;
							if (!gameStateModel.coverDrawingMode)
							{
								if (tanksModel.lastSelectedTank != null && sufficientDragDistance(tanksModel.tankFollowLine.getPoints().Last(), gesturePosition))
								{
									System.Diagnostics.Debug.WriteLine("Writing new point to line...");

									Vector2 safePoint = ensureSafePoint(tanksModel.tankFollowLine.getPoints().Last(), gesturePosition);
									tanksModel.tankFollowLine.addPoint(safePoint);
									tanksModel.lastSelectedTank.addWaypoint(safePoint);
								}
							}
							else
							{
								tanksModel.coverLine.addPoint(detectedGesture.Position);
							}


							lastTouchPosition = gesturePosition;

							break;
						case GestureType.DragComplete:
							if (!gameStateModel.coverDrawingMode)
							{
								if (tanksModel.lastSelectedTank != null)
								{
									tanksModel.lastSelectedTank.setMovementEnabled(true);
								}
							}
							else
							{
								tanksModel.coverLine.addPoint(tanksModel.coverLine.getPoints()[0]);
								Cover cover = new Cover();
								//TODO: Perform union on other bits of cover. Merge connected cover.
								cover.setPoints(tanksModel.coverLine.getPoints());
								coverController.addCover(cover);
							}

							break;
						case GestureType.Pinch:

							break;
						case GestureType.PinchComplete:
							//Fix issue where pinchcomplete detected as flick
							break;
					}
				}
				else
				{
					//gestureStateString = "No gesture detected.";
				}
			}
		}

	}
}