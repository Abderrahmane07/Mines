using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestProject
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _basicFont;

        Texture2D cellBombSprite;
        Texture2D cellCoveredSprite;
        Texture2D cellFlagedSprite;
        Texture2D cellRestartSprite;
        Texture2D cellUncovered1Sprite;
        Texture2D cellUncovered2Sprite;
        Texture2D cellUncovered3Sprite;
        Texture2D cellUncovered4Sprite;
        Texture2D cellUncovered5Sprite;
        Texture2D cellUncoveredBlankSprite;

        Grid grid = new Grid(15, 15, 30);

        MouseState mState;
        int cran = 100;
        bool mReleased = true;
        int timer = 0;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = grid.Width * 48;
            _graphics.PreferredBackBufferHeight = grid.Height * 48 + cran;
            _graphics.ApplyChanges();
            grid.PrepareBombs();
            grid.CountNeighborBombs(grid.Cells);
            grid.GameState = Grid.State.gameReady;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _basicFont = Content.Load<SpriteFont>("File");

            cellBombSprite = Content.Load<Texture2D>("CellBomb");
            cellCoveredSprite = Content.Load<Texture2D>("CellCovered");
            cellFlagedSprite = Content.Load<Texture2D>("CellFlaged");
            cellRestartSprite = Content.Load<Texture2D>("CellRestart");
            cellUncovered1Sprite = Content.Load<Texture2D>("CellUncovered1");
            cellUncovered2Sprite = Content.Load<Texture2D>("CellUncovered2");
            cellUncovered3Sprite = Content.Load<Texture2D>("CellUncovered3");
            cellUncovered4Sprite = Content.Load<Texture2D>("CellUncovered4");
            cellUncovered5Sprite = Content.Load<Texture2D>("CellUncovered5");
            cellUncoveredBlankSprite = Content.Load<Texture2D>("CellUncoveredBlank");
        }

        protected override void Update(GameTime gameTime)
        {
            if(grid.GameState==Grid.State.gamePlayed)
                timer++;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mState = Mouse.GetState();

            if (mState.LeftButton == ButtonState.Pressed)
            {
                
                Vector2 position = mState.Position.ToVector2();
                int abscisse = (int)position.X / 48;
                int ordonnee = (int)(position.Y - cran) / 48;
                if (((grid.Width * 24 - 37) < position.X && position.X < (grid.Width * 24 + 37)) && (0 < position.Y && position.Y < cran))
                {
                    Reset();
                    grid.PrepareBombs();
                    grid.CountNeighborBombs(grid.Cells);
                }
                if (position.Y > cran && (grid.GameState == Grid.State.gamePlayed || grid.GameState == Grid.State.gameReady))
                    grid.Uncover(abscisse, ordonnee);
            }
            if (mState.RightButton == ButtonState.Pressed && mReleased && (grid.GameState == Grid.State.gamePlayed || grid.GameState == Grid.State.gameReady))
            {
                Vector2 position = mState.Position.ToVector2();
                int abscisse = (int)position.X / 48;
                int ordonnee = (int)(position.Y - cran) / 48;
                if (!grid.Cells[abscisse, ordonnee].IsRevealed)
                {
                    if (!grid.Cells[abscisse, ordonnee].IsFlagged)
                        grid.Score++;
                    else
                        grid.Score--;
                    grid.Cells[abscisse, ordonnee].IsFlagged = !grid.Cells[abscisse, ordonnee].IsFlagged;
                }
                if (grid.Score == grid.Bombs)
                    grid.GameState = Grid.State.gameSuccesful;
                mReleased = false;
            }
            if (mState.RightButton == ButtonState.Released)
            {
                mReleased = true;
            }

            base.Update(gameTime);
        }


        private void Reset()
        {
            timer = 0;
            grid = new Grid(15, 15, 30);
            grid.GameState = Grid.State.gameReady;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(_basicFont, "Score: " + grid.Score, new Vector2(50, 30), Color.White);
            _spriteBatch.DrawString(_basicFont, $"Time: {timer/60}", new Vector2(570, 30), Color.White);
            _spriteBatch.Draw(cellRestartSprite, new Vector2((grid.Width * 48) / 2 - 37, 13), Color.White);
            //_spriteBatch.Draw(cellCoveredSprite, new Vector2(0, 0), Color.White);
            //_spriteBatch.Draw(cellBombSprite, new Vector2(ix, jx * 48), Color.White);
            for (int i = 0; i < grid.Width; i++)
            {
                for (int j = 0; j < grid.Height; j++)
                {
                    Texture2D valeur;
                    if (!grid.Cells[i, j].IsFlagged)
                    {
                        if (grid.Cells[i, j].Neighbors > 10)
                            valeur = Content.Load<Texture2D>("CellUncoveredBlank");
                        else
                            valeur = Content.Load<Texture2D>(grid.Cells[i, j].Value);
                    }

                    else
                        valeur = cellFlagedSprite;

                    _spriteBatch.Draw(valeur, new Vector2(i * 48, cran + j * 48), Color.White);
                }


            }
            if (grid.GameState == Grid.State.gameSuccesful)
            {
                _spriteBatch.DrawString(_basicFont, $"You won! Your record is: {timer / 60}", new Vector2(300, 350), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
