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
		Line line;
		Tank tank;
		Texture2D lineTexture;
		Texture2D tankTexture;

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
			// TODO: Add your initialization logic here
			line = new Line();
			line.addPoint(new Vector2(800, 700));
			line.addPoint(new Vector2(600, 500));
			line.addPoint(new Vector2(300, 900));

			tank = new Tank();
			tank.setPosition(new Vector2(100, 100));
			tank.setWaypoints(line.getPoints());

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
			tankTexture = this.Content.Load<Texture2D>("Tank2");

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

		Vector2 nextWaypoint;
		int waypointIndex = 1;

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

			float timeStep = (float)gameTime.ElapsedGameTime.TotalSeconds;

			if (line != null)
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
			line.drawLines(lineTexture, spriteBatch);
			tank.draw(tankTexture, spriteBatch);

			/*spriteBatch.Begin();
			spriteBatch.Draw(tankTexture, new Vector2(100, 100), null, Color.White,
							 0,
							 new Vector2(100, 100),
							 1,
							 SpriteEffects.None, 0f);
			spriteBatch.End();*/

			base.Draw(gameTime);
		}
	}
}
