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

		private Texture2D lineTexture;
		private Texture2D oldLineTexture;
		private Texture2D tankTexture;
		private Texture2D coverTexture;

		private ButtonController buttonController;
		private UserInterfaceController userInterfaceController;
		private TanksController tanksController;
		private GestureController gestureController;
		private CoverController coverController;

		private TanksModel tanksModel;
		private GameStateModel gameStateModel;

		private TanksView tanksView;

		private GestureDetect gestureDetect;


		private Tank getLastSelectedTank()
		{
			return tanksModel.lastSelectedTank;
		}

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


		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{

			tanksModel = new TanksModel();
			gameStateModel = new GameStateModel();


			gestureDetect = new GestureDetect();
			TouchPanel.EnabledGestures = GestureType.Tap | GestureType.FreeDrag | GestureType.DragComplete | GestureType.Pinch | GestureType.PinchComplete;

			tanksModel.tankLineHistory = new TankLineHistory();
			userInterfaceController = new UserInterfaceController(this);

			tanksController = new TanksController(tanksModel);
			tanksController.createTank(new Vector2(100, 100), tanksModel.tankLineHistory);
			tanksController.createTank(new Vector2(300, 100), tanksModel.tankLineHistory);
			tanksController.createTank(new Vector2(1000, 600), tanksModel.tankLineHistory);

			coverController = new CoverController(tanksModel);

			buttonController = new ButtonController(userInterfaceController);
			gestureController = new GestureController(tanksModel, gameStateModel, tanksController, coverController, buttonController, gestureDetect);

			tanksView = new TanksView(tanksModel, gameStateModel, tanksController, coverController, buttonController);

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
			font = Content.Load<SpriteFont>("TanksBodyFont");

			lineTexture = Content.Load<Texture2D>("LineTexture");
			oldLineTexture = Content.Load<Texture2D>("OldLineTexture");

			coverTexture = Content.Load<Texture2D>("CoverTexture");


			tankTexture = Content.Load<Texture2D>("Tank2");

			Dictionary<ButtonType, Texture2D> buttonTextures = new Dictionary<ButtonType, Texture2D>();

			buttonTextures.Add(ButtonType.Undo, this.Content.Load<Texture2D>("UndoButton"));
			buttonTextures.Add(ButtonType.EndTurn, this.Content.Load<Texture2D>("EndTurnButton"));
			buttonTextures.Add(ButtonType.UndoPressed, this.Content.Load<Texture2D>("UndoButtonPressed"));
			buttonTextures.Add(ButtonType.EndTurnPressed, this.Content.Load<Texture2D>("EndTurnButtonPressed"));


			buttonController.setButtonTextures(buttonTextures);

			tanksView.setDrawTextures(lineTexture, oldLineTexture, tankTexture, coverTexture);
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
			gestureController.update(gameTime);
			float timeStep = (float)gameTime.ElapsedGameTime.TotalSeconds;

			/*if (tankFollowLine != null)
			{
				selectedTank.update(timeStep);
			}*/
			tanksController.update(timeStep);

			base.Update(gameTime);
		}



		private void switchDrawingMode()
		{
			if (gameStateModel.coverDrawingMode)
			{
				gameStateModel.coverDrawingMode = false;
				tanksModel.lastSelectedTank = null;

			}
			else
			{
				gameStateModel.coverDrawingMode = true;
			}
		}

		public void endTurn()
		{
			//TODO: Forward end turn event to game state.
			switchDrawingMode();
		}

		public void undoLastAction()
		{
			if (!gameStateModel.coverDrawingMode)
			{
				if (getLastSelectedTank() != null)
				{
					getLastSelectedTank().saveAndClearWaypoints();
				}
				tanksModel.tankLineHistory.undoLast();
			}
			else
			{
				coverController.removeLastCover();
			}
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			// TODO: Add your drawing code here

			tanksView.draw(spriteBatch, gameTime);




			base.Draw(gameTime);
		}
	}
}

