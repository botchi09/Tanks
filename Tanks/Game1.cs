using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
using System;
using Tanks.Buttons;

namespace Tanks
{


	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		private SpriteFont font;

		private Line tankFollowLine;
		private TankLineHistory tankLineHistory;

		Line coverLine;
		Line intersectionLine;
		Line shouldNotIntersect;


		Texture2D lineTexture;
		Texture2D oldLineTexture;

		Texture2D tankTexture;

		ButtonController buttonController;

		private TanksController tanksController;
		Cover cover;


		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			graphics.IsFullScreen = true;
			graphics.PreferredBackBufferWidth = 800;
			graphics.PreferredBackBufferHeight = 480;
			graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;

			IsFixedTimeStep = false; //Framerate independent updates
			TargetElapsedTime = System.TimeSpan.FromMilliseconds(((float)1 / 60) * 1000); //60 fps
		}

		GestureDetect gestureDetect;
		private string gestureStateString = "No gesture yet...";

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			tankFollowLine = new Line();
			tankFollowLine.addPoint(new Vector2(800, 700));
			tankFollowLine.addPoint(new Vector2(600, 500));
			tankFollowLine.addPoint(new Vector2(300, 900));

			//tank.setWaypoints(tankFollowLine.getPoints());

			gestureDetect = new GestureDetect();
			TouchPanel.EnabledGestures = GestureType.Tap | GestureType.FreeDrag | GestureType.DragComplete | GestureType.Pinch | GestureType.PinchComplete;

			tankLineHistory = new TankLineHistory();

			tankFollowLine = new Tanks.Line();
			coverLine = new Line();
			coverLine.addPoint(new Vector2(100, 100));
			coverLine.addPoint(new Vector2(200, 110));
			coverLine.addPoint(new Vector2(300, 120));
			coverLine.addPoint(new Vector2(400, 130));
			coverLine.addPoint(new Vector2(500, 150));
			coverLine.addPoint(new Vector2(450, 600));
			coverLine.addPoint(new Vector2(300, 580));
			coverLine.addPoint(new Vector2(200, 590));
			coverLine.addPoint(new Vector2(105, 600));
			coverLine.addPoint(new Vector2(110, 400));
			coverLine.addPoint(new Vector2(90, 200));
			coverLine.addPoint(new Vector2(100, 100));

			tanksController = new TanksController();
			tanksController.createTank(new Vector2(100, 100), tankLineHistory);
			tanksController.createTank(new Vector2(300, 100), tankLineHistory);
			tanksController.createTank(new Vector2(1000, 600), tankLineHistory);
			intersectionLine = new Line();
			intersectionLine.addPoint(new Vector2(50, 50));
			intersectionLine.addPoint(new Vector2(300, 300));

			shouldNotIntersect = new Line();
			shouldNotIntersect.addPoint(new Vector2(700, 700));
			shouldNotIntersect.addPoint(new Vector2(500, 700));

			cover = new Cover();
			cover.setPoints(coverLine.getPoints());

			Vector2? intersectionPoint = cover.getLineIntersectionPoint(intersectionLine);

			if (intersectionPoint != null)
			{
				Explosion explosion = new Explosion((Vector2)intersectionPoint, 50, cover);
			}

			buttonController = new ButtonController(tankLineHistory);

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here
			font = this.Content.Load<SpriteFont>("TanksBodyFont");

			lineTexture = this.Content.Load<Texture2D>("LineTexture");
			oldLineTexture = this.Content.Load<Texture2D>("OldLineTexture");

			tankTexture = this.Content.Load<Texture2D>("Tank2");

			Dictionary<ButtonType, Texture2D> buttonTextures = new Dictionary<ButtonType, Texture2D>();

			buttonTextures.Add(ButtonType.Undo, this.Content.Load<Texture2D>("UndoButton"));
			buttonTextures.Add(ButtonType.EndTurn, this.Content.Load<Texture2D>("EndTurnButton"));

			buttonController.setButtonTextures(buttonTextures);


		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}


		int minDragDist = 10;

		//Minimise number of points by requiring a minimum drag distance before new waypoint is created
		private bool suffientDragDistance(Vector2 point1, Vector2 point2)
		{
			//TODO: Fix unresponsiveness as result of this code
			return true;
			//return UtilityFuncs.DistanceSquared(point1, point2) > (minDragDist * minDragDist);
		}

		Tank lastSelectedTank = null;
		Vector2 lastTouchPosition;

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				Exit();


			// TODO: Add your update logic here
			DetectedGesture detectedGesture = gestureDetect.getGesture(gameTime.TotalGameTime.TotalMilliseconds); //May be null before game space is fully initialized
			if (detectedGesture != null)
			{
				bool pushSuccess = buttonController.pushButton(detectedGesture.Position);


				if (!pushSuccess) //Ensure button presses are not picked up by gesture detection
				{
					Tank selectedTank = tanksController.getTankFromTouchPosition(detectedGesture.Position);

					if (detectedGesture.GestureType != GestureType.None)
					{

						switch (detectedGesture.GestureType)
						{
							case GestureType.Tap:
								gestureStateString = "Tap!";
								if (lastSelectedTank != null)
								{
									lastSelectedTank.saveAndClearWaypoints();
								}
								tankFollowLine = new Line(); //Clear the line on tap
								break;
							case GestureType.Flick:
								gestureStateString = "Flick!";
								break;
							case GestureType.FreeDrag:
								gestureStateString = "Dragging...";

								//Ensure the user must drag out from the tank to draw. Each new drag creates a new line.
								if (detectedGesture.firstDetection)
								{
									Vector2 firstPosition = detectedGesture.firstDetectedGesture.Position;
									if (selectedTank != null && suffientDragDistance(selectedTank.getPosition(), firstPosition))
									{
										tankFollowLine = new Line();
										tankFollowLine.addPoint(selectedTank.getPosition()); //Create illusion of tank closely following line
										tankFollowLine.addPoint(firstPosition);

										selectedTank.saveAndClearWaypoints();
										selectedTank.addWaypoint(selectedTank.getPosition()); //Necessary for Undo to place in correct pos
										selectedTank.addWaypoint(firstPosition);

										lastSelectedTank = selectedTank;
										lastTouchPosition = firstPosition;

										selectedTank.setMovementEnabled(false);

									}
									else
									{
										lastSelectedTank = null;
									}
								}

								Vector2 gesturePosition = detectedGesture.Position;
								if (lastSelectedTank != null && suffientDragDistance(lastTouchPosition, gesturePosition))
								{
									tankFollowLine.addPoint(gesturePosition);
									lastSelectedTank.addWaypoint(gesturePosition);
								}

								lastTouchPosition = gesturePosition;

								break;
							case GestureType.DragComplete:
								gestureStateString = "Drag complete!";
								if (lastSelectedTank != null)
								{
									lastSelectedTank.setMovementEnabled(true);
								}
								break;
							case GestureType.Pinch:
								gestureStateString = "Pinching...";
								break;
							case GestureType.PinchComplete:
								//Fix issue where pinchcomplete detected as flick
								gestureStateString = "Pinch complete!";
								break;
						}
					}
					else
					{
						//gestureStateString = "No gesture detected.";
					}


					/*if (tankFollowLine != null)
					{
						selectedTank.update(timeStep);
					}*/


				}


			}

			float timeStep = (float)gameTime.ElapsedGameTime.TotalSeconds; //Necessary so tanks move framerate-independently
			tanksController.update(timeStep);

			base.Update(gameTime);

		}

		private void undoLastAction()
		{
			if (lastSelectedTank != null)
			{
				lastSelectedTank.saveAndClearWaypoints();
			}
			tankLineHistory.undoLast();
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			// TODO: Add your drawing code here
			tankFollowLine.drawLines(lineTexture, spriteBatch);
			tanksController.draw(tankTexture, oldLineTexture, spriteBatch);
			tankLineHistory.draw(oldLineTexture, spriteBatch);
			intersectionLine.drawLines(lineTexture, spriteBatch);


			cover.draw(lineTexture, spriteBatch);


			spriteBatch.Begin();

			spriteBatch.DrawString(font, gestureStateString, new Vector2(300, 300), Color.Black);

			spriteBatch.End();

			buttonController.draw(spriteBatch);



			base.Draw(gameTime);
		}
	}
}
