using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

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
		Tank tank;
		Line coverLine;
		Line intersectionLine;
		Line shouldNotIntersect;


		Texture2D lineTexture;
		Texture2D tankTexture;

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

			tank = new Tank();
			tank.setPosition(new Vector2(100, 100));
			//tank.setWaypoints(tankFollowLine.getPoints());

			gestureDetect = new GestureDetect();
			TouchPanel.EnabledGestures = GestureType.Tap | GestureType.FreeDrag | GestureType.DragComplete | GestureType.Pinch | GestureType.PinchComplete;

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
			tankTexture = this.Content.Load<Texture2D>("Tank2");


		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

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
			DetectedGesture detectedGesture = gestureDetect.getGesture(gameTime.TotalGameTime.TotalMilliseconds);
			
			if (detectedGesture.GestureType != GestureType.None)
			{
				switch(detectedGesture.GestureType)
				{
					case GestureType.Tap:
						gestureStateString = "Tap!";
						tank.clearWaypoints();
						tankFollowLine = new Line(); //Clear the line on tap
						break;
					case GestureType.Flick:
						gestureStateString = "Flick!";
						break;
					case GestureType.FreeDrag:
						gestureStateString = "Dragging...";
						if (detectedGesture.firstDetection)
						{
							tankFollowLine.addPoint(detectedGesture.firstDetectedGesture.Position);
							tank.addWaypoint(detectedGesture.firstDetectedGesture.Position);

						}
						tankFollowLine.addPoint(detectedGesture.Position);
						tank.addWaypoint(detectedGesture.Position);

						break;
					case GestureType.DragComplete:
						gestureStateString = "Drag complete!";
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

			float timeStep = (float)gameTime.ElapsedGameTime.TotalSeconds;

			if (tankFollowLine != null)
			{
				tank.update(timeStep);
			}


			base.Update(gameTime);
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
			tank.draw(tankTexture, spriteBatch);
			intersectionLine.drawLines(lineTexture, spriteBatch);


			cover.draw(lineTexture, spriteBatch);


			spriteBatch.Begin();

			spriteBatch.DrawString(font, gestureStateString, new Vector2(300, 300), Color.Black);

			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
