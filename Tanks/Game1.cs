using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
using System;
using Tanks.Buttons;
using Tanks.Messages;
using Tanks.Explosions;

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
		private GameStateController gameStateController;

		private TanksModel tanksModel;
		private GameStateModel gameStateModel;

		private TanksView tanksView;

		private ExplosionModel explosionModel;
		private ExplosionController explosionController;
		private ExplosionView explosionView;

		private GestureDetect gestureDetect;


		private GameStateCallbacks gameStateCallbacks;

		private ScreenMessage screenMessage;
		private MessageController messageController;
		private MessageView messageView;

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

			int tanksPerSide = 4;

			for (int i = 1; i <= tanksPerSide; i++)
			{
				float yPos = (GraphicsDevice.Viewport.Height / (tanksPerSide + 1)) * i;

				Tank leftTank = tanksController.createTank(new Vector2(300, yPos), TankTeam.ONE, tanksModel.tankLineHistory);
				Tank rightTank = tanksController.createTank(new Vector2(1620, yPos), TankTeam.TWO, tanksModel.tankLineHistory);

				rightTank.setRotation(180);
			}
			/*tanksController.createTank(new Vector2(100, 216 * 2), TankTeam.ONE, tanksModel.tankLineHistory);
			tanksController.createTank(new Vector2(100, 216 * 3), TankTeam.ONE, tanksModel.tankLineHistory);
			tanksController.createTank(new Vector2(100, 216 * 4), TankTeam.ONE, tanksModel.tankLineHistory);*/

			//TODO: Remove model from any non-constroller constructors.
			coverController = new CoverController(tanksModel);

			buttonController = new ButtonController(userInterfaceController, graphics.GraphicsDevice);

			tanksView = new TanksView(graphics.GraphicsDevice, tanksModel, gameStateModel, tanksController, coverController, buttonController);

			screenMessage = new ScreenMessage();
			messageView = new MessageView(screenMessage);
			messageController = new MessageController(messageView);

			gameStateCallbacks = new GameStateCallbacks(messageController);


			explosionModel = new ExplosionModel();
			explosionController = new ExplosionController(explosionModel, tanksController, coverController);
			explosionView = new ExplosionView(explosionController);

			gestureController = new GestureController(tanksModel, gameStateModel, tanksController, coverController, buttonController, gestureDetect, explosionController);

			gameStateController = new GameStateController(gameStateCallbacks, gameStateModel, tanksController, messageController);

			gameStateController.setupGameState();

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		//TODO: Delegate to own content loader class
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

			Dictionary<TankTeam, Texture2D> tankTextures = new Dictionary<TankTeam, Texture2D>();

			tankTextures.Add(TankTeam.ONE, this.Content.Load<Texture2D>("Tank_Filled"));
			tankTextures.Add(TankTeam.TWO, this.Content.Load<Texture2D>("Tank_Unfilled"));

			Dictionary<ButtonType, Texture2D> buttonTextures = new Dictionary<ButtonType, Texture2D>();

			buttonTextures.Add(ButtonType.Undo, this.Content.Load<Texture2D>("UndoButton"));
			buttonTextures.Add(ButtonType.EndTurn, this.Content.Load<Texture2D>("EndTurnButton"));
			buttonTextures.Add(ButtonType.UndoPressed, this.Content.Load<Texture2D>("UndoButtonPressed"));
			buttonTextures.Add(ButtonType.EndTurnPressed, this.Content.Load<Texture2D>("EndTurnButtonPressed"));

			buttonController.setButtonTextures(buttonTextures);

			tanksView.setDrawTextures(lineTexture, oldLineTexture, tankTextures, this.Content.Load<Texture2D>("SmokeLines"), this.Content.Load<Texture2D>("dead"), coverTexture);

			List<Texture2D> explosionTextures = new List<Texture2D>();

			//We have ten explosion textures to utilise
			for (var i=1; i<10; i++)
			{
				explosionTextures.Add(this.Content.Load<Texture2D>("explosions/Explosion" + i));
			}

			explosionView.setDrawTextures(explosionTextures);
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
			double timeStep = gameTime.TotalGameTime.TotalMilliseconds;

			
			gestureController.update(timeStep);

			tanksController.update(gameTime.ElapsedGameTime.TotalSeconds); //Uses ElapsedTime to ensure smooth tank movement framerate independently
			messageController.update(timeStep);

			base.Update(gameTime);
		}

		public void endTurn()
		{
			gameStateController.endTurn();
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
			GraphicsDevice.Clear(Color.White);


			tanksView.draw(spriteBatch);

			//GraphicsDevice is necessary to find resolution agnostic centre of screen
			messageView.draw(graphics.GraphicsDevice, spriteBatch, font);

			explosionView.draw(spriteBatch);

			base.Draw(gameTime);
		}
	}
}

