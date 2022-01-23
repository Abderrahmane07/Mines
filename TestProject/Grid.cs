using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject
{
    public class Grid
    {
        public int Width { get; set; }       
        public int Height { get; set; }
        public int Bombs { get; set; }
        public int Score { get; set; }
        public Cell[,] Cells { get; set; }
        public State GameState { get; set; }

        public Grid(int width, int height, int bombs)
        {
            Width = width;
            Height = height;
            Bombs = bombs;
            Score = 0;
            GameState = State.gameReady;
            Cells = new Cell[Width, Height];

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Cells[i, j] = new Cell();
                    Cells[i, j].X = 24 + 48 * i;
                    Cells[i, j].Y = 24 + 48 * j;
                }
            }
        }

        Random rand = new Random();
        public void PrepareBombs()
        {
            int compteur = 0;
            while (compteur < Bombs)
            {
                int x = rand.Next(Width);
                int y = rand.Next(Height);
                if (Cells[x, y].IsBomb == false)
                {
                    Cells[x, y].IsBomb = true;
                    //Cells[x, y].Neighbors = 50;
                    compteur++;
                }

            }
        }


        public void CountNeighborBombs(Cell[,] Cells)
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if ((i > 0 && j > 0) && Cells[i - 1, j - 1].IsBomb)
                        Cells[i, j].Neighbors++;
                    if (j > 0 && Cells[i, j - 1].IsBomb)
                        Cells[i, j].Neighbors++;
                    if ((i < Width - 1 && j > 0) && Cells[i + 1, j - 1].IsBomb)
                        Cells[i, j].Neighbors++;
                    if (i > 0 && Cells[i - 1, j].IsBomb)
                        Cells[i, j].Neighbors++;
                    if (i < Width - 1 && Cells[i + 1, j].IsBomb)
                        Cells[i, j].Neighbors++;
                    if ((i > 0 && j < Height - 1) && Cells[i - 1, j + 1].IsBomb)
                        Cells[i, j].Neighbors++;
                    if (j < Height - 1 && Cells[i, j + 1].IsBomb)
                        Cells[i, j].Neighbors++;
                    if ((i < Width - 1 && j < Height - 1) && Cells[i + 1, j + 1].IsBomb)
                        Cells[i, j].Neighbors++;
                }
            }
        }

        public void Cascade(int i, int j)
        {

            if ((i > 0 && j > 0) && !Cells[i - 1, j - 1].IsRevealed)
                Uncover(i - 1, j - 1);
            if (j > 0 && !Cells[i, j - 1].IsRevealed)
                Uncover(i, j - 1);
            if ((i < Width - 1 && j > 0) && Cells[i + 1, j - 1].IsRevealed)
                Uncover(i + 1, j - 1);
            if (i > 0 && !Cells[i - 1, j].IsRevealed)
                Uncover(i - 1, j);
            if (i < Width - 1 && !Cells[i + 1, j].IsRevealed)
                Uncover(i + 1, j);
            if ((i > 0 && j < Height - 1) && !Cells[i - 1, j + 1].IsRevealed)
                Uncover(i - 1, j + 1);
            if (j < Height - 1 && !Cells[i, j + 1].IsRevealed)
                Uncover(i, j + 1);
            if ((i < Width - 1 && j < Height - 1) && !Cells[i + 1, j + 1].IsRevealed)
                Uncover(i + 1, j + 1);

        }


        public void Uncover(int i, int j)
        {
            if (!Cells[i, j].IsFlagged)
            {
                GameState = State.gamePlayed;
                Cells[i, j].IsRevealed = true;
                if (!Cells[i, j].IsBomb)
                {
                    if (Cells[i, j].Neighbors == 0)
                    {
                        Cells[i, j].Value = "CellUncoveredBlank";
                        Cells[i, j].Neighbors = 100;
                        Cascade(i, j);

                    }

                    else
                        Cells[i, j].Value = "CellUncovered" + Cells[i, j].Neighbors.ToString();
                    //Cells[i, j].Neighbors = 100;
                }

                else
                {
                    Cells[i, j].Value = "CellBomb";
                    GameState = State.gameFailed;
                }
            }
            
        }

        public enum State
        {
            gameReady,
            gamePlayed,
            gameFailed, 
            gameSuccesful
        }


        public void DrawCells()
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Console.Write(Cells[i, j].Value);
                }
                Console.WriteLine();
            }
        }
    }
}
