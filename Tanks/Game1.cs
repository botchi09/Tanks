using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Tanks
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Line tankFollowLine;
		Line coverLine;
		Line intersectionLine;
		Line shouldNotIntersect;


		Texture2D lineTexture;

		Cover cover;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			graphics.IsFullScreen = true;
			graphics.PreferredBackBufferWidth = 800;
			graphics.PreferredBackBufferHeight = 480;
			graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here
			tankFollowLine = new Line();
			tankFollowLine.addPoint(new Vector2(100, 200));
			tankFollowLine.addPoint(new Vector2(200, 100));
			tankFollowLine.addPoint(new Vector2(300, 300));

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
			lineTexture = this.Content.Load<Texture2D>("LineTexture");

			// TODO: use this.Content to load your game content here
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
			//tankFollowLine.drawLines(lineTexture, spriteBatch);
			coverLine.drawLines(lineTexture, spriteBatch);
			intersectionLine.drawLines(lineTexture, spriteBatch);

			Vector2? intersectionPoint = cover.getLineIntersectionPoint(intersectionLine);

			if (intersectionPoint != null)
			{
				//Cover.ExplosionAt(intersectionPoint);
			}
			else
			{

			}
			//cover.doesLineCollide(shouldNotIntersect);


			base.Draw(gameTime);
		}
	}
}
