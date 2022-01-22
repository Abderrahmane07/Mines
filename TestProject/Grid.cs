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
        public Cell[,] Cells { get; set; }

        public Grid(int width, int height, int bombs)
        {
            Width = width;
            Height = height;
            Bombs = bombs;
            Cells = new Cell[Width, Height];

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Cells[i, j] = new Cell();
                    Cells[i, j].X = j + 1;
                    Cells[i, j].Y = i + 1;
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


        public void Uncover()
        {
            int i = Int32.Parse(Console.ReadLine());
            int j = Int32.Parse(Console.ReadLine());
            if (!Cells[i, j].IsBomb)
                Cells[i, j].Value = Cells[i, j].Neighbors.ToString() + " ";
            else
                Cells[i, j].Value = "* ";
            DrawCells();
            Uncover();
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
