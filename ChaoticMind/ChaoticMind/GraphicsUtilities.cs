using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ChaoticMind.Graphics {
    class GraphicsUtilities {

        public SpriteBatch MainSpriteBatch { get; set; }

        Game _mainGame;
        GraphicsDeviceManager _graphics;


        public GraphicsUtilities(Game mainGame, bool goFullscreen) {
            _mainGame = mainGame;

            _graphics = new GraphicsDeviceManager(_mainGame);

            ScreenUtils.Initialize(_graphics, goFullscreen);
        }

        public void LoadContent() {
            _mainGame.Content.RootDirectory = "Content";
            SpriteAnimationSequence.SharedContentManager = _mainGame.Content;

            MainSpriteBatch = new SpriteBatch(_graphics.GraphicsDevice);
        }

        public GraphicsDevice GraphicsDevice { get { return _mainGame.GraphicsDevice; } }
    }
}
