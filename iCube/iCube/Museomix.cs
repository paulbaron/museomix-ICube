#region Using Statements
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace iCube
{
    public class Model
    {
        public String Name
        {
            get;
            set;
        }

        public String Date
        {
            get;
            set;
        }

        public String Colour
        {
            get;
            set;
        }

        public String Material
        {
            get;
            set;
        }
    }

    public class Criteria
    {
        public String Name
        {
            get;
            set;
        }

        public Color Color
        {
            get;
            set;
        }

        public Criteria()
        {
        }
    }

    public class Repository
    {
        public IEnumerable<Model> Models
        {
            get;
            protected set;
        }

        public IEnumerable<Criteria> Criterias
        {
            get;
            protected set;
        }

        public Repository()
        {
            Models = new List<Model>
            {
                new Model
                {
                    Name = "Test",
                    Colour = "Red",
                    Date = "1950",
                    Material = "Brick"
                }
            };

            Criterias = new List<Criteria>
            {
                new Criteria
                {
                    Name = "Couleur",
                    Color = Color.Pink
                },
                new Criteria
                {
                    Name = "Date",
                    Color = Color.Orchid
                },
                new Criteria
                {
                    Name = "Forme",
                    Color = Color.Olive
                },
                new Criteria
                {
                    Name = "Materiau",
                    Color = Color.Brown
                },
                new Criteria
                {
                    Name = "Artiste",
                    Color =  Color.Yellow
                }
            };
        }
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Museomix : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        Texture2D _circleTexture;

        #region Model
        Repository _repository = new Repository();
        #endregion

        public Texture2D CreateCircle(int radius)
        {
            int outerRadius = radius * 2 + 2; // So circle doesn't go out of bounds
            Texture2D texture = new Texture2D(GraphicsDevice, outerRadius, outerRadius);

            Color[] data = new Color[outerRadius * outerRadius];

            // Colour the entire texture transparent first.
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.Transparent;

            // Work out the minimum step necessary using trigonometry + sine approximation.
            double angleStep = 1f / radius;

            for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
            {
                // Use the parametric definition of a circle: http://en.wikipedia.org/wiki/Circle#Cartesian_coordinates
                int x = (int)Math.Round(radius + radius * Math.Cos(angle));
                int y = (int)Math.Round(radius + radius * Math.Sin(angle));

                data[y * outerRadius + x + 1] = Color.White;
            }

            texture.SetData(data);
            return texture;
        }

        public Museomix()
        : base()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _circleTexture = CreateCircle(128);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
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
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            var numberOfPoints = _repository.Criterias.Count();
            var angleIncrement = 360 / numberOfPoints;
            var circleRadius = 128;
            for (var i = 0; i < numberOfPoints; i++)
            {
                Vector2 pos = new Vector2
                {
                    X = (float)(circleRadius * Math.Cos((angleIncrement * i) * (Math.PI / 180.0f))) + GraphicsDevice.Viewport.X / 2.0f,
                    Y = (float)(circleRadius * Math.Sin((angleIncrement * i) * (Math.PI / 180.0f))) + GraphicsDevice.Viewport.Y / 2.0f
                };
                _spriteBatch.Draw(_circleTexture, pos, Color.Red);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
