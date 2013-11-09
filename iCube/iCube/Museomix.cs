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
using QuickGraph;
using GraphSharp;
using GraphSharp.Algorithms.Layout;
using System.Windows;
using GraphSharp.Algorithms.Layout.Simple.Circular;
#endregion

namespace iCube
{
    public class Model
    {
        public enum CriteriaName
        {
            Couleur,
            Date,
            Artiste,
            Forme,
            Materiau
        }

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

        public Color Colour
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

    public class Criteria : IEquatable<Criteria>
    {
        public Model.CriteriaName Name
        {
            get;
            set;
        }

        public Color Colour
        {
            get;
            set;
        }

        public Criteria()
        {
        }

        public Boolean Equals(Criteria other)
        {
            return (other.Name == Name);
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
                    Colour = Color.Red,
                    Date = "1950",
                    Material = "Brick"
                },
                new Model
                {
                    Name = "Test2",
                    Colour = Color.Red,
                    Date = "1960",
                    Material = "Brick"
                },
                new Model
                {
                    Name = "Test3",
                    Colour = Color.Blue,
                    Date = "1950",
                    Material = "Brick"
                },
                new Model
                {
                    Name = "Test4",
                    Colour = Color.Red,
                    Date = "1950",
                    Material = "Brick"
                }
            };

            Criterias = new List<Criteria>
            {
                new Criteria
                {
                    Name = Model.CriteriaName.Couleur,
                    Colour = Color.Pink
                },
                new Criteria
                {
                    Name = Model.CriteriaName.Date,
                    Colour = Color.Orchid
                },
                new Criteria
                {
                    Name = Model.CriteriaName.Forme,
                    Colour = Color.Olive
                },
                new Criteria
                {
                    Name = Model.CriteriaName.Materiau,
                    Colour = Color.Brown
                },
                new Criteria
                {
                    Name = Model.CriteriaName.Artiste,
                    Colour =  Color.Yellow
                }
            };
        }
    }

#if false
    public class MuseomixEdge : Edge<Model>
    {
        public Color Color
        {
            get;
            set;
        }

        public MuseomixEdge(Color color, Model source, Model target)
        : base(source, target)
        {
            Color = color;
        }
    }

    public class MuseomixGraph : BidirectionalGraph<Model, MuseomixEdge>
    {
        public MuseomixGraph()
        {
        }

        public MuseomixGraph(bool allowParallelEdges)
        : base(allowParallelEdges)
        {
        }

        public MuseomixGraph(bool allowParallelEdges, int vertexCapacity)
        : base(allowParallelEdges, vertexCapacity)
        {
        }
    }
#endif
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Museomix : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        Texture2D _circleTexture;
        SpriteFont _font;
        PrimitiveBatch _primBatch;
        private static readonly float SphereRadius = 48;

        #region Model
        Repository _repository = new Repository();
        Diagram _graph;
        //ILayoutAlgorithm<Model, MuseomixEdge, MuseomixGraph> _algo;
        List<Criteria> _criterias = new List<Criteria>();
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
            return (texture);
        }

        #region Private Methods
        private void AddNewGraphEdge(Color colour, Model from, Model to)
        {
            MuseoNode fromNode = new MuseoNode(colour, from);
            fromNode.AddChild(new MuseoNode(colour, to));
            _graph.AddNode(fromNode);
        }
        #endregion

        public Museomix()
        : base()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1080;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.IsFullScreen = false;
            Content.RootDirectory = "Content";
            _graph = new Diagram();
            var models = _repository.Models;
            //var pointDic = new Dictionary<Model, System.Windows.Point>();
            //var pointSize = new Dictionary<Model, Size>();
            float center = _graphics.PreferredBackBufferHeight / 2.0f;
            foreach (var m in models)
            {
                _graph.AddNode(new MuseoNode(Color.White, m));
                //pointDic.Add(m, new System.Windows.Point(center, center));
                //pointSize.Add(m , new Size(64, 64));
            }

            //var layoutCtx = new LayoutContext<Model, MuseomixEdge, MuseomixGraph>(_graph, pointDic, pointSize, LayoutMode.Simple);
            //StandardLayoutAlgorithmFactory<Model, MuseomixEdge, MuseomixGraph> factory = new StandardLayoutAlgorithmFactory<Model, MuseomixEdge, MuseomixGraph>();
            //_algo = factory.CreateAlgorithm("Circular", layoutCtx, new CircularLayoutParameters());
            //_algo.Compute();
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
            _primBatch = new PrimitiveBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("Arial");
            _circleTexture = CreateCircle((int)SphereRadius);
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

            if (Keyboard.GetState().IsKeyDown(Keys.C))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                    _criterias.Remove(_repository.Criterias.Where(c => c.Name == iCube.Model.CriteriaName.Couleur).Single());
                else
                    _criterias.Add(_repository.Criterias.Where(c => c.Name == iCube.Model.CriteriaName.Couleur).Single());
                UpdateCriterias();
            }

            base.Update(gameTime);
        }

        private void UpdateCriterias()
        {
            var models = _repository.Models;
            _graph.Clear();
            foreach (var c in _criterias)
            {
                switch (c.Name)
                {
                    case Model.CriteriaName.Couleur:
                        {
                            foreach (var m1 in models)
                            {
                                var sameColours = from m in models where m.Colour == m1.Colour select m;
                                foreach (var m2 in sameColours)
                                {
                                    AddNewGraphEdge(c.Colour, m1, m2);
                                }
                            }
                        }
                        break;
                    case Model.CriteriaName.Date:
                        {
                            foreach (var m1 in models)
                            {
                                var sameArtists = from m in models where m.Date == m1.Date select m;
                                foreach (var m2 in sameArtists)
                                {
                                    AddNewGraphEdge(c.Colour, m1, m2);
                                }
                            }
                        }
                        break;
                    case Model.CriteriaName.Materiau:
                        {
                            foreach (var m1 in models)
                            {
                                var sameMaterial = from m in models where m.Material == m1.Material select m;
                                foreach (var m2 in sameMaterial)
                                {
                                    AddNewGraphEdge(c.Colour, m1, m2);
                                }
                            }
                        }
                        break;
                }
            }
            _graph.Arrange();
        }

        /// <summary>
        ///  This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            var numberOfPoints = _repository.Criterias.Count();
            var angleIncrement = 360 / numberOfPoints;
            var center = new Vector2(GraphicsDevice.Viewport.Width / 2.0f, GraphicsDevice.Viewport.Height / 2.0f);
            var circleRadius = Math.Min(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height) / 2.0f - SphereRadius;
            for (var i = 0; i < numberOfPoints; i++)
            {
                Vector2 pos = new Vector2
                {
                    X = (float)(circleRadius * Math.Cos((angleIncrement * i) * (Math.PI / 180.0f))) + center.X - SphereRadius,
                    Y = (float)(circleRadius * Math.Sin((angleIncrement * i) * (Math.PI / 180.0f))) + center.Y - SphereRadius
                };
                _spriteBatch.Draw(_circleTexture, pos, _repository.Criterias.ElementAt(i).Colour);
            }
#if false
            foreach (var vert in _algo.VertexPositions)
            {
                _spriteBatch.DrawString(_font, vert.Key.Name, new Vector2((float)vert.Value.X + center.X, (float)vert.Value.Y + center.Y), vert.Key.Colour);
            }
            foreach (var edge in _graph.Edges)
            {
                _primBatch.Begin(PrimitiveType.LineList);
                var vert = _algo.VertexPositions[edge.Source];
                _primBatch.AddVertex(new Vector2((float)vert.X + center.X, (float)vert.Y + center.Y), Color.White);
                vert = _algo.VertexPositions[edge.Target];
                _primBatch.AddVertex(new Vector2((float)vert.X + center.X, (float)vert.Y + center.Y), Color.White);
                _primBatch.End();
            }
#endif
            _primBatch.Begin(PrimitiveType.LineList);
            var ctx = new GraphicsContext(GraphicsDevice, _primBatch, _spriteBatch);
            var x = GraphicsDevice.Viewport.Width / 2.0f - circleRadius;
            _graph.Draw(ctx, new Rectangle((int)x, 0, (int)(GraphicsDevice.Viewport.Width - x - circleRadius), (int)GraphicsDevice.Viewport.Height));
            _spriteBatch.End();
            _primBatch.End();
            base.Draw(gameTime);
        }
    }
}
