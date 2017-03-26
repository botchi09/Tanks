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

		private int minDragDist = 50;
		private int maxDragDist = 80;

		//TODO: Figure out if this efficiency function is needed.
		//Minimise number of points by requiring a minimum drag distance before new waypoint is created
		private bool sufficientDragDistance(Vector2 point1, Vector2 point2)
		{
			//TODO: Fix unresponsiveness as result of this code
			//return true;
			int dist = (int)UtilityFuncs.DistanceSquared(point1, point2);

			return dist > (minDragDist * minDragDist) && dist < (maxDragDist * maxDragDist);
		}

		//TODO: Given an origin, returns a point safely out of collision.
		private Vector2 ensureSafePoint(Vector2 originPoint, Vector2 destinationPoint)
		{
			CoverCollision coverCollision = new CoverCollision();
			Line intersect = new Line();
			intersect.addPoint(originPoint);
			intersect.addPoint(destinationPoint);

			Line safeLine = coverCollision.getSafeIntersectionPoint(intersect, coverController.getCoverList());

			return safeLine.getPoints().Last();
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

		public void update(GameTime gameTime)
		{
			DetectedGesture detectedGesture = gestureDetect.getGesture(gameTime.TotalGameTime.TotalMilliseconds); //May be null before game space is fully initialized

			if (detectedGesture.GestureType != GestureType.None)
			{
				bool pushSuccess = buttonController.pushButton(detectedGesture.Position);
				if (!pushSuccess) //Ensure button presses are not picked up by gesture detection
				{
					Tank selectedTank = null;
					if (!gameStateModel.coverDrawingMode)
					{
						selectedTank = tanksController.getTankFromTouchPosition(detectedGesture.Position);
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
								//TODO: Remove this and replace it with a flick!
								Explosion explosion = new Explosion(detectedGesture.Position, 100, coverController.getCoverList());
								coverController.setCoverList(explosion.Explode());
							}
							break;
						case GestureType.Flick:
							break;
						case GestureType.FreeDrag:

							//Ensure the user must drag out from the tank to draw. Each new drag creates a new line.
							if (detectedGesture.firstDetection)
							{
								Vector2 firstPosition = detectedGesture.firstDetectedGesture.Position;

								if (!gameStateModel.coverDrawingMode)
								{

									if (selectedTank != null && sufficientDragDistance(selectedTank.getPosition(), firstPosition))
									{
										System.Diagnostics.Debug.WriteLine("First draw detection");

										Vector2 safePoint = ensureSafePoint(selectedTank.getPosition(), firstPosition);

										tanksModel.tankFollowLine = new Line();
										tanksModel.tankFollowLine.addPoint(selectedTank.getPosition()); //Create illusion of tank closely following line
										tanksModel.tankFollowLine.addPoint(safePoint);

										selectedTank.saveAndClearWaypoints();
										selectedTank.addWaypoint(selectedTank.getPosition()); //Necessary for Undo to place in correct pos
										selectedTank.addWaypoint(safePoint);

										tanksModel.lastSelectedTank = selectedTank;
										lastTouchPosition = firstPosition;


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
									tanksModel.coverLine.addPoint(firstPosition);
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