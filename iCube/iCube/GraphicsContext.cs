using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace iCube
{
    public class GraphicsContext
    {
        public GraphicsDevice Device
        {
            get;
            protected set;
        }

        public PrimitiveBatch PrimitiveBatch
        {
            get;
            protected set;
        }

        public SpriteBatch SpriteBatch
        {
            get;
            protected set;
        }

        public SpriteFont SpriteFont
        {
            get;
            protected set;
        }

        public GraphicsContext(GraphicsDevice device, PrimitiveBatch pBatch, SpriteBatch sBatch)
        {
            Device = device;
            PrimitiveBatch = pBatch;
            SpriteBatch = sBatch;
        }
    }
}
