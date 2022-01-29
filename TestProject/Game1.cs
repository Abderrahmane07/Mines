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

        //public Grid _grid = new Grid(15, 15, 30);



        Grid grid = new Grid(15, 15, 30);

        MouseState mState;
        int cran = 100;
        bool mReleased = true;
        int timer = 0;
        int scoreToDisplay = 0;

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
            if (grid.GameState == Grid.State.gamePlayed)
                timer++;
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

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
                {
                    if (!grid.HasStarted)
                    {
                        if (grid.Cells[abscisse, ordonnee].IsBomb)
                        {
                            Reset();
                            grid.PrepareBombs();
                            grid.CountNeighborBombs(grid.Cells);
                            //grid.Uncover(abscisse, ordonnee);

                        }
                    }
                    grid.Uncover(abscisse, ordonnee);
                    grid.HasStarted = true;
                }

            }
            if (mState.RightButton == ButtonState.Pressed && mReleased && (grid.GameState == Grid.State.gamePlayed || grid.GameState == Grid.State.gameReady))
            {
                Vector2 position = mState.Position.ToVector2();
                int abscisse = (int)position.X / 48;
                int ordonnee = (int)(position.Y - cran) / 48;
                if (!grid.Cells[abscisse, ordonnee].IsRevealed)
                {
                    if (!grid.Cells[abscisse, ordonnee].IsFlagged)
                    {
                        scoreToDisplay++;
                        if(grid.Cells[abscisse, ordonnee].IsBomb)
                        {
                            grid.Score++;
                        }
                    }
                    else
                    {
                        if (grid.Cells[abscisse, ordonnee].IsBomb)
                        {
                            grid.Score--;
                        }
                        scoreToDisplay--;
                    }                       
                    grid.Cells[abscisse, ordonnee].IsFlagged = !grid.Cells[abscisse, ordonnee].IsFlagged;
                }
                if (grid.Score == grid.Bombs && scoreToDisplay == grid.Score)
                    grid.GameState = Grid.State.gameSuccesful;
                mReleased = false;
            }
            if (mState.RightButton == ButtonState.Released)
            {
                mReleased = true;
            }

            base.Update(gameTime);
        }


        public void Reset()
        {
            timer = 0;
            scoreToDisplay = 0;
            grid = new Grid(15, 15, 30);
            grid.GameState = Grid.State.gameReady;
        }

        public void SevenSegmentDigit(int number, int x, int y)
        {
            Texture2D chiffreHundreds;
            Texture2D chiffreDozens;
            Texture2D chiffreUnits;
            if (number < 1000)
            {
                chiffreHundreds = Content.Load<Texture2D>((number / 100).ToString());
                _spriteBatch.Draw(chiffreHundreds, new Vector2(x, y), Color.White);
            }
            else
            {
                chiffreHundreds = Content.Load<Texture2D>("9");
                _spriteBatch.Draw(chiffreHundreds, new Vector2(x, y), Color.White);
            }
            
            if (number > -1)
            {
                chiffreDozens = Content.Load<Texture2D>(((number / 10) % 10).ToString());
                _spriteBatch.Draw(chiffreDozens, new Vector2(x + 43, y), Color.White);
                chiffreUnits = Content.Load<Texture2D>((number % 10).ToString());
                _spriteBatch.Draw(chiffreUnits, new Vector2(x + 86, y), Color.White);
            }
            else
            {
                chiffreDozens = Content.Load<Texture2D>("0");
                _spriteBatch.Draw(chiffreDozens, new Vector2(x + 43, y), Color.White);
                chiffreUnits = Content.Load<Texture2D>("0");
                _spriteBatch.Draw(chiffreUnits, new Vector2(x + 86, y), Color.White);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            //_spriteBatch.DrawString(_basicFont, "Score: " + grid.Score, new Vector2(50, 30), Color.White);
            SevenSegmentDigit(grid.Bombs-scoreToDisplay, 90, 13);
            SevenSegmentDigit(timer / 60, 500, 13);
            //_spriteBatch.DrawString(_basicFont, $"Time: {timer / 60}", new Vector2(570, 30), Color.White);
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
            if (grid.GameState == Grid.State.gameSuccesful && scoreToDisplay==grid.Score)
            {
                _spriteBatch.DrawString(_basicFont, $"You won! Your record is: {timer / 60}", new Vector2(300, 350), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}