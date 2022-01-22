using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestProject
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D cellBombSprite;
        Texture2D cellCoveredSprite;
        Texture2D cellFlagedSprite;
        Texture2D cellUncovered1Sprite;
        Texture2D cellUncovered2Sprite;
        Texture2D cellUncovered3Sprite;
        Texture2D cellUncovered4Sprite;
        Texture2D cellUncovered5Sprite;
        Texture2D cellUncoveredBlankSprite;

        MouseState mState;
        int ix = 0;
        int jx = 0;
        bool mReleased = true;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Grid grid = new Grid(10, 10, 10);
            grid.PrepareBombs();
            grid.CountNeighborBombs(grid.Cells);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            cellBombSprite = Content.Load<Texture2D>("CellBomb");
            cellCoveredSprite = Content.Load<Texture2D>("CellCovered");
            cellFlagedSprite = Content.Load<Texture2D>("CellFlaged");
            cellUncovered1Sprite = Content.Load<Texture2D>("CellUncovered1");
            cellUncovered2Sprite = Content.Load<Texture2D>("CellUncovered2");
            cellUncovered3Sprite = Content.Load<Texture2D>("CellUncovered3");
            cellUncovered4Sprite = Content.Load<Texture2D>("CellUncovered4");
            cellUncovered5Sprite = Content.Load<Texture2D>("CellUncovered5");
            cellUncoveredBlankSprite = Content.Load<Texture2D>("CellUncoveredBlank");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mState = Mouse.GetState();

            if (mState.LeftButton == ButtonState.Pressed)
            {
                ix++;
            }
            if (mState.RightButton == ButtonState.Pressed && mReleased)
            {
                //IsFlaged==true;
                jx++;
                mReleased = false;
            }
            if(mState.RightButton == ButtonState.Released)
            {
                mReleased = true;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(cellCoveredSprite, new Vector2(0, 0), Color.White);
            _spriteBatch.Draw(cellBombSprite, new Vector2(ix, jx*48), Color.White);
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Console.Write(Cells[i, j].Value);
                    _spriteBatch.Draw(cellCoveredSprite, new Vector2(0, 0), Color.White);
                }
                Console.WriteLine();
            }
            //_spriteBatch.Draw(cellCoveredSprite, new Vector2(48, 0), Color.White);
            //_spriteBatch.Draw(cellCoveredSprite, new Vector2(96, 0), Color.White);
            //_spriteBatch.Draw(cellCoveredSprite, new Vector2(96+48, 0), Color.White);
            //_spriteBatch.Draw(cellCoveredSprite, new Vector2(96+96, 0), Color.White);
            //_spriteBatch.Draw(cellCoveredSprite, new Vector2(192+48, 0), Color.White);
            //_spriteBatch.Draw(cellCoveredSprite, new Vector2(192+96, 0), Color.White);
            //_spriteBatch.Draw(cellCoveredSprite, new Vector2(192+144, 0), Color.White);
            //_spriteBatch.Draw(cellCoveredSprite, new Vector2(192+192, 0), Color.White);
            //_spriteBatch.Draw(cellCoveredSprite, new Vector2(432, 0), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
